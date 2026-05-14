using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Models;
using SocietiesManagementSystem.Services;

namespace SocietiesManagementSystem.Forms
{
    public class LoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private ComboBox cbRole;

        public LoginForm()
        {
            // FULL SCREEN
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Societies Management System - Login";
            this.BackColor = Color.White;

            // MAIN PANEL (CENTERED)
            Panel panel = new Panel()
            {
                Size = new Size(400, 350),
                BackColor = Color.LightGray
            };

            // Center panel manually
            panel.Left = (Screen.PrimaryScreen.WorkingArea.Width - panel.Width) / 2;
            panel.Top = (Screen.PrimaryScreen.WorkingArea.Height - panel.Height) / 2;

            // TITLE
            Label title = new Label()
            {
                Text = "Login",
                Font = new Font("Arial", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(150, 20)
            };

            // EMAIL
            Label lblEmail = new Label() { Text = "Email", Location = new Point(50, 80), AutoSize = true };
            txtEmail = new TextBox() { Location = new Point(150, 78), Width = 180, Font = new Font("Segoe UI", 10) };

            // PASSWORD
            Label lblPassword = new Label() { Text = "Password", Location = new Point(50, 120), AutoSize = true };
            txtPassword = new TextBox() { Location = new Point(150, 118), Width = 180, PasswordChar = '*', Font = new Font("Segoe UI", 10) };

            // ROLE
            Label lblRole = new Label() { Text = "Login As", Location = new Point(50, 160), AutoSize = true };
            cbRole = new ComboBox() { Location = new Point(150, 158), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            cbRole.Items.AddRange(new string[] { "Student", "SocietyHead", "Admin" });
            cbRole.SelectedIndex = 0;

            // LOGIN BUTTON
            Button btnLogin = new Button() { Text = "Login", Width = 120, Height = 40, Location = new Point(50, 220), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnLogin.Click += BtnLogin_Click;

            // REGISTER BUTTON
            Button btnRegister = new Button() { Text = "Register", Width = 120, Height = 40, Location = new Point(200, 220), Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnRegister.Click += BtnRegister_Click;

            // ADD CONTROLS
            panel.Controls.Add(title);
            panel.Controls.Add(lblEmail);
            panel.Controls.Add(txtEmail);
            panel.Controls.Add(lblPassword);
            panel.Controls.Add(txtPassword);
            panel.Controls.Add(lblRole);
            panel.Controls.Add(cbRole);
            panel.Controls.Add(btnLogin);
            panel.Controls.Add(btnRegister);

            this.Controls.Add(panel);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            AuthService service = new AuthService();
            string selectedRole = cbRole.SelectedItem.ToString();
            var user = service.Login(txtEmail.Text, txtPassword.Text, selectedRole);

            if (user != null)
            {
               Session.CurrentUser = user;
               if (user.Role == "Admin")
                   new AdminDashboardForm().Show();
               else if (user.Role == "SocietyHead")
                   new SocietyDashboardForm().Show();
               else
                   new StudentDashboardForm().Show();
               this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Credentials or Role");
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm form = new RegisterForm("Guest");
            form.Show();
        }
    }
}