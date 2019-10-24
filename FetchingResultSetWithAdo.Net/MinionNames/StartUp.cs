using System;
using System.Data.SqlClient;

namespace MinionNames
{
    class StartUp
    {
        private static string ConnectionString =
            "Server = DESKTOP-OU2Q3NF\\SQLEXPRESS;" +
            "Database = MinionsDB;" +
            "Integrated Security = true;";
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();

            int villainId = int.Parse(Console.ReadLine());

            using (connection)
            {
                string selectVilianQuery = $"SELECT Name FROM Villains WHERE Id = {villainId}";

                SqlCommand cmd = new SqlCommand(selectVilianQuery, connection);

                object villainNameReader = cmd.ExecuteScalar();

                if (villainNameReader == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                    return;
                }
                else
                {
                    string villainName = (string)villainNameReader;
                    Console.WriteLine($"Villain: {villainName}");
                }

                string selectMinionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,"+
                                                     "m.Name,"+ 
                                                     "m.Age "+
                                                "FROM MinionsVillains AS mv "+
                                                "JOIN Minions As m ON mv.MinionId = m.Id "+
                                               "WHERE mv.VillainId = " + villainId +
                                            "ORDER BY m.Name ";

                SqlCommand selectMinionsCommand = new SqlCommand(selectMinionsQuery, connection);

                SqlDataReader reader = selectMinionsCommand.ExecuteReader();

                using (reader)
                {
                    bool hasReaded = true;
                    int number = 1;
                    while (reader.Read())
                    {
                        Console.WriteLine($"{number}. {reader["Name"]} {reader["Age"]}");

                        hasReaded = false;
                        number++;
                    }

                    if (hasReaded)
                    {
                        Console.WriteLine("(no minions)");
                    }
                }
            }
        }
    }
}
