using Newtonsoft.Json;
using RMS.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RMS.Component.Common.Helpers;

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
                ofd.Filter = "JSON File|*.json";
                var result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    lblParsedPacketFile.Text = ofd.FileName;
                    var text = File.ReadAllText(ofd.FileName);
                    text = "[" + text.TrimEnd(',') + "]";
                    packets = JsonConvert.DeserializeObject<IEnumerable<RMS.Component.DataAccess.SQLite.Entities.PushApiEntity>>(text);
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
        AWS4Client client;
        private IEnumerable<RMS.Component.DataAccess.SQLite.Entities.PushApiEntity> packets;
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
                        //var datetime = (DateTime)packet.Data[0]["Timestamp"];
                        //datetime = datetime.AddHours(5);
                        //packet.Data[0]["Timestamp"] = datetime;
                        var res = client.PostData(JsonConvert.SerializeObject(packet, Formatting.None));
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

        private void btnZip_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "JSON File|*.json";
                var result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    GunZip.Compress(ofd.FileName);
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

        private void btnUnzip_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "JSON File|*.gz";
                var result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    GunZip.Decompress(ofd.FileName);
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
