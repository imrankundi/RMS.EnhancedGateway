namespace RMS.AWS.ManualIngester
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
            this.btnExecuteQuery = new System.Windows.Forms.Button();
            this.txtQuery = new System.Windows.Forms.RichTextBox();
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.lblPushedPacketCount = new System.Windows.Forms.Label();
            this.btnPushPackets = new System.Windows.Forms.Button();
            this.lblServerInfoFile = new System.Windows.Forms.Label();
            this.btnLoadServerInfo = new System.Windows.Forms.Button();
            this.lblSelectedDatabaseFile = new System.Windows.Forms.Label();
            this.btnLoadParsedPackets = new System.Windows.Forms.Button();
            this.txtMapping = new System.Windows.Forms.TextBox();
            this.txtOffsetHour = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExecuteQuery
            // 
            this.btnExecuteQuery.Location = new System.Drawing.Point(12, 244);
            this.btnExecuteQuery.Name = "btnExecuteQuery";
            this.btnExecuteQuery.Size = new System.Drawing.Size(140, 40);
            this.btnExecuteQuery.TabIndex = 17;
            this.btnExecuteQuery.Text = "Execute Query";
            this.btnExecuteQuery.UseVisualStyleBackColor = true;
            this.btnExecuteQuery.Click += new System.EventHandler(this.btnExecuteQuery_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(12, 111);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(898, 127);
            this.txtQuery.TabIndex = 16;
            this.txtQuery.Text = "";
            // 
            // dgvResult
            // 
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Location = new System.Drawing.Point(12, 300);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.Size = new System.Drawing.Size(898, 159);
            this.dgvResult.TabIndex = 15;
            // 
            // lblPushedPacketCount
            // 
            this.lblPushedPacketCount.AutoSize = true;
            this.lblPushedPacketCount.Location = new System.Drawing.Point(158, 493);
            this.lblPushedPacketCount.Name = "lblPushedPacketCount";
            this.lblPushedPacketCount.Size = new System.Drawing.Size(13, 13);
            this.lblPushedPacketCount.TabIndex = 14;
            this.lblPushedPacketCount.Text = "0";
            // 
            // btnPushPackets
            // 
            this.btnPushPackets.Location = new System.Drawing.Point(12, 479);
            this.btnPushPackets.Name = "btnPushPackets";
            this.btnPushPackets.Size = new System.Drawing.Size(140, 40);
            this.btnPushPackets.TabIndex = 13;
            this.btnPushPackets.Text = "Push Parsed Packets";
            this.btnPushPackets.UseVisualStyleBackColor = true;
            this.btnPushPackets.Click += new System.EventHandler(this.btnPushPackets_Click);
            // 
            // lblServerInfoFile
            // 
            this.lblServerInfoFile.AutoSize = true;
            this.lblServerInfoFile.Location = new System.Drawing.Point(156, 35);
            this.lblServerInfoFile.Name = "lblServerInfoFile";
            this.lblServerInfoFile.Size = new System.Drawing.Size(85, 13);
            this.lblServerInfoFile.TabIndex = 12;
            this.lblServerInfoFile.Text = "No File Selected";
            // 
            // btnLoadServerInfo
            // 
            this.btnLoadServerInfo.Location = new System.Drawing.Point(12, 21);
            this.btnLoadServerInfo.Name = "btnLoadServerInfo";
            this.btnLoadServerInfo.Size = new System.Drawing.Size(140, 40);
            this.btnLoadServerInfo.TabIndex = 11;
            this.btnLoadServerInfo.Text = "Load Server Info";
            this.btnLoadServerInfo.UseVisualStyleBackColor = true;
            this.btnLoadServerInfo.Click += new System.EventHandler(this.btnLoadServerInfo_Click);
            // 
            // lblSelectedDatabaseFile
            // 
            this.lblSelectedDatabaseFile.AutoSize = true;
            this.lblSelectedDatabaseFile.Location = new System.Drawing.Point(156, 79);
            this.lblSelectedDatabaseFile.Name = "lblSelectedDatabaseFile";
            this.lblSelectedDatabaseFile.Size = new System.Drawing.Size(85, 13);
            this.lblSelectedDatabaseFile.TabIndex = 10;
            this.lblSelectedDatabaseFile.Text = "No File Selected";
            // 
            // btnLoadParsedPackets
            // 
            this.btnLoadParsedPackets.Location = new System.Drawing.Point(12, 65);
            this.btnLoadParsedPackets.Name = "btnLoadParsedPackets";
            this.btnLoadParsedPackets.Size = new System.Drawing.Size(140, 40);
            this.btnLoadParsedPackets.TabIndex = 9;
            this.btnLoadParsedPackets.Text = "Load Parsed Packets";
            this.btnLoadParsedPackets.UseVisualStyleBackColor = true;
            this.btnLoadParsedPackets.Click += new System.EventHandler(this.btnLoadParsedPackets_Click);
            // 
            // txtMapping
            // 
            this.txtMapping.Location = new System.Drawing.Point(158, 264);
            this.txtMapping.Name = "txtMapping";
            this.txtMapping.Size = new System.Drawing.Size(132, 20);
            this.txtMapping.TabIndex = 18;
            // 
            // txtOffsetHour
            // 
            this.txtOffsetHour.Location = new System.Drawing.Point(318, 264);
            this.txtOffsetHour.Name = "txtOffsetHour";
            this.txtOffsetHour.Size = new System.Drawing.Size(58, 20);
            this.txtOffsetHour.TabIndex = 19;
            this.txtOffsetHour.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(159, 245);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Mapping";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(315, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Hour Offset";
            // 
            // Ingester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 541);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOffsetHour);
            this.Controls.Add(this.txtMapping);
            this.Controls.Add(this.btnExecuteQuery);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.dgvResult);
            this.Controls.Add(this.lblPushedPacketCount);
            this.Controls.Add(this.btnPushPackets);
            this.Controls.Add(this.lblServerInfoFile);
            this.Controls.Add(this.btnLoadServerInfo);
            this.Controls.Add(this.lblSelectedDatabaseFile);
            this.Controls.Add(this.btnLoadParsedPackets);
            this.Name = "Ingester";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecuteQuery;
        private System.Windows.Forms.RichTextBox txtQuery;
        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.Label lblPushedPacketCount;
        private System.Windows.Forms.Button btnPushPackets;
        private System.Windows.Forms.Label lblServerInfoFile;
        private System.Windows.Forms.Button btnLoadServerInfo;
        private System.Windows.Forms.Label lblSelectedDatabaseFile;
        private System.Windows.Forms.Button btnLoadParsedPackets;
        private System.Windows.Forms.TextBox txtMapping;
        private System.Windows.Forms.TextBox txtOffsetHour;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

