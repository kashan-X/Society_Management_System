using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;
using SocietiesManagementSystem.Models;
using System.Collections.Generic;

namespace SocietiesManagementSystem.Forms
{
    public class ManageMembershipsForm : Form
    {
        private DataGridView grid;
        private Button btnApprove;
        private Button btnReject;
        private int societyId;

        public ManageMembershipsForm(int societyId)
        {
            this.societyId = societyId;
            this.Text = "Manage Memberships";
            
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

            pnlButtons.Controls.Add(btnApprove);
            pnlButtons.Controls.Add(btnReject);

            this.Controls.Add(grid);
            this.Controls.Add(pnlButtons);

            LoadData();
        }

        private void LoadData()
        {
            MembershipService service = new MembershipService();
            // We should get SocietyId for this head. Hardcoding to 1 for demo.
            grid.DataSource = service.GetPendingMembershipsForSociety(societyId);
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
            int membershipId = (int)grid.CurrentRow.Cells["MembershipId"].Value;

            MembershipService service = new MembershipService();
            if (service.UpdateMembershipStatus(membershipId, status))
            {
                MessageBox.Show($"Membership {status}");
                LoadData();
            }
        }
    }
}
