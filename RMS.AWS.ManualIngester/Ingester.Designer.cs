﻿namespace RMS.AWS.ManualIngester
{
    partial class Ingester
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoadParsedPackets = new System.Windows.Forms.Button();
            this.lblParsedPacketFile = new System.Windows.Forms.Label();
            this.btnLoadServerInfo = new System.Windows.Forms.Button();
            this.lblServerInfoFile = new System.Windows.Forms.Label();
            this.btnPushPackets = new System.Windows.Forms.Button();
            this.lblPushedPacketCount = new System.Windows.Forms.Label();
            this.lblLoadSites = new System.Windows.Forms.Label();
            this.btnLoadSites = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadParsedPackets
            // 
            this.btnLoadParsedPackets.Location = new System.Drawing.Point(12, 119);
            this.btnLoadParsedPackets.Name = "btnLoadParsedPackets";
            this.btnLoadParsedPackets.Size = new System.Drawing.Size(140, 40);
            this.btnLoadParsedPackets.TabIndex = 0;
            this.btnLoadParsedPackets.Text = "Load Parsed Packets";
            this.btnLoadParsedPackets.UseVisualStyleBackColor = true;
            this.btnLoadParsedPackets.Click += new System.EventHandler(this.btnLoadParsedPackets_Click);
            // 
            // lblParsedPacketFile
            // 
            this.lblParsedPacketFile.AutoSize = true;
            this.lblParsedPacketFile.Location = new System.Drawing.Point(156, 133);
            this.lblParsedPacketFile.Name = "lblParsedPacketFile";
            this.lblParsedPacketFile.Size = new System.Drawing.Size(85, 13);
            this.lblParsedPacketFile.TabIndex = 1;
            this.lblParsedPacketFile.Text = "No File Selected";
            // 
            // btnLoadServerInfo
            // 
            this.btnLoadServerInfo.Location = new System.Drawing.Point(12, 12);
            this.btnLoadServerInfo.Name = "btnLoadServerInfo";
            this.btnLoadServerInfo.Size = new System.Drawing.Size(140, 40);
            this.btnLoadServerInfo.TabIndex = 2;
            this.btnLoadServerInfo.Text = "Load Server Info";
            this.btnLoadServerInfo.UseVisualStyleBackColor = true;
            this.btnLoadServerInfo.Click += new System.EventHandler(this.btnLoadServerInfo_Click);
            // 
            // lblServerInfoFile
            // 
            this.lblServerInfoFile.AutoSize = true;
            this.lblServerInfoFile.Location = new System.Drawing.Point(156, 26);
            this.lblServerInfoFile.Name = "lblServerInfoFile";
            this.lblServerInfoFile.Size = new System.Drawing.Size(85, 13);
            this.lblServerInfoFile.TabIndex = 3;
            this.lblServerInfoFile.Text = "No File Selected";
            // 
            // btnPushPackets
            // 
            this.btnPushPackets.Location = new System.Drawing.Point(12, 165);
            this.btnPushPackets.Name = "btnPushPackets";
            this.btnPushPackets.Size = new System.Drawing.Size(140, 40);
            this.btnPushPackets.TabIndex = 4;
            this.btnPushPackets.Text = "Push Parsed Packets";
            this.btnPushPackets.UseVisualStyleBackColor = true;
            this.btnPushPackets.Click += new System.EventHandler(this.btnPushPackets_Click);
            // 
            // lblPushedPacketCount
            // 
            this.lblPushedPacketCount.AutoSize = true;
            this.lblPushedPacketCount.Location = new System.Drawing.Point(158, 179);
            this.lblPushedPacketCount.Name = "lblPushedPacketCount";
            this.lblPushedPacketCount.Size = new System.Drawing.Size(13, 13);
            this.lblPushedPacketCount.TabIndex = 5;
            this.lblPushedPacketCount.Text = "0";
            // 
            // lblLoadSites
            // 
            this.lblLoadSites.AutoSize = true;
            this.lblLoadSites.Location = new System.Drawing.Point(156, 72);
            this.lblLoadSites.Name = "lblLoadSites";
            this.lblLoadSites.Size = new System.Drawing.Size(85, 13);
            this.lblLoadSites.TabIndex = 7;
            this.lblLoadSites.Text = "No File Selected";
            // 
            // btnLoadSites
            // 
            this.btnLoadSites.Location = new System.Drawing.Point(12, 58);
            this.btnLoadSites.Name = "btnLoadSites";
            this.btnLoadSites.Size = new System.Drawing.Size(140, 40);
            this.btnLoadSites.TabIndex = 6;
            this.btnLoadSites.Text = "Load Sites";
            this.btnLoadSites.UseVisualStyleBackColor = true;
            this.btnLoadSites.Click += new System.EventHandler(this.btnLoadSites_Click);
            // 
            // Ingester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 541);
            this.Controls.Add(this.lblLoadSites);
            this.Controls.Add(this.btnLoadSites);
            this.Controls.Add(this.lblPushedPacketCount);
            this.Controls.Add(this.btnPushPackets);
            this.Controls.Add(this.lblServerInfoFile);
            this.Controls.Add(this.btnLoadServerInfo);
            this.Controls.Add(this.lblParsedPacketFile);
            this.Controls.Add(this.btnLoadParsedPackets);
            this.Name = "Ingester";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadParsedPackets;
        private System.Windows.Forms.Label lblParsedPacketFile;
        private System.Windows.Forms.Button btnLoadServerInfo;
        private System.Windows.Forms.Label lblServerInfoFile;
        private System.Windows.Forms.Button btnPushPackets;
        private System.Windows.Forms.Label lblPushedPacketCount;
        private System.Windows.Forms.Label lblLoadSites;
        private System.Windows.Forms.Button btnLoadSites;
    }
}

