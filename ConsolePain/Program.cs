using ConsolePain.WinAPI;
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
        ConsoleSettings _ConsoleSettings = new ConsoleSettings();

        _ConsoleSettings.UnsetQuickEdit();


        WindowSize _WindowSize = new WindowSize();
        MousePosition _MousePosition = new MousePosition();
        MouseClickTracking _MouseClickTracking = new MouseClickTracking();
        ConsoleButton consoleButton = new ConsoleButton(0, 0, "", 0, 0, 0, 0, false, ' ');

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
                Console.ForegroundColor = ConsoleColor.White;
                consoleButton.PressingButton();
                //Console.WriteLine($"Позиция в окне по X: {xInWindow}, по Y: {yInWindow}");
            }

            if (_MouseClickTracking.RightMouseButtonPressed())
            {
                //consoleButton.PressingButton();
                //if (consoleButton.ButtonPressed)
                //{
                //    Console.WriteLine($"Клик {consoleButton.Text}");
                //}
                //_MousePosition.MoveCursor(0, 0);
                //_MouseClickTracking.ProgramPressLeftMouseButton();
                //Console.Clear();
            }

            if (_MouseClickTracking.WheelButtonPressed())
            {
                Console.Clear();
                consoleButton.ConsoleButtonList.Clear();
                consoleButton.PrintButton("Start", '-', ConsoleColor.Green);
                consoleButton.PrintButton("Help", '-', ConsoleColor.White);
                consoleButton.PrintButton("Exit", '*', ConsoleColor.Red);


                //Console.WriteLine($"Ширина в символах {Console.BufferWidth} Высота в символах {Console.BufferHeight}");
                //Console.WriteLine($"Ширина окна {windowWidth} Высота окна {windowHeight}");
                //foreach (var buttons in consoleButton.ConsoleButtonList)
                //{
                //    Console.WriteLine($"Ширина кнопки {buttons.Text} {buttons.Width}px");
                //    Console.WriteLine($"Высота кнопки {buttons.Text} {buttons.Height}px");
                //}
            }

            if (_WindowSize.Width > 520 || _WindowSize.Height > 960)
            {
                _WindowSize.Set();
                //Console.SetBufferSize(115, 26);
            }
        }
    }
}


public class ButtonParam
{
    public string Text { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int Position { get; set; }
    public int StartAreaY { get; set; }
    public int EndAreaY { get; set; }
    public int StartAreaX { get; set; }
    public int EndAreaX { get; set; }
}

public class ConsoleButton
{
    public ConsoleButton(int posX, int posY, string text, int width, int height, int fontWidth, int fontHeight, bool buttonPressed, char symbolForBorders)
    {
        PosX = posX;
        PosY = posY;
        Text = text;
        Width = width;
        Height = height;
        FontWidth = fontWidth;
        FontHeight = fontHeight;
        ButtonPressed = buttonPressed;
        SymbolForBorders = symbolForBorders;
    }


    ConsoleSettings _ConsoleSettings = new ConsoleSettings();
    MousePosition _MousePosition = new MousePosition();

    public List<ConsoleButton> ConsoleButtonList = new List<ConsoleButton>();
    public List<ButtonParam> ButtonsParamsList = new List<ButtonParam>();


    public ButtonParam ButtonParam { get; set; }
    public int PosX { get; private set; }
    public int PosY { get; private set; }
    public string Text { get; set; }
    public char SymbolForBorders { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int FontWidth { get; private set; }
    public int FontHeight { get; private set; }
    public bool ButtonPressed;

    private char[] _symbols;
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

    public void PrintButton(string text, char symbolForBorders, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        SymbolForBorders = symbolForBorders;
        Cricuit(text, SymbolForBorders);
        Console.WriteLine();
        Console.WriteLine($"{SymbolForBorders}{text}{SymbolForBorders}");
        Cricuit(text, SymbolForBorders);
        Console.WriteLine();
        Text = text;
        ButtonSize();
        ButtonPosition();
        ConsoleButtonList.Add(new ConsoleButton(PosX, PosY, Text, Width, Height, FontWidth, FontHeight, ButtonPressed, SymbolForBorders));
    }

    private void ButtonSize()
    {
        _ConsoleSettings.GetConsoleFontSize(out _fontHeight, out _fontWidth);
        _symbols = Text.ToCharArray();
        _numberOfSymbols = _symbols.Length;
        _numberOfLines = 3;
        Width = (_numberOfSymbols + 2) * _fontWidth;
        Height = _numberOfLines * _fontHeight;
        FontWidth = _fontWidth;
        FontHeight = _fontHeight;
    }

    public void PressingButton()
    {
        int xInWindow = 0;
        int yInWindow = 0;
        int l = 0;

        _MousePosition.GetPositionInWindow(out xInWindow, out yInWindow);

        if (ConsoleButtonList.Count > 0)
        {
            for (int i = 0; i < ConsoleButtonList.Count; i++)
            {
                var button = ConsoleButtonList[i];

                ButtonParam buttonParam = new ButtonParam
                {
                    Width = button.Width,
                    Height = button.Height,
                    Text = button.Text,
                    Position = i,
                    StartAreaY = 0,
                    EndAreaY = button.Height,
                    StartAreaX = 0,
                    EndAreaX = button.Width
                };


                ButtonsParamsList.Add(buttonParam);

                if (buttonParam.EndAreaY == 0)
                {
                    buttonParam.EndAreaY = buttonParam.Height;
                }

                for (int j = l; j < ButtonsParamsList.Count; j++)
                {
                    var btn = ButtonsParamsList[j];

                    if (buttonParam.Position != 0)
                    {
                        ButtonsParamsList[j].StartAreaY = (ButtonsParamsList[j - 1].Height + 1) * j;
                        ButtonsParamsList[j].EndAreaY = ButtonsParamsList[j].StartAreaY + ButtonsParamsList[j].Height;
                    }

                    if (yInWindow >= ButtonsParamsList[j].StartAreaY && yInWindow <= ButtonsParamsList[j].EndAreaY &&
                        xInWindow >= ButtonsParamsList[j].StartAreaX && xInWindow <= ButtonsParamsList[j].EndAreaX)
                    {
                        Console.WriteLine($"Клик на {ButtonsParamsList[j].Text}");
                        ChangingButtonAppearance(buttonParam.Position);
                        ButtonPressed = true;
                    }
                }
                l++;
            }
        }
        ButtonsParamsList.Clear();
        ButtonPressed = false;
    }   
     
    public void ChangingButtonAppearance(int position)
    {
        int row;
        int column;
        var pos = position;

        Console.SetCursorPosition(2, 0);
        Console.WriteLine("i");
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

