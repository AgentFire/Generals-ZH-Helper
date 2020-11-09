using GenHelper.MainLoop.Services.WinApi.Members;
using System;
using System.Runtime.InteropServices;

namespace GenHelper.MainLoop.Services.WinApi
{
    internal static class Methods
    {
        private const string User32 = "user32.dll";

        [DllImport(User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport(User32)]
        public static extern bool GetClientRect(IntPtr hwnd, out Rect lpRect);

        [DllImport(User32)]
        public static extern IntPtr GetForegroundWindow();
    }
}
