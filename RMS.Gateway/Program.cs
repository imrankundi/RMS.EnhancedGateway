
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;

namespace RMS.Gateway
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        private static bool isService = false;
        [STAThread]
        static void Main()
        {
            if (!isService)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new GatewayForm());
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                        new GatewayService()
                };
                ServiceBase.Run(ServicesToRun);
            }

        }

        //[STAThread]
        //static void Main()
        //{
        //    ServiceBase[] ServicesToRun;
        //    ServicesToRun = new ServiceBase[]
        //    {
        //        new GatewayService()
        //    };
        //    ServiceBase.Run(ServicesToRun);
        //}
    }
}
