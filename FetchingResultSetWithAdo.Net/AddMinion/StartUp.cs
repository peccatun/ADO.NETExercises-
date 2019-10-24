using System;
using System.Data.SqlClient;

namespace AddMinion
{
    class StartUp
    {
        private static string ConnectionString =
            "Server = DESKTOP-OU2Q3NF\\SQLEXPRESS;" +
            "Database = MinionsDB;" +
            "Integrated Security = true;";


        static void Main(string[] args)
        {
            string[] minionInformation = Console.ReadLine()
                .Split(new char[] { ':', ' ' }
                , StringSplitOptions
                .RemoveEmptyEntries);

            string[] villainInformation = Console.ReadLine()
                .Split(new char[] { ':', ' ' }
                , StringSplitOptions
                .RemoveEmptyEntries);

            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            string town = minionInformation[3];
            object currentTown = null;
            using (connection)
            {
                string queryText = "SELECT Id FROM Towns WHERE Name = @townName";
                SqlCommand command = new SqlCommand(queryText, connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@townName", town);

                    currentTown = command.ExecuteScalar();
                }
            }

            if (currentTown == null)
            {

                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (connection)
                {

                    string queryText = "INSERT INTO Towns (Name) VALUES (@Name)";
                    SqlCommand command = new SqlCommand(queryText, connection);

                    using (command)
                    {
                        command.Parameters.AddWithValue("@Name", town);
                        int addedTown = command.ExecuteNonQuery();

                        if (addedTown > 0)
                        {
                            Console.WriteLine($"Town {town} was added to the database.");
                        }
                    }
                }
            }

            object currentVillain = null;
            string villainName = villainInformation[1];

            connection = new SqlConnection(ConnectionString);
            connection.Open();

            using (connection)
            {
                string queryText = "SELECT Id FROM Villains WHERE Name = @Name";
                SqlCommand command = new SqlCommand(queryText, connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@Name", villainName);
                    currentVillain = command.ExecuteScalar();
                }
            }

            if (currentVillain == null)
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (connection)
                {
                    string queryText = "INSERT INTO Villains (Name,EvilnessFactorId) VALUES (@Name , 4)";

                    SqlCommand command = new SqlCommand(queryText,connection);

                    using (command)
                    {
                        command.Parameters.AddWithValue("@Name",villainName);

                        int addedVillain = command.ExecuteNonQuery();

                        if (addedVillain > 0)
                        {
                            Console.WriteLine($"Villain {villainName} was added to the database.");
                        }
                    }
                }
            }

            if (currentTown == null)
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (connection)
                {
                    string queryText = "SELECT Id FROM Towns WHERE Name = @townName";

                    SqlCommand command = new SqlCommand(queryText, connection);

                    using (command)
                    {
                        command.Parameters.AddWithValue("@townName", town);
                        currentTown = command.ExecuteScalar();
                    }
                }
            }

            if (currentVillain == null)
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();

                using (connection)
                {
                    string queryText = "SELECT Id FROM Villains WHERE Name = @Name";

                    SqlCommand command = new SqlCommand(queryText,connection);

                    using (connection)
                    {
                        command.Parameters.AddWithValue("@Name", villainName);
                        currentVillain = command.ExecuteScalar();
                    }
                }
            }

            connection = new SqlConnection(ConnectionString);
            connection.Open();

            using (connection)
            {
                string queryText = "INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";

                SqlCommand command = new SqlCommand(queryText, connection);

                using (command)
                {
                    string minionName = minionInformation[1];
                    command.Parameters.AddWithValue("@nam", minionName);
                    int minionAge = int.Parse(minionInformation[2]);
                    command.Parameters.AddWithValue("@age", minionAge);
                    int townId = (int)currentTown;
                    command.Parameters.AddWithValue("@townId", currentTown);

                    int addedMinion = command.ExecuteNonQuery();

                    if (addedMinion < 1)
                    {
                        Console.WriteLine("Something went wrong while inserting new minion into database");
                    }
                }
            }

            connection = new SqlConnection(ConnectionString);
            connection.Open();

            object minionIdObj = null;

            using (connection)
            {
                string queryText = "SELECT Id FROM Minions WHERE Name = @Name";

                SqlCommand command = new SqlCommand(queryText,connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@Name",minionInformation[1]);
                    minionIdObj = command.ExecuteScalar();
                }
            }

            connection = new SqlConnection(ConnectionString);
            connection.Open();

            using (connection)
            {
                string queryText = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId,@villainId)";

                SqlCommand command = new SqlCommand(queryText,connection);

                using (command)
                {
                    int villainId = (int)currentVillain;
                    command.Parameters.AddWithValue("@villainId", villainId);
                    int minionId = (int)minionIdObj;
                    command.Parameters.AddWithValue("@minionId",minionId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Successfully added {minionInformation[1]} to be minion of {villainInformation[1]}.");
                    }
                }
            }

        }
    }
}
