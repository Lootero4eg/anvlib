using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using anvlib.Classes;
using anvlib.Enums;

namespace anvlib.Interfaces
{
    public interface IAddEditControl
    {
        event EventHandler SaveClick;
        event EventHandler CancelClick;

        object EditableObject { get; set; }

        AddEditFormState ControlState { get; set; }
        void ClearAll();
        void SetEditableItem(object edited_item);
    }
}
