using Npgsql;
public class ResultsRepository
{
    public static string InsertResults(int userId, int correctAnswers, int totalAnswers)
    {
        using (var conn = DbConnection.GetDbConnection())
        {
            try
            {
                conn.Open();
                string insertQuery = "INSERT INTO \"past-results\" (user_id, correct_answers, total_answers) VALUES (@userId, @correctAnswers, @totalAnswers)";
                using (var insertCmd = new NpgsqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("userId", userId);
                    insertCmd.Parameters.AddWithValue("correctAnswers", correctAnswers);
                    insertCmd.Parameters.AddWithValue("totalAnswers", totalAnswers);

                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return "New result inserted";
                    }
                    else
                    {
                        return "Failed to insert results.";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Failed to insert results.";
            }
        }
    }

    public static List<PastResultsModel> GetPastResults(int userId, int numberOfResults)
    {
        List<PastResultsModel> pastResults = new List<PastResultsModel>();

        using (var conn = DbConnection.GetDbConnection())
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM \"past-results\" WHERE user_id = @userId ORDER BY created_at DESC LIMIT @numberOfResults";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("userId", userId);
                    cmd.Parameters.AddWithValue("numberOfResults", numberOfResults);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PastResultsModel pastResult = new PastResultsModel
                            {
                                CreatedAt = reader.GetDateTime(1),
                                CorrectAnswers = reader.GetInt32(3),
                                TotalAnswers = reader.GetInt32(4)
                            };
                            pastResults.Add(pastResult);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return pastResults;
    }


}

