using GenHelper.MainLoop.Services.WinApi;
using GenHelper.MainLoop.Services.WinApi.Members;
using System;
using System.Diagnostics;

namespace GenHelper.MainLoop.Services
{
    internal sealed class GenService
    {
        public Rect? TryGetActiveMainWindowRect(Process gameProcess)
        {
            IntPtr gameWindow;

            try
            {
                gameWindow = gameProcess.MainWindowHandle;
            }
            catch (InvalidOperationException) // Process has exited.
            {
                return null;
            }

            if (gameWindow != Methods.GetForegroundWindow())
            {
                return null;
            }

            if (!Methods.GetClientRect(gameWindow, out Rect result))
            {
                return null;
            }

            return result;
        }
    }
}
