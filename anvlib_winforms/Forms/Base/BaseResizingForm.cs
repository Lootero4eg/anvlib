using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Windows.Forms;
using anvlib.Classes;
using anvlib.Interfaces;

namespace anvlib.Forms.Base
{
    [DesignTimeVisible(false)]
    public class BaseResizingForm: Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private ResizeFormSettingsManager _resizeSettingsManager { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected IFormsSettingsForSettings _settings { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnLoad(EventArgs e)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Runtime)
            {
                try
                {
                    var classes = from z in this.GetType().Assembly.GetTypes()
                                  where (z.IsClass || z.IsInterface || z.IsEnum)
                                  && z.Namespace != null
                                  && z.Namespace.IndexOf(".Properties") > -1
                                  && z.Name == "Settings"
                                  select z;
                    var classes_items = classes.ToList();
                    if (classes_items.Count > 0)
                        _settings = (anvlib.Interfaces.IFormsSettingsForSettings)this.GetType().Assembly.GetType(classes_items[0].FullName).GetProperty("Default").GetValue(this, null);

                    _resizeSettingsManager = new ResizeFormSettingsManager(_settings, this);
                    this.ResizeEnd += _resizeSettingsManager.OnResizeEnd;
                    this.Resize += _resizeSettingsManager.OnResize;
                    _resizeSettingsManager.LoadFormSettings(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибки при инициализации маштабирумости окна!\r\nОригинальный текст: " + ex.Message,
                        "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (!this.IsDisposed)
                    base.OnLoad(e);
            }
        }
    }
}
