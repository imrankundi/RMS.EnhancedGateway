using RMS.Simulator.Logic;
using System;
using System.Windows.Forms;

namespace RMS.Simulator
{
    public partial class Main : Form, IRequestListener
    {
        public static WebServer Server;
        public Main()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            Server = new WebServer();
            Server.RequestListener = this;
        }


        private void Main_Load(object sender, EventArgs e)
        {
            Server.Start();
        }

        public void LogMessage(object request)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object>(LogMessage), new object[] { request });
                return;
            }
            var msg = string.Format("[{0}]: {1}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), request);
            txtServerLog.AppendText(msg);
            txtServerLog.Focus();
        }

        public Logic.IRequestHandler GetRequestHandler()
        {
            return Server?.RequestHandler;
        }

        public void NotifyRequest(object request)
        {
            LogMessage(request);
        }

        
        private void UpdateTextBox(RichTextBox richTextBox, string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<RichTextBox, string>(UpdateTextBox), new object[] { richTextBox, text });
                return;
            }
            richTextBox.Text = text;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.Stop();
            Environment.Exit(Environment.ExitCode);
        }

        private void btnScanQr_Click(object sender, EventArgs e)
        {
        }

        
        private void chkShowDecodedQr_CheckedChanged(object sender, EventArgs e)
        {
           
        }


      
        private void UpdateLabel(Label label, string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Label, string>(UpdateLabel), new object[] { label, msg });
                return;
            }
            label.Text = msg;
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
        }

        private void btnStartSession_Click(object sender, EventArgs e)
        {
        }

        private void btnCreateReceiptData_Click(object sender, EventArgs e)
        {
        
        }

        private void btnEndSession_Click(object sender, EventArgs e)
        {
        }

        private void btnTransactionEndedState_Click(object sender, EventArgs e)
        {
        }

        private void btnUpdateTransactionStatus_Click(object sender, EventArgs e)
        {
        }

        
    }
}
