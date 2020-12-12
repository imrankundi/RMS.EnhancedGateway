using RMS.Models;
using RMS.UserControls;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace RMS
{
    public partial class MainWindow : Form, IMainForm
    {
        public string ClassName { get; private set; }
        public string MethodName { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            ClassName = nameof(MainWindow);
        }

        private void LoadLoginControl(UcBase sender)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                UcLogin control = new UcLogin();
                control.Dock = DockStyle.Fill;
                LoadChildControl(sender, control);
            }
            catch (Exception ex)
            {
                //AdminLogger.Instance.Log.Error(ClassName, MethodName, ex.ToString());
                MessageBox.Show(ex.Message);
            }
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }

        public void LoadChildControl(UcBase sender, UcBase control)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                control.MainWindow = this;
                control.Dock = DockStyle.Fill;
                MainPanel.Controls.Clear();
                MainPanel.Controls.Add(control);
                sender?.Dispose();
            }
            catch (Exception ex)
            {
                //AdminLogger.Instance.Log.Error(ClassName, MethodName, ex.ToString());
                MessageBox.Show(ex.Message);
            }
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }

        public void ResetChildControl()
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                MainPanel.Controls.Clear();
            }
            catch (Exception ex)
            {
                //AdminLogger.Instance.Log.Error(ClassName, MethodName, ex.ToString());
                MessageBox.Show(ex.Message);
            }
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }

        private void MainWindow_Load(object sender, System.EventArgs e)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            LoadLoginControl(null);
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }

        public void ShowLoginControl(UcBase sender)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                LoadLoginControl(sender);
                MainPanel.Controls.Remove(sender);
            }
            catch (Exception ex)
            {
                //AdminLogger.Instance.Log.Error(ClassName, MethodName, ex.ToString());
                MessageBox.Show(ex.Message);
            }
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }
    }
}
