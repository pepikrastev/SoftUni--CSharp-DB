using System;
using System.Data.SqlClient;
using System.Linq;

namespace _09.IncreaseAgeStoredProcedure
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
                    string executeProcQuery = @"EXEC usp_GetOlder @id";

                    using (SqlCommand command = new SqlCommand(executeProcQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

                string getMinionsQuery = @"SELECT Name, Age FROM Minions WHERE Id = @Id";

                using (SqlCommand secondCommand = new SqlCommand(getMinionsQuery, connection))
                {
                    using (SqlDataReader reader = secondCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{(string)reader[0]} – {(string)reader[1]} years old");
                        }
                    }
                }
            }
        }
    }
}
