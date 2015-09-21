using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace anvlib.Interfaces
{
    public interface IExportTableMethod
    {
        void Export(DataTable table);
    }
}
