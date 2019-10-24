using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PrintAllMinionNames
{
    class StartUp
    {
        private static string ConnectionString = "" +
            "Server = DESKTOP-OU2Q3NF\\SQLEXPRESS;" +
            "Database = MinionsDB;" +
            "Integrated Security = true;";
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            List<string> minionsNames = new List<string>();
            using (connection)
            {
                string queryText = "SELECT Name FROM Minions";

                SqlCommand getNames = new SqlCommand(queryText,connection);

                using (getNames)
                {
                    SqlDataReader namesReader = getNames.ExecuteReader();

                    using (namesReader)
                    {
                        while (namesReader.Read())
                        {
                            minionsNames.Add((string)namesReader["Name"]);
                        }
                    }
                }
                if (minionsNames.Count % 2 == 0)
                {
                    for (int i = 0; i < minionsNames.Count; i++)
                    {
                        Console.WriteLine(minionsNames[i]);
                        Console.WriteLine(minionsNames[minionsNames.Count - (i + 1)]);

                        if (minionsNames.Count - (i+1) == minionsNames.Count / 2)
                        {
                            break;
                        }
                    }
                }

                else if (minionsNames.Count % 2 != 0)
                {
                    for (int i = 0; i < minionsNames.Count; i++)
                    {
                        Console.WriteLine(minionsNames[i]);
                        if (minionsNames.Count - (i+1) < Math.Ceiling(((double)minionsNames.Count / 2)))
                        {
                            break;
                        }
                        Console.WriteLine(minionsNames[minionsNames.Count - (i + 1)]);
                    }
                }
            }
        }
    }
}
