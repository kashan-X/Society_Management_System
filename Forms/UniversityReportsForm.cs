using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using System.Data;

namespace SocietiesManagementSystem.Forms
{
    public class UniversityReportsForm : Form
    {
        private DataGridView grid;

        public UniversityReportsForm()
        {
            this.Text = "University Reports";

            Label lblTitle = new Label() { Text = "University Wide Society Statistics", Dock = DockStyle.Top, Height = 40, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 14, FontStyle.Bold) };
            grid = new DataGridView() { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            this.Controls.Add(grid);
            this.Controls.Add(lblTitle);

            LoadReport();
        }

        private void LoadReport()
        {
            DbHelper db = new DbHelper();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = @"
                    SELECT 
                        s.Name AS SocietyName, 
                        s.Status,
                        COUNT(DISTINCT m.MembershipId) AS TotalMembers,
                        COUNT(DISTINCT e.EventId) AS TotalEvents
                    FROM Societies s
                    LEFT JOIN Memberships m ON s.SocietyId = m.SocietyId AND m.Status = 'Approved'
                    LEFT JOIN Events e ON s.SocietyId = e.SocietyId
                    GROUP BY s.Name, s.Status
                ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                grid.DataSource = dt;
            }
        }
    }
}
