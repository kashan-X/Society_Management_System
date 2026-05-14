using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;

namespace SocietiesManagementSystem.Forms
{
    public class ApproveEventsForm : Form
    {
        private DataGridView grid;
        private Button btnApprove;
        private Button btnReject;

        private NumericUpDown numMaxTickets;

        public ApproveEventsForm()
        {
            this.Text = "Approve Events";
            
            grid = new DataGridView()
            {
                Dock = DockStyle.Top,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Panel pnlButtons = new Panel() { Dock = DockStyle.Bottom, Height = 50 };

            btnApprove = new Button() { Text = "Approve", Width = 100, Location = new Point(50, 10) };
            btnApprove.Click += BtnApprove_Click;
            
            btnReject = new Button() { Text = "Reject", Width = 100, Location = new Point(160, 10) };
            btnReject.Click += BtnReject_Click;

            Label lblMax = new Label() { Text = "Max Tickets:", Location = new Point(280, 15), AutoSize = true };
            numMaxTickets = new NumericUpDown() { Location = new Point(360, 12), Width = 80, Maximum = 10000, Value = 100 };

            pnlButtons.Controls.Add(btnApprove);
            pnlButtons.Controls.Add(btnReject);
            pnlButtons.Controls.Add(lblMax);
            pnlButtons.Controls.Add(numMaxTickets);

            this.Controls.Add(grid);
            this.Controls.Add(pnlButtons);

            LoadData();
        }

        private void LoadData()
        {
            EventService service = new EventService();
            grid.DataSource = service.GetEvents("Pending");
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            UpdateStatus("Approved");
        }

        private void BtnReject_Click(object sender, EventArgs e)
        {
            UpdateStatus("Cancelled");
        }

        private void UpdateStatus(string status)
        {
            if (grid.CurrentRow == null) return;
            int eventId = (int)grid.CurrentRow.Cells["EventId"].Value;
            int maxTickets = (int)numMaxTickets.Value;

            EventService service = new EventService();
            if (service.UpdateEventStatus(eventId, status, status == "Approved" ? maxTickets : 0))
            {
                MessageBox.Show($"Event {status}");
                LoadData();
            }
        }
    }
}
