using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using anvlib.Interfaces;

namespace anvlib.Classes
{
    /// <summary>
    /// Класс управляющий набором действий, которые в него добавили
    /// </summary>
    public class ActionManager
    {
        private static Collection<IAction> _actionList = new Collection<IAction>();

        public void AddAction(IAction action)
        {            
            foreach (var item in _actionList)
            {
                if (action.ActionId == item.ActionId || action.ActionName == item.ActionName)
                    throw new Exception("Action must be with unique id and name!");
            }
            
            if (action != null)
                _actionList.Add(action);            
        }

        public void RemoveAction(IAction action)
        {
            if (_actionList.Contains(action))
                _actionList.Remove(action);
        }

        public IAction GetActionById(int actionId)
        {
            foreach (var item in _actionList)
                if (item.ActionId == actionId)
                    return item;

            return null;
        }

        public IAction GetActionByName(string actionName)
        {
            foreach (var item in _actionList)
                if (item.ActionName == actionName)
                    return item;

            return null;
        }

        public void ExecuteActionById(int actionId)
        {
            IAction res = GetActionById(actionId);
            if (res != null)
                res.Execute();
        }

        public void ExecuteActionByName(string actionName)
        {
            IAction res = GetActionByName(actionName);
            if (res != null)
                res.Execute();
        }

        public void ExecuteActionById(int actionId, params object[] pars)
        {
            IAction res = GetActionById(actionId);
            if (res != null)
                res.ExecuteWithParams(pars);
        }

        public void ExecuteActionByName(string actionName, params object[] pars)
        {
            IAction res = GetActionByName(actionName);
            if (res != null)
                res.ExecuteWithParams(pars);
        }

        public object GetActionResultById(int actionId)
        {
            IAction res = GetActionById(actionId);
            if (res != null)
                return res.ActionResult;

            return null;
        }

        public object GetActionResultByName(string actionName)
        {
            IAction res = GetActionByName(actionName);
            if (res != null)
                return res.ActionResult;

            return null;
        }
    }
}
