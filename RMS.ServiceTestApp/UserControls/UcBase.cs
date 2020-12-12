using RMS.Component.WebApi.Responses;
using RMS.Models;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace RMS.UserControls
{
    public partial class UcBase : UserControl, IResponseHandler
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public IMainForm MainWindow { get; set; }
        public UcBase()
        {
            ClassName = this.GetType().Name;
            MethodName = MethodBase.GetCurrentMethod().Name;
            InitializeComponent();
            InitializeProperties();
        }

        public virtual void SetDateTimePickerFormat(DateTimePicker control)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");

            control.Format = DateTimePickerFormat.Custom;
            control.CustomFormat = "MMM dd, yyyy";
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }
        public virtual void InitializeProperties()
        {

            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            var controls = this.Controls;
            foreach (Control control in controls)
            {
                control.Font = new Font("Calibri", 10.0f, FontStyle.Bold);
                control.ForeColor = Color.FromArgb(15, 39, 93);


            }

            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }

        public virtual void OnResponseReceived(BaseResponse response)
        {

        }

        public void PerformInternationalization()
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");

            var form = this as Control;
            var formResource = LanguageManager.Instance.GetFormResource(form.Name);

            if (formResource == null)
                return;

            var labels = form.GetAllControls(typeof(Label));
            var dropdowns = form.GetAllControls(typeof(ComboBox));
            var groupBoxes = form.GetAllControls(typeof(GroupBox));
            var buttons = form.GetAllControls(typeof(Button));

            var labelResource = formResource.GetLabels();
            foreach (var label in labels)
            {
                var labelText = labelResource.ContainsKey(label.Name) ? labelResource[label.Name].Text : label.Text;
                label.Text = labelText;
            }

            foreach (var button in buttons)
            {
                var labelText = labelResource.ContainsKey(button.Name) ? labelResource[button.Name].Text : button.Text;
                button.Text = labelText;
            }

            foreach (var groupBox in groupBoxes)
            {
                var labelText = labelResource.ContainsKey(groupBox.Name) ? labelResource[groupBox.Name].Text : groupBox.Text;
                groupBox.Text = labelText;
            }

            var listResource = formResource.GetLists();
            foreach (var dropdown in dropdowns)
            {
                var items = listResource.ContainsKey(dropdown.Name) ? listResource[dropdown.Name] : null;

                if (items != null)
                {
                    var cmb = (ComboBox)dropdown;
                    cmb.Items.Clear();

                    if (items.Items != null)
                    {
                        foreach (var item in items.Items)
                        {
                            cmb.Items.Add(item);
                        }
                    }

                    cmb.SelectedIndex = 0;

                }

            }

            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }

        //protected virtual void ShowMessageBox(string caption, string text, ResponseStatus status)
        //{
        //    MethodName = MethodBase.GetCurrentMethod().Name;
        //    //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
        //    if (InvokeRequired)
        //    {
        //        Invoke(new Action<string, string, ResponseStatus>(ShowMessageBox), caption, text, status);
        //        return;
        //    }

        //    MessageBoxIcon icon = MessageBoxIcon.Information;
        //    if (status == ResponseStatus.Error)
        //        icon = MessageBoxIcon.Error;
        //    MessageBox.Show(text, caption, MessageBoxButtons.OK, icon);

        //    //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");

        //}

        protected virtual void Fail(string text, string caption)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            if (InvokeRequired)
            {
                Invoke(new Action<string, string>(Fail), text, caption);
                return;
            }

            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);

            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }

    }
}
