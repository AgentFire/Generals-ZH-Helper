using AgentFire.Performance.SystemClock;
using GenHelper.MainLoop.Services;
using GenHelper.MainLoop.Services.WinApi;
using GenHelper.MainLoop.Services.WinApi.Members;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenHelper.MainLoop
{
    internal class RealSpinner : IDisposable
    {
        static RealSpinner()
        {
            Resolution.TrySet(Resolution.Max);
        }

        private static readonly TimeSpan _longDelay = TimeSpan.FromSeconds(15);
        private static readonly TimeSpan _middleDelay = TimeSpan.FromMilliseconds(500);
        private static readonly TimeSpan _shortDelay = TimeSpan.FromMilliseconds(50);

        private readonly ProcessDetector _processDetector = new ProcessDetector();
        private readonly GenService _genService = new GenService();
        private readonly MouseTranslator _translator = new MouseTranslator(10);

        public async Task Spin(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Process? gameProc = _processDetector.TryGetRunningProcess();

                if (gameProc == null)
                {
                    _translator.Release();
                    await Task.Delay(_longDelay, token);
                    continue;
                }

                Rect? rect = _genService.TryGetActiveMainWindowRect(gameProc);

                if (rect == null)
                {
                    _translator.Release();
                    await Task.Delay(_middleDelay, token);
                    continue;
                }

                _translator.Tick(MouseService.GetMousePosition(), rect.Value);

                await Task.Delay(_shortDelay, token);
            }
        }

        public void Dispose()
        {
            _processDetector.Dispose();
            _translator.Release();
        }
    }
}
