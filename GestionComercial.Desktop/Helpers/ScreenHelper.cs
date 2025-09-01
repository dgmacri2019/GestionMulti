using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace GestionComercial.Desktop.Helpers
{
    public static class ScreenHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;   // resolución total
            public RECT rcWork;      // área de trabajo (sin barra de tareas)
            public int dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
        /// Obtiene la resolución del monitor donde está la ventana WPF.
        /// </summary>
        public static (int Width, int Height) ObtenerResolucion(Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            IntPtr hMonitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            var mi = new MONITORINFO();
            mi.cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            if (GetMonitorInfo(hMonitor, ref mi))
            {
                int width = mi.rcMonitor.Right - mi.rcMonitor.Left;
                int height = mi.rcMonitor.Bottom - mi.rcMonitor.Top;
                return (width, height);
            }

            return ((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
        }
    }
}
