using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Excel = Microsoft.Office.Interop.Excel;

namespace anvlib_reports.Classes
{
    public class ExcelCommon
    {        
    }

    public class Cell
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Cell() { }

        public Cell(int r, int c)
        {
            Row = r;
            Column = c;
        }

        public Cell(Point p)
        {
            Row = p.Y;
            Column = p.X;
        }
    }

    public class CellInfo: Cell
    {
        public double ColumnWidth { get; set; }
        public double RowHeight { get; set; }
        public Excel.Interior Interior { get; set; }
        public Excel.Font Font { get; set; }
        public dynamic HorizontalAlignment { get; set; }
        public dynamic VerticalAlignment { get; set; }
    }

    public enum HAlignment { Left, Center, Right }
}
