
using RMS.Component.RestHelper;
using RMS.Component.WebApi.Responses;
using RMS.Server.DataTypes.Requests;
using RMS.Server.DataTypes.Responses;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ServiceTestApp
{
    public partial class RequestForm : Form
    {
        public RequestForm()
        {
            InitializeComponent();
        }

        public void OnResponseReceived(BaseResponse response)
        {
            if (response != null)
                UpdateTextBox(txtResponse, response.ToFormattedJson());
            else
                UpdateTextBox(txtResponse, "NULL Response Received");
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

        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            UpdateTextBox(txtResponse, "Please Wait... (PerformTerminalCommandRequest)");
            TerminalCommandRequest request = new TerminalCommandRequest();
            request.RequestType = GatewayRequestType.TerminalCommand;
            request.TerminalId = txtTerminalId.Text;
            request.Data = txtCommand.Text;
            Task.Run(() => PerformTerminalCommandRequest(request));
        }

        public async Task PerformTerminalCommandRequest(TerminalCommandRequest request)
        {
            RestClientFactory factory = new RestClientFactory("RMS");
            var response = await factory.PostCallAsync<TerminalCommandResponse, TerminalCommandRequest>
                (factory.apiConfiguration.Apis["TerminalCommand"], request);

            OnResponseReceived(response);


        }

    }
}
