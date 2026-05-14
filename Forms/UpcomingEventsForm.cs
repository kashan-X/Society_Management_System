using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;
using SocietiesManagementSystem.Models;

namespace SocietiesManagementSystem.Forms
{
    public class UpcomingEventsForm : Form
    {
        private DataGridView grid;
        private Button btnRegister;

        public UpcomingEventsForm()
        {
            this.Text = "Upcoming Events";
            grid = new DataGridView()
            {
                Dock = DockStyle.Top,
                Height = 400,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            btnRegister = new Button()
            {
                Text = "Request Ticket",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnRegister.Click += BtnRegister_Click;

            this.Controls.Add(grid);
            this.Controls.Add(btnRegister);
            LoadData();
        }

        private void LoadData()
        {
            EventService service = new EventService();
            grid.DataSource = service.GetEvents("Approved");
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null) return;
            int eventId = (int)grid.CurrentRow.Cells["EventId"].Value;

            EventService service = new EventService();
            if (service.RegisterForEvent(Session.CurrentUser.UserId, eventId))
                MessageBox.Show("Ticket requested! Pending Society Head approval.");
            else
                MessageBox.Show("Failed to request ticket.");
        }
    }
}
