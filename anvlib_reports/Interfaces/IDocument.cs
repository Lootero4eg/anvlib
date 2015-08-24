using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib_reports.Interfaces
{
    public interface IDocument
    {
        string Title { get; set; }
        string Header { get; set; }
        string Footer { get; set; }
    }
}
