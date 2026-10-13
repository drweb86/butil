package com.siarheikuchuk.butil.core.services;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.util.List;

/**
 * Standard Brotli (RFC 7932) encoder.
 *
 * <p>The desktop app writes Brotli with {@code BrotliStream} at
 * {@code CompressionLevel.SmallestSize}. This encoder writes the same format: a window of 16,
 * Huffman-coded meta-blocks when that is smaller, otherwise uncompressed meta-blocks. Any
 * Brotli decoder, including .NET {@code BrotliStream}, can read the result. Streams produced
 * by the desktop app are decoded with {@code org.brotli.dec.BrotliInputStream}.
 */
final class BrotliEncoder {
    private static final int CHUNK_SIZE = 65536;
    private static final int[] INSERT_LENGTH_N_BITS = {
        0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 7, 8, 9, 10, 12, 14, 24
    };
    private static final int[] COPY_LENGTH_N_BITS = {
        0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 7, 8, 9, 10, 24
    };
    private static final int[] CODE_LENGTH_CODE_ORDER = {
        1, 2, 3, 4, 0, 5, 17, 6, 16, 7, 8, 9, 10, 11, 12, 13, 14, 15
    };
    private static final int[] INSERT_OFFSET = new int[24];
    private static final int[] COMMAND_FOR_INSERT = new int[24];

    static {
        for (int i = 0; i < 23; i++) {
            INSERT_OFFSET[i + 1] = INSERT_OFFSET[i] + (1 << INSERT_LENGTH_N_BITS[i]);
        }
        Arrays.fill(COMMAND_FOR_INSERT, -1);
        for (int command = 0; command < 704; command++) {
            int range = command >> 6;
            if (range >= 2) {
                range -= 2;
            }
            int insertCode = (((0x29850 >> (range * 2)) & 0x3) << 3) | ((command >> 3) & 7);
            int copyCode = (((0x26244 >> (range * 2)) & 0x3) << 3) | (command & 7);
            if (COPY_LENGTH_N_BITS[copyCode] == 0 && COMMAND_FOR_INSERT[insertCode] < 0) {
                COMMAND_FOR_INSERT[insertCode] = command;
            }
        }
        for (int command : COMMAND_FOR_INSERT) {
            if (command < 0) {
                throw new IllegalStateException("Missing Brotli insert command");
            }
        }
    }

    private BrotliEncoder() {
    }

    static void compress(InputStream input, OutputStream output) throws IOException {
        byte[] first = readChunk(input, -1);
        if (first == null) {
            output.write(6);
            return;
        }
        BitWriter writer = new BitWriter();
        writer.writeBits(0, 1);
        byte[] chunk = first;
        while (true) {
            int next = input.read();
            boolean last = next < 0;
            writeSmaller(writer, chunk, last);
            if (last) {
                break;
            }
            chunk = readChunk(input, next);
        }
        writer.writeTo(output);
    }

    private static byte[] readChunk(InputStream input, int already) throws IOException {
        byte[] buffer = new byte[CHUNK_SIZE];
        int filled = 0;
        if (already >= 0) {
            buffer[0] = (byte) already;
            filled = 1;
        }
        while (filled < buffer.length) {
            int read = input.read(buffer, filled, buffer.length - filled);
            if (read < 0) {
                break;
            }
            filled += read;
        }
        if (filled == 0) {
            return null;
        }
        return filled == buffer.length ? buffer : Arrays.copyOf(buffer, filled);
    }

    private static void writeSmaller(BitWriter writer, byte[] chunk, boolean last) {
        int align = writer.bitLength & 7;
        int plainBits = countBits(align, trial -> writeUncompressed(trial, chunk, last));
        int codedBits = Integer.MAX_VALUE;
        try {
            codedBits = countBits(align, trial -> writeCompressed(trial, chunk, last));
        } catch (RuntimeException ignored) {
            codedBits = Integer.MAX_VALUE;
        }
        if (codedBits < plainBits) {
            writeCompressed(writer, chunk, last);
        } else {
            writeUncompressed(writer, chunk, last);
        }
    }

    private interface Trial {
        void write(BitWriter writer);
    }

    private static int countBits(int align, Trial trial) {
        BitWriter writer = new BitWriter();
        if (align != 0) {
            writer.writeBits(0, align);
        }
        int start = writer.bitLength;
        trial.write(writer);
        return writer.bitLength - start;
    }

