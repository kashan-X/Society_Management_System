using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using SocietiesManagementSystem.Models;
using System.Collections.Generic;

namespace SocietiesManagementSystem.Services
{
    public class SocietyService
    {
        private DbHelper db = new DbHelper();

        public List<Society> GetSocieties(string status = "Approved")
        {
            List<Society> list = new List<Society>();

            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "SELECT * FROM Societies WHERE Status = @Status";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Status", status);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Society
                    {
                        SocietyId = (int)reader["SocietyId"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Status = reader["Status"].ToString(),
                        HeadId = reader["HeadId"] != DBNull.Value ? (int?)reader["HeadId"] : null
                    });
                }
            }
            return list;
        }

        public bool CreateSociety(Society society)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "INSERT INTO Societies (Name, Description, Status, HeadId) VALUES (@Name, @Description, 'Pending', @HeadId)";
                
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", society.Name);
                cmd.Parameters.AddWithValue("@Description", society.Description);
                cmd.Parameters.AddWithValue("@HeadId", society.HeadId ?? (object)DBNull.Value);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
        
        public bool UpdateSocietyStatus(int societyId, string status)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "UPDATE Societies SET Status = @Status WHERE SocietyId = @SocietyId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@SocietyId", societyId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public int? GetManagedSocietyId(int headId)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "SELECT SocietyId FROM Societies WHERE HeadId = @HeadId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@HeadId", headId);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                return null;
            }
        }
    }
}