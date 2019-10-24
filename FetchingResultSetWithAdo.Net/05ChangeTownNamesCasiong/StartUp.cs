using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _05ChangeTownNamesCasiong
{
    class StartUp
    {
        private static string ConnectionString =
            "Server = DESKTOP-OU2Q3NF\\SQLEXPRESS;" +
            "Database = MinionsDB;" +
            "Integrated Security = true;";
        static void Main(string[] args)
        {
            string countryToChange = Console.ReadLine();

            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();
            bool hasAffected = false;

            using (connection)
            {
                string queryText = @"UPDATE Towns
                                  SET Name = UPPER(Name)
                                  WHERE CountryCode = (SELECT c.Id 
                                                        FROM Countries AS c 
                                                        WHERE c.Name =    @countryName)";

                SqlCommand command = new SqlCommand(queryText, connection);
                using (command)
                {
                    command.Parameters.AddWithValue("@countryName", countryToChange);

                    int countriesAffected = command.ExecuteNonQuery();

                    if (countriesAffected == 0)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                    else
                    {
                        Console.WriteLine($"{countriesAffected} town names were affected.");
                        hasAffected = true;
                    }
                }

            }
            if (hasAffected)
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (connection)
                {
                    string queryText = @" SELECT t.Name 
                                           FROM Towns as t
                                           JOIN Countries AS c ON c.Id = t.CountryCode
                                          WHERE c.Name = @countryName";

                    SqlCommand command = new SqlCommand(queryText, connection);

                    using (command)
                    {
                        command.Parameters.AddWithValue("@countryName",countryToChange);

                        SqlDataReader reader = command.ExecuteReader();

                        using (reader)
                        {
                            List<string> townNames = new List<string>();

                            while (reader.Read())
                            {
                                townNames.Add((string)reader["Name"]);
                            }

                            Console.WriteLine($"[{string.Join(", ",townNames)}]");
                        }
                    }
                }
            }
        }
    }
}
