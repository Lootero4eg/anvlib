using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Interfaces
{
    public interface IAddEditDeleteButtonsEvents
    {
        void AddButtonClick(object sender, EventArgs e);
        void EditButtonClick(object sender, EventArgs e);
        void DeleteButtonClick(object sender, EventArgs e);
    }
}
