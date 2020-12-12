using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace RMS.UserControls
{
    public partial class UcHeader : UcBase
    {
        public UcBase ParentControl { get; set; }
        public UcHeader()
        {
            ClassName = nameof(UcHeader);
            InitializeComponent();
            InitializeProperties();
            PerformInternationalization();
        }

        private void UcHeader_Load(object sender, EventArgs e)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                var path = string.Format(@"{0}\Resources\Logo.png", AppDomain.CurrentDomain.BaseDirectory);
                var logo = Image.FromFile(path);
                //lblUserName.Text = SessionManager.Instance.User.FullName;
                ImgLogo.Image = logo;
                ImgLogo.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                //AdminLogger.Instance.Log.Error(ClassName, MethodName, ex.ToString());
            }
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                MainWindow?.ShowLoginControl(ParentControl);
            }
            catch (Exception ex)
            {
                //AdminLogger.Instance.Log.Error(ClassName, MethodName, ex.ToString());
            }

            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }
    }
}
