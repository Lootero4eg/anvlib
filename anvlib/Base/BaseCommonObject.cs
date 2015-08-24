using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Base
{
    /// <summary>
    /// Класс стандартного объекта 
    /// </summary>
    public class BaseCommonObject: ICloneable, IBaseCommonObject
    {
        protected int _id;
        protected string _name;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public BaseCommonObject() { }

        public BaseCommonObject(int id, string name)
        {
            Id = id;
            Name = name;
        } 

        public virtual object Clone()
        {
            BaseCommonObject obj = new BaseCommonObject(this.Id, this.Name);

            return obj;
        }
    }
}
