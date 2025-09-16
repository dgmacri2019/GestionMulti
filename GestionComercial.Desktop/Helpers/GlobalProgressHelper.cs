using System.Windows.Controls;
using System.Windows.Threading;

namespace GestionComercial.Desktop.Helpers
{
    internal static class GlobalProgressHelper
    {
        private static ProgressBar _progressBar;
        private static TextBlock _statusText;
        private static Dispatcher _dispatcher;

        public static void Initialize(ProgressBar progressBar, TextBlock statusText, Dispatcher dispatcher)
        {
            _progressBar = progressBar;
            _statusText = statusText;
            _dispatcher = dispatcher;
        }

        // 🔹 Progreso con % conocido
        public static void Report(int current, int total, string message)
        {
            if (_progressBar == null || _statusText == null) return;

            _dispatcher.Invoke(() =>
            {
                _progressBar.IsIndeterminate = false;
                _progressBar.Value = total > 0 ? (current * 100) / total : 0;
                _statusText.Text = message;
            });
        }

        // 🔹 Progreso indeterminado
        public static void ReportIndeterminate(string message)
        {
            if (_progressBar == null || _statusText == null) return;

            _dispatcher.Invoke(() =>
            {
                _progressBar.IsIndeterminate = true;
                _statusText.Text = message;
            });
        }

        // 🔹 Terminar y resetear luego de 2 seg.
        public static async Task CompleteAsync()
        {
            if (_progressBar == null || _statusText == null) return;

            await _dispatcher.InvokeAsync(async () =>
            {
                _progressBar.IsIndeterminate = false;
                _progressBar.Value = 100;
                _statusText.Text = "Proceso terminado";

                await Task.Delay(2000);
                _progressBar.Value = 0;
                _statusText.Text = "";
            });
        }
    }
}