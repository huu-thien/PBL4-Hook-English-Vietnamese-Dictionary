using System.Runtime.InteropServices;

namespace Gma.System.MouseKeyHook.WinApi
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct MouseStruct
    {
        [FieldOffset(0x00)] public Point Point;

        [FieldOffset(0x0A)] public short MouseData;

        [FieldOffset(0x10)] public int Timestamp;
    }
}