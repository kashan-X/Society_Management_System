using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;
using SocietiesManagementSystem.Models;
using System.Collections.Generic;

namespace SocietiesManagementSystem.Forms
{
    public class AssignTasksForm : Form
    {
        private DataGridView grid;
        private TextBox txtTitle;
        private TextBox txtDesc;
        private ComboBox cbMembers;
        private Button btnAssign;
        private int societyId;

        public AssignTasksForm(int societyId)
        {
            this.societyId = societyId;
            this.Text = "Assign Tasks";

            Panel topPanel = new Panel() { Dock = DockStyle.Top, Height = 150 };

            topPanel.Controls.Add(new Label() { Text = "Title", Location = new Point(20, 20), AutoSize = true });
            txtTitle = new TextBox() { Location = new Point(100, 18), Width = 150, Font = new Font("Segoe UI", 10) };
            topPanel.Controls.Add(txtTitle);

            topPanel.Controls.Add(new Label() { Text = "Desc", Location = new Point(20, 60), AutoSize = true });
            txtDesc = new TextBox() { Location = new Point(100, 58), Width = 150, Font = new Font("Segoe UI", 10) };
            topPanel.Controls.Add(txtDesc);

            topPanel.Controls.Add(new Label() { Text = "Member", Location = new Point(300, 20), AutoSize = true });
            cbMembers = new ComboBox() { Location = new Point(380, 18), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            topPanel.Controls.Add(cbMembers);

            btnAssign = new Button() { Text = "Assign", Location = new Point(380, 58), Width = 120, Height = 35, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnAssign.Click += BtnAssign_Click;
            topPanel.Controls.Add(btnAssign);

            grid = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(grid);
            this.Controls.Add(topPanel);

            LoadData();
        }

        private void LoadData()
        {
            // Load approved members into ComboBox
            MembershipService mService = new MembershipService();
            List<Membership> members = mService.GetApprovedMembersForSociety(societyId);

            cbMembers.DataSource = members;
            cbMembers.DisplayMember = "StudentName";  // show name in dropdown
            cbMembers.ValueMember = "StudentId";       // use StudentId as value

            // Load tasks into grid
            TaskService tService = new TaskService();
            grid.DataSource = tService.GetTasksBySociety(societyId);
        }

        private void BtnAssign_Click(object sender, EventArgs e)
        {
            if (cbMembers.SelectedValue == null)
            {
                MessageBox.Show("Please select a member.");
                return;
            }

            SocietyTask task = new SocietyTask()
            {
                Title = txtTitle.Text,
                Description = txtDesc.Text,
                SocietyId = societyId,
                AssignedTo = (int)cbMembers.SelectedValue
            };

            TaskService service = new TaskService();
            if (service.CreateTask(task))
            {
                MessageBox.Show("Task assigned!");
                LoadData();
            }
        }
    }
}
