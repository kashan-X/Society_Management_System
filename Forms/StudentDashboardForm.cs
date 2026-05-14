using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Models;

namespace SocietiesManagementSystem.Forms
{
    public class StudentDashboardForm : Form
    {
        private Panel mainPanel;
        private Panel sidebar;

        public StudentDashboardForm()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Student Module - " + Session.CurrentUser?.Name;

            sidebar = new Panel() { Width = 250, Dock = DockStyle.Left, BackColor = Color.FromArgb(30, 30, 60) };
            mainPanel = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White };

            this.Controls.Add(mainPanel);
            this.Controls.Add(sidebar);

            LoadSidebarButtons();
            
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
        }

        private void LoadSidebarButtons()
        {
            int yPos = 50;
            AddButton("Browse Societies", yPos, (s, e) => LoadForm(new SocietyForm())); yPos += 60;
            AddButton("My Memberships", yPos, (s, e) => LoadForm(new MyMembershipsForm())); yPos += 60;
            AddButton("Upcoming Events", yPos, (s, e) => LoadForm(new UpcomingEventsForm())); yPos += 60;
            AddButton("My Tickets", yPos, (s, e) => LoadForm(new MyTicketsForm())); yPos += 60;
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