    /** Non-last uncompressed meta-block, plus an empty last meta-block when this ends the stream. */
    private static void writeUncompressed(BitWriter writer, byte[] chunk, boolean last) {
        writer.writeBits(0, 1);
        writeLength(writer, chunk.length);
        writer.writeBits(1, 1);
        writer.align();
        writer.writeBytes(chunk);
        if (last) {
            writer.writeBits(1, 1);
            writer.writeBits(1, 1);
        }
    }

    private static void writeCompressed(BitWriter writer, byte[] chunk, boolean last) {
        if (last) {
            writer.writeBits(1, 1);
            writer.writeBits(0, 1);
        } else {
            writer.writeBits(0, 1);
        }
        writeLength(writer, chunk.length);
        if (!last) {
            writer.writeBits(0, 1);
        }

        writer.writeBits(0, 1);
        writer.writeBits(0, 1);
        writer.writeBits(0, 1);
        writer.writeBits(0, 2);
        writer.writeBits(0, 4);
        writer.writeBits(0, 2);
        writer.writeBits(0, 1);
        writer.writeBits(0, 1);

        int[] literalLengths = literalCodeLengths(chunk);
        int[] literalKeys = canonicalKeys(literalLengths);
        writeLiteralTree(writer, chunk, literalLengths);

        int insertCode = insertCodeFor(chunk.length);
        writeSimpleSingleSymbol(writer, COMMAND_FOR_INSERT[insertCode], 704);
        writeSimpleSingleSymbol(writer, 0, 64);

        int extra = chunk.length - INSERT_OFFSET[insertCode];
        writer.writeBits(extra, INSERT_LENGTH_N_BITS[insertCode]);
        writeLiterals(writer, chunk, literalLengths, literalKeys);
    }

    private static void writeLength(BitWriter writer, int length) {
        if (length <= 0 || length > CHUNK_SIZE) {
            throw new IllegalArgumentException("length");
        }
        writer.writeBits(0, 2);
        int stored = length - 1;
        for (int nibble = 0; nibble < 4; nibble++) {
            writer.writeBits((stored >> (4 * nibble)) & 0xF, 4);
        }
    }

    private static int insertCodeFor(int length) {
        for (int code = 0; code < 23; code++) {
            if (length < INSERT_OFFSET[code + 1]) {
                return code;
            }
        }
        return 23;
    }

    private static void writeSimpleSingleSymbol(BitWriter writer, int symbol, int alphabetSize) {
        writer.writeBits(1, 2);
        writer.writeBits(0, 2);
        writer.writeBits(symbol, 1 + log2Floor(alphabetSize - 1));
    }

    private static int[] literalCodeLengths(byte[] chunk) {
        int[] frequency = new int[256];
        int distinct = 0;
        for (byte value : chunk) {
            if (frequency[value & 0xFF]++ == 0) {
                distinct++;
            }
        }
        int[] lengths = new int[256];
        if (distinct <= 4) {
            int[] symbols = symbolsByFrequency(frequency);
            int[] pattern = simpleLengths(distinct);
            for (int i = 0; i < distinct; i++) {
                lengths[symbols[i]] = pattern[i];
            }
            return lengths;
        }
        int[] huffman = huffmanLengths(frequency);
        if (huffman == null) {
            throw new IllegalStateException("Huffman");
        }
        return huffman;
    }

    private static int[] simpleLengths(int distinct) {
        if (distinct == 1) {
            return new int[] {1};
        }
        if (distinct == 2) {
            return new int[] {1, 1};
        }
        if (distinct == 3) {
            return new int[] {1, 2, 2};
        }
        if (distinct == 4) {
            return new int[] {1, 2, 3, 3};
        }
        throw new IllegalArgumentException();
    }

    private static int[] symbolsByFrequency(int[] frequency) {
        Integer[] order = new Integer[256];
        for (int i = 0; i < order.length; i++) {
            order[i] = i;
        }
        Arrays.sort(order, Comparator.comparingInt((Integer symbol) -> frequency[symbol]).reversed()
                .thenComparingInt(symbol -> symbol));
        int used = 0;
        for (int frequencyValue : frequency) {
            if (frequencyValue > 0) {
                used++;
            }
        }
        int[] symbols = new int[used];
        for (int i = 0; i < used; i++) {
            symbols[i] = order[i];
        }
        return symbols;
    }

