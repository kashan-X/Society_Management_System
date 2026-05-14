using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;
using SocietiesManagementSystem.Models;

namespace SocietiesManagementSystem.Forms
{   
    
    public class SocietyForm : Form
    {
        private DataGridView grid;
        private Button btnJoin;
        
        public SocietyForm()
        {
            this.Text = "Societies";

            grid = new DataGridView()
            {
                Dock = DockStyle.Top,
                Height = 300,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            btnJoin = new Button()
            {
                Text = "Join Selected Society",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnJoin.Click += BtnJoin_Click;

            this.Controls.Add(grid);
            this.Controls.Add(btnJoin);

            LoadData();
        }

        private void LoadData()
        {
            SocietyService service = new SocietyService();
            grid.DataSource = service.GetSocieties();
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null)
            {
                MessageBox.Show("Select a society first");
                return;
            }

            int societyId = (int)grid.CurrentRow.Cells["SocietyId"].Value;

            MembershipService service = new MembershipService();
            bool success = service.ApplyForMembership(Session.CurrentUser.UserId, societyId);

            if (success)
                MessageBox.Show("Request Sent!");
            else
                MessageBox.Show("Error joining society");
        }
    }
}