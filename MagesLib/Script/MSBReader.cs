using System;
using System.IO;
using System.Text;

using Mages.Script.Tokens;

namespace Mages.Script
{
    public class MSBReader : BinaryReader, Reader
    {
        public readonly uint StringTable = 0, ReturnAddressTable = 0, Version = 0;

        public uint StringNum { get; set; }

        public string Charset = null;
        public Encoding Encoding = Encoding.UTF8;

        public MSBReader(byte[] data, string charset) : this(new MemoryStream(data), charset) { }

        public MSBReader(Stream stream, string charset, string header = "MES", Encoding encoding = null, bool leaveOpen = false) : base(stream, encoding ?? Encoding.UTF8, leaveOpen)
        {
            if (encoding != null)
            {
                Encoding = encoding;
            }
            if (Encoding.GetString(ReadBytes(header.Length)) != header || ReadByte() != 0)
            {
                throw new InvalidDataException("MSB header mismatch");
            }
            Charset = charset;
            Version = ReadUInt32();
            StringNum = ReadUInt32();
            StringTable = 0x10;
            ReturnAddressTable = ReadUInt32();
        }

        public T ReadTable<T>(int offset, Func<T> callback)
        {
            long current = BaseStream.Position;
            BaseStream.Position = StringTable + offset * 8 + 4;
            BaseStream.Position = ReadUInt32() + ReturnAddressTable;
            var result = callback();
            BaseStream.Position = current;
            return result;
        }

        public SCXString ReadString(int offset) => ReadTable(offset, () => new SCXString(this));

        public uint ReadReturnAddress(int offset) => ReadTable(offset, () => ReadUInt32());

        public char ReadChar(byte first) => Charset[((first & ~(byte)TokenType.TextMask) << 8) | ReadByte()];
    }
}
