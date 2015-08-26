using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using anvlib.Interfaces;

namespace anvlib.Classes
{
    /// <summary>
    /// Вспомогательный класс для работы с изменением размеров и местоположениями форм и сохранением их в конфиг.
    /// Класс можно вызывать только после инициализации компонентов формы и не в коем случае вызывать перед инициализацией
    /// иначе форма будет по координатам 0,0
    /// </summary>
    public class ResizeFormSettingsManager
    {        
        private IFormsSettingsForSettings _settings;
        private System.Windows.Forms.Form _form;
        private bool _saveMinimizedState = false;

        public bool SaveMinimizedState
        {
            get { return _saveMinimizedState; }
            set { _saveMinimizedState = value; }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Settings">Специальный интерфейс для форм, чтобы их можно было сохранять</param>
        /// <param name="Form">Форма, которую мы будем сохранять/загружать</param>
        public ResizeFormSettingsManager(IFormsSettingsForSettings Settings, System.Windows.Forms.Form Form)
        {            
            _settings = Settings;
            _form = Form;
        }

        /// <summary>
        /// Метод сохранения формы в конфиг
        /// </summary>
        /// <param name="FormName">Имя формы</param>
        /// <param name="StartPos">Позиция при создании формы</param>
        /// <param name="Location">Последнее местоположение</param>
        /// <param name="Size">Размер формы</param>
        /// <param name="WinState">Состояние окна(мин, макс, хайд)</param>
        public void SaveResizedForm(string FormName, System.Windows.Forms.FormStartPosition StartPos,
            System.Drawing.Point Location, System.Drawing.Size Size, System.Windows.Forms.FormWindowState WinState)
        {
            if ((_settings as IFormsSettingsForSettings) == null)
                return;

            if ((_settings as IFormsSettingsForSettings).Forms == null)
                (_settings as IFormsSettingsForSettings).Forms = new FormsSettings();

            FormProperties fSettings = (_settings as IFormsSettingsForSettings).Forms.FindFormByName(FormName);
            bool add = false;
            if (fSettings == null)
            {
                fSettings = new FormProperties();
                add = true;
            }
            fSettings.FormName = FormName;
            fSettings.StartPosition = StartPos;
            fSettings.FormLocation = Location;
            fSettings.FormSize = Size;
            fSettings.FormState = WinState;
            if (add)
                (_settings as IFormsSettingsForSettings).Forms.Add(fSettings);
            Save();
        }

        /// <summary>
        /// Метод сохранение состояния окна в конфиг
        /// </summary>
        /// <param name="FormName">Имя формы</param>
        /// <param name="State">Состояние окна(мин, макс, хайд)</param>
        public void SaveFormWindowState(string FormName, System.Windows.Forms.FormWindowState State)
        {
            if (!(_settings is IFormsSettingsForSettings))
                return;

            if ((_settings as IFormsSettingsForSettings).Forms == null)
                (_settings as IFormsSettingsForSettings).Forms = new FormsSettings();

            FormProperties fSettings = (_settings as IFormsSettingsForSettings).Forms.FindFormByName(FormName);
            bool add = false;
            if (fSettings == null)
            {
                fSettings = new FormProperties();
                fSettings.FormName = FormName;
                add = true;
            }            

            fSettings.FormState = State;
            if (add)
                (_settings as IFormsSettingsForSettings).Forms.Add(fSettings);
            Save();
        }

        /// <summary>
        /// Оберточный метод сохранения
        /// </summary>
        private void Save()
        {
            if (_settings != null)
                _settings.Save();
        }

        /// <summary>
        /// Метод загрузки сохраненных параметров формы
        /// </summary>
        /// <param name="form">Форма, в которую мы будем загружать наши параметры</param>
        public void LoadFormSettings(System.Windows.Forms.Form form)
        {
            if (!(_settings is IFormsSettingsForSettings))
                return;

            if ((_settings as IFormsSettingsForSettings).Forms != null)
            {
                if (form != null)
                {
                    FormProperties fSettings = (_settings as IFormsSettingsForSettings).Forms.FindFormByName(form.Name);
                    if (fSettings != null)
                    {
                        form.Resize -= OnResize;
                        form.ResizeEnd -= OnResizeEnd;
                        form.StartPosition = fSettings.StartPosition;
                        form.Location = fSettings.FormLocation;
                        form.WindowState = fSettings.FormState;                        
                        form.Size = fSettings.FormSize;
                        form.Resize += OnResize;
                        form.ResizeEnd += OnResizeEnd;
                    }
                }
            }
        }

        /// <summary>
        /// Хендлер для местоположения и ресайза
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnResizeEnd(object sender, EventArgs e)
        {
            if (_form.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                if (_saveMinimizedState)
                    SaveResizedForm(_form.Name, _form.StartPosition, _form.Location, _form.Size, _form.WindowState);
            }
            else
                SaveResizedForm(_form.Name, _form.StartPosition, _form.Location, _form.Size, _form.WindowState);
        }

        /// <summary>
        /// Хэндлер для ресайза
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnResize(object sender, EventArgs e)
        {
            if (_form.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                if (_saveMinimizedState)
                    SaveFormWindowState(_form.Name, _form.WindowState);
            }
            else
                SaveFormWindowState(_form.Name, _form.WindowState);
        }
    }
}
