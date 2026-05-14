using System;
using System.Drawing;
using System.Windows.Forms;
using SocietiesManagementSystem.Services;
using SocietiesManagementSystem.Models;

namespace SocietiesManagementSystem.Forms
{
    public class ManageEventsForm : Form
    {
        private DataGridView grid;
        private TextBox txtTitle;
        private TextBox txtDesc;
        private DateTimePicker dtpDate;
        private Button btnCreate;
        private Button btnCancel;
        private int societyId;

        public ManageEventsForm(int societyId)
        {
            this.societyId = societyId;
            this.Text = "Manage Events";
            
            Panel topPanel = new Panel() { Dock = DockStyle.Top, Height = 150 };
            
            topPanel.Controls.Add(new Label() { Text = "Title", Location = new Point(20, 20), AutoSize = true });
            txtTitle = new TextBox() { Location = new Point(100, 18), Width = 150, Font = new Font("Segoe UI", 10) };
            topPanel.Controls.Add(txtTitle);

            topPanel.Controls.Add(new Label() { Text = "Desc", Location = new Point(20, 60), AutoSize = true });
            txtDesc = new TextBox() { Location = new Point(100, 58), Width = 150, Font = new Font("Segoe UI", 10) };
            topPanel.Controls.Add(txtDesc);

            topPanel.Controls.Add(new Label() { Text = "Date", Location = new Point(300, 20), AutoSize = true });
            dtpDate = new DateTimePicker() { Location = new Point(380, 18), Width = 200, Font = new Font("Segoe UI", 10) };
            topPanel.Controls.Add(dtpDate);

            btnCreate = new Button() { Text = "Create Event", Location = new Point(620, 18), Width = 150, Height = 35, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnCreate.Click += BtnCreate_Click;
            topPanel.Controls.Add(btnCreate);

            btnCancel = new Button() { Text = "Cancel Event", Location = new Point(620, 58), Width = 150, Height = 35, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnCancel.Click += BtnCancel_Click;
            topPanel.Controls.Add(btnCancel);

            grid = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(grid);
            this.Controls.Add(topPanel);

            LoadData();
        }

        private void LoadData()
        {
            EventService service = new EventService();
            grid.DataSource = service.GetEvents("Pending"); // or show all events for this society
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            Event newEvent = new Event()
            {
                Title = txtTitle.Text,
                Description = txtDesc.Text,
                Date = dtpDate.Value,
                SocietyId = societyId
            };

            EventService service = new EventService();
            if (service.CreateEvent(newEvent))
            {
                MessageBox.Show("Event created and pending Admin approval.");
                LoadData();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (grid.CurrentRow == null) return;
            int eventId = (int)grid.CurrentRow.Cells["EventId"].Value;

            EventService service = new EventService();
            if (service.UpdateEventStatus(eventId, "Cancelled"))
            {
                MessageBox.Show("Event cancelled.");
                LoadData();
            }
        }
    }
}
