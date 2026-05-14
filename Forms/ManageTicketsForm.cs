using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;

namespace SocietiesManagementSystem.Forms
{
    public class ManageTicketsForm : Form
    {
        private int societyId;
        private DataGridView grid;
        private Button btnApprove;
        private Button btnReject;

        public ManageTicketsForm(int societyId)
        {
            this.societyId = societyId;
            this.Text = "Manage Ticket Requests";
            this.Size = new Size(600, 400);
            
            grid = new DataGridView()
            {
                Dock = DockStyle.Top,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Panel pnlButtons = new Panel() { Dock = DockStyle.Bottom, Height = 50 };

            btnApprove = new Button() { Text = "Approve Ticket", Width = 150, Location = new Point(50, 10) };
            btnApprove.Click += BtnApprove_Click;
            
            btnReject = new Button() { Text = "Reject Ticket", Width = 150, Location = new Point(220, 10) };
            btnReject.Click += BtnReject_Click;

            pnlButtons.Controls.Add(btnApprove);
            pnlButtons.Controls.Add(btnReject);

            this.Controls.Add(grid);
            this.Controls.Add(pnlButtons);

            LoadData();
        }

        private void LoadData()
        {
            EventService service = new EventService();
            grid.DataSource = service.GetPendingTicketRequests(societyId);
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            UpdateStatus("Approved");
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            UpdateStatus("Rejected");
        }

        private void UpdateStatus(string status)
        {
            if (grid.CurrentRow == null) return;
            int registrationId = (int)grid.CurrentRow.Cells["RegistrationId"].Value;

            EventService service = new EventService();
            bool success = service.UpdateTicketStatus(registrationId, status);
            
            if (success)
            {
                MessageBox.Show($"Ticket {status}");
                LoadData();
            }
            else
            {
                MessageBox.Show(status == "Approved" ? "Approval failed! Event might have reached maximum ticket capacity." : "Rejection failed.");
            }
        }
    }
}
