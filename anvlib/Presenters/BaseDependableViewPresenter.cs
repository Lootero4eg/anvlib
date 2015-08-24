using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using anvlib.Classes;

namespace anvlib.Presenters
{
    public abstract class BaseDependableViewPresenter<T>: BasePresenter
    {
        /// <summary>
        /// Зависимые друг от друга контролсы
        /// </summary>
        protected ViewsDependenceChain<T> view;

        /// <summary>
        /// Данные, которые будут выводиться на экран для основного вида
        /// </summary>
        protected object main_view_data;
        
        /// <summary>
        /// Данные, которые будут выводиться на экран для зависимого вида
        /// </summary>
        protected object depend_view_data;        

        /// <summary>
        /// Метод устанавливающий основной и зависимый
        /// </summary>
        /// <param name="MainView">Основной вид</param>
        /// <param name="DependableView">Зависимый вид от основного</param>
        public virtual void SetDependableView(T MainView, T DependableView)
        {
            if (MainView != null && DependableView != null)
            {
                view = new ViewsDependenceChain<T>();
                view.View = MainView;
                ViewsDependenceChain<T> dep_view = new ViewsDependenceChain<T>();
                dep_view.View = DependableView;
                dep_view.DependableViews = null;
                view.DependableViews.Add(dep_view);
            }
        }

        /// <summary>
        /// Метод установки следующих зависимых звеньев цепи
        /// </summary>
        /// <param name="MainView">Основной вид</param>
        /// <param name="DependableView">Зависимый вид от основного</param>        
        public virtual void AddNextLevelChainLink(T MainView, T DependableView)
        {
            if (view != null)
            {
                var dep_view = GetChainLinkByControl(view, MainView);
                if (dep_view == null)
                    throw new Exception("Wrong chain level!!!");
                ViewsDependenceChain<T> dep_chain = new ViewsDependenceChain<T>();
                dep_chain.View = DependableView;
                if (dep_view.DependableViews == null)
                    dep_view.DependableViews = new Collection<ViewsDependenceChain<T>>();
                dep_view.DependableViews.Add(dep_chain);
            }
        }

        /// <summary>
        /// Вспомогательный рекурсивный метод, для поиска нужного звена цепи и установки в него заисимого звена
        /// </summary>
        /// <param name="chainlink"></param>
        /// <param name="ctrl"></param>
        /// <returns></returns>
        private ViewsDependenceChain<T> GetChainLinkByControl(ViewsDependenceChain<T> chainlink, T ctrl)
        {
            if (chainlink != null)
            {
                if (chainlink.View as Control == ctrl as Control)
                    return chainlink;

                if (chainlink.DependableViews != null)
                {
                    ViewsDependenceChain<T> chain = new ViewsDependenceChain<T>();
                    foreach (var depchain in chainlink.DependableViews)
                    {
                        if (depchain.View as Control == ctrl as Control)
                            chain = depchain;
                        else
                            chain = GetChainLinkByControl(depchain, ctrl);

                        if (chain !=null && chain.View != null)
                            break;
                    }

                    return chain;
                }
            }

            return null;
        }

        /// <summary>
        /// Метод вызывающий делегата "изменения котролса"
        /// </summary>
        /// <param name="ctrl">Заданный контролс</param>
        public void ViewChangedTrigger(T ctrl)
        {
            var dep_view = GetChainLinkByControl(view, ctrl);
            if (dep_view != null)
            {
                if (dep_view.ViewChanged != null)
                    dep_view.ViewChanged(dep_view.View);
            }
        }

        /// <summary>
        /// Метод проверяющий наличие представления данных
        /// </summary>
        protected void CheckView()
        {
            if (view == null)
                throw new Exception("This presenter won't work without initialize Views!!!");
        }

        /// <summary>
        /// Метод вызывающий делегат у вида найденного под Контролсу
        /// </summary>
        /// <param name="ctrl"></param>
        protected virtual void FillView(T ctrl)
        {
            CheckView();
            var chain = GetChainLinkByControl(view, ctrl);
            if (chain != null)
            {
                if (chain.FillView != null)
                    chain.FillView(chain);
            }
        }

        /// <summary>
        /// Метод вызывающий делегат у вида найденного под Контролсу
        /// </summary>
        /// <param name="ctrl"></param>
        protected virtual void Clear(T ctrl)
        {
            CheckView();
            var chain = GetChainLinkByControl(view, ctrl);
            if (chain != null)
            {
                if (chain.Clear != null)
                    chain.Clear();
            }
        }

        /// <summary>
        /// Базовая инициализация презентера
        /// </summary>
        protected virtual void DoBaseInit()
        {
            CheckView();
            if (view != null)
            {
                view.FillView += TopLevelView_FillHandler;
                view.Clear += TopLevelView_ClearHandler;
                view.ViewChanged += TopLevelView_ChangedHandler;
                if (view.DependableViews != null)
                {
                    view.DependableViews[0].FillView += TopLevelDependableView_FillHandler;
                    view.DependableViews[0].Clear += TopLevelDependableView_ClearHandler;
                }
                Clear(view.View);
                FillView(view.View);
            }
        }

        /// <summary>
        /// Метод установки дополнительных параметров для заданного вида.
        /// </summary>
        /// <param name="View">Заданный вид</param>
        /// <param name="parameters">Набор абсолютно любых параметров</param>
        public virtual void SetAdditionalParametersForView(T View, params object[] parameters)
        {
            CheckView();
            if (view != null)
            {
                var dep_view = GetChainLinkByControl(view, View);
                if (dep_view == null)
                    throw new Exception("Wrong chain level!!!");
                dep_view.SetAdditionalParams(parameters);
            }
        }

        protected abstract void TopLevelView_FillHandler(object sender);
        protected abstract void TopLevelView_ClearHandler();

        protected abstract void TopLevelDependableView_FillHandler(object sender);
        protected abstract void TopLevelDependableView_ClearHandler();

        protected abstract void TopLevelView_ChangedHandler(object sender);

        /// <summary>
        /// Метод перерисовывающий основной вид
        /// </summary>
        public virtual void RefreshMainView()
        {
            CheckView();
            if (view != null)
            {
                if (view.Clear != null)
                    view.Clear();
                if (view.FillView != null)
                    view.FillView(view);
            }
        }
    }
}
