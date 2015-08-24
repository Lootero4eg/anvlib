using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace anvlib.Classes
{
    public class PluginsService
    {
        public List<AvailablePlugin> FindPlugins(string strPath, string strInterface)
        {
            List<AvailablePlugin> Plugins = new List<AvailablePlugin>();
            string[] strDLLs;
            Assembly objDLL;

            strDLLs = Directory.GetFileSystemEntries(strPath, "*.dll");
            for (int intIndex = 0; (intIndex <= strDLLs.Length - 1); intIndex++)
            {
                try
                {
                    objDLL = Assembly.LoadFrom(strDLLs[intIndex]);
                    ExamineAssembly(objDLL, strInterface, Plugins);
                }
                catch (Exception)
                {
                    //ошибка загрузки DLL - мы тут ничего не делаем
                }
            }
            if (Plugins.Count != 0)
                return Plugins;
            else
                return null;
        }

        public static void ExamineAssembly(Assembly objDLL, string strInterface, List<AvailablePlugin> Plugins)
        {
            //Type objType;
            Type objInterface;
            AvailablePlugin Plugin;
            Type[] otps = objDLL.GetTypes();

            //Цикл по всем типам в DLL
            foreach (Type objType in otps)
            {
                //Смотрим только типы public
                if (objType.IsPublic == true)
                {
                    //игнорируем абстрактные классы
                    if ((objType.Attributes & TypeAttributes.Abstract) != TypeAttributes.Abstract)
                    {
                        //Смотрим, реализует ли этот тип наш интерфейс
                        objInterface = objType.GetInterface(strInterface, true);
                        if (objInterface != null)
                        {
                            Plugin = new AvailablePlugin();
                            Plugin.AssemblyPath = objDLL.Location;
                            Plugin.ClassName = objType.FullName;
                            Plugins.Add(Plugin);
                        }
                    }
                }
            }
        }

        public object CreatePluginInstance(AvailablePlugin Plugin)
        {
            Assembly objDLL;
            object objPlugin;
            try
            {
                //Загружаем dll
                objDLL = Assembly.LoadFrom(Plugin.AssemblyPath);
                //создаём и возвращаем экземпляр класса
                objPlugin = objDLL.CreateInstance(Plugin.ClassName);
            }
            catch (Exception)
            {
                return null;
            }
            return objPlugin;
        }
    }
}
