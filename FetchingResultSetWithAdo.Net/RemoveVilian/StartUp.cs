using System;
using System.Data.SqlClient;

namespace RemoveVilian
{
    class StartUp
    {
        private static string ConnectionString =
                           "Server = DESKTOP-OU2Q3NF\\SQLEXPRESS;" +
                           "Database = MinionsDB;" +
                           "Integrated Security = true;";

        private static SqlConnection connection = new SqlConnection(ConnectionString);

        private static SqlTransaction transaction;
        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());
            connection.Open();

            using (connection)
            {
                transaction = connection.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;
                    cmd.CommandText = "SELECT Name FROM Villains WHERE Id = @villainId";
                    cmd.Parameters.AddWithValue("@villainId", id);

                    object value = cmd.ExecuteScalar();

                    if (value == null)
                    {
                        throw new ArgumentException("no such villain was found");
                    }

                    string villainName = (string)value;

                    cmd.CommandText = @"DELETE FROM MinionsVillains 
                                        WHERE VillainId = @villainId";

                    int minionsDeleted = cmd.ExecuteNonQuery();

                    cmd.CommandText = @"DELETE FROM Villains
                                        WHERE Id = @villainId";

                    cmd.ExecuteNonQuery();

                    transaction.Commit();

                    Console.WriteLine($"{villainName} was deleted.");
                    Console.WriteLine($"{minionsDeleted} minions were released");
                }
                catch (ArgumentException ane)
                {

                    try
                    {
                        Console.WriteLine(ane.Message);
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }
                }
                catch(Exception e)
                {
                    try
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                    catch (Exception re)
                    {

                        Console.WriteLine(re.Message);
                    }
                }

            }
        }
    }
}
