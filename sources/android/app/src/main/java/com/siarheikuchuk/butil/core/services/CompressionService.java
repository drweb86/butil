package com.siarheikuchuk.butil.core.services;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import org.brotli.dec.BrotliInputStream;

/**
 * Brotli compress and decompress, matching {@code CompressionService} in the desktop app.
 */
public final class CompressionService implements ICompressionService {
    @Override
    public void compressBrotliFile(String inputFile, String outputFile) throws IOException {
        try (InputStream input = new FileInputStream(inputFile);
                OutputStream output = new FileOutputStream(outputFile)) {
            compressBrotliStream(input, output);
        }
    }

    public void compressBrotliStream(InputStream inputStream, OutputStream outputStream) throws IOException {
        BrotliEncoder.compress(inputStream, outputStream);
    }

    @Override
    public void decompressBrotliFile(String inputFile, String outputFile) throws IOException {
        try (InputStream input = new FileInputStream(inputFile);
                OutputStream output = new FileOutputStream(outputFile)) {
            decompressBrotliStream(input, output);
        }
    }

    public void decompressBrotliStream(InputStream inputStream, OutputStream outputStream) throws IOException {
        BrotliInputStream brotli = new BrotliInputStream(inputStream);
        try {
            byte[] buffer = new byte[8192];
            int read;
            while ((read = brotli.read(buffer)) >= 0) {
                outputStream.write(buffer, 0, read);
            }
        } finally {
            brotli.close();
        }
    }
}
