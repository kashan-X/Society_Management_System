using Microsoft.Data.SqlClient;
using SocietiesManagementSystem.Data;
using SocietiesManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace SocietiesManagementSystem.Services
{
    public class EventService
    {
        private DbHelper db = new DbHelper();

        public bool CreateEvent(Event ev)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "INSERT INTO Events (Title, Description, Date, SocietyId, Status) VALUES (@Title, @Description, @Date, @SocietyId, 'Pending')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Title", ev.Title);
                cmd.Parameters.AddWithValue("@Description", ev.Description);
                cmd.Parameters.AddWithValue("@Date", ev.Date);
                cmd.Parameters.AddWithValue("@SocietyId", ev.SocietyId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<Event> GetEvents(string status = "Approved")
        {
            List<Event> list = new List<Event>();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = @"SELECT e.EventId, e.Title, e.Description, e.Date, e.SocietyId, s.Name AS SocietyName, e.Status, e.MaxTickets
                                 FROM Events e
                                 JOIN Societies s ON e.SocietyId = s.SocietyId
                                 WHERE e.Status = @Status";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Status", status);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Event
                    {
                        EventId = (int)reader["EventId"],
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        Date = (DateTime)reader["Date"],
                        SocietyId = (int)reader["SocietyId"],
                        SocietyName = reader["SocietyName"].ToString(),
                        Status = reader["Status"].ToString(),
                        MaxTickets = reader["MaxTickets"] != DBNull.Value ? (int)reader["MaxTickets"] : 0
                    });
                }
            }
            return list;
        }

        public bool RegisterForEvent(int studentId, int eventId)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "INSERT INTO EventRegistrations (StudentId, EventId, Status) VALUES (@StudentId, @EventId, 'Pending')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@EventId", eventId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<EventRegistration> GetPendingTicketRequests(int societyId)
        {
            List<EventRegistration> list = new List<EventRegistration>();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = @"SELECT er.RegistrationId, er.EventId, e.Title AS EventTitle, u.Name AS StudentName, er.Status 
                                 FROM EventRegistrations er
                                 JOIN Events e ON er.EventId = e.EventId
                                 JOIN Users u ON er.StudentId = u.UserId
                                 WHERE e.SocietyId = @SocietyId AND er.Status = 'Pending'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SocietyId", societyId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new EventRegistration
                    {
                        RegistrationId = (int)reader["RegistrationId"],
                        EventId = (int)reader["EventId"],
                        EventTitle = reader["EventTitle"].ToString(),
                        StudentName = reader["StudentName"].ToString(),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return list;
        }

        public bool UpdateTicketStatus(int registrationId, string status)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                
                if (status == "Approved")
                {
                    // Check capacity
                    string checkQuery = @"
                        DECLARE @EventId INT;
                        SELECT @EventId = EventId FROM EventRegistrations WHERE RegistrationId = @RegId;
                        
                        DECLARE @Max INT;
                        DECLARE @Approved INT;
                        
                        SELECT @Max = MaxTickets FROM Events WHERE EventId = @EventId;
                        SELECT @Approved = COUNT(*) FROM EventRegistrations WHERE EventId = @EventId AND Status = 'Approved';
                        
                        IF (@Approved < @Max OR @Max = 0)
                        BEGIN
                            UPDATE EventRegistrations SET Status = 'Approved' WHERE RegistrationId = @RegId;
                            SELECT 1;
                        END
                        ELSE
                        BEGIN
                            SELECT 0;
                        END";
                    SqlCommand cmd = new SqlCommand(checkQuery, con);
                    cmd.Parameters.AddWithValue("@RegId", registrationId);
                    object result = cmd.ExecuteScalar();
                    return result != null && Convert.ToInt32(result) == 1;
                }
                else
                {
                    string query = "UPDATE EventRegistrations SET Status = @Status WHERE RegistrationId = @RegId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@RegId", registrationId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<EventRegistration> GetMyTickets(int studentId)
        {
            List<EventRegistration> list = new List<EventRegistration>();
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = @"SELECT er.RegistrationId, er.EventId, e.Title AS EventTitle, u.Name AS StudentName 
                                 FROM EventRegistrations er
                                 JOIN Events e ON er.EventId = e.EventId
                                 JOIN Users u ON er.StudentId = u.UserId
                                 WHERE er.StudentId = @StudentId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new EventRegistration
                    {
                        RegistrationId = (int)reader["RegistrationId"],
                        EventId = (int)reader["EventId"],
                        EventTitle = reader["EventTitle"].ToString(),
                        StudentId = studentId,
                        StudentName = reader["StudentName"].ToString()
                    });
                }
            }
            return list;
        }

        public bool UpdateEventStatus(int eventId, string status, int maxTickets = 0)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();
                string query = "UPDATE Events SET Status = @Status, MaxTickets = @MaxTickets WHERE EventId = @EventId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@MaxTickets", maxTickets);
                cmd.Parameters.AddWithValue("@EventId", eventId);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
