package com.siarheikuchuk.butil.core.services;

import java.io.IOException;

/**
 * Brotli file tools used by the desktop app ({@code BUtil.Core.Services.ICompressionService}).
 */
public interface ICompressionService {
    void compressBrotliFile(String inputFile, String outputFile) throws IOException;

    void decompressBrotliFile(String inputFile, String outputFile) throws IOException;
}
