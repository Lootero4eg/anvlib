using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace anvlib.Utilities
{
    /// <summary>
    /// Конвертер из системных типов в тип базы данных.
    /// Думаю тут будет куча ошибок, по крайней мере, кастомные поля не сделать... такие как анси строки...
    /// </summary>
    public static class SystemTypeToDbTypeConverter
    {
        public static DbType Convert(Type type)
        {
            DbType res = DbType.String;

            if (type == typeof(string))
                res = DbType.String;                    

            if(type == typeof(byte[]))
                res = DbType.Binary;                    

            if(type == typeof(bool))
                res = DbType.Boolean;  
            
            if(type == typeof(byte))
                res = DbType.Byte;  

            if(type == typeof(DateTime))
                res = DbType.DateTime;
            
            if(type == typeof(DateTimeOffset))
                res = DbType.DateTimeOffset;

            if(type == typeof(decimal))
                res = DbType.Decimal;

            if(type == typeof(double))
                res = DbType.Double;            

            if(type == typeof(Guid))
                res = DbType.Guid;                

            if(type == typeof(Int16))
                res = DbType.Int16;                

            if(type == typeof(Int32))
                res = DbType.Int32;                

            if(type == typeof(Int64))
                res = DbType.Int64;                               

            if(type == typeof(object))
                res = DbType.Object;                               

            if(type == typeof(sbyte))
                res = DbType.SByte;                  
                                
            if(type == typeof(Single))
                res = DbType.Single;                  

            if(type == typeof(UInt16))
                res = DbType.UInt16;                

            if(type == typeof(UInt32))
                res = DbType.UInt32;                

            if(type == typeof(UInt64))
                res = DbType.UInt64;                              

            return res;
        }
    }
}
