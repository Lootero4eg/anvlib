using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using Excel = Microsoft.Office.Interop.Excel;

namespace anvlib_reports.Classes
{
    public static class ExcelWriter
    {
        private static Excel.Application excelApp = new Excel.Application();
        private static Excel._Worksheet workSheet;

        /// <summary>
        /// Текущая ячейка на открытом или созданном листе
        /// </summary>
        public static Cell ActiveCell
        {
            get
            {
                if (workSheet != null)
                    return new Cell(excelApp.ActiveCell.Row, excelApp.ActiveCell.Column);
                else return new Cell(-1, -1);
            }
        }

        /// <summary>
        /// Метод открытия уже существующего файла
        /// </summary>
        /// <param name="template_filename">Имя и путь файла</param>
        /// <param name="background">Создать в бэкграунде</param>
        public static void OpenFile(string filename, bool background)
        {
            if (!background)
                excelApp.Visible = true;
            else
                excelApp.Visible = false;
            excelApp.Workbooks.Open(filename);
            workSheet = excelApp.ActiveSheet;
        }

        /// <summary>
        /// Метдо установки темплейта для дальнейшего заполнения
        /// </summary>
        /// <param name="template_filename">Имя и путь файла</param>
        /// <param name="background">Создать в бэкграунде</param>
        public static void SetTemplate(string template_filename, bool background)
        {
            if (!background)
                excelApp.Visible = true;
            else
                excelApp.Visible = false;
            excelApp.Workbooks.Add(template_filename);
            workSheet = excelApp.ActiveSheet;
        }

        /// <summary>
        /// Метод создающий новый файл
        /// </summary>
        /// <param name="filename">Имя и путь файла</param>
        /// <param name="background">Создать в бэкграунде</param>
        /// <param name="overwrite">Флаг перезаписи файле, если таковой уже существует</param>
        public static void CreateNewFile(string filename, bool background, bool overwrite)
        {
            if (!background)
                excelApp.Visible = true;
            else
                excelApp.Visible = false;
            excelApp.Workbooks.Add();
            workSheet = excelApp.ActiveSheet;
            if (File.Exists(filename))
                if (overwrite)
                    File.Delete(filename);
            workSheet.SaveAs(filename);
        }

        /// <summary>
        /// Метод сохраняющий файл
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="overwrite"></param>
        public static void SaveFile(string filename, bool overwrite)
        {
            if (workSheet != null)
            {
                if (File.Exists(filename))
                    if (overwrite)
                        File.Delete(filename);
                workSheet.SaveAs(filename);
            }
        }

        /// <summary>
        /// Метод записывающий данные в заданную ячейку
        /// </summary>
        /// <param name="cell">Ячейка. Специальный тип по примеру Point</param>
        /// <param name="value">Записываемое значение</param>
        public static void WriteToCell(Cell cell, object value)
        {
            if (workSheet != null)
                workSheet.Cells[cell.Row, cell.Column] = value;
        }

        public static void WriteToCell(string range, object value)
        {
            if (workSheet != null)
                workSheet.Range[range].Value = value;
        }

        /// <summary>
        /// Метод выравнивающий границы колонок по значению
        /// </summary>
        /// <param name="ColumnsRange">Пример "A:Z", все поля от A до Z будут выровнены по значениям!</param>
        public static void AutoFitColumns(string ColumnsRange)
        {
            workSheet.Columns[ColumnsRange].EntireColumn.AutoFit(); 
        }

        /// <summary>
        /// Метод выравнивающий границы строк по значению
        /// </summary>
        /// <param name="RowRange">Пример "1:2", будет выровнены 1-я и 2-я строки</param>
        public static void AutoFitRow(string RowRange)
        {
            workSheet.Rows[RowRange].EntireRow.AutoFit();
        }

        /// <summary>
        /// Метод минимизирующий Excel
        /// </summary>
        public static void MinimizeExcel()
        {
            excelApp.WindowState = Excel.XlWindowState.xlMinimized;
        }

