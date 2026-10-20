package com.siarheikuchuk.butil.core.services;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.EOFException;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.nio.charset.StandardCharsets;
import java.security.GeneralSecurityException;
import java.security.SecureRandom;
import java.util.Arrays;
import java.util.Base64;
import javax.crypto.Cipher;
import javax.crypto.Mac;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

/**
 * AES-256-CBC with the desktop app's version-1 header.
 *
 * <p>Layout, matching {@code EncryptionService} in the C# app: little-endian version (1),
 * key size in bits, block size in bits, PBKDF2 iterations, 7-bit-length ASCII hash name,
 * key salt, IV, then PKCS7 ciphertext. The key is PBKDF2-HMAC-SHA512 of the UTF-8 password,
 * with the iteration count from the header and an output length equal to the salt.
 */
public final class EncryptionService implements IEncryptionService {
    private static final int VERSION = 1;
    private static final int KEY_SIZE_BITS = 256;
    private static final int BLOCK_SIZE_BITS = 128;
    private static final int CREATE_KEY_ITERATIONS = 1000;
    private static final String HASH_ALGORITHM = "SHA512";

    @Override
    public byte[] encryptAes256(byte[] data, String password) throws IOException {
        ByteArrayInputStream input = new ByteArrayInputStream(data);
        ByteArrayOutputStream output = new ByteArrayOutputStream();
        encryptAes256(input, output, password);
        return output.toByteArray();
    }

    @Override
    public String encryptTextAes256Base64(String text, String password) throws IOException {
        byte[] plain = text.getBytes(StandardCharsets.UTF_8);
        return Base64.getEncoder().encodeToString(encryptAes256(plain, password));
    }

    @Override
    public void encryptAes256File(String inputFile, String outputFile, String password) throws IOException {
        try (InputStream input = new FileInputStream(inputFile);
                OutputStream output = new FileOutputStream(outputFile)) {
            encryptAes256(input, output, password);
        }
    }

    @Override
    public void encryptAes256(InputStream inputStream, OutputStream outputStream, String password) throws IOException {
        byte[] keySalt = new byte[KEY_SIZE_BITS / 8];
        byte[] iv = new byte[BLOCK_SIZE_BITS / 8];
        SecureRandom random = new SecureRandom();
        random.nextBytes(keySalt);
        random.nextBytes(iv);

        writeInt32Le(outputStream, VERSION);
        writeInt32Le(outputStream, KEY_SIZE_BITS);
        writeInt32Le(outputStream, BLOCK_SIZE_BITS);
        writeInt32Le(outputStream, CREATE_KEY_ITERATIONS);
        writeAsciiString(outputStream, HASH_ALGORITHM);
        outputStream.write(keySalt);
        outputStream.write(iv);

        byte[] key = createKey(password, keySalt, CREATE_KEY_ITERATIONS, HASH_ALGORITHM);
        transform(inputStream, outputStream, key, iv, Cipher.ENCRYPT_MODE);
    }

    @Override
    public String decryptTextAes256Base64(String text, String password) throws IOException {
        byte[] encrypted = Base64.getDecoder().decode(text);
        return new String(decryptAes256(encrypted, password), StandardCharsets.UTF_8);
    }

    @Override
    public byte[] decryptAes256(byte[] data, String password) throws IOException {
        ByteArrayInputStream input = new ByteArrayInputStream(data);
        ByteArrayOutputStream output = new ByteArrayOutputStream();
        decryptAes256(input, output, password);
        return output.toByteArray();
    }

    @Override
    public void decryptAes256(InputStream inputStream, OutputStream outputStream, String password) throws IOException {
        int version = readInt32Le(inputStream);
        if (version != VERSION) {
            throw new IllegalStateException("Unsupported version");
        }

        int keySizeBits = readInt32Le(inputStream);
        int blockSizeBits = readInt32Le(inputStream);
        int iterations = readInt32Le(inputStream);
        String hashAlgorithm = readAsciiString(inputStream);
        if (keySizeBits % 8 != 0 || blockSizeBits % 8 != 0 || keySizeBits <= 0 || blockSizeBits <= 0) {
            throw new IllegalArgumentException("hashAlgorithm");
        }

        byte[] keySalt = readExactly(inputStream, keySizeBits / 8);
        byte[] key = createKey(password, keySalt, iterations, hashAlgorithm);
        byte[] iv = readExactly(inputStream, blockSizeBits / 8);
        transform(inputStream, outputStream, key, iv, Cipher.DECRYPT_MODE);
    }

    @Override
    public void decryptAes256File(String inputFile, String outputFile, String password) throws IOException {
        try (InputStream input = new FileInputStream(inputFile);
                OutputStream output = new FileOutputStream(outputFile)) {
            decryptAes256(input, output, password);
        }
    }

