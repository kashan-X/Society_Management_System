using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using System.Data;

namespace SocietiesManagementSystem.Forms
{
    public class ManageUsersForm : Form
    {
        private DataGridView grid;

        public ManageUsersForm()
        {
            this.Text = "Manage Users";
            
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
            DbHelper db = new DbHelper();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "SELECT UserId, Name, Email, Role FROM Users";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                grid.DataSource = dt;
            }
        }
    }
}
