using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace anvlib.Utilities
{
    /// <summary>
    /// Вспомогательный класс для работы с файлами конфигов (.conf, .ini, .cfg)
    /// Недописан...в основном я пользуюсь классами Сеттингов
    /// </summary>
    public class ConfigManager
    {
        private string _filename;
        private bool _eof = true;
        private StreamReader _streamReader;
        private FileStream _fstream;
             
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="filename">Путь и имя конфига</param>
        public ConfigManager(string filename)
        {
            _filename = filename;
            if (File.Exists(_filename))
            {
                _streamReader = new StreamReader(_filename);
                _eof = _streamReader.EndOfStream;
            }
            else
            {
                _fstream = new FileStream(_filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                _eof = !_fstream.CanRead;
            }
        }

        /// <summary>
        /// Метод считывающий строки конфига в "Словарь"
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ReadParameterizedLines()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();            

            if (_streamReader != null)
            {
                while (!_streamReader.EndOfStream)
                {                    
                    string line = _streamReader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        continue;
                    if (line[0] == '#' || line[0] == ';')
                        continue;
                    if (line.IndexOf('=') > 0)
                    {
                        string[] tmpstrs = line.Split('=');
                        if (tmpstrs.Length == 2)
                            res.Add(tmpstrs[0], tmpstrs[1]);                            
                    }
                }
            }
            return res;
        }

        public string ReadLine()
        {
            return string.Empty;
        }

        //public void WriteLine();

        public bool Eof
        {
            get { return _eof; }
        }
    }
}
