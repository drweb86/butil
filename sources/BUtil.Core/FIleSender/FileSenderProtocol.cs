using BUtil.Core.FileSystem;
using BUtil.Core.Logs;
using BUtil.Core.Misc;
using BUtil.Core.State;
using BUtil.Core.TasksTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BUtil.Core.FIleSender;
public interface IFileSenderProtocol
{
    void WriteCommandForServer(BinaryWriter writer, FileTransferProtocolServerCommand command);
    FileTransferProtocolServerCommand ReadCommandForServer(BinaryReader reader);

    void WriteCommandForClient(NetworkStream stream, FileTransferProtocolClientCommand command);
    FileTransferProtocolClientCommand ReadCommandForClient(NetworkStream stream);

    void WriteFileHeader(BinaryWriter writer, FileState fileState);
    FileState ReadFileHeader(BinaryReader reader);

    Task WriteFile(BinaryWriter writer, FileState fileState);
    Task ReadFile(BinaryReader reader, FileState fileState);

    void WriteProtocolVersion(BinaryWriter writer);
    void ReadCheckProtocolVersion(BinaryReader reader);

    SourceItemState TemporaryMoveMe();
}

public enum FileTransferProtocolServerCommand
{
    ReceiveFile = 13,
    Disconnect = 84,
}

public enum FileTransferProtocolClientCommand
{
    Cancel = 17,
    Continue = 23
}

internal class FileSenderProtocol: IFileSenderProtocol
{
    private const Int32 _version = 1;
    private readonly FileSenderIoc _ioc;

    public FileSenderProtocol(FileSenderIoc ioc)
    {
        _ioc = ioc;
    }

    public void WriteProtocolVersion(BinaryWriter writer)
    {
        writer.Write(_version);
    }

    public void ReadCheckProtocolVersion(BinaryReader reader)
    {
        var protocolVersion = reader.ReadInt32();
        if (_version != protocolVersion)
            throw new NotSupportedException("Protocol versions of server and client part are mismatching. Install same version of software.");
    }

    public SourceItemState TemporaryMoveMe()
    {
        var task = new GetStateOfSourceItemTask(new Events.TaskEvents(), new ConfigurationFileModels.V2.SourceItemV2 { IsFolder = true, Target = _ioc.Model.Folder }, new List<string> { }, _ioc);
        task.Execute();
        return task.SourceItemState!;
    }

    public void WriteCommandForServer(BinaryWriter writer, FileTransferProtocolServerCommand command)
    {
        writer.Write((Int32)command);
        writer.Flush();
        writer.BaseStream.Flush();
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

    public FileTransferProtocolClientCommand ReadCommandForClient(NetworkStream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.UTF8, true);
        return (FileTransferProtocolClientCommand)reader.ReadInt32();
    }

    public void WriteFileHeader(BinaryWriter writer, FileState fileState)
    {
        string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(_ioc.Model.Folder, fileState);
        var fileStateClone = new FileState(fileState);
        fileStateClone.FileName = relativeFileName;
        var json = JsonSerializer.Serialize(fileStateClone);
        using var inputStream = StringHelper.StringToMemoryStream(json);
        using var outputStream = new MemoryStream();
        _ioc.EncryptionService.EncryptAes256(inputStream, outputStream, _ioc.Model.Password);
        writer.Write(outputStream.Length);
        writer.Write(outputStream.ToArray());
        writer.Flush();
        writer.BaseStream.Flush();
    }

    public FileState ReadFileHeader(BinaryReader reader)
    {
        long length = reader.ReadInt64();
        using var inputStream = new MemoryStream(reader.ReadBytes((int)length));
        using var outputStream = new MemoryStream();
        _ioc.EncryptionService.DecryptAes256(inputStream, outputStream, _ioc.Model.Password);
        outputStream.Position = 0;
        var json = StringHelper.StringFromMemoryStream(outputStream);
        var fileState = JsonSerializer.Deserialize<FileState>(json)!;
        if (fileState.FileName.Contains("..") || Path.IsPathRooted(fileState.FileName))
            throw new SecurityException("No relative dirs in state! No full paths in state.");
        fileState.FileName = new FileInfo(Path.Combine(_ioc.Model.Folder, fileState.FileName)).FullName;
        return fileState;
    }

    public async Task WriteFile(BinaryWriter writer, FileState fileState)
    {
        using var tempFolder = new TempFolder();
        var encryptedFile = Path.Combine(tempFolder.Folder, "file");
        _ioc.EncryptionService.EncryptAes256File(fileState.FileName, encryptedFile, _ioc.Model.Password);
        long fileSize = new FileInfo(encryptedFile).Length;
        using var inputStream = File.OpenRead(encryptedFile);

        writer.Write(fileSize);
        writer.Flush();
        writer.BaseStream.Flush();

        await inputStream.CopyToAsync(writer.BaseStream);
    }

    public async Task ReadFile(BinaryReader reader, FileState fileState)
    {
        string parentDir = Path.GetDirectoryName(fileState.FileName)!;
        if (!Directory.Exists(parentDir))
            Directory.CreateDirectory(parentDir);

        var blobSize = reader.ReadInt64();

        using var tempFolder = new TempFolder();
        var encryptedFile = Path.Combine(tempFolder.Folder, "file");

        using (var fileStream = File.Create(encryptedFile))
        {
            long bytesRemaining = blobSize;
            byte[] buffer = new byte[8192];

            while (bytesRemaining > 0)
            {
                int bytesToRead = (int)Math.Min(buffer.Length, bytesRemaining);
                int bytesRead = await reader.BaseStream.ReadAsync(buffer, 0, bytesToRead);
                if (bytesRead == 0)
                    throw new EndOfStreamException();

                await fileStream.WriteAsync(buffer, 0, bytesRead);
                bytesRemaining -= bytesRead;
            }
        }

        _ioc.EncryptionService.DecryptAes256File(encryptedFile, fileState.FileName, _ioc.Model.Password);
    }
}
