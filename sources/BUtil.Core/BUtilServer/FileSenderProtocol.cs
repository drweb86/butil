using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree.BUtilServer.Client;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace BUtil.Core.FIleSender;
public interface IFileSenderClientProtocol
{
    void WriteCommandForServer(BinaryWriter writer, FileTransferProtocolServerCommand command);
    FileTransferProtocolClientCommand ReadCommandForClient(NetworkStream stream);
    void WriteFileHeader(BinaryWriter writer, FileState fileState);
    void WriteFile(BinaryWriter writer, FileState fileState);
    void WriteProtocolVersion(BinaryWriter writer);
}

internal class FileSenderProtocol: IFileSenderClientProtocol
{
    private const Int32 _version = 1;
    private readonly BUtilClientIoc _ioc;
    private readonly string _folder;
    private readonly string _password;

    public FileSenderProtocol(BUtilClientIoc ioc, string folder, string password)
    {
        _ioc = ioc;
        _folder = folder;
        _password = password;
    }

    public void WriteProtocolVersion(BinaryWriter writer)
    {
        writer.Write(_version);
    }

    public void WriteCommandForServer(BinaryWriter writer, FileTransferProtocolServerCommand command)
    {
        writer.Write((Int32)command);
        writer.Flush();
        writer.BaseStream.Flush();
    }

    public FileTransferProtocolClientCommand ReadCommandForClient(NetworkStream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.UTF8, true);
        return (FileTransferProtocolClientCommand)reader.ReadInt32();
    }

    public void WriteFileHeader(BinaryWriter writer, FileState fileState)
    {
        string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(_folder, fileState);
        var fileStateClone = new FileState(fileState);
        fileStateClone.FileName = relativeFileName;
        var json = JsonSerializer.Serialize(fileStateClone);
        using var inputStream = StringHelper.StringToMemoryStream(json);
        using var outputStream = new MemoryStream();
        _ioc.Common.EncryptionService.EncryptAes256(inputStream, outputStream, _password);
        writer.Write(outputStream.Length);
        writer.Write(outputStream.ToArray());
        writer.Flush();
        writer.BaseStream.Flush();
    }

    public void WriteFile(BinaryWriter writer, FileState fileState)
    {
        using var tempFolder = new TempFolder();
        var encryptedFile = Path.Combine(tempFolder.Folder, "file");
        _ioc.Common.EncryptionService.EncryptAes256File(fileState.FileName, encryptedFile, _password);
        long fileSize = new FileInfo(encryptedFile).Length;
        using var inputStream = System.IO.File.OpenRead(encryptedFile);

        writer.Write(fileSize);
        writer.Flush();
        writer.BaseStream.Flush();

        inputStream.CopyTo(writer.BaseStream);
    }

    public void ReadFile(BinaryReader reader, FileState fileState)
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

        _ioc.Common.EncryptionService.DecryptAes256File(encryptedFile, fileState.FileName, _password);
        var sha512 = _ioc.Common.CachedHashService.GetSha512(fileState.FileName, false);
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
