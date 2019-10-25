using System;
using System.Data.SqlClient;

namespace _04.AddMinion
{
    class StartUp
    {
        private static string connectionString =
            "Server=.\\SQLEXPRESS; Database=MinionsDB; Integrated Security=True";

        private static SqlConnection connection = new SqlConnection(connectionString);

        static void Main(string[] args)
        {
            string[] minionInput = Console.ReadLine().Split();
            string minionName = minionInput[1];
            int minionAge = int.Parse(minionInput[2]);
            string townName = minionInput[3];

            string[] villainInput = Console.ReadLine().Split();
            string villainName = villainInput[1];

            using (connection)
            {
                connection.Open();

                int? townId = GetTownId(connection, townName);

                if (townId == null)
                {
                    AddTown(connection, townName);
                    townId = GetTownId(connection, townName);
                }

                int? villainId = GetVillainId(connection, villainName);

                if (villainId == null)
                {
                    AddVillain(connection, villainName);
                    villainId = GetVillainId(connection, villainName);
                }

                AddMinion(connection, minionName, minionAge, (int)townId);
                int minionId = GetMinionId(connection, minionName);

                PutMinionToVillain(connection ,minionId, minionName, villainId, villainName);
            }
        }

        private static void PutMinionToVillain(SqlConnection connection, int minionId, string minionName, int? villainId, string villainName)
        {
            string putMinionToVillainQuery = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

            using (SqlCommand command = new SqlCommand(putMinionToVillainQuery, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                command.Parameters.AddWithValue("@minionId", minionId);

                int affectedRows = 0;

                try
                {
                    affectedRows = (int) command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    Console.WriteLine($"{minionName} is already a slave of {villainName}");
                }

                if (affectedRows > 0)
                {
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }

            }
        }

        private static int GetMinionId(SqlConnection connection, string name)
        {
            string getMinionIdQuery = @"SELECT Id FROM Minions WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(getMinionIdQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                int minionId = (int)command.ExecuteScalar();
                return minionId;
            }
        }

        private static void AddMinion(SqlConnection connection, string name, int age, int townId)
        {
            string insertMinionQuery = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";

            using (SqlCommand command = new SqlCommand(insertMinionQuery, connection))
            {
                command.Parameters.AddWithValue("@nam", name);
                command.Parameters.AddWithValue("@age", age);
                command.Parameters.AddWithValue("@townId", townId);

                command.ExecuteScalar();
            }
        }

        private static void AddVillain(SqlConnection connection, string name)
        {
            string insertVillainQuery = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(insertVillainQuery, connection))
            {
                command.Parameters.AddWithValue("@villainName", name);

                int affectedRows = (int) command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    Console.WriteLine($"Villain {name} was added to the database.");
                }
                else
                {
                    throw new InvalidOperationException("Someting when wrong when attempting to add a villain");
                }
            }
        }

        private static int? GetVillainId(SqlConnection connection, string name)
        {
            string getVillainIdQuery = @"SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(getVillainIdQuery, connection))
            {
                command.Parameters.AddWithValue("@Name", name);

                int? villainId = (int?) command.ExecuteScalar();
                return villainId;
            }
        }

        private static void AddTown(SqlConnection connection, string name)
        {
            string insertTownQuery = @"INSERT INTO Towns (Name) VALUES (@townName)";
            using (SqlCommand command = new SqlCommand(insertTownQuery, connection))
            {
                command.Parameters.AddWithValue("@townName", name);

                int affectedRows = (int)command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    Console.WriteLine($"Town {name} was added to the database.");
                }
                else
                {
                    throw  new InvalidOperationException("Someting when wrong when attempting to add a town");
                }
            }
        }

        private static int? GetTownId(SqlConnection connection, string name)
        {
            string getTownIdQuery = @"SELECT Id FROM Towns WHERE Name = @townName";

            using (SqlCommand command = new SqlCommand(getTownIdQuery, connection))
            {
                command.Parameters.AddWithValue("@townName", name);

                int? townId = (int?)command.ExecuteScalar();

                return townId;
            }
        }
    }
}
