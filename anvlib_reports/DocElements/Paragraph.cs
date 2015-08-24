using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anvlib_reports.DocElements
{
    //--незабудь убрать абстрактность
    public abstract class Paragraph
    {        
        public HorizontalAlignment Aligment { get; set; }

        public abstract int CharsCount { get; }

        public abstract string PlainText { get; }

        //public ElementsCollection Elements {get;set;}
    }
}
