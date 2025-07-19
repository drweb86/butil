using BUtil.Core.FileSystem;
using BUtil.Core.Hashing;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using BUtil.Core.TasksTree.BUtilServer.Server;
using System;
using System.IO;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Text.Json;

namespace BUtil.Core.FIleSender;
public interface IBUtilServerProtocol
{
    FileTransferProtocolServerCommand ReadCommandForServer(BinaryReader reader);
    void WriteCommandForClient(NetworkStream stream, FileTransferProtocolClientCommand command);
    FileState ReadFileHeader(BinaryReader reader, string password);
    void ReadFile(BinaryReader reader, FileState fileState, string password);
    void ReadCheckProtocolVersion(BinaryReader reader);
}

internal class BUtilServerProtocol : IBUtilServerProtocol
{
    private const Int32 _version = 1;
    private readonly ICachedHashService _cachedHashService;
    private readonly IEncryptionService _encryptionService;

    public BUtilServerProtocol(ICachedHashService cachedHashService, IEncryptionService encryptionService)
    {
        _cachedHashService = cachedHashService;
        _encryptionService = encryptionService;
    }

    public void ReadCheckProtocolVersion(BinaryReader reader)
    {
        var protocolVersion = reader.ReadInt32();
        if (_version != protocolVersion)
            throw new NotSupportedException("Protocol versions of server and client part are mismatching. Install same version of software.");
    }

    public void WriteCommandForClient(NetworkStream stream, FileTransferProtocolClientCommand command)
    {
        using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
        writer.Write((Int32)command);
        writer.Flush();
        writer.BaseStream.Flush();
    }

    public FileTransferProtocolServerCommand ReadCommandForServer(BinaryReader reader)
    {
        return (FileTransferProtocolServerCommand)reader.ReadInt32();
    }

    public FileState ReadFileHeader(BinaryReader reader, string password)
    {
        long length = reader.ReadInt64();
        using var inputStream = new MemoryStream(reader.ReadBytes((int)length));
        using var outputStream = new MemoryStream();
        _encryptionService.DecryptAes256(inputStream, outputStream, password);
        outputStream.Position = 0;
        var json = StringHelper.StringFromMemoryStream(outputStream);
        var fileState = JsonSerializer.Deserialize<FileState>(json)!;
        if (fileState.FileName.Contains("..") || Path.IsPathRooted(fileState.FileName))
            throw new SecurityException("No relative dirs in state! No full paths in state.");
        return fileState;
    }

    public void ReadFile(BinaryReader reader, FileState fileState, string password)
    {
        string parentDir = Path.GetDirectoryName(fileState.FileName)!;
        if (!Directory.Exists(parentDir))
            Directory.CreateDirectory(parentDir);

        var blobSize = reader.ReadInt64();

        using var tempFolder = new TempFolder();
        var encryptedFile = Path.Combine(tempFolder.Folder, "file");

        using (var fileStream = System.IO.File.Create(encryptedFile))
        {
            long bytesRemaining = blobSize;
            byte[] buffer = new byte[8192];

            while (bytesRemaining > 0)
            {
                int bytesToRead = (int)Math.Min(buffer.Length, bytesRemaining);
                int bytesRead = reader.BaseStream.Read(buffer, 0, bytesToRead);
                if (bytesRead == 0)
                    throw new EndOfStreamException();

                fileStream.Write(buffer, 0, bytesRead);
                bytesRemaining -= bytesRead;
            }
        }

        _encryptionService.DecryptAes256File(encryptedFile, fileState.FileName, password);
        var sha512 = _cachedHashService.GetSha512(fileState.FileName, false);
        if (fileState.Sha512 != sha512)
        {
            System.IO.File.Delete(fileState.FileName);
            throw new InvalidDataException($"Hash of {fileState.FileName} is {sha512}, but expected {fileState.Sha512}");
        }
        if (new FileInfo(fileState.FileName).Length != fileState.Size)
        {
            System.IO.File.Delete(fileState.FileName);
            throw new InvalidDataException($"Size of {fileState.FileName} is {new FileInfo(fileState.FileName).Length}, but expected {fileState.Size}");
        }
        System.IO.File.SetLastWriteTimeUtc(fileState.FileName, fileState.LastWriteTimeUtc);
    }
}
