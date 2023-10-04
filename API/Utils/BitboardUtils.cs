using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Utils
{
    public static class BitboardUtils
    {

        public static ulong ShiftNorth(ulong bb) => bb << 8;
        public static ulong ShiftSouth(ulong bb) => bb >> 8;
        public static ulong ShiftEast(ulong bb) => (bb & 0xFEFEFEFEFEFEFEFEUL) << 1;
        public static ulong ShiftWest(ulong bb) => (bb & 0x7F7F7F7F7F7F7F7FUL) >> 1;

    }
}