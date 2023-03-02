using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConsolePain.WinAPI
{
    internal class WindowSize
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int x, int y, int nx, int ny, uint uFlags);
        [DllImport("User32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        IntPtr hWnd = FindWindow(null, Console.Title);


        public int Height { get; private set; }
        public int Width { get; private set; }

        public void Set()
        {
            Width = 960;
            Height = 520;    
            SetWindowPos(hWnd, IntPtr.Zero, 0, 0, Width, Height, 2);
        }

        public void Get()
        { 
            var wndRect = new RECT();
            GetWindowRect(hWnd, out wndRect);
            Width = wndRect.Right - wndRect.Left;
            Height = wndRect.Bottom - wndRect.Top;
        }

        public void DisableWindowResizing(bool disable)
        {
            if (disable)
            {
                const int WS_SIZEBOX = 0x00040000;
                const int GWL_STYLE = -16;
                IntPtr hWnd = FindWindow(null, Console.Title);
                int style = GetWindowLong(hWnd, GWL_STYLE);
                SetWindowLong(hWnd, GWL_STYLE, style & ~WS_SIZEBOX);
            }
            
        }
    }
}
