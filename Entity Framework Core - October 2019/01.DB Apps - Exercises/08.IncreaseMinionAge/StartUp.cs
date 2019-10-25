using System;
using System.Data.SqlClient;
using System.Linq;

namespace _08.IncreaseMinionAge
{
    public class StartUp
    {
        private static string connectionString =
            "Server=.\\SQLEXPRESS; Database=MinionsDB; Integrated Security=True";

        private static SqlConnection connection = new SqlConnection(connectionString);

        public static void Main(string[] args)
        {
            int[] minionsIds = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();

            using (connection)
            {
                connection.Open();

                foreach (var id in minionsIds)
                {
                    string updateMinionQuery = @"UPDATE Minions
                                     SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                     WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(updateMinionQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }

                string getMinionsQuery = $"SELECT Name, Age FROM Minions";

                using (SqlCommand secondCommand = new SqlCommand(getMinionsQuery, connection))
                {
                    using (SqlDataReader reader = secondCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{(string)reader[0]} {(int)reader[1]}");
                        }
                    }
                }
            }
        }
    }
}
