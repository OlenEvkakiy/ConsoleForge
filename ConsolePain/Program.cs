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
        ConsoleButton consoleButton = new ConsoleButton(0, 0, "", 0, 0, 0, 0, false, ' ', 0);

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

            if (_MouseClickTracking.LeftMouseButtonPressed() && xInWindow > 0 && yInWindow > 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                consoleButton.PressingButton();
                //Console.WriteLine($"Позиция в окне по X: {xInWindow}, по Y: {yInWindow}");
            }

            if (_MouseClickTracking.RightMouseButtonPressed())
            {
                consoleButton.PressingButton();
                //if (consoleButton.ButtonPressed)
                //{
                //    Console.WriteLine($"Клик {consoleButton.Text}");
                //}
                _MousePosition.MoveCursor(0, 0);
                //_MouseClickTracking.ProgramPressLeftMouseButton();
                Console.Clear();
            }

            if (_MouseClickTracking.WheelButtonPressed())
            {
                Console.Clear();
                consoleButton.ConsoleButtonList.Clear();
                consoleButton.CreateButton("Start", '/', ConsoleColor.Green);
                consoleButton.CreateButton("Help", '-', ConsoleColor.White);
                consoleButton.CreateButton("Exit", '*', ConsoleColor.Red);


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

    public int NumberOfSymbols { get; set; }
    public int HeightPx { get; set; }
    public int WidthPx { get; set; }
    public int PositionInLayer { get; set; }
    public int StartAreaY { get; set; }
    public int EndAreaY { get; set; }
    public int StartAreaX { get; set; }
    public int EndAreaX { get; set; }
}

public class ConsoleButton
{
    public ConsoleButton(int posX, int posY, string text, int width, int height, int fontWidth, int fontHeight, bool buttonPressed, char symbolForBorders, int numberOfSymbols)
    {
        PosX = posX;
        PosY = posY;
        Text = text;
        WidthPx = width;
        HeightPx = height;
        FontWidth = fontWidth;
        FontHeight = fontHeight;
        ButtonPressed = buttonPressed;
        SymbolForBorders = symbolForBorders;
        NumberOfSymbols = numberOfSymbols;
    }


    ConsoleSettings _ConsoleSettings = new ConsoleSettings();
    MousePosition _MousePosition = new MousePosition();

    public List<ConsoleButton> ConsoleButtonList = new List<ConsoleButton>();
    public List<ButtonParam> ButtonsParamsList = new List<ButtonParam>();


    public ButtonParam ButtonParam { get; set; }
    
    public string Text { get; set; }
    public char SymbolForBorders { get; private set; }

    public int WidthPx { get; private set; }
    public int HeightPx { get; private set; }
    public int FontWidth { get; private set; }
    public int FontHeight { get; private set; }
    public int PosX { get; private set; }
    public int PosY { get; private set; }
    public int NumberOfSymbols { get; private set; } //количество букв в кнопке(слове)

    public bool ButtonPressed;

    private char[] _symbols;
    private int _fontHeight;
    private int _fontWidth;
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

    public void CreateButton(string text, char symbolForBorders, ConsoleColor color)
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
    }

    private void ButtonSize()
    {
        _ConsoleSettings.GetConsoleFontSize(out _fontHeight, out _fontWidth);
        _symbols = Text.ToCharArray();
        _numberOfLines = 3;
        NumberOfSymbols = _symbols.Length;
        WidthPx = (NumberOfSymbols + 2) * _fontWidth;
        HeightPx = _numberOfLines * _fontHeight;
        FontWidth = _fontWidth;
        FontHeight = _fontHeight;
        ConsoleButtonList.Add(new ConsoleButton(PosX, PosY, Text, WidthPx, HeightPx, FontWidth, FontHeight, ButtonPressed, SymbolForBorders, NumberOfSymbols));
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
                    WidthPx = button.WidthPx,
                    HeightPx = button.HeightPx,
                    Text = button.Text,
                    PositionInLayer = i,
                    StartAreaY = 0,
                    EndAreaY = button.HeightPx,
                    StartAreaX = 0,
                    EndAreaX = button.WidthPx,
                    NumberOfSymbols = button.NumberOfSymbols                    
                };


                ButtonsParamsList.Add(buttonParam);

                if (buttonParam.EndAreaY == 0)
                {
                    buttonParam.EndAreaY = buttonParam.HeightPx;
                }

                for (int j = l; j < ButtonsParamsList.Count; j++)
                {
                    var btn = ButtonsParamsList[j];

                    if (buttonParam.PositionInLayer != 0)
                    {
                        ButtonsParamsList[j].StartAreaY = (ButtonsParamsList[j - 1].HeightPx + 1) * j;
                        ButtonsParamsList[j].EndAreaY = ButtonsParamsList[j].StartAreaY + ButtonsParamsList[j].HeightPx;
                    }

                    if (yInWindow >= ButtonsParamsList[j].StartAreaY && yInWindow <= ButtonsParamsList[j].EndAreaY &&
                        xInWindow >= ButtonsParamsList[j].StartAreaX && xInWindow <= ButtonsParamsList[j].EndAreaX)
                    {
                        Console.WriteLine($"Клик на {ButtonsParamsList[j].Text}");

                       // ChangingButtonAppearance(ButtonsParamsList[j].NumberOfSymbols);
                        ButtonPressed = true;
                    }
                }
                l++;
            }
        }
        ButtonsParamsList.Clear();
        ButtonPressed = false;
    }   
     
    public void ChangingButtonAppearance(int numberOfSymbols)
    {
        int row = 0; //строка;
        int column = 0; //столбец;     
        int num = ButtonsParamsList.Count * 3;

        Console.WriteLine(numberOfSymbols);
        for (int i = 0; i <= numberOfSymbols+1; i++)
        {
            Console.SetCursorPosition(column, row);
            Console.Write("*");
            column++;
            Console.SetCursorPosition(0, num);
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

