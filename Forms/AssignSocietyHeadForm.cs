using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using System.Data;

namespace SocietiesManagementSystem.Forms
{
    public class AssignSocietyHeadForm : Form
    {
        private ComboBox cbSociety;
        private ComboBox cbHead;
        private Button btnAssign;

        public AssignSocietyHeadForm()
        {
            this.Text = "Assign Society Head";
            this.Size = new Size(400, 300);

            Label lblSociety = new Label() { Text = "Select Society:", Location = new Point(50, 50), AutoSize = true };
            cbSociety = new ComboBox() { Location = new Point(180, 48), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            Label lblHead = new Label() { Text = "Select Head:", Location = new Point(50, 100), AutoSize = true };
            cbHead = new ComboBox() { Location = new Point(180, 98), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10) };

            btnAssign = new Button() { Text = "Assign", Location = new Point(180, 150), Width = 100, Height = 40, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnAssign.Click += BtnAssign_Click;

            this.Controls.Add(lblSociety);
            this.Controls.Add(cbSociety);
            this.Controls.Add(lblHead);
            this.Controls.Add(cbHead);
            this.Controls.Add(btnAssign);

            LoadData();
        }

        private void LoadData()
        {
            DbHelper db = new DbHelper();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                // Load Societies
                SqlDataAdapter daSociety = new SqlDataAdapter("SELECT SocietyId, Name FROM Societies", con);
                DataTable dtSociety = new DataTable();
                daSociety.Fill(dtSociety);
                cbSociety.DataSource = dtSociety;
                cbSociety.DisplayMember = "Name";
                cbSociety.ValueMember = "SocietyId";

                // Load Heads
                SqlDataAdapter daHead = new SqlDataAdapter("SELECT UserId, Name FROM Users WHERE Role = 'SocietyHead'", con);
                DataTable dtHead = new DataTable();
                daHead.Fill(dtHead);
                cbHead.DataSource = dtHead;
                cbHead.DisplayMember = "Name";
                cbHead.ValueMember = "UserId";
            }
        }

        private void BtnAssign_Click(object sender, EventArgs e)
        {
            if (cbSociety.SelectedValue == null || cbHead.SelectedValue == null)
            {
                MessageBox.Show("Please select both a Society and a Head.");
                return;
            }

            int societyId = (int)cbSociety.SelectedValue;
            int headId = (int)cbHead.SelectedValue;

            DbHelper db = new DbHelper();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "UPDATE Societies SET HeadId = @HeadId WHERE SocietyId = @SocietyId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@HeadId", headId);
                cmd.Parameters.AddWithValue("@SocietyId", societyId);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Society Head assigned successfully!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to assign head.");
                }
            }
        }
    }
}
