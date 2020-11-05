using System;
using System.Windows.Forms;

namespace RMS.Gateway
{
    public partial class GatewayForm : Form, IContext
    {
        private Gateway gateway = new Gateway();
        private int lineCount = 0;

        public GatewayForm()
        {
            InitializeComponent();
            gateway.Context = this;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            gateway.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            gateway.Stop();
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            gateway.Restart();
        }

        public void TransferText(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(TransferText), new object[] { text });
                return;
            }
            txtReceivedData.AppendText(text);
            txtReceivedData.AppendText(Environment.NewLine);
            lineCount++;

            if (lineCount > 100)
            {
                txtReceivedData.Clear();
                lineCount = 0;

            }
        }

    }
}
