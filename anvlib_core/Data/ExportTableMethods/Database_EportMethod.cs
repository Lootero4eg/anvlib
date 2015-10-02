using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using anvlib.Data;
using anvlib.Data.Database;
using anvlib.Interfaces;
using anvlib.Enums;

namespace anvlib.Data.ExportTableMethods
{
    /// <summary>
    /// Класс прослойка между реальным экспортером и интерфесом
    /// </summary>
    public class Database_EportMethod: IExportTableMethod
    {
        private BaseDbManager sqlmgr;

        public event EventHandler ExportComplete;
        public event EventHandler ExportException;

        public Database_EportMethod(BaseDbManager dbmanager)
        {
            sqlmgr = dbmanager;
        }
        
        /// <summary>
        /// Реализация метода экспорта таблицы
        /// </summary>
        /// <param name="table"></param>
        /// <param name="additionaldata"></param>
        public void Export(DataTable table)
        {
            if (sqlmgr != null && sqlmgr.Connected)
            {
                //--пока так, но лучше всетаки сделать еще и апдейт в будущем!
                try
                {
                    if (!sqlmgr.IsObjectExists(table.TableName, DataBaseObjects.table, true))
                        sqlmgr.CreateTable(table);
                    else
                    {
                        sqlmgr.DropTable(table.TableName);
                        sqlmgr.CreateTable(table);
                    }

                    if (ExportComplete != null)
                        ExportComplete(this, new EventArgs());
                }
                catch (Exception e)
                {
                    if (ExportException != null)
                        ExportException(e, new EventArgs());
                    else
                        throw e;
                }
            }
            else
                throw new Exception("You must connect first!");
        }        
    }
}
