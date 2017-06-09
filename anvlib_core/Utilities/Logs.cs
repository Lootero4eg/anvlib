using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace anvlib.Utilities
{
    public enum FileSizeExceededEnum { DisableWriting, ThrowException, RewriteFile }

    /// <summary>
    /// Класс для лог файлов
    /// </summary>
    public sealed class Log
    {
        private static TextWriter log;
        private string path = string.Empty;
        private bool _isWriteTimeStamp = true;
        private string _dateTimeFormatString = "dd.MM.yyyy HH:mm:ss";
        private bool _isConsoleOutputEnabled = true;
        private double _maxLogFileSizeInMB = 0;//--Неограничено
        private FileSizeExceededEnum _fileSizeExceededAction = FileSizeExceededEnum.DisableWriting;//--Этот параметр работает исключительно с _maxLogFileSizeInMB > 0
        private bool _isFileSizeExceeded = false;

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
                {
                    if (!Directory.Exists(Path.GetDirectoryName(path)))
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    log = new StreamWriter(path);
                }
                else
                {
                    if (_maxLogFileSizeInMB > 0)
                    {
                        FileInfo fi = new FileInfo(path);
                        if (fi.Length > 0)
                        {
                            double fsize = (double)(fi.Length / 1024) / 1024;
                            if (fsize > _maxLogFileSizeInMB)
                            {
                                switch (_fileSizeExceededAction)
                                {
                                    case FileSizeExceededEnum.DisableWriting:
                                        _isFileSizeExceeded = true;
                                        return true;
                                    case FileSizeExceededEnum.ThrowException:
                                        throw new Exception("Size of log file is exceeded. See MaxLogFileSizeInMB Properrty.");
                                    case FileSizeExceededEnum.RewriteFile:
                                        log = new StreamWriter(fi.Open(FileMode.Truncate));
                                        break;
                                }
                            }
                            else
                                log = File.AppendText(path);
                        }
                    }
                    else
                        log = File.AppendText(path);
                }

                return true;
            }
            catch(Exception ex)
            {
                if (ex.Message == "Size of log file is exceeded. See MaxLogFileSizeInMB Properrty.")
                    throw ex;
                else
                    return false;
            }
        }

        /// <summary>
        /// Максимальный размер лог файла в мегабайтах
        /// </summary>
        public double MaxLogFileSizeInMB
        {
            get { return _maxLogFileSizeInMB; }
            set { _maxLogFileSizeInMB = value; }
        }

        /// <summary>
        /// Флаг, узывающий что делать в случае превышения размера лог файла
        /// </summary>
        public FileSizeExceededEnum FileSizeExceededAction
        {
            get { return _fileSizeExceededAction; }
            set { _fileSizeExceededAction = value; }
        }

        /// <summary>
        /// Флаг, по которому определяется писать ли в лог дату и время каждой записи.
        /// По умолчанию он включен.
        /// </summary>
        public bool IsWriteTimeStamp { get { return _isWriteTimeStamp; } set { _isWriteTimeStamp = value; } }

        /// <summary>
        /// Формат даты и времени.
        /// По умолчанию "dd.mm.yyyy hh:mm:ss", можно менять по своему усмотрению
        /// </summary>
        public string DateTimeFormatString { get { return _dateTimeFormatString; } set { _dateTimeFormatString = value; } }

        /// <summary>
        /// Флаг, разрешающий выводить записываемую в лог строку в консоль.
        /// По умолчанию он включен.
        /// </summary>
        public bool IsConsoleOutputEnabled { get { return _isConsoleOutputEnabled; } set { _isConsoleOutputEnabled = value; } }        

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

            if (_isFileSizeExceeded)
                return;

            foreach (var line in Lines)
            {
                string linetext = "";
                if (_isWriteTimeStamp)
                    linetext = string.Format("{0}: {1}", DateTime.Now.ToString(_dateTimeFormatString), line);
                else
                    linetext = line;

                log.WriteLine(linetext);
                if (_isConsoleOutputEnabled)
                    Console.WriteLine(linetext);
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

            if (_isFileSizeExceeded)
                return;

            try
            {
                string linetext = "";
                if (_isWriteTimeStamp)
                    linetext = string.Format("{0}: {1}", DateTime.Now.ToString(_dateTimeFormatString), msg);
                else
                    linetext = msg;

                log.WriteLine(linetext);
                if (_isConsoleOutputEnabled)
                    Console.WriteLine(linetext);
                
                log.Close();
            }
            catch { } //--нафигу тут сделал кэтч, если не проверяешь ничего... надо будет убрать
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

        /// <summary>
        /// Метод очищающий лог файл
        /// </summary>
        public void ClearLogData()
        {
            if (CanWrite())
            {
                log.Close();
                File.Delete(path);
                log = new StreamWriter(path);
                log.Close();
            }
        }
    }
}
