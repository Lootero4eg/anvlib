using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace anvlib.Presenters.Classes
{
    /// <summary>
    /// Зависимые друг от друга представления данных
    /// </summary>
    public class ViewsDependenceChain<T>
    {
        /// <summary>
        /// Вид(он же контролс)
        /// </summary>
        public T View { get; set; }

        /// <summary>
        /// Делегат для функции срабатываемой после изменения главного вида.
        /// Привязывается в перезентере
        /// </summary>
        public ViewChanged ViewChanged { get; set; }

        /// <summary>
        /// Заивисмый вид от главного
        /// </summary>
        public Collection<ViewsDependenceChain<T>> DependableViews { get; set; }

        /// <summary>
        /// Массив дополнительных параметров View-а
        /// </summary>
        public object[] AdditionalParams { get; private set; }

        /// <summary>
        /// Метод устанавливающий дополнительные параметры
        /// </summary>
        /// <param name="Params"></param>
        public void SetAdditionalParams(params object[] Params)
        {
            if (Params != null && Params.Length > 0)
                AdditionalParams = Params;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ViewsDependenceChain()
        {
            DependableViews = new Collection<ViewsDependenceChain<T>>();
        }

        /// <summary>
        /// Делегат для функции заполняющей контрол, должна быть привязана в презентере
        /// </summary>
        public FillViewData FillView { get; set; }

        /// <summary>
        /// Делегат для функции очищающей контрол, должна быть привязана в презентере
        /// </summary>
        public ClearView Clear { get; set; }

        /// <summary>
        /// Определение возможности работы с DataSource у "Представления данных"
        /// </summary>
        public bool HasDataSourceProperty
        {
            get
            {
                return anvlib.Utilities.ObjectInspector.HasObjectDataSourceProperty(View);
            }
        }

        /// <summary>
        /// Определение возможности работы с DisplayMember у "Представления данных"
        /// </summary>
        public bool HasDisplayMemberProperty
        {
            get
            {
                return anvlib.Utilities.ObjectInspector.HasObjectDisplayMemberProperty(View);
            }
        }

        /// <summary>
        /// Метод позволяющий установить DataSource у объекта, если оно имеется
        /// </summary>
        /// <param name="datasource"></param>
        public void SetDataSource(object datasource)
        {
            if (View != null)
            {
                if (HasDataSourceProperty)
                    anvlib.Utilities.ObjectInspector.SetDataSource(View, datasource);
            }
        }

        /// <summary>
        /// Метод позволяющий установить DisplayMember у объекта, если оно имеется
        /// </summary>
        /// <param name="datasource"></param>
        public void SetDisplayMember(object displaymember)
        {
            if (View != null)
            {
                if (HasDisplayMemberProperty)
                    anvlib.Utilities.ObjectInspector.SetDisplayMember(View, displaymember);
            }
        }

#warning Отладить и дописать поиск по имени!!!!
        public Collection<object> GetDependableViewByType(ViewsDependenceChain<T> dep_chain, Type search_type)
        {
            Collection<object> res = new Collection<object>();
            if (DependableViews != null)
            {
                foreach (var item in dep_chain.DependableViews)
                {
                    if (item.View.GetType() == search_type)
                        res.Add(item.View);

                    if (item.DependableViews != null && item.DependableViews.Count > 0)
                    {
                        var subitems = item.GetDependableViewByType(item, search_type);
                        for (int i = 0; i < subitems.Count; i++)
                            res.Add(subitems[i]);
                    }
                }
            }

            return res;
        }
    }
}