    private static int[] huffmanLengths(int[] frequency) {
        List<Node> leaves = new ArrayList<>();
        for (int symbol = 0; symbol < frequency.length; symbol++) {
            if (frequency[symbol] > 0) {
                leaves.add(new Node(frequency[symbol], symbol));
            }
        }
        if (leaves.size() < 2) {
            return null;
        }
        List<Node> heap = new ArrayList<>(leaves);
        while (heap.size() > 1) {
            heap.sort(Comparator.comparingInt((Node node) -> node.weight).thenComparingInt(node -> node.order));
            Node left = heap.remove(0);
            Node right = heap.remove(0);
            heap.add(new Node(left.weight + right.weight, left, right));
        }
        int[] lengths = new int[256];
        assignDepth(heap.get(0), 0, lengths);
        for (int length : lengths) {
            if (length > 15) {
                return null;
            }
        }
        int space = 32768;
        for (int length : lengths) {
            if (length > 0) {
                space -= 32768 >> length;
            }
        }
        return space == 0 ? lengths : null;
    }

    private static void assignDepth(Node node, int depth, int[] lengths) {
        if (node.symbol >= 0) {
            lengths[node.symbol] = depth;
            return;
        }
        assignDepth(node.left, depth + 1, lengths);
        assignDepth(node.right, depth + 1, lengths);
    }

    private static void writeLiteralTree(BitWriter writer, byte[] chunk, int[] lengths) {
        int distinct = 0;
        for (int length : lengths) {
            if (length > 0) {
                distinct++;
            }
        }
        if (distinct <= 4) {
            int[] frequency = new int[256];
            for (byte value : chunk) {
                frequency[value & 0xFF]++;
            }
            int[] symbols = symbolsByFrequency(frequency);
            writer.writeBits(1, 2);
            writer.writeBits(distinct - 1, 2);
            for (int symbol : symbols) {
                writer.writeBits(symbol, 8);
            }
            if (distinct == 4) {
                writer.writeBits(1, 1);
            }
            return;
        }
        writeComplexHuffman(writer, lengths);
    }

    private static void writeLiterals(BitWriter writer, byte[] chunk, int[] lengths, int[] keys) {
        int distinct = 0;
        for (int length : lengths) {
            if (length > 0) {
                distinct++;
            }
        }
        if (distinct == 1) {
            return;
        }
        for (byte value : chunk) {
            int symbol = value & 0xFF;
            writer.writeBits(keys[symbol], lengths[symbol]);
        }
    }

    private static void writeComplexHuffman(BitWriter writer, int[] codeLengths) {
        int emitted = emittedLengthCount(codeLengths);
        int[] sequence = Arrays.copyOf(codeLengths, emitted);
        boolean uniform = true;
        for (int i = 1; i < sequence.length; i++) {
            if (sequence[i] != sequence[0]) {
                uniform = false;
                break;
            }
        }
        int[] codeLengthCodeLengths = new int[18];
        if (uniform) {
            codeLengthCodeLengths[sequence[0]] = 1;
        } else {
            int[] frequency = new int[16];
            for (int length : sequence) {
                frequency[length]++;
            }
            assignCodeLengthCodes(codeLengthCodeLengths, frequency);
        }

        writer.writeBits(0, 2);
        int space = 32;
        for (int index = 0; index < CODE_LENGTH_CODE_ORDER.length; index++) {
            int symbol = CODE_LENGTH_CODE_ORDER[index];
            int length = codeLengthCodeLengths[symbol];
            writeFixedCodeLength(writer, length);
            if (length != 0) {
                space -= 32 >> length;
                if (space <= 0) {
                    break;
                }
            }
        }
        if (uniform) {
            return;
        }
        int[] keys = canonicalKeys(Arrays.copyOf(codeLengthCodeLengths, 16));
        for (int length : sequence) {
            writer.writeBits(keys[length], codeLengthCodeLengths[length]);
        }
    }

    private static int emittedLengthCount(int[] codeLengths) {
        int space = 32768;
        int count = 0;
        for (int length : codeLengths) {
            if (space <= 0) {
                break;
            }
            count++;
            if (length != 0) {
                space -= 32768 >> length;
            }
        }
        if (space != 0) {
            throw new IllegalStateException("Incomplete Huffman code");
        }
        return count;
    }

