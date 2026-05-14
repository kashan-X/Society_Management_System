using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;

namespace SocietiesManagementSystem.Forms
{
    public class ManageSocietiesForm : Form
    {
        private DataGridView grid;
        private Button btnApprove;
        private Button btnSuspend;

        public ManageSocietiesForm()
        {
            this.Text = "Manage Societies";
            
            grid = new DataGridView()
            {
                Dock = DockStyle.Top,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Panel pnlButtons = new Panel() { Dock = DockStyle.Bottom, Height = 50 };

            btnApprove = new Button() { Text = "Approve", Width = 100, Location = new Point(50, 10) };
            btnApprove.Click += BtnApprove_Click;
            
            btnSuspend = new Button() { Text = "Suspend", Width = 100, Location = new Point(160, 10) };
            btnSuspend.Click += BtnSuspend_Click;

            pnlButtons.Controls.Add(btnApprove);
            pnlButtons.Controls.Add(btnSuspend);

            this.Controls.Add(grid);
            this.Controls.Add(pnlButtons);

            LoadData();
        }

        private void LoadData()
        {
            SocietyService service = new SocietyService();
            grid.DataSource = service.GetSocieties("Pending"); // Could show all
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            UpdateStatus("Approved");
        }

        private void BtnSuspend_Click(object sender, EventArgs e)
        {
            UpdateStatus("Suspended");
        }

        private void UpdateStatus(string status)
        {
            if (grid.CurrentRow == null) return;
            int societyId = (int)grid.CurrentRow.Cells["SocietyId"].Value;

            SocietyService service = new SocietyService();
            if (service.UpdateSocietyStatus(societyId, status))
            {
                MessageBox.Show($"Society {status}");
                LoadData();
            }
        }
    }
}
