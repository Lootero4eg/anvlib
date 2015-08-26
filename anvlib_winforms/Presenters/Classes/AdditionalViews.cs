using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace anvlib.Presenters.Classes
{       
    /// <summary>
    /// Коллекция доп. видов для презентеров
    /// </summary>
    public class AdditionalViews : Collection<object>
    {
        public Collection<object> GetViewsByType(Type search_type)
        {
            Collection<object> res = new Collection<object>();

            foreach (var item in this.Items)
            {
                if (item.GetType() == search_type)
                    res.Add(item);
            }

            return res;
        }

        public object GetViewByName(string search_name, bool case_sensivity)
        {
            //object res;

            foreach (var item in this.Items)
            {
                if (item is Control)
                {
                    if (case_sensivity)
                    {
                        if ((item as Control).Name == search_name)
                            return item;
                    }
                    else
                    {
                        if ((item as Control).Name.ToLower() == search_name.ToLower())
                            return item;
                    }
                }
            }

            return null;
        }

        public object GetViewByName(string search_name)
        {
            return GetViewByName(search_name, false);
        }
    }
}
