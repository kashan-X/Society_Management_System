using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;
using SocietiesManagementSystem.Models;
using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;

namespace SocietiesManagementSystem.Forms
{
    public class ManageSocietyProfileForm : Form
    {
        private TextBox txtName;
        private TextBox txtDesc;
        private Button btnUpdate;
        private int societyId;

        public ManageSocietyProfileForm(int societyId)
        {
            this.societyId = societyId;
            this.Text = "Manage Society Profile";
            
            Label lblName = new Label() { Text = "Name", Location = new Point(50, 50), AutoSize = true };
            txtName = new TextBox() { Location = new Point(150, 48), Width = 200, Font = new Font("Segoe UI", 10) };
            
            Label lblDesc = new Label() { Text = "Description", Location = new Point(50, 100), AutoSize = true };
            txtDesc = new TextBox() { Location = new Point(150, 100), Width = 200, Height = 100, Multiline = true, Font = new Font("Segoe UI", 10) };

            btnUpdate = new Button() { Text = "Update Profile", Location = new Point(150, 230), Width = 150, Height = 40, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnUpdate.Click += BtnUpdate_Click;

            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblDesc);
            this.Controls.Add(txtDesc);
            this.Controls.Add(btnUpdate);

            LoadProfile();
        }

        private void LoadProfile()
        {
            DbHelper db = new DbHelper();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "SELECT Name, Description FROM Societies WHERE SocietyId = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", societyId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtName.Text = reader["Name"].ToString();
                    txtDesc.Text = reader["Description"].ToString();
                }
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            DbHelper db = new DbHelper();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "UPDATE Societies SET Name = @Name, Description = @Desc WHERE SocietyId = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Desc", txtDesc.Text);
                cmd.Parameters.AddWithValue("@Id", societyId);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Profile updated successfully!");
                }
            }
        }
    }
}
