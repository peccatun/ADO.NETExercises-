﻿using System;
using System.Data.SqlClient;

namespace VIllain_Names
{
    class StartUp
    {
        private static string ConnectionString =
            "Server = DESKTOP-OU2Q3NF\\SQLEXPRESS;" +
            "Database = MinionsDB;" +
            "Integrated Security = true;";

        private static SqlConnection connection = new SqlConnection(ConnectionString);

        static void Main(string[] args)
        {
            connection.Open();

            using (connection)
            {
                string queryText = @"  SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                            FROM Villains AS v 
                                            JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                        GROUP BY v.Id, v.Name 
                                          HAVING COUNT(mv.VillainId) > 3 
                                        ORDER BY COUNT(mv.VillainId)";
                SqlCommand cmd = new SqlCommand(queryText, connection);

                SqlDataReader reader = cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
                    }
                }
            }
        }

    }
}
