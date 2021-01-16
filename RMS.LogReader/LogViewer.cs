using RMS.Component.DataAccess.SQLite.Repositories;
using System;
using System.Windows.Forms;

namespace RMS.LogReader
{
    public partial class LogViewer : Form
    {
        public LogViewer()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SQLite Database|*.sqlite|All Files|*.*";
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblSelectedDatabaseFile.Text = ofd.FileName;
            }
        }

        private void btnExecuteQuery_Click(object sender, EventArgs e)
        {
            LogRepository repo = new LogRepository(lblSelectedDatabaseFile.Text);
            var result = repo.ReadPushApiLog(txtQuery.Text);
            dgvResult.DataSource = result;

        }
    }
}
