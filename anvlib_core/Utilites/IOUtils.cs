using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace anvlib.Utilites
{
    /// <summary>
    /// Вспомогательный класс работающий с файловой системой
    /// </summary>
    public static class IOUtils
    {
        /// <summary>
        /// Метод записывающий список всех директорий в файл
        /// </summary>
        /// <param name="DirPath">Путь к директории, содержимое которой нужно записать в файл</param>
        /// <param name="FileName">Путь и имя файла, в который мы будем записывать список директорий</param>
        public static void WriteFolderDirsListToFile(string DirPath, string FileName)
        {
            DirectoryInfo[] cDirs = new DirectoryInfo(DirPath).GetDirectories();

            if (!Directory.Exists(Path.GetDirectoryName(FileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(FileName));

            string path = AppDomain.CurrentDomain.BaseDirectory + FileName;

            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (DirectoryInfo dir in cDirs)
                    sw.WriteLine(dir.Name);
            }
        }

        /// <summary>
        /// Метод записывающий List<> заданных нами директорий в файл
        /// </summary>
        /// <param name="dirs">Лист директорий</param>
        /// <param name="FileName">Путь и имя файла, в который мы будем записывать список директорий</param>
        public static void WriteDirsListToFile(List<string> dirs, string FileName)
        {
            string path = "";
            if (FileName[1] != ':')
                path = AppDomain.CurrentDomain.BaseDirectory + FileName;
            else
                path = FileName;

            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (var item in dirs)
                    sw.WriteLine(item);
            }
        }

        /// <summary>
        /// Метод считывающий в List, список директорий из файла
        /// </summary>
        /// <param name="FileName">Путь и имя файла, в котором храниться список директорий</param>
        /// <returns></returns>
        public static List<string> ReadDirsListFromFile(string FileName)
        {
            List<string> DirsList = new List<string>();
            string path = "";
            if (FileName[1] != ':')
                path = AppDomain.CurrentDomain.BaseDirectory + FileName;
            else
                path = FileName;

            if (!File.Exists(path))
                return null;

            string line = "";
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                    DirsList.Add(line);
            }

            return DirsList;
        }

        /// <summary>
        /// Метод возвращающий по заданному пути все подкаталоги
        /// </summary>
        /// <param name="DirPath"></param>
        /// <returns></returns>
        public static List<string> GetSubDirsList(string DirPath)
        {
            List<string> DirsList = new List<string>();

            DirectoryInfo[] cDirs = new DirectoryInfo(DirPath).GetDirectories();

            foreach (DirectoryInfo dir in cDirs)
                DirsList.Add(dir.Name);

            return DirsList;
        }

        /// <summary>
        /// Обертка для домашнего каталога из которого запускается программа
        /// </summary>
        /// <returns></returns>
        public static string GetBaseDirPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Обертка для имени запускаемого файла нашей программы
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentExecutableName()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        /// <summary>
        /// Обертка для считывания размера файла
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static long GetFileSize(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                FileInfo file = new FileInfo(FilePath);
                return file.Length;
            }

            return -1;
        }

        /// <summary>
        /// Обертка для считивания "Даты модификации файла"
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static DateTime GetFileLastModifyDate(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                FileInfo file = new FileInfo(FilePath);
                return file.LastWriteTime;
            }

            return new DateTime(1, 1, 1);
        }

        /// <summary>
        /// Обертка для проверки директории на существование
        /// </summary>
        /// <param name="DirPath"></param>
        /// <returns></returns>
        public static bool DirectoryExists(string DirPath)
        {
            return Directory.Exists(DirPath);
        }

        /// <summary>
        /// Обертка для проверки файла на существование
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool FileExists(string FilePath)
        {
            return File.Exists(FilePath);
        }

        /// <summary>
        /// Обертка для создания директории
        /// </summary>
        /// <param name="DirName"></param>
        public static void MakeDirectory(string DirName)
        {
            if (!Directory.Exists(DirName))
                Directory.CreateDirectory(DirName);
        }

        /// <summary>
        /// Обертка для переименования файла
        /// </summary>
        /// <param name="old_fn"></param>
        /// <param name="new_fn"></param>
        public static void RenameFile(string old_fn, string new_fn)
        {            
            File.Move(old_fn, new_fn);
        }

        /// <summary>
        /// Метод переименования директории
        /// </summary>
        /// <param name="old_dr"></param>
        /// <param name="new_dr"></param>
        public static void RenameDirectory(string old_dr, string new_dr)
        {            
            Directory.Move(old_dr, new_dr);
        }

        /// <summary>
        /// Метод возвращающий все имена файлов заданной директории
        /// </summary>
        /// <param name="dir_path">Путь до директории с файлами</param>
        /// <returns></returns>
        public static List<string> GetDirectoryFilesList(string dir_path)
        {
            List<string> res = new List<string>();

            DirectoryInfo di = new DirectoryInfo(dir_path);
            var files = di.GetFiles();

            foreach (var file in files)
                res.Add(file.Name);

            return res;
        }

        /// <summary>
        /// Метод возвращающий все имена файлов заданной директории
        /// </summary>
        /// <param name="dir_path">Путь до директории с файлами</param>
        /// <param name="file_pattern">можно задать макску поиска</param>
        /// <returns></returns>
        public static List<string> GetDirectoryFilesList(string dir_path, string file_pattern)
        {
            List<string> res = new List<string>();

            DirectoryInfo di = new DirectoryInfo(dir_path);
            var files = di.GetFiles(file_pattern);

            foreach (var file in files)
                res.Add(file.Name);

            return res;
        }
    }
}
