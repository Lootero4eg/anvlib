using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

using anvlib.Base;
using anvlib.Interfaces;
using anvlib.Interfaces.Collections;

namespace anvlib.Base.Collections
{
    /// <summary>
    /// Базовая коллекция стандартных объектов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseCommonCollection<T> : Collection<T>, IBaseCommonCollection where T : BaseCommonObject, new()
    {
        private bool _isReadOnly = false;

        public void Add(BaseCommonObject item)
        {
            base.Add((T)item);
        }

        public bool Contains(BaseCommonObject item)
        {            
            return base.Contains((T)item);
        }

        public void CopyTo(BaseCommonObject[] items, int index)
        {
            base.CopyTo((T[])items, index);
        }

        public bool IsReadOnly { get { return _isReadOnly; } set { _isReadOnly = value; } }

        public bool Remove(BaseCommonObject item)
        {
            return base.Remove((T)item);
        }

        public new IEnumerator<BaseCommonObject> GetEnumerator()
        {            
            return Items.GetEnumerator();
        }
    }
}
