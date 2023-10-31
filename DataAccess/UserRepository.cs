using Npgsql;

public class UserRepository
{
    public static UserModel GetUserExist(string findUsername)
    {
        UserModel? user = null;
        using (var conn = DbConnection.GetDbConnection())
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM \"user-list\" WHERE username = @findUsername";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("findUsername", findUsername);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserModel
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return user!;
    }

    public static string UpdateUserStats(int Id, int correctAnswers)
    {
        using (var conn = DbConnection.GetDbConnection())
        {
            try
            {
                conn.Open();

                // First, retrieve the user's current statistics.
                string selectQuery = "SELECT * FROM \"user-list\" WHERE id = @Id";
                using (var selectCmd = new NpgsqlCommand(selectQuery, conn))
                {
                    selectCmd.Parameters.AddWithValue("Id", Id);
                    using (var reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int currentId = reader.GetInt32(0);
                            int currentCorrectAnswers = reader.GetInt32(2); // Assuming the correct_answers column is the third column (0-based index).
                            int currentAttempts = reader.GetInt32(3); // Assuming the attempts column is the fourth column (0-based index);

                            // Close the reader before executing the UPDATE command.
                            reader.Close();

                            // Update the user's statistics.
                            int newCorrectAnswers = currentCorrectAnswers + correctAnswers;
                            int newAttempts = currentAttempts + 1;

                            // Now, update the user's statistics in the database.
                            string updateQuery = "UPDATE \"user-list\" SET correct_answers = @newCorrectAnswers, attempts = @newAttempts WHERE id = @currentId";
                            using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("newCorrectAnswers", newCorrectAnswers);
                                updateCmd.Parameters.AddWithValue("newAttempts", newAttempts);
                                updateCmd.Parameters.AddWithValue("currentId", currentId);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return "Updated";
    }

     public static List<TopUserModel> GetTopUsers(int numberOfUsers)
    {
        List<TopUserModel> topUsers = new List<TopUserModel>();

        using (var conn = DbConnection.GetDbConnection())
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM \"user-list\" ORDER BY correct_answers DESC LIMIT @numberOfUsers";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("numberOfUsers", numberOfUsers);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TopUserModel user = new TopUserModel
                            {
                                Username = reader.GetString(1),
                                CorrectAnswers = reader.GetInt32(2), 
                                Attempts = reader.GetInt32(3) 
                            };
                            topUsers.Add(user);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return topUsers;
    }
}
