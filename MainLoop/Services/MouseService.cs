using GenHelper.MainLoop.Services.WinApi;
using GenHelper.MainLoop.Services.WinApi.Members;

namespace GenHelper.MainLoop.Services
{
    internal static class MouseService
    {
        public static Win32Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();

            Methods.GetCursorPos(ref w32Mouse);

            return w32Mouse;
        }
    }
}
