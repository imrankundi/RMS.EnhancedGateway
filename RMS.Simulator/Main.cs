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

        long count;
        public void LogMessage(object request)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<object>(LogMessage), new object[] { request });
                return;
            }

            if (count < long.MaxValue)
                count++;
            else
                count = 0;

            var msg = string.Format("Timestamp: {0}\nMessage Count: {1}\n{2}\n", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), count, request);
            txtServerLog.Text = msg;
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

    }
}
