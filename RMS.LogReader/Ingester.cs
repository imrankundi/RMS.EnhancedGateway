using Newtonsoft.Json;
using RMS.Component.DataAccess.SQLite.Repositories;
using RMS.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RMS.AWS.ManualIngester
{
    public partial class Ingester : Form
    {
        public Ingester()
        {
            InitializeComponent();
            btnPushPackets.Enabled = false;
            btnLoadParsedPackets.Enabled = true;
            btnPushPackets.Enabled = false;
            btnExecuteQuery.Enabled = false;
        }

        private void btnLoadParsedPackets_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "SQLite Database|*.sqlite|All Files|*.*";
                var result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    lblSelectedDatabaseFile.Text = ofd.FileName;
                    btnExecuteQuery.Enabled = true;
                }
                else
                {
                    lblSelectedDatabaseFile.Text = "No File Selected.";
                }
            }
            catch (Exception ex)
            {

                ShowMessage(ex);
            }
        }

        private void ShowMessage(Exception ex)
        {
            MessageBox.Show(ex?.Message);
        }

        private ServerInfo serverInfo;
        AWS4Client client;
        private IEnumerable<ReonParsedPacket> packets;
        private void btnLoadServerInfo_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "JSON File|*.json";
                var result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    lblServerInfoFile.Text = ofd.FileName;
                    var text = File.ReadAllText(ofd.FileName);
                    serverInfo = JsonConvert.DeserializeObject<ServerInfo>(text);
                    client = new AWS4Client(serverInfo);
                    btnLoadParsedPackets.Enabled = true;
                }
                else
                {
                    lblServerInfoFile.Text = "No File Selected";
                }


            }
            catch (Exception ex)
            {

                ShowMessage(ex);
            }
        }

        private void Push()
        {
            int count = 0;
            try
            {
                foreach (var packet in packets)
                {
                    try
                    {
                        var datetime = (DateTime)packet.Data[0]["Timestamp"];
                        datetime = datetime.AddHours(5);
                        packet.Data[0]["Timestamp"] = datetime;
                        packet.Mapping = txtMapping.Text;

                        string json = JsonConvert.SerializeObject(packet, Formatting.None);



                        var res = client.PostData(json);
                        
                        if (res)
                        {
                            count++;
                            UpdateText(count);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

                //ShowMessage(ex);
            }
        }
        int totalPackets = 0;
        private void btnPushPackets_Click(object sender, EventArgs e)
        {
            totalPackets = packets.ToList().Count;
            btnPushPackets.Enabled = false;
            foreach (var packet in packets)
            {
                var datetime = (DateTime)packet.Data[0]["Timestamp"];
                datetime = datetime.AddHours(5);
                packet.Data[0]["Timestamp"] = datetime;
                packet.Mapping = txtMapping.Text;
            }
            //Push();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JSON|*.json";
            var result = sfd.ShowDialog();
            
            if(result == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, JsonConvert.SerializeObject(packets, Formatting.Indented));
            }
        }

        private void UpdateText(int count)
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action<int>(UpdateText), new int[] { count });
                    return;
                }
                lblPushedPacketCount.Text = count.ToString();
                if (count == totalPackets)
                {
                    lblPushedPacketCount.Text = "[Completed...]";
                    btnPushPackets.Enabled = true;
                }

            }
            catch (Exception ex)
            {

            }

        }

        private void btnExecuteQuery_Click(object sender, EventArgs e)
        {
            LogRepository repo = new LogRepository(lblSelectedDatabaseFile.Text);
            var result = repo.ReadPushApiLog(txtQuery.Text);


            var filteredResult = result.Select(x => JsonConvert.DeserializeObject<ReonParsedPacket>(x.Request)).ToList();

            packets = filteredResult;
            dgvResult.DataSource = filteredResult;


            btnPushPackets.Enabled = true;

        }
    }
}
