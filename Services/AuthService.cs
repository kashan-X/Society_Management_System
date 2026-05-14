using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using SocietiesManagementSystem.Models;

namespace SocietiesManagementSystem.Services
{
    public class AuthService
    {
        private DbHelper db = new DbHelper();

        public bool Register(User user)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                // Check if role is empty, default to Student
                string roleToInsert = string.IsNullOrEmpty(user.Role) ? "Student" : user.Role;
                string query = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@Name, @Email, @Password, @Role)";
                
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", roleToInsert);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public User Login(string email, string password, string role)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "SELECT * FROM Users WHERE Email=@Email AND Password=@Password AND Role=@Role";
                
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Role", role);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        UserId = (int)reader["UserId"],
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
                return null;
            }
        }
    }
}
