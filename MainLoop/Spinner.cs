using System;
using System.Threading;
using System.Threading.Tasks;

namespace GenHelper.MainLoop
{
    public sealed class Spinner : IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly ManualResetEventSlim _mre = new ManualResetEventSlim(false);
        private readonly RealSpinner _spinner = new RealSpinner();
        private readonly Task _mainTask;

        public Spinner()
        {
            _mainTask = Task.Run(async () =>
            {
                try
                {
                    await _spinner.Spin(_cts.Token);
                }
                catch (OperationCanceledException) { }
                catch (Exception)
                {
                    // Uncatched! TODO.
                }
                finally
                {
                    _mre.Set();
                }
            }, _cts.Token);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _mre.Wait();

            _spinner.Dispose();
            _mre.Dispose();
            _cts.Dispose();
        }
    }
}
