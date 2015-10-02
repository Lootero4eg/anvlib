using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Interfaces;

namespace anvlib.Classes.Base
{
    public class BaseCommonStruct: IBaseCommonObject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public BaseCommonStruct()
        { 
        }

        public BaseCommonStruct(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
