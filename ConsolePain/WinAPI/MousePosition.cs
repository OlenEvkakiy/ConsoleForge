using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsolePain.WinAPI
{
    internal class MousePosition
    {
        public struct POINT
        {
            public int X;
            public int Y;
        }
        POINT point;
        static int _x;
        static int _y;

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("User32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        public void ShowMousePosition()
        {
            if (GetCursorPos(out point) && point.X != _x && point.Y != _y)
            {
                // Console.Clear();
                Console.Write("Координаты курсора по X и Y относительно экрана (WinAPI) ");
                Console.WriteLine("({0}, {1})", point.X, point.Y);
                _x = point.X;
                _y = point.Y;
            }
        }

        public void GetPositionInWindow(out int X, out int Y)
        {
            GetCursorPos(out point);
            IntPtr hWnd = FindWindow(null, Console.Title);
            ScreenToClient(hWnd, ref point);
            X = point.X;
            Y = point.Y;
        }


        //public void GetPositionInScreen(out int X, out int Y)
        //{
        //    X = 0;
        //    Y = 0;
        //    if (GetCursorPos(out point) && point.X != _x && point.Y != _y)
        //    {
        //        X = point.X;
        //        Y = point.Y;
        //        _x = point.X;
        //        _y = point.Y;
        //    }
        //}

        public void GetPositionInScreen(out int X, out int Y)
        {
            X = 0;
            Y = 0;
            if (GetCursorPos(out point))
            {
                X = point.X;
                Y = point.Y;
                //_x = point.X;
                //_y = point.Y;
            }
        }

        public void MoveCursor(int x, int y)
        {           
            SetCursorPos(x, y);
        }

    }
}
