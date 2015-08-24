using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.ComponentModel.Design;
using System.Runtime;

using anvlib.Classes;
using anvlib.Controls.Dialogs;
using anvlib.Controls.UITypeEditors;
using anvlib.Controls.Designers;

namespace anvlib.Controls
{    
    [Designer(typeof(TypedListViewDesigner))]
    public class TypedListView : ListView
    {
        private object _datasource;
        private TypedListViewDisplayMember _tlvdm = new TypedListViewDisplayMember();

        public TypedListView()
        {
            DataSourceDescription = new TypedListViewDisplayMember();
            if (_tlvdm.Caption=="")
            {
                _tlvdm.Caption = "Item0";
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [EditorAttribute(typeof(TypedListViewEditor), typeof(UITypeEditor))]
        [Description("Описание дерева, по которому из DataSource будет строиться ListView")]
        public TypedListViewDisplayMember DataSourceDescription
        {
            get { return _tlvdm; }
            set
            {
                _tlvdm = value;                
            }
        }

        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Description("Источник данных. Таблица, Класс, Датасет")]
        public object DataSource 
        {
            get
            {
                return _datasource;
            }

            set
            {
                if(this.Items.Count>0)
                    this.Items.Clear();

                _datasource = value;
                if (value == null)
                    this.Items.Clear();
                else if (_tlvdm != null)
                    ControlFiller.FillListView(this, this.DataSource, _tlvdm);
            }
        }

        public void RefreshDataSource()
        {
            var ds = _datasource;
            DataSource = ds;
        }

        /*[Description("Планируется использовать если в классе нужен не весь класс, а лишь его часть или если используется DataSet")]
        public string DisplayMember { get; set; }*/
    }
}
