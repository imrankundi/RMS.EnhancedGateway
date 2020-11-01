using System.ServiceProcess;

namespace RMS.Gateway
{

    partial class GatewayService : ServiceBase
    {
        private Gateway gateway = new Gateway();
        public GatewayService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            gateway.Start();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            gateway.Stop();
        }
    }
}
