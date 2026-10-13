package com.siarheikuchuk.butil.core.services;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

/**
 * AES-256 file format used by the desktop app ({@code BUtil.Core.Services.IEncryptionService}).
 */
public interface IEncryptionService {
    void encryptAes256File(String inputFile, String outputFile, String password) throws IOException;

    void decryptAes256File(String inputFile, String outputFile, String password) throws IOException;

    String encryptTextAes256Base64(String text, String password) throws IOException;

    String decryptTextAes256Base64(String text, String password) throws IOException;

    void decryptAes256(InputStream inputStream, OutputStream outputStream, String password) throws IOException;

    void encryptAes256(InputStream inputStream, OutputStream outputStream, String password) throws IOException;

    byte[] encryptAes256(byte[] data, String password) throws IOException;

    byte[] decryptAes256(byte[] data, String password) throws IOException;
}
