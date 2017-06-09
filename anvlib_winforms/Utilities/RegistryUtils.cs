using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;

namespace anvlib.Utilities
{
    /// <summary>
    /// Вспомогательный класс для параметров реестра Windows
    /// </summary>
    public class RegistryParam
    {
        public string Param;
        public RegistryValueKind ParamType;
        public object Value;

        public RegistryParam(string param, RegistryValueKind paramtype, object value)
        {
            Param = param;
            ParamType = paramtype;
            Value = value;
        }
    }

//-- Сделать считывание всего массива ключей!!!!

    /// <summary>
    /// Вспомогательный класс для работы с Реестром Windows
    /// </summary>
    public static class RegistryUtils
    {
        /// <summary>
        /// Метод возвращающий по заданному энуму, ключ в реестре
        /// </summary>
        /// <param name="root">Элемен энума</param>
        /// <returns>Строка с ключем</returns>
        private static string GetRootByHive(RegistryHive root)
        {
            switch (root)
            { 
                case RegistryHive.ClassesRoot:
                    return "HKEY_CLASSES_ROOT";
                case RegistryHive.CurrentUser:
                    return "HKEY_CURRENT_USER";
                case RegistryHive.LocalMachine:
                    return "HKEY_LOCAL_MACHINE";
                case RegistryHive.Users:
                    return "HKEY_USERS";
                case RegistryHive.CurrentConfig:
                    return "HKEY_CURRENT_CONFIG";
            }

            return string.Empty;
        }
        
        /// <summary>
        /// Записать в реестр значение
        /// </summary>
        /// <param name="root">Ветка реестра</param>
        /// <param name="key">Ключ реестра</param>
        /// <param name="param">Параметр ключа</param>
        public static void WriteToRegistry(RegistryHive root, string key, RegistryParam param)
        {
            string Root = string.Empty;                        
            if (!string.IsNullOrEmpty((Root = GetRootByHive(root))))
            {
                string fullkey = Root + "\\" + key;
                Registry.SetValue(fullkey, param.Param, param.Value, param.ParamType);
            }
            else
                throw new Exception("Пожалуйста выберите правильный корневой раздел!");
        }

        /// <summary>
        /// Записать в реестр значение
        /// </summary>
        /// <param name="root">Ветка реестра</param>
        /// <param name="key">Ключ реестра</param>
        /// <param name="param">Параметры ключа</param>
        public static void WriteToRegistry(RegistryHive root, string key, params RegistryParam[] pars)
        {
            string Root = string.Empty;
            if (!string.IsNullOrEmpty((Root = GetRootByHive(root))))
            {
                string fullkey = Root + "\\" + key;
                if (pars != null && pars.Length > 0)
                {
                    foreach (var par in pars)
                    {
                        Registry.SetValue(fullkey, par.Param, par.Value, par.ParamType);
                    }
                }
                else
                    throw new Exception("Пожалуйста выберите правильный корневой раздел!");
            }
        }

        /// <summary>
        /// Удалить ключ реестра
        /// </summary>
        /// <param name="root">Ветка реестра</param>
        /// <param name="key">Ключ для удаления</param>
        public static void RemoveRegistrySubKey(RegistryHive root, string key)
        {
            switch (root)
            {
                case RegistryHive.ClassesRoot:
                    Registry.ClassesRoot.DeleteSubKey(key);
                    break;

                case RegistryHive.CurrentUser:
                    Registry.CurrentUser.DeleteSubKey(key);
                    break;

                case RegistryHive.LocalMachine:
                    Registry.LocalMachine.DeleteSubKey(key);
                    break;

                case RegistryHive.Users:
                    Registry.Users.DeleteSubKey(key);
                    break;

                case RegistryHive.CurrentConfig:
                    Registry.CurrentConfig.DeleteSubKey(key);
                    break;
            }
        }

        /// <summary>
        /// Удалить дочернюю ветку реестра
        /// </summary>
        /// <param name="root">Втека реестра</param>
        /// <param name="key">Ключи и все что внутри на удаление</param>
        public static void RemoveRegistrySubKeyTree(RegistryHive root, string key)
        {
            switch (root)
            {
                case RegistryHive.ClassesRoot:
                    Registry.ClassesRoot.DeleteSubKeyTree(key);
                    break;

                case RegistryHive.CurrentUser:
                    Registry.CurrentUser.DeleteSubKeyTree(key);
                    break;

                case RegistryHive.LocalMachine:
                    Registry.LocalMachine.DeleteSubKeyTree(key);
                    break;

                case RegistryHive.Users:
                    Registry.Users.DeleteSubKeyTree(key);
                    break;

                case RegistryHive.CurrentConfig:
                    Registry.CurrentConfig.DeleteSubKeyTree(key);
                    break;
            }            
        }

        /// <summary>
        /// Считать данные из реестра
        /// </summary>
        /// <param name="root">Ветка реестра</param>
        /// <param name="key">Ключ</param>
        /// <param name="param_name">Имя параметра</param>
        /// <returns></returns>
        public static object ReadFromRegistry(RegistryHive root, string key, string param_name)
        {            
            string Root = string.Empty;
            
            if (!string.IsNullOrEmpty((Root = GetRootByHive(root))))
            {
                string fullkey = Root + "\\" + key;
                return Registry.GetValue(fullkey, param_name, "");
            }
            else
                throw new Exception("Пожалуйста выберите правильный корневой раздел!");            
        }
    }
}
