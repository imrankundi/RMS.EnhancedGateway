namespace RMS.Gateway
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GatewayServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.GatewayServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // GatewayServiceProcessInstaller
            // 
            this.GatewayServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.GatewayServiceProcessInstaller.Password = null;
            this.GatewayServiceProcessInstaller.Username = null;
            // 
            // GatewayServiceInstaller
            // 
            this.GatewayServiceInstaller.Description = "RMS 4 Gateway Service";
            this.GatewayServiceInstaller.DisplayName = "RMS 4 Gateway Service";
            this.GatewayServiceInstaller.ServiceName = "RMS 4 Gateway Service";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.GatewayServiceProcessInstaller,
            this.GatewayServiceInstaller});

        }

        #endregion
        public System.ServiceProcess.ServiceInstaller GatewayServiceInstaller;
        public System.ServiceProcess.ServiceProcessInstaller GatewayServiceProcessInstaller;
    }
}