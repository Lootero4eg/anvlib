using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace anvlib.Enums
{
    [Flags]
    public enum TextBoxEventType 
    { 
        DigitsOnly, 
        FloatDigits,
        FloatDigitsCommaOnly,
        FloatDigitsDotOnly,
        DisableShiftInsert, 
        DisableShiftDelete,
        DisableControlInsert,
        DisableControlX,
        DisableControlC,
        DisableControlV,
        DisableCut,
        DisableCopy,
        DisablePaste,
        DisableEdit
    }
}
