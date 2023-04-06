using ConsolePain.WinAPI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Windows.Forms;
using static WindowPosition;

public class Program
{
    static void Main(string[] args)
    {
        Console.Title = "ConsolePain";
        Console.ForegroundColor = ConsoleColor.Yellow;
        ConsoleSettings _ConsoleSettings = new ConsoleSettings();

        _ConsoleSettings.UnsetQuickEdit();


        WindowSize _WindowSize = new WindowSize();
        MousePosition _MousePosition = new MousePosition();
        MouseClickTracking _MouseClickTracking = new MouseClickTracking();
        ConsoleButton consoleButton = new ConsoleButton(0, 0, "", 0, 0, 0, 0);


        _WindowSize.DisableWindowResizing(true);

        int xInScreen;
        int yInScreen;

        int xInWindow;
        int yInWindow;


        while (true)
        {
            _WindowSize.Get();
            _MousePosition.GetPositionInScreen(out xInScreen, out yInScreen);
            _MousePosition.GetPositionInWindow(out xInWindow, out yInWindow);
            int windowWidth = _WindowSize.Width;
            int windowHeight = _WindowSize.Height;
            Thread.Sleep(100);

            if (_MouseClickTracking.LeftMouseButtonPressed())
            {
                Console.WriteLine($"Позиция в окне по X: {xInWindow}, по Y: {yInWindow}");
                if (xInWindow >= 0 && xInWindow <= consoleButton.Width && yInWindow >= 0 && yInWindow <= consoleButton.Height * consoleButton.ConsoleButtonList.Count)
                {
                    Console.WriteLine("Клик");
                }
            }

            if (_MouseClickTracking.RightMouseButtonPressed())
            {
                consoleButton.PressingButton();
                //_MousePosition.MoveCursor(0, 0);
                //_MouseClickTracking.ProgramPressLeftMouseButton();
                //Console.Clear();
            }

            if (_MouseClickTracking.WheelButtonPressed())
            {
                Console.Clear();
                consoleButton.ConsoleButtonList.Clear();
                consoleButton.PrintButton("Start", '-');
                consoleButton.PrintButton("Help", '-');
                consoleButton.PrintButton("Exit", '-');

                Console.WriteLine($"Ширина в символах {Console.BufferWidth} Высота в символах {Console.BufferHeight}");
                Console.WriteLine($"Ширина окна {windowWidth} Высота окна {windowHeight}");
                foreach (var buttons in consoleButton.ConsoleButtonList)
                {
                    Console.WriteLine($"Ширина кнопки {buttons.Text} {buttons.Width}px");
                    Console.WriteLine($"Высота кнопки {buttons.Text} {buttons.Height}px");
                }
            }

            if (_WindowSize.Width > 520 || _WindowSize.Height > 960)
            {
                _WindowSize.Set();
                //Console.SetBufferSize(115, 26);
            }
        }
    }
}


public class CursorPos
{
    int row;
    int column;

    public void CheckIt(int x, int y)
    {
        Console.SetCursorPosition(column + x, row + y);
    }

    public void Shit()
    {
        row = Console.CursorTop;
        column = Console.CursorLeft;
        while (true)
        {

        }
    }
}

public class ConsoleButton
{
    public ConsoleButton(int posX, int posY, string text, int width, int height, int fontWidth, int fontHeight)
    {
        PosX = posX;
        PosY = posY;
        Text = text;
        Width = width;
        Height = height;
        FontWidth = fontWidth;
        FontHeight = fontHeight;
    }

    ConsoleSettings _ConsoleSettings = new ConsoleSettings();
    public List<ConsoleButton> ConsoleButtonList = new List<ConsoleButton>();
    MousePosition _MousePosition = new MousePosition();



