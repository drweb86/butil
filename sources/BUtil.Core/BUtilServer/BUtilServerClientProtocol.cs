using BUtil.Core.ConfigurationFileModels.V2;
using BUtil.Core.FIleSender;
using BUtil.Core.FileSystem;
using BUtil.Core.Misc;
using BUtil.Core.Services;
using BUtil.Core.State;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace BUtil.Core.BUtilServer;

public interface IBUtilServerClientProtocol
{
    void WriteCommandForServer(BinaryWriter writer, FileTransferProtocolServerCommand command);
    FileTransferProtocolClientCommand ReadCommandForClient(NetworkStream stream);
    void WriteFileHeader(BinaryWriter writer, FileState fileState, string rootFolder, string password);
    void WriteFile(BinaryWriter writer, FileState fileState, string password);
    void WriteProtocolVersion(BinaryWriter writer);
}

internal class BUtilServerClientProtocol: IBUtilServerClientProtocol
{
    private const int _version = 1;
    private readonly IEncryptionService _encryptionService;

    public BUtilServerClientProtocol(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    public void WriteProtocolVersion(BinaryWriter writer)
    {
        writer.Write(_version);
    }

    public void WriteCommandForServer(BinaryWriter writer, FileTransferProtocolServerCommand command)
    {
        writer.Write((int)command);
        writer.Flush();
        writer.BaseStream.Flush();
    }

    public FileTransferProtocolClientCommand ReadCommandForClient(NetworkStream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.UTF8, true);
        return (FileTransferProtocolClientCommand)reader.ReadInt32();
    }

    public void WriteFileHeader(BinaryWriter writer, FileState fileState, string rootFolder, string password)
    {
        string relativeFileName = SourceItemHelper.GetSourceItemRelativeFileName(rootFolder, fileState);
        var fileStateClone = new FileState(fileState);
        fileStateClone.FileName = relativeFileName;
        var json = JsonSerializer.Serialize(fileStateClone);
        using var inputStream = StringHelper.StringToMemoryStream(json);
        using var outputStream = new MemoryStream();
        _encryptionService.EncryptAes256(inputStream, outputStream, password);
        writer.Write(outputStream.Length);
        writer.Write(outputStream.ToArray());
        writer.Flush();
        writer.BaseStream.Flush();
    }

    public void WriteFile(BinaryWriter writer, FileState fileState, string password)
    {
        using var tempFolder = new TempFolder();
        var encryptedFile = Path.Combine(tempFolder.Folder, "file");
        _encryptionService.EncryptAes256File(fileState.FileName, encryptedFile, password);
        long fileSize = new FileInfo(encryptedFile).Length;
        using var inputStream = File.OpenRead(encryptedFile);

        writer.Write(fileSize);
        writer.Flush();
        writer.BaseStream.Flush();

        inputStream.CopyTo(writer.BaseStream);
    }
}
