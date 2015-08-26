using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace anvlib.Controls
{
    public class AddEditContextMenuStrip: ContextMenuStrip
    {
        private ToolStripMenuItem _addTsmi;
        private ToolStripMenuItem _editTsmi;
        private ToolStripMenuItem _removeTsmi;
        private bool _isObjSelected = false;
        private bool _ifObjSelected_DisableAddItem = true;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="IfObjSelected_DisableAddItem"></param>
        public AddEditContextMenuStrip(bool IfObjSelected_DisableAddItem)
        {
            _ifObjSelected_DisableAddItem = IfObjSelected_DisableAddItem;            
        }

        /// <summary>
        /// Флаг указывающий на то, разрешать ли удаление и редактирование
        /// </summary>
        public bool IsObjectSelected
        {
            get { return _isObjSelected; }
            set { _isObjSelected = value; }
        }

        public void CreateAddMenuItem(string Caption, EventHandler ClickHandler)
        {
            _addTsmi = new ToolStripMenuItem(Caption);
            _addTsmi.Click += ClickHandler;
            this.Items.Add(_addTsmi);
        }

        public void CreateEditMenuItem(string Caption, EventHandler ClickHandler)
        {
            _editTsmi = new ToolStripMenuItem(Caption);
            _editTsmi.Click += ClickHandler;
            this.Items.Add(_editTsmi);
        }

        public void CreateRemoveMenuItem(string Caption, EventHandler ClickHandler)
        {
            _removeTsmi = new ToolStripMenuItem(Caption);
            _removeTsmi.Click += ClickHandler;
            this.Items.Add(_removeTsmi);
        }

        protected override void OnOpening(System.ComponentModel.CancelEventArgs e)
        {
            if (_isObjSelected)
            {
                if (_ifObjSelected_DisableAddItem)
                {
                    if (_addTsmi != null)
                        _addTsmi.Enabled = false;
                }
                else if (_addTsmi != null)
                        _addTsmi.Enabled = true;
                if (_editTsmi != null)
                    _editTsmi.Enabled = true;
                if (_removeTsmi != null)
                    _removeTsmi.Enabled = true;
            }
            else
            {
                if (_addTsmi != null)
                    _addTsmi.Enabled = true;
                //if(DisableEditIfNotSelected)
                if (_editTsmi != null)
                    _editTsmi.Enabled = false;
                if (_removeTsmi != null)
                    _removeTsmi.Enabled = false;
            }

            base.OnOpening(e);
        }
    }
}
