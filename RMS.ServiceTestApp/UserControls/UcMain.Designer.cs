namespace RMS.UserControls
{
    partial class UcMain
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Manage ITM");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Manage User");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Cash Deposit");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Check Deposit");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Signature");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Transaction Reversal");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Withdrawal");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Reports", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7});
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.Header = new RMS.UserControls.UcHeader();
            this.FooterPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.TreeMenu = new System.Windows.Forms.TreeView();
            this.ChildControlPanel = new System.Windows.Forms.Panel();
            this.MainLayout.SuspendLayout();
            this.HeaderPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 1;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Controls.Add(this.HeaderPanel, 0, 0);
            this.MainLayout.Controls.Add(this.FooterPanel, 0, 2);
            this.MainLayout.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 3;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.MainLayout.Size = new System.Drawing.Size(1063, 628);
            this.MainLayout.TabIndex = 0;
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.Azure;
            this.HeaderPanel.Controls.Add(this.Header);
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderPanel.Location = new System.Drawing.Point(3, 3);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(1057, 94);
            this.HeaderPanel.TabIndex = 0;
            // 
            // Header
            // 
            this.Header.BackColor = System.Drawing.SystemColors.Control;
            this.Header.ClassName = "UcHeader";
            this.Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.MainWindow = null;
            this.Header.MethodName = "InitializeProperties";
            this.Header.Name = "Header";
            this.Header.ParentControl = null;
            this.Header.Size = new System.Drawing.Size(1057, 94);
            this.Header.TabIndex = 0;
            // 
            // FooterPanel
            // 
            this.FooterPanel.BackColor = System.Drawing.Color.Transparent;
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FooterPanel.Location = new System.Drawing.Point(3, 571);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.Size = new System.Drawing.Size(1057, 54);
            this.FooterPanel.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.TreeMenu, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ChildControlPanel, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 103);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1057, 462);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // TreeMenu
            // 
            this.TreeMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeMenu.FullRowSelect = true;
            this.TreeMenu.HotTracking = true;
            this.TreeMenu.Location = new System.Drawing.Point(3, 3);
            this.TreeMenu.Name = "TreeMenu";
            treeNode1.Name = "Node_ITM";
            treeNode1.Tag = "ITM";
            treeNode1.Text = "Manage ITM";
            treeNode2.Name = "Node_User";
            treeNode2.Tag = "User";
            treeNode2.Text = "Manage User";
            treeNode3.Name = "Node_Report_CashDeposit";
            treeNode3.Tag = "CashDeposit";
            treeNode3.Text = "Cash Deposit";
            treeNode4.Name = "Node_Report_CheckDeposit";
            treeNode4.Tag = "CheckDeposit";
            treeNode4.Text = "Check Deposit";
            treeNode5.Name = "Node_Reporot_Signature";
            treeNode5.Tag = "Signature";
            treeNode5.Text = "Signature";
            treeNode6.Name = "Node_Report_TransactionReversal";
            treeNode6.Tag = "TransactionReversal";
            treeNode6.Text = "Transaction Reversal";
            treeNode7.Name = "Node_Report_Withdrawal";
            treeNode7.Tag = "Withdrawal";
            treeNode7.Text = "Withdrawal";
            treeNode8.Name = "Node_Report";
            treeNode8.Tag = "Report";
            treeNode8.Text = "Reports";
            this.TreeMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode8});
            this.TreeMenu.Size = new System.Drawing.Size(194, 456);
            this.TreeMenu.TabIndex = 0;
            // 
            // ChildControlPanel
            // 
            this.ChildControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChildControlPanel.Location = new System.Drawing.Point(203, 3);
            this.ChildControlPanel.Name = "ChildControlPanel";
            this.ChildControlPanel.Size = new System.Drawing.Size(851, 456);
            this.ChildControlPanel.TabIndex = 1;
            // 
            // UcMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainLayout);
            this.Name = "UcMain";
            this.Size = new System.Drawing.Size(1063, 628);
            this.Load += new System.EventHandler(this.UcMain_Load);
            this.MainLayout.ResumeLayout(false);
            this.HeaderPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Panel FooterPanel;
        private UcHeader Header;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView TreeMenu;
        private System.Windows.Forms.Panel ChildControlPanel;
    }
}
