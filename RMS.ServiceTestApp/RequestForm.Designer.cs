namespace ServiceTestApp
{
    partial class RequestForm
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
            this.btnSend = new System.Windows.Forms.Button();
            this.txtCommand = new System.Windows.Forms.RichTextBox();
            this.txtResponse = new System.Windows.Forms.RichTextBox();
            this.txtTerminalId = new System.Windows.Forms.TextBox();
            this.lblTerminalId = new System.Windows.Forms.Label();
            this.lblCommand = new System.Windows.Forms.Label();
            this.lblResponse = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(225, 13);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(107, 29);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSendCommand_Click);
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(13, 78);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(506, 259);
            this.txtCommand.TabIndex = 1;
            this.txtCommand.Text = "";
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(525, 78);
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.Size = new System.Drawing.Size(502, 259);
            this.txtResponse.TabIndex = 2;
            this.txtResponse.Text = "";
            // 
            // txtTerminalId
            // 
            this.txtTerminalId.Location = new System.Drawing.Point(88, 17);
            this.txtTerminalId.Name = "txtTerminalId";
            this.txtTerminalId.Size = new System.Drawing.Size(131, 23);
            this.txtTerminalId.TabIndex = 3;
            // 
            // lblTerminalId
            // 
            this.lblTerminalId.AutoSize = true;
            this.lblTerminalId.Location = new System.Drawing.Point(12, 20);
            this.lblTerminalId.Name = "lblTerminalId";
            this.lblTerminalId.Size = new System.Drawing.Size(70, 15);
            this.lblTerminalId.TabIndex = 4;
            this.lblTerminalId.Text = "Terminal ID";
            // 
            // lblCommand
            // 
            this.lblCommand.AutoSize = true;
            this.lblCommand.Location = new System.Drawing.Point(12, 60);
            this.lblCommand.Name = "lblCommand";
            this.lblCommand.Size = new System.Drawing.Size(62, 15);
            this.lblCommand.TabIndex = 5;
            this.lblCommand.Text = "Command";
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(525, 60);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(59, 15);
            this.lblResponse.TabIndex = 6;
            this.lblResponse.Text = "Response";
            // 
            // RequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 349);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.lblCommand);
            this.Controls.Add(this.lblTerminalId);
            this.Controls.Add(this.txtTerminalId);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.btnSend);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "RequestForm";
            this.Text = "Terminal Command Sender";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox txtCommand;
        private System.Windows.Forms.RichTextBox txtResponse;
        private System.Windows.Forms.TextBox txtTerminalId;
        private System.Windows.Forms.Label lblTerminalId;
        private System.Windows.Forms.Label lblCommand;
        private System.Windows.Forms.Label lblResponse;
    }
}

