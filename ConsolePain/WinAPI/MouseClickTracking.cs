using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePain.WinAPI
{
    internal class MouseClickTracking
    {
        public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const uint MOUSEEVENTF_MOVE = 0x0001;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        public const uint MOUSEEVENTF_XDOWN = 0x0080;
        public const uint MOUSEEVENTF_XUP = 0x0100;
        public const uint MOUSEEVENTF_WHEEL = 0x0800;
        public const uint MOUSEEVENTF_HWHEEL = 0x01000;    
        
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);               

        public void ProgramPressLeftMouseButton()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(10);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public bool LeftMouseButtonPressed()
        {
            if (GetAsyncKeyState(Keys.LButton) != 0)
            {
                return true;
            }
            return false;
        }

        public bool RightMouseButtonPressed()
        {
            if (GetAsyncKeyState(Keys.RButton) != 0)
            {
                return true;
            }
            return false;
        }

        public bool WheelButtonPressed()
        {
            if (GetAsyncKeyState(Keys.MButton) != 0)
            {
                return true;
            }
            return false;
        }

        public bool CtrlButtonPressed()
        {
            if (GetAsyncKeyState(Keys.LControlKey) != 0)
            {
                return true;
            }
            return false;
        }
        
    }
}
