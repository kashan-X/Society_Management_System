using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;
using SocietiesManagementSystem.Models;

namespace SocietiesManagementSystem.Forms
{
    public class MyTicketsForm : Form
    {
        private DataGridView grid;

        public MyTicketsForm()
        {
            this.Text = "My Tickets";
            grid = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(grid);
            LoadData();
        }

        private void LoadData()
        {
            EventService service = new EventService();
            grid.DataSource = service.GetMyTickets(Session.CurrentUser.UserId);
        }
    }
}
