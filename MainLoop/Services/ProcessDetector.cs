using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GenHelper.MainLoop.Services
{
    public sealed class ProcessDetector : IDisposable
    {
        private const string ProcessName = "generals";

        private static Func<Process, bool> BuildProcessCorrectnessPredicate()
        {
            try
            {
                using var view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                using var key = view32.OpenSubKey(@"SOFTWARE\Electronic Arts\EA Games\Command and Conquer Generals Zero Hour", false);

                string installPath = (string)key.GetValue("InstallPath");
                string normalizedInstallPath = PathHelpers.NormalizePath(installPath);

                return T =>
                {
                    string processPath = Path.GetDirectoryName(T.MainModule.FileName);
                    string normalizedProcessPath = PathHelpers.NormalizePath(processPath);

                    return normalizedProcessPath == normalizedInstallPath;
                };
            }
            catch
            {
                return _ => false;
            }
        }

        private Process? _gameProcess = null;

        public Process? TryGetRunningProcess()
        {
            if (_gameProcess != null && _gameProcess.HasExited)
            {
                _gameProcess = null;
            }

            if (_gameProcess == null)
            {
                try
                {
                    _gameProcess = Process.GetProcessesByName(ProcessName).Where(BuildProcessCorrectnessPredicate()).OrderByDescending(T => T.PrivateMemorySize64).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }

            return _gameProcess;
        }

        public void Dispose()
        {
            _gameProcess?.Dispose();
        }
    }
}
