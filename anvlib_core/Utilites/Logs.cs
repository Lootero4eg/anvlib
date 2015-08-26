using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace anvlib.Utilites
{
    /// <summary>
    /// Класс для лог файлов
    /// </summary>
    public sealed class Log
    {
        private static TextWriter log;
        private string path = string.Empty;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="LogFileName">Имя и путь лог файла</param>
        public Log(string LogFileName)
        {
            if (LogFileName.IndexOf(':') == -1)
                path = AppDomain.CurrentDomain.BaseDirectory + LogFileName;
            else
                path = LogFileName;
        }

        /// <summary>
        /// Флаг показывающий, можно ли записть данные в файл или он занят системой
        /// </summary>
        /// <returns></returns>
        private bool CanWrite()
        {
            try
            {
                if (!File.Exists(path))
                    log = new StreamWriter(path);
                else
                    log = File.AppendText(path);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Метод записывающий сразу несколько строк в файл
        /// </summary>
        /// <param name="Lines">Строки, которые надо записать в файл</param>
        private void WriteMultipleRows(List<string> Lines)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Path is not set!!! Go to config window and set it!");
            }

            while (!CanWrite())
            { }

            foreach (var line in Lines)
            {
                log.WriteLine(DateTime.Now.ToString() + ": " + line);
                Console.WriteLine(DateTime.Now.ToString() + ": " + line);
            }
            log.Close();
        }

        /// <summary>
        /// Метод записывающиц одну строку в файл
        /// </summary>
        /// <param name="msg">Строка</param>
        public void WriteLine(string msg)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Path is not set!!! Go to config window and set it!");
            }
            while (!CanWrite())
            { }

            try
            {
                log.WriteLine(DateTime.Now.ToString() + ": " + msg);
                Console.WriteLine(DateTime.Now.ToString() + ": " + msg);
                log.Close(); log.Flush();
            }
            catch { }
        }

        /// <summary>
        /// Метод записывающиц одну параметрическую строку в файл
        /// </summary>
        /// <param name="msg">Параметрическая строка</param>
        /// <param name="pararray">параметры параметрической строки</param>
        public void WriteLine(string msg, params string[] pararray)
        {
            string tmpstr = msg;
            if (pararray != null && pararray.Length > 0)
            {
                for (int i = 0; i < pararray.Length; i++)
                    tmpstr = tmpstr.Replace("{" + i.ToString() + "}", pararray[i]);
            }

            WriteLine(tmpstr);
        }

        /// <summary>
        /// Метод записывающий сразу несколько строк в файл
        /// </summary>
        /// <param name="Lines"></param>
        public void WriteLines(List<string> Lines)
        {
            WriteMultipleRows(Lines);
        }

        /// <summary>
        /// Метод записывающий сразу несколько строк в файл
        /// </summary>
        /// <param name="Lines"></param>
        public void WriteLines(string[] Lines)
        {
            List<string> lines = new List<string>();
            lines.AddRange(Lines);

            WriteMultipleRows(lines);
        }
    }
}
