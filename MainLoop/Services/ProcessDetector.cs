using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GenHelper.MainLoop.Services
{
    public sealed class ProcessDetector : IDisposable
    {
        private const string ProcessName = "generals";
        private static readonly IReadOnlyList<(string Path, string Key)> RegistryPaths = new List<(string Path, string Key)> {
            (@"SOFTWARE\Electronic Arts\EA Games\Command and Conquer Generals Zero Hour", "InstallPath"),
            (@"SOFTWARE\Electronic Arts\EA Games\Command and Conquer The First Decade", "InstallFolder")
        };

        private static readonly RegistryKey _view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

        private static Func<Process, bool> ProcessCorrectnessPredicate { get; } = T =>
        {
            foreach (var regPair in RegistryPaths)
            {
                try
                {
                    using var key = _view32.OpenSubKey(regPair.Path, false);

                    string installPath = (string)key.GetValue(regPair.Key);
                    string normalizedInstallPath = PathHelpers.NormalizePath(installPath);

                    string processPath = Path.GetDirectoryName(T.MainModule.FileName);
                    string normalizedProcessPath = PathHelpers.NormalizePath(processPath);

                    if (normalizedProcessPath == normalizedInstallPath)
                    {
                        return true;
                    }
                }
                catch (Exception ex) { }
            }

            return false;
        };

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
                    _gameProcess = Process.GetProcessesByName(ProcessName).Where(ProcessCorrectnessPredicate).OrderByDescending(T => T.PrivateMemorySize64).FirstOrDefault();
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
