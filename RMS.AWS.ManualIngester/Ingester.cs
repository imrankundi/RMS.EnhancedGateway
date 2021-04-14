using Newtonsoft.Json;
using RMS.Component.Common.Helpers;
using RMS.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TestConsoleApp;

namespace RMS.AWS.ManualIngester
{
    public partial class Ingester : Form
    {
        public Ingester()
        {
            InitializeComponent();
            btnPushPackets.Enabled = false;
            btnLoadParsedPackets.Enabled = true;
        }

        private void btnLoadParsedPackets_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Log File|*.log";
                var result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    lblParsedPacketFile.Text = ofd.FileName;
                    ReadReceivedPackets(ofd.FileName);
                    File.Move(ofd.FileName, ofd.FileName + ".working");
                    btnPushPackets.Enabled = true;
                }
                else
                {
                    lblParsedPacketFile.Text = "No File Selected";
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
        AwsSqsClient client;
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
                    client = new AwsSqsClient(serverInfo);
                    btnLoadSites.Enabled = true;
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
                        //var datetime = (DateTime)packet.Data[0]["Timestamp"];
                        //datetime = datetime.AddHours(5);
                        //packet.Data[0]["Timestamp"] = datetime;
                        var res = client.PostData(JsonConvert.SerializeObject(packet, Formatting.None));
                        if (res.Result)
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
            Push();
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
                }

            }
            catch (Exception ex)
            {

            }

        }


        static Dictionary<string, SiteInfo> dict = new Dictionary<string, SiteInfo>();

        public void ReadSitesCsv(string path)
        {
            dict.Clear();
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var str = line.Split(',');

                var siteId = str[1];
                var siteName = str[2];

                dict.Add(siteId, new SiteInfo
                {
                    Id = siteId,
                    Name = siteName
                });
                Console.WriteLine("Id: {0}, Name: {1}", siteId, siteName);
            }
        }
        List<ReceivedPacket> packets;
        private void ReadReceivedPackets(string path)
        {
            var text = File.ReadAllText(path);
            text = "[" + text.TrimEnd(',') + "]";
            var json = JsonConvert.DeserializeObject<IEnumerable<ReceivedPacket>>(text);
            packets = FilteredList(json);

        }
        private List<ReceivedPacket> FilteredList(IEnumerable<ReceivedPacket> json)
        {
            List<ReceivedPacket> list = new List<ReceivedPacket>();
            foreach (var packet in json)
            {
                if (dict.ContainsKey(packet.TerminalId))
                {
                    list.Add(packet);
                }
            }

            return list;
        }

        private void btnLoadSites_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "CSV File|*.csv";
                var result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    lblLoadSites.Text = ofd.FileName;
                    ReadSitesCsv(ofd.FileName);
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
    }
}
