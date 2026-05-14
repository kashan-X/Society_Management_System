using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Models;
using SocietiesManagementSystem.Services;

namespace SocietiesManagementSystem.Forms
{
    public class SocietyDashboardForm : Form
    {
        private Panel mainPanel;
        private Panel sidebar;
        private int? managedSocietyId;

        public SocietyDashboardForm()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Society Module - " + Session.CurrentUser?.Name;

            SocietyService sService = new SocietyService();
            managedSocietyId = sService.GetManagedSocietyId(Session.CurrentUser.UserId);

            sidebar = new Panel() { Width = 250, Dock = DockStyle.Left, BackColor = Color.FromArgb(30, 30, 60) };
            mainPanel = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White };

            this.Controls.Add(mainPanel);
            this.Controls.Add(sidebar);

            Button btnLogout = new Button()
            {
                Text = "Logout", ForeColor = Color.White, BackColor = Color.FromArgb(200, 50, 50),
                FlatStyle = FlatStyle.Flat, Dock = DockStyle.Bottom, Height = 60,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnLogout.Click += (s, e) => 
            {
                Session.CurrentUser = null;
                new LoginForm().Show();
                this.Hide();
            };
            sidebar.Controls.Add(btnLogout);

            if (managedSocietyId == null)
            {
                Label lblError = new Label() { Text = "You are not currently assigned to manage any society.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 14) };
                mainPanel.Controls.Add(lblError);
            }
            else
            {
                LoadSidebarButtons();
            }
        }

        private void LoadSidebarButtons()
        {
            int yPos = 50;
            int societyId = managedSocietyId.Value;
            AddButton("Create Student", yPos, (s, e) => { new RegisterForm("SocietyHead").Show(); }); yPos += 60;
            AddButton("Manage Profile", yPos, (s, e) => LoadForm(new ManageSocietyProfileForm(societyId))); yPos += 60;
            AddButton("Manage Events", yPos, (s, e) => LoadForm(new ManageEventsForm(societyId))); yPos += 60;
            AddButton("Manage Tickets", yPos, (s, e) => LoadForm(new ManageTicketsForm(societyId))); yPos += 60;
            AddButton("Memberships", yPos, (s, e) => LoadForm(new ManageMembershipsForm(societyId))); yPos += 60;
            AddButton("Assign Tasks", yPos, (s, e) => LoadForm(new AssignTasksForm(societyId))); yPos += 60;
            AddButton("Reports", yPos, (s, e) => LoadForm(new SocietyReportsForm(societyId))); yPos += 60;
        }

        private void AddButton(string text, int y, EventHandler onClick)
        {
            Button btn = new Button()
            {
                Text = text, ForeColor = Color.White, BackColor = Color.FromArgb(40, 40, 80),
                FlatStyle = FlatStyle.Flat, Width = 230, Height = 50, Location = new Point(10, y),
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btn.Click += onClick;
            sidebar.Controls.Add(btn);
        }

        public void LoadForm(Form form)
        {
            mainPanel.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(form);
            form.Show();
        }
    }
}
