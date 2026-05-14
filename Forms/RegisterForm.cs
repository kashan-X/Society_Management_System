using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Models;
using SocietiesManagementSystem.Services;

namespace SocietiesManagementSystem.Forms
{
    public class RegisterForm : Form
    {
        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private ComboBox cbRole;
        private Button btnSubmit;
        private string creatorRole;

        public RegisterForm(string creatorRole = "Guest")
        {
            this.creatorRole = creatorRole;
            this.Text = "Register";
            this.Size = new Size(400, 350);

            Label lblName = new Label() { Text = "Name", Location = new Point(50, 50), AutoSize = true };
            txtName = new TextBox() { Location = new Point(150, 48), Width = 180, Font = new Font("Segoe UI", 10) };

            Label lblEmail = new Label() { Text = "Email", Location = new Point(50, 100), AutoSize = true };
            txtEmail = new TextBox() { Location = new Point(150, 98), Width = 180, Font = new Font("Segoe UI", 10) };

            Label lblPassword = new Label() { Text = "Password", Location = new Point(50, 150), AutoSize = true };
            txtPassword = new TextBox() { Location = new Point(150, 148), Width = 180, PasswordChar = '*', Font = new Font("Segoe UI", 10) };

            Label lblRole = new Label() { Text = "Role", Location = new Point(50, 200), AutoSize = true };
            cbRole = new ComboBox() { Location = new Point(150, 198), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };
            
            if (creatorRole == "Admin")
            {
                cbRole.Items.AddRange(new string[] { "Student", "SocietyHead", "Admin" });
            }
            else
            {
                cbRole.Items.Add("Student");
            }
            cbRole.SelectedIndex = 0;

            btnSubmit = new Button() { Text = "Submit", Location = new Point(150, 250), Width = 150, Height = 40, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnSubmit.Click += BtnSubmit_Click;

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblRole);
            this.Controls.Add(cbRole);
            this.Controls.Add(btnSubmit);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            User user = new User()
            {
                Name = txtName.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Role = cbRole.SelectedItem.ToString()
            };

            AuthService service = new AuthService();
            bool success = service.Register(user);

            if (success)
            {
                MessageBox.Show("Registered Successfully!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Registration Failed");
            }
        }
    }
}