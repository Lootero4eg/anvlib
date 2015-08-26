using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace anvlib.Xml
{
    /// <summary>
    /// Вспомогательный класс для работы с XML
    /// </summary>
    public static class XmlClassManager
    {
        /// <summary>
        /// Метод записывающий класс в XML
        /// </summary>
        /// <param name="filename">Имя и путь файла в который будем сохранять наш класс</param>
        /// <param name="Class">Записываемый класс</param>
        public static void WriteClassToXmlFile(string filename, object Class)
        {
            Stream _file;
            XmlSerializer _serializer;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
                        
            _file = new StreamWriter(filename, false).BaseStream;
            _serializer = new XmlSerializer(Class.GetType());
            _serializer.Serialize(_file, Class, ns);             
            _file.Close();
        }

        /// <summary>
        /// Метод считывающий данные из XML
        /// </summary>
        /// <param name="filename">Имя и путь файла, в котором хранится заданный класс</param>
        /// <param name="type">Тип считываемого класса</param>
        /// <returns>Заполненный класс</returns>
        public static object ReadClassFromXmlFile(string filename, Type type)
        {
            Stream _file;
            XmlSerializer _serializer;
            
            if (!File.Exists(filename))
                throw new Exception("File not found!");
                        
            _file = new StreamReader(filename).BaseStream;
            _serializer = new XmlSerializer(type);
            object res = _serializer.Deserialize(_file);
            _file.Close();

            return res;
        }
    }
}
