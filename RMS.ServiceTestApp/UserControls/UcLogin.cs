using RMS.Component.WebApi.Responses;
using System;
using System.Reflection;

namespace RMS.UserControls
{
    public partial class UcLogin : UcBase
    {
        public UcLogin()
        {
            ClassName = nameof(UcLogin);
            InitializeComponent();
            InitializeProperties();
            PerformInternationalization();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                btnLogin.Text = "Loading...";
                btnLogin.Enabled = false;
                //LoginModel model = new LoginModel
                //{
                //    UserName = txtUserName.Text,
                //    Password = txtPassword.Text
                //};
                //Task.Run(() => RequestManager.PerformLoginRequest(this, model));
            }
            catch (Exception ex)
            {
                //ShowMessageBox("LOGIN ERROR", ex.ToString(), ResponseStatus.Failed);
                //AdminLogger.Instance.Log.Error(ClassName, MethodName, ex.ToString());
                btnLogin.Text = "Login";
                btnLogin.Enabled = true;
            }
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");



        }
        private void GoToMain()
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            if (InvokeRequired)
            {
                Invoke(new Action(GoToMain));
                return;
            }

            UcMain control = new UcMain();
            control.MainWindow = MainWindow;
            MainWindow.LoadChildControl(this, control);
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }
        public override void OnResponseReceived(BaseResponse response)
        {
            MethodName = MethodBase.GetCurrentMethod().Name;
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "Start");
            try
            {
                //AdminLogger.Instance.Log.Debug(ClassName, MethodName, string.Format("Response:\n{0}", response.ToFormattedJson()));
                if (response.ResponseStatus == ResponseStatus.Success)
                {
                    //var resp = response as LoginResponse;
                    //if (resp.Authenticated)
                    //{
                    //    CrossThreadControlOperations.EnableButton(this, btnLogin, true);
                    //    SessionManager.Instance.User = resp.User;
                    //    SessionManager.Instance.SessionToken = resp.SessionToken;
                    //    GoToMain();
                    //}
                }
                else
                {
                    CrossThreadControlOperations.EnableButton(this, btnLogin, true);
                    CrossThreadControlOperations.ChangeButtonText(this, btnLogin, "Login");
                    //AdminLogger.Instance.Log.Error(ClassName, MethodName, response.Message);
                }
            }
            catch (Exception ex)
            {
                CrossThreadControlOperations.EnableButton(this, btnLogin, true);
                CrossThreadControlOperations.ChangeButtonText(this, btnLogin, "Login");
                //AdminLogger.Instance.Log.Error(ClassName, MethodName, ex.ToString());
            }
            //AdminLogger.Instance.Log.Verbose(ClassName, MethodName, "End");
        }
    }
}