    private static void assignCodeLengthCodes(int[] codeLengthCodeLengths, int[] frequency) {
        List<Integer> symbols = new ArrayList<>();
        for (int symbol = 0; symbol < frequency.length; symbol++) {
            if (frequency[symbol] > 0) {
                symbols.add(symbol);
            }
        }
        symbols.sort(Comparator.comparingInt((Integer symbol) -> frequency[symbol]).reversed()
                .thenComparingInt(symbol -> symbol));
        List<Integer> lengths = balancedLengths(symbols.size());
        lengths.sort(Comparator.naturalOrder());
        for (int i = 0; i < symbols.size(); i++) {
            int length = lengths.get(i);
            if (length > 5) {
                throw new IllegalStateException("Code-length alphabet is too wide");
            }
            codeLengthCodeLengths[symbols.get(i)] = length;
        }
    }

    private static List<Integer> balancedLengths(int count) {
        if (count == 1) {
            return new ArrayList<>(List.of(1));
        }
        List<Integer> lengths = new ArrayList<>();
        lengths.add(1);
        lengths.add(1);
        while (lengths.size() < count) {
            int splitAt = 0;
            for (int i = 1; i < lengths.size(); i++) {
                if (lengths.get(i) < lengths.get(splitAt)) {
                    splitAt = i;
                }
            }
            int length = lengths.remove(splitAt);
            lengths.add(length + 1);
            lengths.add(length + 1);
        }
        return lengths;
    }

    private static int[] canonicalKeys(int[] codeLengths) {
        int[] keys = new int[codeLengths.length];
        int[] count = new int[16];
        for (int length : codeLengths) {
            if (length > 0) {
                count[length]++;
            }
        }
        int key = 0;
        for (int length = 1; length <= 15; length++) {
            for (int symbol = 0; symbol < codeLengths.length && count[length] > 0; symbol++) {
                if (codeLengths[symbol] != length) {
                    continue;
                }
                keys[symbol] = key;
                key = nextKey(key, length);
                count[length]--;
            }
        }
        return keys;
    }

    private static int nextKey(int key, int length) {
        int step = 1 << (length - 1);
        while ((key & step) != 0) {
            step >>= 1;
        }
        return (key & (step - 1)) + step;
    }

    private static void writeFixedCodeLength(BitWriter writer, int value) {
        if (value == 0) {
            writer.writeBits(0b00, 2);
        } else if (value == 1) {
            writer.writeBits(0b0111, 4);
        } else if (value == 2) {
            writer.writeBits(0b011, 3);
        } else if (value == 3) {
            writer.writeBits(0b10, 2);
        } else if (value == 4) {
            writer.writeBits(0b01, 2);
        } else if (value == 5) {
            writer.writeBits(0b1111, 4);
        } else {
            throw new IllegalArgumentException("code length " + value);
        }
    }

    private static int log2Floor(int value) {
        int result = -1;
        int step = 16;
        int current = value;
        while (step > 0) {
            int next = current >> step;
            if (next != 0) {
                result += step;
                current = next;
            }
            step >>= 1;
        }
        return result + current;
    }

    private static final class Node {
        private static int nextOrder;
        final int weight;
        final int order;
        final int symbol;
        final Node left;
        final Node right;

        Node(int weight, int symbol) {
            this.weight = weight;
            this.order = nextOrder++;
            this.symbol = symbol;
            this.left = null;
            this.right = null;
        }

        Node(int weight, Node left, Node right) {
            this.weight = weight;
            this.order = nextOrder++;
            this.symbol = -1;
            this.left = left;
            this.right = right;
        }
    }

    private static final class BitWriter {
        private byte[] data = new byte[64];
        private int bitLength;

        void writeBits(int value, int count) {
            for (int i = 0; i < count; i++) {
                int position = bitLength >> 3;
                int shift = bitLength & 7;
                ensure(position + 1);
                if (shift == 0) {
                    data[position] = 0;
                }
                if (((value >>> i) & 1) != 0) {
                    data[position] |= (byte) (1 << shift);
                }
                bitLength++;
            }
        }

        void align() {
            int remainder = bitLength & 7;
            if (remainder != 0) {
                writeBits(0, 8 - remainder);
            }
        }

        void writeBytes(byte[] bytes) {
            if ((bitLength & 7) != 0) {
                throw new IllegalStateException("Unaligned copy");
            }
            int position = bitLength >> 3;
            ensure(position + bytes.length);
            System.arraycopy(bytes, 0, data, position, bytes.length);
            bitLength += bytes.length * 8;
        }

        void writeTo(OutputStream output) throws IOException {
            output.write(data, 0, (bitLength + 7) >> 3);
        }

        private void ensure(int size) {
            if (size <= data.length) {
                return;
            }
            int grown = data.length;
            while (grown < size) {
                grown *= 2;
            }
            data = Arrays.copyOf(data, grown);
        }
    }
}