    public int PosX { get; private set; }
    public int PosY { get; private set; }
    public string Text { get; set; }
    public char SymbolForBorders { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int FontWidth { get; private set; }
    public int FontHeight { get; private set; }


    private int _fontHeight;
    private int _fontWidth;
    private int _numberOfSymbols;
    private int _numberOfLines = 3;



    private void Cricuit(string text, char symbolForBorders)
    {
        SymbolForBorders = symbolForBorders;
        char[] chars = text.ToCharArray();
        for (int i = 0; i < chars.Length + 2; i++)
        {
            Console.Write(SymbolForBorders);
        }
    }

    public void PrintButton(string text, char symbolForBorders)
    {
        SymbolForBorders = symbolForBorders;
        Cricuit(text, SymbolForBorders);
        Console.WriteLine();
        Console.WriteLine($"{SymbolForBorders}{text}{SymbolForBorders}");
        Cricuit(text, SymbolForBorders);
        Console.WriteLine();
        Text = text;
        ButtonSize();
        ButtonPosition();
        ConsoleButtonList.Add(new ConsoleButton(PosX, PosY, Text, Width, Height, FontWidth, FontHeight));
    }

    private void ButtonSize()
    {
        _ConsoleSettings.GetConsoleFontSize(out _fontHeight, out _fontWidth);
        char[] chars = Text.ToCharArray();
        _numberOfSymbols = chars.Length;
        _numberOfLines = 3;
        Width = (_numberOfSymbols + 2) * _fontWidth;
        Height = _numberOfLines * _fontHeight;
        FontWidth = _fontWidth;
        FontHeight = _fontHeight;
    }


    //struct ButtonParam
    //{
    //    public string Text { get; set; }
    //    public int Height { get; set; }
    //    public int Width { get; set; }
    //    public int Position { get; set; }
    //    public int StartAreaY { get; set; }
    //    public int EndAreaY { get; set; }
    //}

    public void PressingButton()
    {
        int xInWindow = 0;
        int yInWindow = 0;
        int l = 0;

        Dictionary<string, object> ButtonParams = new Dictionary<string, object>()
        {
            {"Text", "" },
            {"Height", 0 },
            {"Width", 0 },
            {"Position", 0},
            {"StartAreaY", 0},
            {"EndAreaY", 0 }
        };

        _MousePosition.GetPositionInWindow(out xInWindow, out yInWindow);
        //ButtonParam buttonParams = new ButtonParam();
        List<Dictionary<string, object>> buttons = new List<Dictionary<string, object>> ();
        if (ConsoleButtonList.Count > 0)
        {
            for (int i = 0; i < ConsoleButtonList.Count; i++)
            {
                var button = ConsoleButtonList[i];
                ButtonParams["Text"] = button.Text;
                ButtonParams["Height"] = button.Height;
                ButtonParams["Width"] = button.Width;
                ButtonParams["Position"] = i;
                
                buttons.Add(ButtonParams);

                if ((int)ButtonParams["EndAreaY"] == 0)
                {
                    ButtonParams["EndAreaY"] = ButtonParams["Height"];
                }

                for (int j = l; j < buttons.Count; j++)
                {
                    var btn = buttons[j];

                    if ((int)ButtonParams["Position"] != 0)
                    {
                        buttons[j]["StartAreaY"] = (int)btn["Height"]+1;
                    }
                }
                l++;

                //if (xInWindow >= 0 && xInWindow <= width && yInWindow >= 0 && yInWindow <= height * 3)
                //{
                //    Console.WriteLine($"Клик {button.Text}");
                //}
            }
        }
    }
    private void ButtonPosition()
    {
        PosX = 0;
        PosY = 0;
    }
}

public class WindowPosition
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    public void Find()
    {
        Console.Title = "Shit";
        var hWnd = FindWindow(null, Console.Title);
        var wndRect = new RECT();
        GetWindowRect(hWnd, out wndRect);
        var cWidth = wndRect.Right - wndRect.Left;
        var cHeight = wndRect.Bottom - wndRect.Top;
        var SWP_NOSIZE = 0x1;
        var HWND_TOPMOST = -1;
        var Width = 1366;
        var Height = 768;
        SetWindowPos(hWnd, HWND_TOPMOST, Width / 2 - cWidth / 2, Height / 2 - cHeight / 2, 0, 0, SWP_NOSIZE);
    }

}

public class DifferentShit
{
    void OutputTextWithBeep()
    {
        string text = "Помоги мне, господи!";
        char[] chars = text.ToCharArray();
        for (int i = 0; i <= chars.Length - 1; i++)
        {
            Thread.Sleep(50);
            Console.Beep(245, 50);
            Console.Write(chars[i]);
        }
    }
}

