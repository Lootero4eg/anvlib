using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Utilites
{
    public static class ObjectInspector
    {
        public static bool HasObjectDataSourceProperty(object obj)
        {
            if (obj != null)
            {
                if (obj.GetType().GetProperty("DataSource") != null)
                    return true;
            }

            return false;
        }

        public static bool HasObjectDisplayMemberProperty(object obj)
        {
            if (obj != null)
            {
                if (obj.GetType().GetProperty("DisplayMember") != null)
                    return true;
            }

            return false;
        }

        public static void SetDataSource(object obj, object datasource)
        {
            if (obj != null)
            {
                if (HasObjectDataSourceProperty(obj))
                    obj.GetType().GetProperty("DataSource").SetValue(obj, datasource, null);
            }
        }

        public static void SetDisplayMember(object obj, object displaymember)
        {
            if (obj != null)
            {
                if (HasObjectDisplayMemberProperty(obj))
                    obj.GetType().GetProperty("DisplayMember").SetValue(obj, displaymember, null);
            }
        }

        public static bool HasObjectPropertyByName(object obj, string prop_name)
        {
            if (obj != null)
            {
                if (obj.GetType().GetProperty(prop_name) != null)
                    return true;
            }

            return false;
        }

        public static void SetObjectPropertyValue(object obj, string prop_name, object prop_value)
        {
            if (obj != null)
            {
                if (HasObjectPropertyByName(obj, prop_name))
                    obj.GetType().GetProperty(prop_name).SetValue(obj, prop_value, null);
            }
        }

        public static object GetObjectPropertyValue(object obj, string prop_name)
        {
            if (obj != null)
            {
                if (HasObjectPropertyByName(obj, prop_name))
                    return obj.GetType().GetProperty(prop_name).GetValue(obj, null);
            }

            return null;
        }

        public static List<string> GetPropertiesList(object obj)
        {
            if (obj != null)
            {
                List<string> res = new List<string>();
                foreach (var item in obj.GetType().GetProperties())
                    res.Add(item.Name);
                
                return res;
            }

            return null;
        }

        public static object GetObjectPropertyValueRecursive(object obj, string prop_name)
        {
            if (obj != null)
            {
                if (prop_name.IndexOf('.') > -1)
                {
                    var tmpprops = prop_name.Split('.');
                    var value = obj.GetType().GetProperty(tmpprops[0]).GetValue(obj, null);
                    string[] array2 = new string[tmpprops.Length - 1];
                    for (int i = 1; i < tmpprops.Length; i++)
                        array2[i - 1] = tmpprops[i];
                    if (array2.Length > 1)
                        return GetObjectPropertyValueRecursive(value, string.Join(".", array2));
                    else
                        return GetObjectPropertyValueRecursive(value, array2[0]);
                }
                if (HasObjectPropertyByName(obj, prop_name))
                    return obj.GetType().GetProperty(prop_name).GetValue(obj, null);
            }

            return null;
        }
    }
}
