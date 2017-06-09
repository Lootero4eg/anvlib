using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using Microsoft.CSharp.RuntimeBinder;
using System.IO;

using anvlib.Interfaces;
using anvlib.Enums;

using EnvDTE80;

namespace anvlib.Controls.Dialogs
{
    internal struct AssemblyDllInfo
    {
        public string FileName { get; set; }
        public DateTime ModifyDate { get; set; }
    }    

    public partial class ClassChooserDialogForm : Form, IAddEditCommonForm, IEditableItem
    {
        private AddEditFormState _formState= AddEditFormState.None;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal List<AssemblyDllInfo> LoadedAssemblies { get; set; }

        internal class AssemblyInfo
        {
            public string AssemblyName { get; set; }
            public List<Namespace> Namespaces { get; set; }

            public AssemblyInfo()
            {
                Namespaces = new List<Namespace>();
            }
        }

        internal class Namespace
        {
            public string NamespaceName { get; set; }
            public List<Type> Classes { get; set; }

            public Namespace()
            {
                Classes = new List<Type>();
            }
        }

        internal class Assemblies : List<AssemblyInfo> { }           

        public object EditableItem { get; set; }

        public ClassChooserDialogForm()
        {
            InitializeComponent();
        }

        public void SetFormState(AddEditFormState formState)
        {
            _formState = formState;
        }

        public void SetItemForEditing(object editableItem)
        {
            EditableItem = editableItem;
        }
               
        private void ClassChooserDialogForm_Load(object sender, EventArgs e)
        {
            //--собираем все дерево, пока тут потом видно будет
            Assemblies asmTree = new Assemblies();
            List<AssemblyDllInfo> assemblies = new List<AssemblyDllInfo>();

            try
            {
                DTE2 dte2 = (DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.11.0");
                foreach (EnvDTE.Project item in dte2.Solution.Projects)
                {
                    string asm_fname = System.IO.Path.Combine(
                        item.Properties.Item("FullPath").Value.ToString(),
                        item.ConfigurationManager.ActiveConfiguration.Properties.Item("CodeAnalysisInputAssembly").Value.ToString());
                    
                    AssemblyDllInfo asdi = new AssemblyDllInfo();
                    asdi.FileName = asm_fname;
                    asdi.ModifyDate = File.GetLastWriteTime(asm_fname);
                    assemblies.Add(asdi);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }            

            foreach (var assembly in assemblies)
            {
                AssemblyInfo ai = new AssemblyInfo();

                if (Directory.Exists(System.IO.Path.GetDirectoryName(assembly.FileName)))
                {
                    if (File.Exists(assembly.FileName))
                    {
                        try
                        {
                            Assembly asm = null;
                            byte[] asmbytes = File.ReadAllBytes(assembly.FileName);
                            var CurDomAsm = AppDomain.CurrentDomain.GetAssemblies().LastOrDefault(a => a.FullName == AssemblyName.GetAssemblyName(assembly.FileName).FullName);
                            if (LoadedAssemblies.Count == 0)
                            {                                
                                LoadedAssemblies.Add(assembly);
                                asm = Assembly.Load(asmbytes);                                
                            }
                            else
                            {                                
                                bool found = LoadedAssemblies.Contains(assembly);
                                for (int i = 0; i < LoadedAssemblies.Count; i++)
                                {
                                    if (LoadedAssemblies[i].FileName == assembly.FileName && LoadedAssemblies[i].ModifyDate != assembly.ModifyDate)
                                    {
                                        LoadedAssemblies.Remove(LoadedAssemblies[i]);
                                        found = false;
                                        break;
                                    }
                                }                                

                                if (found)
                                {
                                    if (CurDomAsm != null)
                                        if (CurDomAsm is Assembly)
                                            asm = (CurDomAsm as Assembly);
                                }
                                else
                                {                                    
                                    LoadedAssemblies.Add(assembly);
                                    asm = Assembly.Load(asmbytes);
                                }
                            }

                            ai.AssemblyName = asm.FullName.Substring(0, asm.FullName.IndexOf(","));

                            foreach (var ns in anvlib.Utilities.AssemblyHelper.GetAssemblyNamespaces(asm))
                            {
                                Namespace nspace = new Namespace();
                                if (ai.AssemblyName != ns)
                                    nspace.NamespaceName = ns;
                                else
                                    nspace.NamespaceName = "<Root Namespace>";
                                nspace.Classes.AddRange(anvlib.Utilities.AssemblyHelper.GetAssemblyClasses(asm, ns));
                                if (nspace.Classes.Count > 0)
                                    ai.Namespaces.Add(nspace);
                            }
                            asmTree.Add(ai);
                        }
                        catch (Exception eb)
                        {
                            MessageBox.Show(eb.Message);
                        }
                    }
                }                
            }
            
            AssembliesTree.DataSource = asmTree;

            //--Настройка дополнительных параметров, в зависимости от того, что мы делаем на форме, добавляем или редактируем
            /*if (_formState == AddEditFormState.None || _formState == AddEditFormState.Add)
            {
                MessageBox.Show("Add");
            }
            else
            {
                MessageBox.Show("Edit");
            }*/
        }

        private void OkB_Click(object sender, EventArgs e)
        {
            if (AssembliesTree.SelectedNode.Tag is Type)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                EditableItem = AssembliesTree.SelectedNode.Tag;
                Close();
            }
        }

        private void AssembliesTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OkB.PerformClick();
        }
    }
}
