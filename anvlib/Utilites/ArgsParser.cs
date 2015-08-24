using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Utilites
{
    /// <summary>
    /// Вспомогательный класс для работы с параметрами коммандной строки
    /// </summary>
    public static class ArgsParser
    {
        /// <summary>
        /// Метод проверяющий наличие заданного аргумента из коммандной строки
        /// </summary>
        /// <param name="args">Коммандная строка</param>
        /// <param name="search_option">Искомый аргумент</param>
        /// <param name="OptionsPrefixes">Массив префиксов, с котором воможно задали наш аргумент (-,/ и т.д.)</param>
        /// <returns></returns>
        public static bool IsOptionKeyInArgs(string[] args, string search_option, params string[] OptionsPrefixes)
        {
            foreach (var item in args)
            {
                if (OptionsPrefixes != null && OptionsPrefixes.Length > 0)
                {
                    foreach (var prefix in OptionsPrefixes)
                    {
                        if (item == prefix + search_option)
                            return true;
                    }
                }
                else
                    if (item == search_option)
                        return true;
            }

            return false;
        }

        /// <summary>
        /// Метод проверяющий наличие заданного аргумента из коммандной строки
        /// </summary>
        /// <param name="args">Коммандная строка</param>
        /// <param name="search_option">Искомый аргумент</param>
        /// <param name="OptionsPrefixes">Массив префиксов, с котором воможно задали наш аргумент (-,/ и т.д.)</param>
        /// <returns></returns>
        public static bool IsOptionKeyInArgs(string[] args, string search_option, bool case_sensitive, params string[] OptionsPrefixes)
        {
            foreach (var item in args)
            {
                if (OptionsPrefixes != null && OptionsPrefixes.Length > 0)
                {
                    foreach (var prefix in OptionsPrefixes)
                    {
                        if (!case_sensitive)
                        {
                            if (item == prefix + search_option)
                                return true;
                        }
                        else
                        {
                            if (item.ToLower() == (prefix + search_option).ToLower())
                                return true;
                        }
                    }
                }
                else
                    if (!case_sensitive)
                    {
                        if (item == search_option)
                            return true;

                    }
                    else
                    {
                        if (item.ToLower() == search_option.ToLower())
                            return true;
                    }
            }

            return false;
        }

        /// <summary>
        /// Метод счиатавающий значение искомого нами аргумента
        /// </summary>
        /// <param name="args">Коммандная строка</param>
        /// <param name="option">Иискомый аргумент</param>
        /// <param name="OptionsPrefixes">Массив префиксов, с котором воможно задали наш аргумент (-,/ и т.д.)</param>
        /// <returns></returns>
        public static string GetOptionValue(string[] args, string option, params string[] OptionsPrefixes)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (OptionsPrefixes != null && OptionsPrefixes.Length > 0)
                {
                    foreach (var prefix in OptionsPrefixes)
                    {
                        if (args[i] == prefix + option)
                        {
                            if ((i + 1) < args.Length)
                                return args[i + 1];
                            else
                                break;
                        }
                    }
                }
                else
                    if (args[i] == option)
                    {
                        if ((i + 1) <= args.Length)
                            return args[i + 1];
                    }
            }

            return string.Empty;
        }

        /// <summary>
        /// Метод счиатавающий значение искомого нами аргумента
        /// </summary>
        /// <param name="args">Коммандная строка</param>
        /// <param name="option">Иискомый аргумент</param>
        /// <param name="OptionsPrefixes">Массив префиксов, с котором воможно задали наш аргумент (-,/ и т.д.)</param>
        /// <returns></returns>
        public static string GetOptionValue(string[] args, string option, bool case_sensitive, params string[] OptionsPrefixes)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (OptionsPrefixes != null && OptionsPrefixes.Length > 0)
                {
                    foreach (var prefix in OptionsPrefixes)
                    {
                        if (case_sensitive)
                        {
                            if (args[i] == prefix + option)
                            {
                                if ((i + 1) < args.Length)
                                    return args[i + 1];
                                else
                                    break;
                            }
                        }
                        else
                        {
                            if (args[i].ToLower() == (prefix + option).ToLower())
                            {
                                if ((i + 1) < args.Length)
                                    return args[i + 1];
                                else
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (case_sensitive)
                    {
                        if (args[i] == option)
                        {
                            if ((i + 1) <= args.Length)
                                return args[i + 1];
                        }
                    }
                    else
                    {
                        if (args[i].ToLower() == option.ToLower())
                        {
                            if ((i + 1) <= args.Length)
                                return args[i + 1];

                        }
                    }
                }                
            }
            return string.Empty;
        }
    }
}
