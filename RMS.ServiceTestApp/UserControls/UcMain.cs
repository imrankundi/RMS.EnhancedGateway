using System;
using System.Reflection;
using System.Windows.Forms;

namespace RMS.UserControls
{
    public partial class UcMain : UcBase
    {
        public UcMain()
        {
            ClassName = nameof(UcMain);
            InitializeComponent();
            InitializeProperties();
            PerformInternationalization();
        }

        private void UcMain_Load(object sender, EventArgs e)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                Header.MainWindow = MainWindow;
                Header.ParentControl = this;
                TreeMenu.ExpandAll();
                TreeMenu.AfterSelect += TreeMenu_AfterSelect;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MAIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }

        private void TreeMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                if (e == null)
                    return;

                //if (e.Node.FullPath.Contains("Reports"))
                //{
                //    UcReportViewer uc = new UcReportViewer();
                //    uc.Dock = DockStyle.Fill;
                //    Enum.TryParse(e.Node.Tag.ToString(), out ReportType type);
                //    uc.ReportType = type;
                //    ChildControlPanel.Controls.Clear();
                //    ChildControlPanel.Controls.Add(uc);
                //}
                //else if (e.Node.Tag.Equals("User"))
                //{
                //    UcUser uc = new UcUser();
                //    uc.Dock = DockStyle.Fill;
                //    ChildControlPanel.Controls.Clear();
                //    ChildControlPanel.Controls.Add(uc);
                //}
                //else if (e.Node.Tag.Equals("ITM"))
                //{
                //    UcTerminal uc = new UcTerminal();
                //    uc.Dock = DockStyle.Fill;
                //    ChildControlPanel.Controls.Clear();
                //    ChildControlPanel.Controls.Add(uc);
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MAIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");

        }
    }
}
