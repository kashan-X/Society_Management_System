using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using SocietiesManagementSystem.Models;
using System.Collections.Generic;

namespace SocietiesManagementSystem.Services
{
    public class TaskService
    {
        private DbHelper db = new DbHelper();

        public bool CreateTask(SocietyTask task)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "INSERT INTO Tasks (Title, Description, AssignedTo, SocietyId, Status) VALUES (@Title, @Description, @AssignedTo, @SocietyId, 'Pending')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Title", task.Title);
                cmd.Parameters.AddWithValue("@Description", task.Description);
                cmd.Parameters.AddWithValue("@AssignedTo", task.AssignedTo);
                cmd.Parameters.AddWithValue("@SocietyId", task.SocietyId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<SocietyTask> GetTasksBySociety(int societyId)
        {
            List<SocietyTask> list = new List<SocietyTask>();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = @"SELECT t.TaskId, t.Title, t.Description, t.AssignedTo, u.Name AS AssignedToName, t.SocietyId, t.Status 
                                 FROM Tasks t
                                 JOIN Users u ON t.AssignedTo = u.UserId
                                 WHERE t.SocietyId = @SocietyId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SocietyId", societyId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new SocietyTask
                    {
                        TaskId = (int)reader["TaskId"],
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        AssignedTo = (int)reader["AssignedTo"],
                        AssignedToName = reader["AssignedToName"].ToString(),
                        SocietyId = (int)reader["SocietyId"],
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return list;
        }
        
        public bool UpdateTaskStatus(int taskId, string status)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "UPDATE Tasks SET Status = @Status WHERE TaskId = @TaskId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@TaskId", taskId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
