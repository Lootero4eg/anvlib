using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

using anvlib.Presenters;
using anvlib.Interfaces;
using anvlib.Enums;

namespace anvlib.Forms.Base
{
    public class BaseProfileForm: Form, IProfileForm
    {
        protected BaseProfilePresenter _presenter;
        private Control _profileDisplayControl;
        private IAddEditCommonForm _addEditForm;
        
        protected ContextMenuStrip _defaultContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CreateTSMI;
        private System.Windows.Forms.ToolStripMenuItem EditTSMI;
        private System.Windows.Forms.ToolStripMenuItem RemoveTSMI;

        //--Прикрутить протектед, чтобы из вне можно было настраивать
        private bool _defaultItemsLoader = true;
        private bool _isDefaultContextMenuEnabled = true;
        
        public BaseProfileForm()
        {
            if (_isDefaultContextMenuEnabled)
            {
                _defaultContextMenu = new ContextMenuStrip();
                CreateTSMI = new ToolStripMenuItem("Добавить");
                EditTSMI = new ToolStripMenuItem("Изменить");
                RemoveTSMI = new ToolStripMenuItem("Удалить");
                CreateTSMI.Click += AddButtonClick;
                EditTSMI.Click += EditButtonClick;
                RemoveTSMI.Click += RemoveButtonClick;
                _defaultContextMenu.Items.Add(CreateTSMI);
                _defaultContextMenu.Items.Add(EditTSMI);
                _defaultContextMenu.Items.Add(RemoveTSMI);
                _defaultContextMenu.Opening += DefaultContextMenuOpening;                
            }
        }

        [Browsable(false)]        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object SelectedProfile
        {
            get
            {
                var ctrltype = GetDisplayControlType();
                
                switch (ctrltype)
                { 
                    case PresenterEnums.ProfileDisplayControlType.ListBox:
                        var lbox = (ListBox)ProfileDisplayControl;
                        if (lbox != null)
                        {
                            if (lbox.SelectedIndex >= 0)
                                return lbox.Items[lbox.SelectedIndex];
                        }
                        break;

                    case PresenterEnums.ProfileDisplayControlType.ComboBox:
                        var cbox = (ComboBox)ProfileDisplayControl;
                        if (cbox != null)
                        {
                            if (cbox.SelectedIndex >= 0)
                                return cbox.Items[cbox.SelectedIndex];
                        }
                        break;
                    
                    case PresenterEnums.ProfileDisplayControlType.ListView:
                        var lview = (ListView)ProfileDisplayControl;
                        if (lview != null)
                        {
                            if (lview.SelectedIndices.Count >= 0)
                            {
                                return lview.SelectedItems[lview.SelectedIndices[0]];
                            }
                        }
                        break;
                }
                
                return null;                
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Control ProfileDisplayControl 
        {
            get
            {                                
                return _profileDisplayControl;
            }
            set
            {
                _profileDisplayControl = value;
                if (_isDefaultContextMenuEnabled && value !=null)
                    _profileDisplayControl.ContextMenuStrip = _defaultContextMenu;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IAddEditCommonForm AddEditProfileForm 
        {
            get { return _addEditForm; }
            set { _addEditForm = value; } 
        }

        public void CreatePresenter(BaseProfilePresenter presenter)
        {
            _presenter = presenter;                    
        }

        public void SetProfilesCollection(object ProfilesCollection)
        {
            _presenter.SetProfilesCollection(ProfilesCollection);                        
        }

        public void SetAddEditProfileForm(IAddEditCommonForm form)
        {
            if (_addEditForm != form)
                _addEditForm = form;
            _presenter.SetAddEditProfileForm(AddEditProfileForm);
        }

        private PresenterEnums.ProfileDisplayControlType GetDisplayControlType()
        {            
            if (_profileDisplayControl != null)
            {
                if (_profileDisplayControl is ListBox)
                    return PresenterEnums.ProfileDisplayControlType.ListBox;
                if (_profileDisplayControl is ListView)
                    return PresenterEnums.ProfileDisplayControlType.ListView;
                if (_profileDisplayControl is ComboBox)
                    return PresenterEnums.ProfileDisplayControlType.ComboBox;
            }

            return PresenterEnums.ProfileDisplayControlType.ListBox;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (_defaultItemsLoader)
            {
                ProfileFormLoad();
                base.OnLoad(e);
            }
        }

        //--Можно вызывать отдельно отдельно, если надо по каким то причинам
        protected void ProfileFormLoad()
        {
            if (_presenter != null)
                _presenter.FillProfiles(ProfileDisplayControl, GetDisplayControlType(), "Name");
        }

        protected void AddClick()
        {
            if (_presenter != null)
            {
                _presenter.AddProfileButtonClick();
                _presenter.FillProfiles(ProfileDisplayControl, GetDisplayControlType(), "Name");                
            }
        }

        protected void AddButtonClick(object sender, EventArgs e)
        {
            AddClick();
        }

        protected void EditClick()
        {
            if (_presenter != null)
            {
                _presenter.EditProfileButtonClick(SelectedProfile);
                _presenter.FillProfiles(ProfileDisplayControl, GetDisplayControlType(), "Name");
            }
        }

        protected void EditButtonClick(object sender, EventArgs e)
        {
            EditClick();
        }

        protected void RemoveClick()
        {
            if (_presenter != null)
            {
                _presenter.RemoveProfileButtonClick(SelectedProfile);
                _presenter.FillProfiles(ProfileDisplayControl, GetDisplayControlType(), "Name");
            }
        }

        protected void RemoveButtonClick(object sender, EventArgs e)
        {
            RemoveClick();
        }        

        private void DefaultContextMenuOpening(object sender, CancelEventArgs e)
        {
            if (SelectedProfile == null)
            {
                EditTSMI.Enabled = false;
                RemoveTSMI.Enabled = false;
            }
            else
            {
                EditTSMI.Enabled = true;
                RemoveTSMI.Enabled = true;
            }
        }

        protected void OKClick()
        {
            if (SelectedProfile != null)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        protected void OKButtonClick(object sender, EventArgs e)
        {
            OKClick();
        }

        protected void CancelClick()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        protected void CancelButtonClick(object sender, EventArgs e)
        {
            CancelClick();
        }

        protected void DisplayControlDoubleClick(object sender, EventArgs e)
        {
            OKClick();
        }

        protected void EnterKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                e.Handled = true;
                OKClick();
            }
            base.OnKeyPress(e);
        }
    }    
}