        /// <summary>
        /// Метод максимизирующий Excel
        /// </summary>
        public static void MaximizeExcel()
        {
            excelApp.WindowState = Excel.XlWindowState.xlMaximized;
        }

        /// <summary>
        /// Метод вставляющий новую строку со сдвигом в низ по указанному номеру строки
        /// </summary>
        /// <param name="row_index">Номер строки</param>
        public static void InsertRowAt(int row_index)
        {
            if (workSheet != null)
                workSheet.Rows[row_index].Insert();
        }

        /// <summary>
        /// Метод получающий информацию об заданной ячейке
        /// </summary>
        /// <param name="CellAddr">Адрес ячейки(пока без проверки на рэндж)</param>
        /// <returns></returns>
        public static CellInfo GetCellInfo(string CellAddr)
        {
            CellInfo res = new CellInfo();

            if (workSheet != null)
            {                
                res.Row = workSheet.Range[CellAddr].Row;
                res.Column = workSheet.Range[CellAddr].Column;
                res.ColumnWidth = workSheet.Range[CellAddr].Width;
                res.RowHeight = workSheet.Range[CellAddr].Height;
                res.Interior = workSheet.Range[CellAddr].Interior;
                res.Font = workSheet.Range[CellAddr].Font;
                res.HorizontalAlignment = workSheet.Range[CellAddr].HorizontalAlignment;
                res.VerticalAlignment = workSheet.Range[CellAddr].VerticalAlignment;
            }

            return res;
        }

        /// <summary>
        /// Метод устанавливающий высоту строки
        /// </summary>
        /// <param name="row_idx">Номер строки</param>
        /// <param name="Height">Высота строки</param>
        public static void SetRowHeight(int row_idx, double Height)
        {
            if (workSheet != null)
                workSheet.Rows[row_idx].RowHeight = Height;
        }

        /// <summary>
        /// Метод устанавливающий ширину столбца
        /// </summary>
        /// <param name="col_idx">Номер столбца</param>
        /// <param name="Width">Ширина столбца</param>
        public static void SetColumnWidth(int col_idx, double Width)
        {
            if (workSheet != null)
                workSheet.Columns[col_idx].ColumnWidth = Width;
        }

        /// <summary>
        /// Метод позволяющий объединять ячейки, по заданному диапазону
        /// </summary>
        /// <param name="range">Диапозон ячеек</param>
        public static void MergeCells(string range)
        {
            if (workSheet != null)
            {
                workSheet.Range[range].Cells.Merge(true);
            }
        }

        /// <summary>
        /// Метод устанавливающий настройку ячейки
        /// </summary>
        /// <param name="cell"></param>
        public static void SetCellDecor(CellInfo cell)
        {
            if (workSheet != null)
            {                
                //--Font settings
                workSheet.Cells[cell.Row, cell.Column].Font.Name = cell.Font.Name;
                workSheet.Cells[cell.Row, cell.Column].Font.Size = cell.Font.Size;
                workSheet.Cells[cell.Row, cell.Column].Font.Bold = cell.Font.Bold;
                workSheet.Cells[cell.Row, cell.Column].Font.Italic = cell.Font.Italic;
                workSheet.Cells[cell.Row, cell.Column].Font.Underline = cell.Font.Underline;
                workSheet.Cells[cell.Row, cell.Column].Font.Background = cell.Font.Background;
                workSheet.Cells[cell.Row, cell.Column].Font.Color = cell.Font.Color;
                workSheet.Cells[cell.Row, cell.Column].Font.ColorIndex = cell.Font.ColorIndex;
                workSheet.Cells[cell.Row, cell.Column].Font.FontStyle = cell.Font.FontStyle;
                workSheet.Cells[cell.Row, cell.Column].Font.Strikethrough = cell.Font.Strikethrough;                
                workSheet.Cells[cell.Row, cell.Column].Font.ThemeFont = cell.Font.ThemeFont;
                workSheet.Cells[cell.Row, cell.Column].Font.TintAndShade = cell.Font.TintAndShade;
                //--Interior settings
                workSheet.Cells[cell.Row, cell.Column].Interior.Color = cell.Interior.Color;
                workSheet.Cells[cell.Row, cell.Column].Interior.ColorIndex = cell.Interior.ColorIndex;
                workSheet.Cells[cell.Row, cell.Column].Interior.Pattern = cell.Interior.Pattern;
                workSheet.Cells[cell.Row, cell.Column].Interior.PatternColor = cell.Interior.PatternColor;
                workSheet.Cells[cell.Row, cell.Column].Interior.PatternColorIndex = cell.Interior.PatternColorIndex;                
                workSheet.Cells[cell.Row, cell.Column].Interior.PatternTintAndShade = cell.Interior.PatternTintAndShade;

                workSheet.Cells[cell.Row, cell.Column].HorizontalAlignment = cell.HorizontalAlignment;
                workSheet.Cells[cell.Row, cell.Column].VerticalAlignment = cell.VerticalAlignment;                
            }
        }

