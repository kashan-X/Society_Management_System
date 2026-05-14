using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using SocietiesManagementSystem.Models;
using System.Collections.Generic;

namespace SocietiesManagementSystem.Services
{
    public class MembershipService
    {
        private DbHelper db = new DbHelper();

        public bool ApplyForMembership(int studentId, int societyId)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "INSERT INTO Memberships (StudentId, SocietyId, Status) VALUES (@StudentId, @SocietyId, 'Pending')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@SocietyId", societyId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<Membership> GetMembershipsByStudent(int studentId)
        {
            List<Membership> list = new List<Membership>();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = @"SELECT m.MembershipId, m.SocietyId, s.Name AS SocietyName, m.Status 
                                 FROM Memberships m
                                 JOIN Societies s ON m.SocietyId = s.SocietyId
                                 WHERE m.StudentId = @StudentId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Membership
                    {
                        MembershipId = (int)reader["MembershipId"],
                        SocietyId = (int)reader["SocietyId"],
                        SocietyName = reader["SocietyName"].ToString(),
                        Status = reader["Status"].ToString(),
                        StudentId = studentId
                    });
                }
            }
            return list;
        }

        public List<Membership> GetPendingMembershipsForSociety(int societyId)
        {
            List<Membership> list = new List<Membership>();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = @"SELECT m.MembershipId, m.StudentId, u.Name AS StudentName, m.Status 
                                 FROM Memberships m
                                 JOIN Users u ON m.StudentId = u.UserId
                                 WHERE m.SocietyId = @SocietyId AND m.Status = 'Pending'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SocietyId", societyId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Membership
                    {
                        MembershipId = (int)reader["MembershipId"],
                        StudentId = (int)reader["StudentId"],
                        StudentName = reader["StudentName"].ToString(),
                        Status = reader["Status"].ToString(),
                        SocietyId = societyId
                    });
                }
            }
            return list;
        }

        public List<Membership> GetApprovedMembersForSociety(int societyId)
        {
            List<Membership> list = new List<Membership>();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = @"SELECT m.MembershipId, m.StudentId, u.Name AS StudentName, m.Status 
                                 FROM Memberships m
                                 JOIN Users u ON m.StudentId = u.UserId
                                 WHERE m.SocietyId = @SocietyId AND m.Status = 'Approved'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SocietyId", societyId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Membership
                    {
                        MembershipId = (int)reader["MembershipId"],
                        StudentId = (int)reader["StudentId"],
                        StudentName = reader["StudentName"].ToString(),
                        Status = reader["Status"].ToString(),
                        SocietyId = societyId
                    });
                }
            }
            return list;
        }

        public bool UpdateMembershipStatus(int membershipId, string status)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "UPDATE Memberships SET Status = @Status WHERE MembershipId = @MembershipId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@MembershipId", membershipId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}