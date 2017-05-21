using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace anvlib.Utilites
{
    /// <summary>
    /// Вспомогательный класс для работы с процессами системы Windows
    /// </summary>
    public static class ProcessManager
    {
        /// <summary>
        /// Метод проверяющий запущем ли процесс с заданным именем
        /// </summary>
        /// <param name="procname">Имя процесса или имя запускаемого файла</param>
        /// <returns></returns>
        public static bool IsProcessRuning(string procname)
        {
            var proc = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(procname));
            
            if(proc.Length > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Метод проверяющий запущем ли процесс с заданным id
        /// </summary>
        /// <param name="procname">Id процесса</param>
        /// <returns></returns>
        public static bool IsProcessRuning(int proc_id)
        {
            var proc = Process.GetProcessById(proc_id);

            if (proc != null)
                return true;

            return false;
        }

        /// <summary>
        /// Метод возвращающий список Id процессов, по имени исполняемого файла
        /// </summary>
        /// <param name="procname"></param>
        /// <returns></returns>
        public static int[] GetProcessesIds(string procname)
        {
            int[] res;
            var proc = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(procname));

            if (proc.Length > 0)
            {
                res = new int[proc.Length];
                for (int i = 0; i < proc.Length; i++)
                    res[i] = (proc.GetValue(i) as Process).Id;

                return res;
            }

            return null;
        }

        public static Process[] GetProcesses()
        {
            return Process.GetProcesses();
        }

        /// <summary>
        /// Метод завершающий процесс по имени исполняемого файла
        /// </summary>
        /// <param name="procname"></param>
        public static void KillProcess(string procname)
        {
            foreach (var proc in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(procname)))
                proc.Kill();            
        }

        /// <summary>
        /// Метод завершающий все процессы, кроме заданного Id
        /// </summary>
        /// <param name="procname"></param>
        /// <param name="owner_proc_id"></param>
        public static void KillProcess(string procname, int owner_proc_id)
        {
            foreach (var proc in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(procname)))
            {
                if (proc.Id != owner_proc_id)
                    proc.Kill();
            }
        }

        /// <summary>
        /// Метод завершающий процесс по заданному Id
        /// </summary>
        /// <param name="proc_id"></param>
        public static void KillProcess(int proc_id)
        {
            var proc = Process.GetProcessById(proc_id);
            proc.Kill();            
        }

        /// <summary>
        /// Запустить внешнее приложение
        /// </summary>
        /// <param name="exe_full_path">путь до исполняемого файла</param>
        /// <param name="args">аргументы командной строки для вызываемого исполняемого файла</param>
        public static void RunProgram(string exe_full_path, string args)
        {
            Process application = new Process();
            application.EnableRaisingEvents = true;
            application.StartInfo.UseShellExecute = true;
            application.StartInfo.FileName = exe_full_path;
            application.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            application.StartInfo.Arguments = args;
            application.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            application.Start();
        }

        public static void RunProgram(string exe_full_path, string args, string working_dir)
        {
            Process application = new Process();
            application.EnableRaisingEvents = true;
            application.StartInfo.UseShellExecute = true;
            application.StartInfo.FileName = exe_full_path;
            application.StartInfo.WorkingDirectory = working_dir;
            application.StartInfo.Arguments = args;
            application.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            application.Start();
        }

        /// <summary>
        /// Запустить внешнее приложение в background-е
        /// </summary>
        /// <param name="exe_full_path">путь до исполняемого файла</param>
        /// <param name="args">аргументы командной строки для вызываемого исполняемого файла</param>
        public static void RunProgramInBackgrond(string exe_full_path, string args)
        {
            Process application = new Process();
            application.EnableRaisingEvents = true;
            application.StartInfo.UseShellExecute = true;
            application.StartInfo.FileName = exe_full_path;
            application.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            application.StartInfo.Arguments = args;
            application.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            application.Start();
        }

        public static void RunProgramInBackgrond(string exe_full_path, string args, string working_dir)
        {
            Process application = new Process();
            application.EnableRaisingEvents = true;
            application.StartInfo.UseShellExecute = true;
            application.StartInfo.FileName = exe_full_path;
            application.StartInfo.WorkingDirectory = working_dir;
            application.StartInfo.Arguments = args;
            application.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            application.Start();
        }

        /// <summary>
        /// Метод возвращающий Id текущего процесса
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentProcessId()
        {            
            return Process.GetCurrentProcess().Id;
        }
    }
}
