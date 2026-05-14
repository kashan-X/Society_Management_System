using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using System.Data;

namespace SocietiesManagementSystem.Forms
{
    public class SocietyReportsForm : Form
    {
        private DataGridView gridMembers;
        private DataGridView gridEvents;
        private int societyId;

        public SocietyReportsForm(int societyId)
        {
            this.societyId = societyId;
            this.Text = "Society Reports";

            Label lblMembers = new Label() { Text = "Members Report", Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12, FontStyle.Bold) };
            gridMembers = new DataGridView() { Dock = DockStyle.Top, Height = 200, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            Label lblEvents = new Label() { Text = "Events Report", Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12, FontStyle.Bold) };
            gridEvents = new DataGridView() { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            this.Controls.Add(gridEvents);
            this.Controls.Add(lblEvents);
            this.Controls.Add(gridMembers);
            this.Controls.Add(lblMembers);

            LoadReports();
        }

        private void LoadReports()
        {
            DbHelper db = new DbHelper();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                // Members Report
                string mQuery = @"SELECT u.Name, u.Email, m.Status FROM Memberships m JOIN Users u ON m.StudentId = u.UserId WHERE m.SocietyId = @Id";
                SqlCommand mCmd = new SqlCommand(mQuery, con);
                mCmd.Parameters.AddWithValue("@Id", societyId);
                SqlDataAdapter mAdapter = new SqlDataAdapter(mCmd);
                DataTable mDt = new DataTable();
                mAdapter.Fill(mDt);
                gridMembers.DataSource = mDt;

                // Events Report
                string eQuery = @"SELECT Title, Date, Status FROM Events WHERE SocietyId = @Id";
                SqlCommand eCmd = new SqlCommand(eQuery, con);
                eCmd.Parameters.AddWithValue("@Id", societyId);
                SqlDataAdapter eAdapter = new SqlDataAdapter(eCmd);
                DataTable eDt = new DataTable();
                eAdapter.Fill(eDt);
                gridEvents.DataSource = eDt;
            }
        }
    }
}
