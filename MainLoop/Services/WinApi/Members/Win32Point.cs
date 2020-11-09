using System.Runtime.InteropServices;

namespace GenHelper.MainLoop.Services.WinApi.Members
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Win32Point
    {
        public int X;
        public int Y;
    };
}
