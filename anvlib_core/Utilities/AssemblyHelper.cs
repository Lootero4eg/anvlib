using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
//using System.Windows.Forms;

//using anvlib.Controls;

namespace anvlib.Utilities
{
    /// <summary>
    /// Надо переделать почти все методы, с использованием доплнитлеьного домена вместо Assembly.Load
    /// </summary>
    public static class AssemblyHelper
    {
        [Obsolete]
        public static List<string> GetSolutionAssemblies()
        {
            List<string> res = new List<string>();
            
            var asms = Assembly.GetEntryAssembly();            
            for (int i = 0; i < asms.GetReferencedAssemblies().Length; i++)
                res.Add(asms.GetReferencedAssemblies()[i].FullName);
            
            /*var asms = from t in AppDomain.CurrentDomain.GetAssemblies()
                       where !t.GlobalAssemblyCache && t.FullName.IndexOf("vshost") == -1
                       select t;*/
            //MessageBox.Show(res.Count.ToString());
            /*var asmlist=asms.ToList();
            for (int i = 0; i < asmlist.Count; i++)
                res.Add(asmlist[i].FullName);*/

            return res;
        }

        public static List<string> GetSolutionAssemblies(Assembly asm)
        {
            //MessageBox.Show(asm.FullName);
            List<string> res = new List<string>();            
           // var asm = Assembly.GetAssembly(assembly);            
            res.Add(asm.FullName);            
            for (int i = 0; i < asm.GetReferencedAssemblies().Length; i++)
                res.Add(asm.GetReferencedAssemblies()[i].FullName);
            
            return res;
        }

        public static List<string> GetExecutedSolutionAssemblies()
        {
            List<string> res = new List<string>();
            
            var asms = from t in AppDomain.CurrentDomain.GetAssemblies()
                       where !t.GlobalAssemblyCache && t.FullName.IndexOf("vshost") == -1
                       select t;
            
            var asmlist=asms.ToList();
            for (int i = 0; i < asmlist.Count; i++)
                res.Add(asmlist[i].FullName);

            return res;
        }

        public static List<string> GetAssemblyNamespaces(Assembly assembly)
        {
            try
            {
                var namespaces = assembly.GetTypes().Select(t => t.Namespace).Distinct();
                return namespaces.ToList();
            }
            catch (Exception e)
            {                
                throw e;                
            }
            //return null;
        }

        public static List<string> GetAssemblyNamespaces(string assembly_name)
        {
            try
            {
                var assembly = Assembly.Load(assembly_name);
                var namespaces = assembly.GetTypes().Select(t => t.Namespace).Distinct();
                return namespaces.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }

            //return null;
        }

        public static List<Type> GetAssemblyClasses(string assembly_name, string _namespace)
        {
            var assembly = Assembly.Load(assembly_name);            
            var classes = from z in assembly.GetTypes()
                          where (z.IsClass || z.IsInterface || z.IsEnum) 
                          && z.Namespace == _namespace 
                          //&& z.IsPublic
                          select z;
            return classes.ToList();
        }

        public static List<Type> GetAssemblyClasses(Assembly assembly, string _namespace)
        {            
            var classes = from z in assembly.GetTypes()
                          where (z.IsClass || z.IsInterface || z.IsEnum)
                          && z.Namespace == _namespace
                          //&& z.IsPublic
                          select z;
            return classes.ToList();
        }

        [Obsolete]//--Отладочный метод
        public static void test()
        {
            var asms = Assembly.GetEntryAssembly();
            var tps = asms.GetTypes().Select(t=>t.Namespace).Distinct();
            var aaa = asms.GetReferencedAssemblies();            
            var anv = Assembly.Load(aaa[4]);
            var jk = from z in anv.GetTypes()
                     where z.IsClass
                     select z;
            var gt = anv.GetTypes().Select(t => t.Namespace).Distinct().ToList();                       
        }

        
    }
}
