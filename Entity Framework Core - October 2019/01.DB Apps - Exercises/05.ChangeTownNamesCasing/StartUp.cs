using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _05.ChangeTownNamesCasing
{
    public class StartUp
    {
        private static string connectionString =
            "Server=.\\SQLEXPRESS; Database=MinionsDB; Integrated Security=True";

        private static SqlConnection connection = new SqlConnection(connectionString);

        public static void Main(string[] args)
        {
            string countryName = Console.ReadLine();

            using (connection)
            {
                connection.Open();

                string updatesTownsNamesQuery = @"UPDATE Towns
                                                  SET Name = UPPER(Name)
                                                  WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                using (SqlCommand command = new SqlCommand(updatesTownsNamesQuery, connection))
                {
                    command.Parameters.AddWithValue("@countryName", countryName);

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows == 0)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                    else
                    {
                        Console.WriteLine($"{affectedRows} town names were affected. ");

                        string getTownsNamesQuery = @" SELECT t.Name 
                                                        FROM Towns as t
                                                        JOIN Countries AS c ON c.Id = t.CountryCode
                                                       WHERE c.Name = @countryName";

                        using (SqlCommand secondCommand = new SqlCommand(getTownsNamesQuery, connection))
                        {
                            secondCommand.Parameters.AddWithValue("@countryName", countryName);

                            using (SqlDataReader reader = secondCommand.ExecuteReader())
                            {
                                List <string> towns = new List<string>();

                                while (reader.Read())
                                {
                                    towns.Add((string)reader[0]);
                                }

                                Console.WriteLine($"[{string.Join(", ", towns)}]");
                            }
                        }
                    }
                }
            }
        }
    }
}
