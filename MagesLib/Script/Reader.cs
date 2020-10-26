using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mages.Script
{
    public interface Reader
    {
        Stream BaseStream { get; }

        uint StringNum { get; }

        T ReadTable<T>(int offset, Func<T> callback);

        SCXString ReadString(int offset);

        uint ReadReturnAddress(int offset);

        char ReadChar(byte first);

        byte ReadByte();

        short ReadInt16();
        sbyte ReadSByte();

    }
}
