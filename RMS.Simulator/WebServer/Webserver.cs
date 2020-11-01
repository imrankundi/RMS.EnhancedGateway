using Microsoft.Owin.Hosting;
using RMS.Simulator.Configuration;
using RMS.Simulator.Logic;
using RMS.Simulator.Requests;
using System;

namespace RMS.Simulator
{
    public class WebServer
    {
        private IDisposable _webapp;
        public RequestHandler RequestHandler { get; private set; }
        public TransactionAdviceManager TransactionAdviceManager { get; private set; }
        public IRequestListener RequestListener { get; set; }

        private SimulatorConfig configurations = null;

        public WebServer()
        {

        }
        public void Start()
        {
            configurations = SimulatorConfigManager.Instance.Configurations;

            _webapp = WebApp.Start<Startup>(configurations.Url);
            RequestHandler = new RequestHandler(RequestListener);
            TransactionAdviceManager = new TransactionAdviceManager(RequestListener);
        }

        public void Stop()
        {

            _webapp?.Dispose();
        }

    }
}
