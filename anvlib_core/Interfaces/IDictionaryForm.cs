using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using anvlib.Enums;

namespace anvlib.Interfaces
{
    public interface IDictionaryForm
    {
        AddEditFormState FormState { get; }
        object EditableItem { get; }
        void SetItemForEditing(object item);
        void CreateNewItem(object item);
        void SetClassItemDataSource(string ItemName, object DataSource, string DisplayName);
    }
}
