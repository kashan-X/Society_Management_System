using Microsoft.Data.SqlClient;

namespace SocietiesManagementSystem.Data
{
    public class DbHelper
    {
        private string connectionString =
    "Server=DESKTOP-QI6H2EA\\SQLEXPRESS01;Database=SocietiesDB;Trusted_Connection=True;TrustServerCertificate=True;";
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}