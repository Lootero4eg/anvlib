// Type: System.Windows.Forms.Design.EditorServiceContext
// Assembly: System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: C1FDF7EE-5D71-4DDC-A656-8AC6988A7728
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Design.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Runtime;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace anvlib.Controls.Designers.Utils
{
    internal class EditorServiceContext : IWindowsFormsEditorService, ITypeDescriptorContext, IServiceProvider
    {
        private ComponentDesigner _designer;
        private IComponentChangeService _componentChangeSvc;
        private PropertyDescriptor _targetProperty;

        private IComponentChangeService ChangeService
        {
            get
            {
                if (this._componentChangeSvc == null)
                    this._componentChangeSvc = (IComponentChangeService)(this as IServiceProvider).GetService(typeof(IComponentChangeService));
                return this._componentChangeSvc;
            }
        }

        IContainer ITypeDescriptorContext.Container
        {
            get
            {
                if (this._designer.Component.Site != null)
                    return this._designer.Component.Site.Container;
                else
                    return (IContainer)null;
            }
        }

        object ITypeDescriptorContext.Instance
        {
            get
            {
                return (object)this._designer.Component;
            }
        }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
        {
            get
            {
                return this._targetProperty;
            }
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal EditorServiceContext(ComponentDesigner designer)
        {
            this._designer = designer;
        }

        internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
        {
            this._designer = designer;
            this._targetProperty = prop;
            if (prop != null)
                return;
            prop = TypeDescriptor.GetDefaultProperty((object)designer.Component);
            if (prop == null || !typeof(ICollection).IsAssignableFrom(prop.PropertyType))
                return;
            this._targetProperty = prop;
        }

        internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop, string newVerbText)
            : this(designer, prop)
        {
            this._designer.Verbs.Add(new DesignerVerb(newVerbText, new EventHandler(this.OnEditItems)));
        }

        public static object EditValue(ComponentDesigner designer, object objectToChange, string propName)
        {
            PropertyDescriptor prop = TypeDescriptor.GetProperties(objectToChange)[propName];
            EditorServiceContext editorServiceContext = new EditorServiceContext(designer, prop);
            UITypeEditor uiTypeEditor = prop.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
            object obj1 = prop.GetValue(objectToChange);
            object obj2 = uiTypeEditor.EditValue((ITypeDescriptorContext)editorServiceContext, (IServiceProvider)editorServiceContext, obj1);
            if (obj2 != obj1)
            {
                try
                {
                    prop.SetValue(objectToChange, obj2);
                }
                catch (CheckoutException ex)
                {
                }
            }
            return obj2;
        }

        void ITypeDescriptorContext.OnComponentChanged()
        {
            this.ChangeService.OnComponentChanged((object)this._designer.Component, (MemberDescriptor)this._targetProperty, (object)null, (object)null);
        }

        bool ITypeDescriptorContext.OnComponentChanging()
        {
            try
            {
                this.ChangeService.OnComponentChanging((object)this._designer.Component, (MemberDescriptor)this._targetProperty);
            }
            catch (CheckoutException ex)
            {
                if (ex == CheckoutException.Canceled)
                    return false;
                throw;
            }
            return true;
        }

        object IServiceProvider.GetService(System.Type serviceType)
        {
            if (serviceType == typeof(ITypeDescriptorContext) || serviceType == typeof(IWindowsFormsEditorService))
                return (object)this;
            if (this._designer.Component != null && this._designer.Component.Site != null)
                return this._designer.Component.Site.GetService(serviceType);
            else
                return (object)null;
        }

        void IWindowsFormsEditorService.CloseDropDown()
        {
        }

        void IWindowsFormsEditorService.DropDownControl(Control control)
        {
        }

        DialogResult IWindowsFormsEditorService.ShowDialog(Form dialog)
        {
            IUIService uiService = (IUIService)(this as IServiceProvider).GetService(typeof(IUIService));
            if (uiService != null)
                return uiService.ShowDialog(dialog);
            else
                return dialog.ShowDialog(this._designer.Component as IWin32Window);
        }

        private void OnEditItems(object sender, EventArgs e)
        {
            object component = this._targetProperty.GetValue((object)this._designer.Component);
            if (component == null)
                return;
            CollectionEditor collectionEditor = TypeDescriptor.GetEditor(component, typeof(UITypeEditor)) as CollectionEditor;
            if (collectionEditor == null)
                return;
            collectionEditor.EditValue((ITypeDescriptorContext)this, (IServiceProvider)this, component);
        }
    }
}
