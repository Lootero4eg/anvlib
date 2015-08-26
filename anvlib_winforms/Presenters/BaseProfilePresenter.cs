using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Classes;
using anvlib.Base;
using anvlib.Base.Collections;
using anvlib.Interfaces;
using anvlib.Interfaces.Collections;
using anvlib.Enums;

namespace anvlib.Presenters
{    
    /// <summary>
    /// Базовый класс для Профильных презентеров
    /// </summary>
    public abstract class BaseProfilePresenter
    {
        /// <summary>
        /// Профильная коллекция
        /// </summary>
        protected object _profilesCollection;

        /// <summary>
        /// Форма добавления и изменения профилей
        /// </summary>
        protected IAddEditCommonForm _addFrm;        

        /// <summary>
        /// Метод установки профильной коллекции
        /// </summary>
        /// <param name="ProfilesCollection"></param>
        public virtual void SetProfilesCollection(object ProfilesCollection)
        {
            _profilesCollection = ProfilesCollection;            
        }

        /// <summary>
        /// Метод установки формы редактора для профилей
        /// </summary>
        /// <param name="form"></param>
        public virtual void SetAddEditProfileForm(IAddEditCommonForm form)
        {
            _addFrm = form;
        }

        /// <summary>
        /// Метод заполняющий Представление данных - данными.
        /// Устарел, надо бы переделать на контролфиллер.
        /// Как только возникнет новая задача, переделаю.
        /// </summary>
        /// <param name="ProfilesList"></param>
        /// <param name="ControlType"></param>
        /// <param name="DisplayMember"></param>
        public virtual void FillProfiles(Control ProfilesList, PresenterEnums.ProfileDisplayControlType ControlType, string DisplayMember)
        {
            switch (ControlType)
            {
                case PresenterEnums.ProfileDisplayControlType.ListBox:
                    (ProfilesList as ListBox).DataSource = null;                    
                    (ProfilesList as ListBox).DataSource = _profilesCollection;
                    (ProfilesList as ListBox).DisplayMember = DisplayMember;                    
                    break;

                case PresenterEnums.ProfileDisplayControlType.ComboBox:
                    (ProfilesList as ListBox).DataSource = null;                    
                    (ProfilesList as ComboBox).DataSource = _profilesCollection;
                    (ProfilesList as ComboBox).DisplayMember = DisplayMember;
                    break;

                case PresenterEnums.ProfileDisplayControlType.ListView:
                    throw new NotImplementedException();
            } 
        }

        public abstract void AddProfileButtonClick();

        public abstract void EditProfileButtonClick(object Profile);

        public abstract void RemoveProfileButtonClick(object Profile);

        public abstract void SaveChanges();
    }
}
