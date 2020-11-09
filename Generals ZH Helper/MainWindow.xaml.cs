using GenHelper.MainLoop;
using GenHelper.MainLoop.Services;
using System.Windows;

namespace GenHelper
{
    public partial class MainWindow : Window
    {
        private readonly Spinner _spinner = new Spinner();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            _spinner.Dispose();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Kill_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var pd = new ProcessDetector();
                pd.TryGetRunningProcess()?.Kill();
            }
            catch { }
        }
    }
}
