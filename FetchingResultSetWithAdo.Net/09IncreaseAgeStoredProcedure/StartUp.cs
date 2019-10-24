using System;
using System.Data.SqlClient;

namespace _09IncreaseAgeStoredProcedure
{
    class StartUp
    {
        private static string ConnectionString = "" +
            "Server = DESKTOP-OU2Q3NF\\SQLEXPRESS;" +
            "Database = MinionsDB;" +
            "Integrated Security = true;";
        static void Main(string[] args)
        {
            int minionId = int.Parse(Console.ReadLine());

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            using (connection)
            {
                string queryText = "EXEC dbo.usp_GetOlder @Id";

                SqlCommand command = new SqlCommand(queryText,connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@Id", minionId);

                    command.ExecuteReader();

                }

            }

            connection = new SqlConnection(ConnectionString);
            connection.Open();

            using (connection)
            {
                string queryText = "SELECT Name, Age FROM Minions WHERE Id = @Id";

                SqlCommand command = new SqlCommand(queryText, connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@Id", minionId);

                    SqlDataReader reader = command.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} – {reader["Age"]} years old");
                        }
                    }
                }
            }
        }
    }
}
