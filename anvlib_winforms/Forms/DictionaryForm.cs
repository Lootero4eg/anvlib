using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Enums;
using anvlib.Interfaces;
using anvlib.Utilites;

namespace anvlib.Forms
{
    public partial class DictionaryForm : Form, IDictionaryForm
    {
        protected AddEditFormState _formState;
        protected object _editableItem;
        protected Dictionary<string, string> _controlsBindings;        

        public AddEditFormState FormState { get { return _formState; } }
        public object EditableItem { get { return _editableItem; } }
   
        public DictionaryForm()
        {
            InitializeComponent();
            _controlsBindings = new Dictionary<string, string>();
        }

        public void SetItemForEditing(object item)
        {
            _formState = AddEditFormState.Edit;
            _editableItem = item;
            CreateControls();
            DisplayEditableItem();
        }

        public void CreateNewItem(object item)
        {
            _formState = AddEditFormState.Add;
            _editableItem = item;
            CreateControls();
            DisplayEditableItem();
        }

        public void SetClassItemDataSource(string ItemName, object DataSource, string DisplayName)
        { 
        }

        private void CreateControls()
        {
            #region Fields Creation
            foreach (var item in _editableItem.GetType().GetProperties())
            {
                bool standard = false;
                bool generic = false;
                bool array = false;

                //--Standard type
                if (TypesHelper.IsStandardType(item.PropertyType))
                {
                    if (item.PropertyType != typeof(bool))
                    {
                        Label lb = new Label();
                        lb.Text = item.Name + ":";
                        lb.Top = (this.Controls.Count * lb.Height);
                        lb.Left = 2;
                        lb.AutoSize = true;
                        lb.SendToBack();
                        this.Controls.Add(lb);
                        TextBox tb = new TextBox();
                        tb.Top = (this.Controls.Count * tb.Height);
                        tb.Left = 2;
                        tb.Name = string.Format("{0}_{1}", item.Name, "TextBox");
                        this.Controls.Add(tb);
                        _controlsBindings.Add(tb.Name, item.Name);
                    }
                    standard = true;
                }

                #region Generic type
                if (item.PropertyType.IsGenericType)
                {                    
                    generic = true;
                }
                #endregion

                #region Array Type
                if (item.PropertyType.IsArray)
                {
                    Type eltype = item.PropertyType.GetElementType();
                    if (TypesHelper.IsStandardType(eltype))
                    {                        
                    }

                    if (eltype.IsGenericType)
                    {                     
                    }

                    array = true;
                }
                #endregion

                #region Class type
                if (!standard && !generic && !array)
                {                    
                }
                #endregion
            }
            #endregion

            #region Ok & Cancel Buttons Creation
            Button but = new Button();
            but.Text = "ОК";
            but.Top = (this.Controls.Count * but.Height);
            but.Left = 2;            
            this.Controls.Add(but);
            but = new Button();
            but.Text = "Отмена";
            but.Top = this.Controls[this.Controls.Count - 1].Top;
            but.Left = this.Controls[this.Controls.Count - 1].Width + 6;
            this.Controls.Add(but);
            #endregion
        }

        private void DisplayEditableItem()
        { 
        }
    }
}
