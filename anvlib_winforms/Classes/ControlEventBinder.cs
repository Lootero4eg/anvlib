using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using anvlib.Enums;
using anvlib.Controls.EventHandlers;

namespace anvlib.Classes
{
    public static class ControlEventBinder
    {
        public static void BindTextBoxEvent(Control ctrl, params TextBoxEventType[] Events)
        {
            for (int i = 0; i < Events.Length; i++)
            {
                switch (Events[i])
                {
                    case TextBoxEventType.DigitsOnly:
                        ctrl.KeyDown += TextBoxesEventHandlers.OnlyDigitsTextBox_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.FloatDigits:
                        ctrl.KeyDown += TextBoxesEventHandlers.FloatDataTextBox_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.FloatDigitsCommaOnly:
                        ctrl.KeyDown += TextBoxesEventHandlers.FloatDataCommaOnlyTextBox_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.FloatDigitsDotOnly:
                        ctrl.KeyDown += TextBoxesEventHandlers.FloatDataDotOnlyTextBox_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.DisableShiftDelete:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableShiftDelete_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.DisableShiftInsert:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableShiftInsert_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.DisableControlInsert:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlInsert_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.DisableControlX:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlX_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.DisableControlC:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlC_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.DisableControlV:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlV_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.DisableCut:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlX_KeyDownEventHandler;
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableShiftDelete_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.DisableCopy:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlC_KeyDownEventHandler;
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlInsert_KeyDownEventHandler;
                        break;
                    case TextBoxEventType.DisablePaste:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlV_KeyDownEventHandler;
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableShiftInsert_KeyDownEventHandler;
                        break;

                    case TextBoxEventType.DisableEdit:
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlX_KeyDownEventHandler;
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableShiftDelete_KeyDownEventHandler;
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlC_KeyDownEventHandler;
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlInsert_KeyDownEventHandler;                        
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableControlV_KeyDownEventHandler;
                        ctrl.KeyDown += TextBoxesEventHandlers.DisableShiftInsert_KeyDownEventHandler;
                        break;
                }
            }            
        }
    }
}
