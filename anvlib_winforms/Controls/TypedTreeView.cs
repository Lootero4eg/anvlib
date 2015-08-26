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
    [Designer(typeof(TypedTreeViewDesigner))]
    public class TypedTreeView : CommonTreeView
    {
        private TypedTreeViewDisplayMemberList _tvdml = new TypedTreeViewDisplayMemberList();
        private object _datasource;                

        public TypedTreeView()
        {            
            _tvdml = new TypedTreeViewDisplayMemberList();
            _tvdml.Parent = this;
        }        

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [EditorAttribute(typeof(TypedTreeViewEditor), typeof(UITypeEditor))]
        [Description("Описание дерева, по которому из DataSource будет строиться TreeView")]             
        public TypedTreeViewDisplayMemberList DataSourceDescription
        {
            get { return _tvdml; }
            set 
            {
                _tvdml = value;
                _tvdml.Parent = this;
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
                _datasource = value;
                if (value == null)
                {
                    this.Nodes.Clear();
                }
                else if (_tvdml != null && _tvdml.Count > 0)
                {
                    ControlFiller.FillTreeViewControl(this, this.DataSource, _tvdml);
                }
            }
        }

        /*[Description("Планируется использовать если в классе нужен не весь класс, а лишь его часть или если используется DataSet")]
        public string DisplayMember { get; set; }*/                      
    }
}