        /// <summary>
        /// Метод устанавливающий шрифт у заданной ячейки
        /// Метод не проверен, с большой долей вероятности будет ошибка
        /// </summary>
        /// <param name="cell">Заданная ячейка</param>
        /// <param name="font">Шрифт</param>
        public static void SetCellFont(Cell cell, Font font)
        {
            if (workSheet != null)
            {
                workSheet.Cells[cell.Row, cell.Column].Font.Name = font.Name;
                workSheet.Cells[cell.Row, cell.Column].Font.Size = font.Size;
                workSheet.Cells[cell.Row, cell.Column].Font.Bold = font.Bold;                
                workSheet.Cells[cell.Row, cell.Column].Font.Italic = font.Italic;
                workSheet.Cells[cell.Row, cell.Column].Font.Underline = font.Underline;
            }
        }

        //public static void SetPageSetup - надо придумать как сделать этот метод

        /// <summary>
        /// Установка центрального колонтитуала
        /// </summary>
        /// <param name="text">Текст колонтитула</param>
        public static void SetPageCenterHeader(string text)
        {
            if (workSheet != null)
                workSheet.PageSetup.CenterHeader = text;            
        }

        public static void SetPageLeftFooter(string text)
        {
            if (workSheet != null)
                workSheet.PageSetup.LeftFooter = text;            
        }

        /// <summary>
        /// Метод устанавливающий Горизонтальное выравнивание текста в ячейке
        /// </summary>
        /// <param name="cell">Ячейка</param>
        /// <param name="halign">Энум с типами выравнивания</param>
        public static void SetCellHAlignment(Cell cell,HAlignment halign)
        {
            if (workSheet != null)
            {
                switch (halign) 
                { 
                    case HAlignment.Left:
                        workSheet.Cells[cell.Row, cell.Column].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        break;
                    case HAlignment.Center:
                        workSheet.Cells[cell.Row, cell.Column].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        break;
                    case HAlignment.Right:
                        workSheet.Cells[cell.Row, cell.Column].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        break;
                }
                                
            }
        }

        /// <summary>
        /// Метод удаляющий границы у ячейки
        /// </summary>
        /// <param name="Range"></param>
        public static void RemoveRangeBorder(string Range)
        {
            if (workSheet != null)
                workSheet.Range[Range].Borders.LineStyle = 0;
        }

        public static void SetRangeBorder(string Range)//--надо добабивать параметр стиля линий и чего именно рисовать, пока рисует полную клетку.
        {
            if (workSheet != null)
                workSheet.Range[Range].Borders.LineStyle = 1;            
            
        }

        /*public static void SetRangeFont(string Range, Font font)
        {
            if (workSheet != null)
                workSheet.Range[Range].Font = font; //Select().Font = font;            
        }*/
    }
}
