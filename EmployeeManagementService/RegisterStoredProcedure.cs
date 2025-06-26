
using Microsoft.Data.SqlClient;

namespace EmployeeManagementService
{

    public static class RegisterStoredProcedure
    {
        public static void Main()
        {
            var connectionString = "Server=localhost,1433;Database=YourDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True;";
            var scriptPath = "StoredProcedures/UserStoredProcedure.sql";

            string script = File.ReadAllText(scriptPath);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var commands = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var command in commands)
                {
                    using (SqlCommand sqlCommand = new SqlCommand(command, connection))
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Stored procedures registered successfully.");
            }
        }
    }

}