    private static void transform(
            InputStream input, OutputStream output, byte[] key, byte[] iv, int mode) throws IOException {
        try {
            Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
            cipher.init(mode, new SecretKeySpec(key, "AES"), new IvParameterSpec(iv));
            byte[] buffer = new byte[8192];
            int read;
            while ((read = input.read(buffer)) >= 0) {
                byte[] part = cipher.update(buffer, 0, read);
                if (part != null && part.length > 0) {
                    output.write(part);
                }
            }
            byte[] tail = cipher.doFinal();
            if (tail != null && tail.length > 0) {
                output.write(tail);
            }
        } catch (javax.crypto.BadPaddingException ex) {
            if (mode == Cipher.DECRYPT_MODE) {
                throw new InvalidPasswordException(ex);
            }
            throw new IOException(ex.getMessage(), ex);
        } catch (GeneralSecurityException ex) {
            throw new IOException(ex.getMessage(), ex);
        }
    }

    /**
     * PBKDF2-HMAC-SHA512. Output length is {@code salt.length}, matching
     * {@code Rfc2898DeriveBytes.Pbkdf2} in the desktop app. The password is UTF-8.
     */
    static byte[] createKey(String password, byte[] salt, int iterations, String hashAlgorithm) {
        if (!HASH_ALGORITHM.equals(hashAlgorithm)) {
            throw new IllegalArgumentException("hashAlgorithm");
        }
        if (iterations <= 0) {
            throw new IllegalArgumentException("iterations");
        }
        byte[] passwordBytes = password.getBytes(StandardCharsets.UTF_8);
        try {
            Mac mac = Mac.getInstance("HmacSHA512");
            mac.init(new SecretKeySpec(passwordBytes, "HmacSHA512"));
            int hashLength = mac.getMacLength();
            int blocks = (salt.length + hashLength - 1) / hashLength;
            byte[] derived = new byte[salt.length];
            byte[] block = Arrays.copyOf(salt, salt.length + 4);
            int offset = 0;
            for (int index = 1; index <= blocks; index++) {
                block[salt.length] = (byte) (index >>> 24);
                block[salt.length + 1] = (byte) (index >>> 16);
                block[salt.length + 2] = (byte) (index >>> 8);
                block[salt.length + 3] = (byte) index;
                byte[] mixed = mac.doFinal(block);
                byte[] chunk = mixed.clone();
                for (int round = 1; round < iterations; round++) {
                    mixed = mac.doFinal(mixed);
                    for (int i = 0; i < mixed.length; i++) {
                        chunk[i] ^= mixed[i];
                    }
                }
                int copy = Math.min(hashLength, derived.length - offset);
                System.arraycopy(chunk, 0, derived, offset, copy);
                offset += copy;
            }
            return derived;
        } catch (GeneralSecurityException ex) {
            throw new IllegalStateException(ex.getMessage(), ex);
        }
    }

    private static void writeInt32Le(OutputStream output, int value) throws IOException {
        output.write(value & 0xFF);
        output.write((value >>> 8) & 0xFF);
        output.write((value >>> 16) & 0xFF);
        output.write((value >>> 24) & 0xFF);
    }

    private static void writeAsciiString(OutputStream output, String value) throws IOException {
        byte[] bytes = value.getBytes(StandardCharsets.US_ASCII);
        write7BitEncodedInt(output, bytes.length);
        output.write(bytes);
    }

    /** .NET {@code BinaryWriter.Write7BitEncodedInt}. */
    private static void write7BitEncodedInt(OutputStream output, int value) throws IOException {
        int remaining = value;
        while (remaining > 0x7F) {
            output.write((remaining & 0x7F) | 0x80);
            remaining >>>= 7;
        }
        output.write(remaining);
    }

    private static int readInt32Le(InputStream input) throws IOException {
        int b0 = readByte(input);
        int b1 = readByte(input);
        int b2 = readByte(input);
        int b3 = readByte(input);
        return b0 | (b1 << 8) | (b2 << 16) | (b3 << 24);
    }

    private static String readAsciiString(InputStream input) throws IOException {
        int length = read7BitEncodedInt(input);
        return new String(readExactly(input, length), StandardCharsets.US_ASCII);
    }

    private static int read7BitEncodedInt(InputStream input) throws IOException {
        int result = 0;
        int shift = 0;
        while (shift <= 28) {
            int current = readByte(input);
            result |= (current & 0x7F) << shift;
            if ((current & 0x80) == 0) {
                return result;
            }
            shift += 7;
        }
        throw new IOException("Invalid 7-bit encoded integer");
    }

    private static byte[] readExactly(InputStream input, int length) throws IOException {
        byte[] buffer = new byte[length];
        int offset = 0;
        while (offset < length) {
            int read = input.read(buffer, offset, length - offset);
            if (read < 0) {
                throw new EOFException();
            }
            offset += read;
        }
        return buffer;
    }

    private static int readByte(InputStream input) throws IOException {
        int value = input.read();
        if (value < 0) {
            throw new EOFException();
        }
        return value;
    }
}
