using System;
using System.Runtime.InteropServices;

namespace Gma.System.MouseKeyHook.WinApi
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct AppMouseStruct
    {
        [FieldOffset(0x00)] public Point Point;

        [FieldOffset(0x16)] public short MouseData_x86;

        [FieldOffset(0x22)] public short MouseData_x64;

        public MouseStruct ToMouseStruct()
        {
            var tmp = new MouseStruct();
            tmp.Point = Point;
            tmp.MouseData = IntPtr.Size == 4 ? MouseData_x86 : MouseData_x64;
            tmp.Timestamp = Environment.TickCount;
            return tmp;
        }
    }
}