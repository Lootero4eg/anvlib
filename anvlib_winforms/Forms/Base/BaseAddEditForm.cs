using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Interfaces;
using anvlib.Presenters;
using anvlib.Enums;

//--В дизайнере не работют формы с использованием шаблонных классов!!!
namespace anvlib.Forms.Base
{    
    public partial class BaseAddEditForm: Form, IAddEditCommonForm, ISetCaption
    {
        // редактируемый объект
        protected object _editableObject;

        /// <summary>
        /// Конкретный Презентер
        /// </summary>                
        protected BaseAddEditPresenter Presenter { get; set; }

        /// <summary>
        /// Редактируемый или новый объект
        /// </summary>
        public virtual object EditableItem 
        {
            get
            {                
                if (Presenter != null && _editableObject == null)
                    _editableObject = Presenter.ReadEditableItemFromDisplay();

                return _editableObject;
            }
            
            set
            {
                if (Presenter != null)
                    Presenter.SetItemForEditing(value);
                _editableObject = value;
            }
        }

        protected AddEditFormState _formState = AddEditFormState.None;

        /// <summary>
        /// Состояние формы. Добавление/Изменение или ничего
        /// </summary>
        public AddEditFormState FormState { get {return _formState;} }

        public virtual void SetFormState(AddEditFormState state)
        {
            _formState = state;
        }

        /// <summary>
        /// Метод установки заголовка формы
        /// </summary>
        /// <param name="caption"></param>
        public void SetCaption(string caption)
        {
            this.Text = caption;
        }

        public virtual void CreateNewItem()
        {
            CheckPresenter();
            _formState = AddEditFormState.Add;
            (Presenter as BaseAddEditPresenter).CreateNewItem();
        }

        /// <summary>
        /// Метод устанавливающий редактируемый объект
        /// </summary>
        /// <param name="editItem"></param>
        public virtual void SetItemForEditing(object editItem)
        {
            CheckPresenter();  
            EditableItem = editItem;
            _formState = AddEditFormState.Edit;
            //(Presenter as BaseAddEditPresenter).SetItemForEditing(editItem);
        }

        /// <summary>
        /// Дефолтный хендлер для кнопки ОК
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkB_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        //--Т.к. нету CreatePresenter, надо после инициализации формы создать конкретный презентер
        //--Presenter.DisplayEditableItem выводить надо в ОнЛоад форм               

        protected void CheckPresenter()
        {
            if (Presenter == null)
                throw new Exception("You must initialize Presenter first!");
        }
    }
}
