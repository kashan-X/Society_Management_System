using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;
using SocietiesManagementSystem.Models;

namespace SocietiesManagementSystem.Forms
{
    public class MyMembershipsForm : Form
    {
        private DataGridView grid;

        public MyMembershipsForm()
        {
            this.Text = "My Memberships";
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
            MembershipService service = new MembershipService();
            grid.DataSource = service.GetMembershipsByStudent(Session.CurrentUser.UserId);
        }
    }
}
