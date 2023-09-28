using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePain.Button
{
    internal class Button
    {
        public event EventHandler Click;

        public string Text { get; set; }
        public string ButtonId { get; set; }

        public char SymbolForBorder { get; set; }

        public bool ButtonIsPressed { get; set; }
        public bool IsEnabled { get; set; }

        public int NumberOfSymbols { get; set; }
        public int HeightPx { get; set; }
        public int WidthPx { get; set; }
        public int HeightInSymbols { get; set; }
        public int WidthInSymbols { get; set; }
        public int StartAreaY { get; set; }
        public int EndAreaY { get; set; }
        public int StartAreaX { get; set; }
        public int EndAreaX { get; set; }
        public int FontWidthPx { get; set; }
        public int FontHeightPx { get; set; }
        public int PosXInPx { get; set; }
        public int PosYInPx { get; set; }
        public int PosXInSymbols { get; set; }
        public int PosYInSymbols { get; set; }
    }
}
