using System;
using System.Data.SqlClient;
using System.Linq;

namespace _08IncreaseMinionAge
{
    class StartUp
    {
        private static string ConnectionString = "" +
            "Server = DESKTOP-OU2Q3NF\\SQLEXPRESS;" +
            "Database = MinionsDB;" +
            "Integrated Security = true;";
        static void Main(string[] args)
        {
            int[] minionsId = Console.ReadLine()
                .Split(' ',StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            for (int i = 0; i < minionsId.Length; i++)
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (connection)
                {
                    string queryText = @" UPDATE Minions
                                           SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2,LEN(Name)),
                                                        Age += 1
                                          WHERE Id = @Id";

                    SqlCommand command = new SqlCommand(queryText, connection);

                    using (command)
                    {
                        command.Parameters.AddWithValue("@Id", minionsId[i]);
                        command.ExecuteNonQuery();
                    }
                }
            }

            SqlConnection secondConnection = new SqlConnection(ConnectionString);
            secondConnection.Open();

            using (secondConnection)
            {
                string queryText = "SELECT Name, Age FROM Minions";
                SqlCommand command = new SqlCommand(queryText,secondConnection);

                using (command)
                {
                    SqlDataReader reader = command.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader["Name"] + " " + reader["Age"]);
                        }
                    }
                }
            }
        }
    }
}
