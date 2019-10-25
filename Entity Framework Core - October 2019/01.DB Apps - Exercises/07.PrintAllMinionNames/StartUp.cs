using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace _07.PrintAllMinionNames
{
    public class StartUp
    {
        private static string connectionString =
            "Server=.\\SQLEXPRESS; Database=MinionsDB; Integrated Security=True";

        private static SqlConnection connection = new SqlConnection(connectionString);

        public static void Main(string[] args)
        {
            using (connection)
            {
                connection.Open();

                List<string> minions = new List<string>();

                string getMinionsNamesQuery = @"SELECT Name FROM Minions";

                using (SqlCommand command = new SqlCommand(getMinionsNamesQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            minions.Add((string)reader[0]);
                        }
                    }
                }

                for (int i = 0; i < minions.Count / 2; i++)
                {
                  
                    Console.WriteLine(minions[i]);
                    Console.WriteLine(minions[minions.Count - i - 1] );
                }

                if (minions.Count % 2 != 0)
                {
                    Console.WriteLine(minions[minions.Count / 2]);
                }
            }
        }
    }
}
