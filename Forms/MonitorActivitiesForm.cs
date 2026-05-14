using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using System.Data;

namespace SocietiesManagementSystem.Forms
{
    public class MonitorActivitiesForm : Form
    {
        private DataGridView gridEvents;
        private DataGridView gridTasks;

        public MonitorActivitiesForm()
        {
            this.Text = "Monitor Activities";

            Label lblEvents = new Label() { Text = "All Events", Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12, FontStyle.Bold) };
            gridEvents = new DataGridView() { Dock = DockStyle.Top, Height = 200, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            Label lblTasks = new Label() { Text = "All Tasks", Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12, FontStyle.Bold) };
            gridTasks = new DataGridView() { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            this.Controls.Add(gridTasks);
            this.Controls.Add(lblTasks);
            this.Controls.Add(gridEvents);
            this.Controls.Add(lblEvents);

            LoadActivities();
        }

        private void LoadActivities()
        {
            DbHelper db = new DbHelper();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                // Events
                string eQuery = @"SELECT e.Title, e.Date, s.Name AS Society, e.Status FROM Events e JOIN Societies s ON e.SocietyId = s.SocietyId";
                SqlCommand eCmd = new SqlCommand(eQuery, con);
                SqlDataAdapter eAdapter = new SqlDataAdapter(eCmd);
                DataTable eDt = new DataTable();
                eAdapter.Fill(eDt);
                gridEvents.DataSource = eDt;

                // Tasks
                string tQuery = @"SELECT t.Title, u.Name AS AssignedTo, s.Name AS Society, t.Status FROM Tasks t JOIN Users u ON t.AssignedTo = u.UserId JOIN Societies s ON t.SocietyId = s.SocietyId";
                SqlCommand tCmd = new SqlCommand(tQuery, con);
                SqlDataAdapter tAdapter = new SqlDataAdapter(tCmd);
                DataTable tDt = new DataTable();
                tAdapter.Fill(tDt);
                gridTasks.DataSource = tDt;
            }
        }
    }
}
