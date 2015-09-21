using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace anvlib.Utilites
{
    public static class DbTypeToSystemTypeConverter
    {
        public static Type Convert(DbType dbtype)
        {
            Type res = null;

            switch (dbtype)
            { 
                case DbType.AnsiString:
                    res = typeof(string);
                    break;
                
                case DbType.AnsiStringFixedLength:
                    res = typeof(string);
                    break;

                //--непроверено, скорее всего будет ошибка
                case DbType.Binary:
                    res = typeof(byte[]);
                    break;

                case DbType.Boolean:
                    res = typeof(bool);
                    break;

                case DbType.Byte:
                    res = typeof(byte);
                    break;

                //--непроверено, скорее всего будет ошибка
                case DbType.Currency:
                    res = typeof(double);
                    break;

                case DbType.Date:
                    res = typeof(DateTime);
                    break;

                case DbType.DateTime:
                    res = typeof(DateTime);
                    break;

                case DbType.DateTime2:
                    res = typeof(DateTime);
                    break;

                case DbType.DateTimeOffset:
                    res = typeof(DateTimeOffset);
                    break;

                case DbType.Decimal:
                    res = typeof(decimal);
                    break;

                case DbType.Double:
                    res = typeof(double);
                    break;

                case DbType.Guid:
                    res = typeof(Guid);
                    break;

                case DbType.Int16:
                    res = typeof(Int16);
                    break;

                case DbType.Int32:
                    res = typeof(Int32);
                    break;

                case DbType.Int64:
                    res = typeof(Int64);
                    break;

                case DbType.Object:
                    res = typeof(object);
                    break;
                
                case DbType.SByte:
                    res = typeof(sbyte);
                    break;

                case DbType.Single:
                    res = typeof(Single);
                    break;

                case DbType.String:
                    res = typeof(string);
                    break;

                case DbType.StringFixedLength:
                    res = typeof(string);
                    break;

                case DbType.Time:
                    res = typeof(DateTime);
                    break;

                case DbType.UInt16:
                    res = typeof(UInt16);
                    break;

                case DbType.UInt32:
                    res = typeof(UInt32);
                    break;

                case DbType.UInt64:
                    res = typeof(UInt64);
                    break;

                //--непроверено, скорее всего будет ошибка
                case DbType.VarNumeric:
                    res = typeof(int);
                    break;
            }

            return res;
        }
    }
}
