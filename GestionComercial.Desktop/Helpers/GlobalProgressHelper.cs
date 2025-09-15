using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Helpers
{
    internal static class GlobalProgressHelper
    {
        private static ProgressBar _progressBar;
        private static TextBlock _progressText;

        public static void Initialize(ProgressBar pb, TextBlock lbl)
        {
            _progressBar = pb;
            _progressText = lbl;
        }

        public static void Report(int value, int max, string message = "")
        {
            if (_progressBar == null || _progressText == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                _progressBar.Maximum = max;
                _progressBar.Value = value;
                _progressText.Text = message;
            });
        }

        public static async Task ShowCompletedAsync(string message = "Proceso terminado", int delayMs = 2000)
        {
            if (_progressText == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                _progressText.Text = message;
            });

            await Task.Delay(delayMs);

            Application.Current.Dispatcher.Invoke(() =>
            {
                _progressText.Text = "";
                _progressBar.Value = 0;
            });
        }
    }
}
}
