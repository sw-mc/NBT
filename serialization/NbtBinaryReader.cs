﻿using System.Diagnostics;
using System.Text;
using SkyWing.NBT.Tag;
using SkyWing.NBT.Utils;

namespace SkyWing.NBT.Serialization;

internal sealed class NbtBinaryReader : BinaryReader, NbtStreamReader {
    
    private readonly byte[] _buffer = new byte[sizeof(double)];
    private readonly byte[] _seekBuffer = Array.Empty<byte>();
    private const int TAG_SeekBufferSize = 8 * 1024;
    
    private readonly bool _swapNeeded;
    
    private readonly byte[] _stringConversionBuffer = new byte[64];
    
    public NbtBinaryReader(Stream input, bool bigEndian)
        : base(input) {
        _swapNeeded = (BitConverter.IsLittleEndian == bigEndian);
    }

    public TagType ReadTagType() {
        int type = ReadByte();
        if (type > (int) TagType.LongArray) {
            throw new NbtDataException("NBT tag type out of range: " + type);
        }

        return (TagType) type;
    }

    public override short ReadInt16() {
        return _swapNeeded ? Swap(base.ReadInt16()) : base.ReadInt16();
    }

    public override int ReadInt32() {
        return _swapNeeded ? Swap(base.ReadInt32()) : base.ReadInt32();
    }

    public override long ReadInt64() {
        return _swapNeeded ? Swap(base.ReadInt64()) : base.ReadInt64();
    }

    public override float ReadSingle() {
        if (!_swapNeeded) return base.ReadSingle();
        FillBuffer(sizeof(float));
        Array.Reverse(_buffer, 0, sizeof(float));
        return BitConverter.ToSingle(_buffer, 0);
    }

    public sbyte ReadSignedByte() {
        return ReadSByte();
    }

    public override double ReadDouble() {
        if (!_swapNeeded) return base.ReadDouble();
        FillBuffer(sizeof(double));
        Array.Reverse(_buffer);
        return BitConverter.ToDouble(_buffer, 0);
    }

    public byte[] ReadByteArray() {
        return ReadBytes(ReadInt32());
    }

    public override string ReadString() {
        var length = ReadInt16();
        if (length < 0) {
            throw new NbtDataException("Negative string length given!");
        }

        if (length < _stringConversionBuffer.Length) {
            var stringBytesRead = 0;
            while (stringBytesRead < length) {
                var bytesToRead = length - stringBytesRead;
                var bytesReadThisTime = BaseStream.Read(_stringConversionBuffer, stringBytesRead, bytesToRead);
                if (bytesReadThisTime == 0) {
                    throw new EndOfStreamException();
                }

                stringBytesRead += bytesReadThisTime;
            }

            return Encoding.UTF8.GetString(_stringConversionBuffer, 0, length);
        }

        var stringData = ReadBytes(length);
        if (stringData.Length < length) {
            throw new EndOfStreamException();
        }

        return Encoding.UTF8.GetString(stringData);
    }

    public int[] ReadIntArray() {
        var length = ReadInt32();
        var ints = new int[length];
        for (var i = 0; i < length; i++) {
            ints[i] = ReadInt32();
        }

        return ints;
    }

    public long[] ReadLongArray() {
        var length = ReadInt32();
        var longs = new long[length];
        for (var i = 0; i < length; i++) {
            longs[i] = ReadInt64();
        }

        return longs;
    }

    public void Skip(int bytesToSkip) {
        if (bytesToSkip < 0) {
            throw new ArgumentOutOfRangeException(nameof(bytesToSkip));
        }

        if (BaseStream.CanSeek) {
            BaseStream.Position += bytesToSkip;
        }
        else if (bytesToSkip != 0) {
            var bytesSkipped = 0;
            while (bytesSkipped < bytesToSkip) {
                var bytesToRead = Math.Min(TAG_SeekBufferSize, bytesToSkip - bytesSkipped);
                var bytesReadThisTime = BaseStream.Read(_seekBuffer, 0, bytesToRead);
                if (bytesReadThisTime == 0) {
                    throw new EndOfStreamException();
                }

                bytesSkipped += bytesReadThisTime;
            }
        }
    }

    private new void FillBuffer(int numBytes) {
        var offset = 0;
        do {
            var num = BaseStream.Read(_buffer, offset, numBytes - offset);
            if (num == 0) throw new EndOfStreamException();
            offset += num;
        } while (offset < numBytes);
    }

    public void SkipString() {
        var length = ReadInt16();
        if (length < 0) {
            throw new NbtDataException("Negative string length given!");
        }

        Skip(length);
    }

    [DebuggerStepThrough]
    private static short Swap(short v) {
        unchecked {
            return (short) ((v >> 8) & 0x00FF |
                            (v << 8) & 0xFF00);
        }
    }

    [DebuggerStepThrough]
    private static int Swap(int v) {
        unchecked {
            var v2 = (uint) v;
            return (int) ((v2 >> 24) & 0x000000FF |
                          (v2 >> 8) & 0x0000FF00 |
                          (v2 << 8) & 0x00FF0000 |
                          (v2 << 24) & 0xFF000000);
        }
    }

    [DebuggerStepThrough]
    private static long Swap(long v) {
        unchecked {
            return (Swap((int) v) & uint.MaxValue) << 32 |
                   Swap((int) (v >> 32)) & uint.MaxValue;
        }
    }
}