
using Npgsql;

public class QuestionRepository
{


    public static string GetCorrectAnswerId(int questionId)
    {
        string? correctAnswerId = "No answer found";

        using (var conn = DbConnection.GetDbConnection())
        {
            try
            {
                conn.Open();
                string query = "SELECT correct_answer_id FROM \"new-questions\" WHERE id = @questionId";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@questionId", questionId);

                    object? result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        correctAnswerId = result.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        if (correctAnswerId != null)
        {
            return correctAnswerId;
        }
        else
        {
            return "No answer found";
        }
    }


    public static List<QuestionModel> GetRandomQuestions(int numberOfQuestions)
    {
        List<QuestionModel> randomQuestions = new List<QuestionModel>();
        using (var conn = DbConnection.GetDbConnection())
        {
            try
            {
                conn.Open();

                string questionQuery = "SELECT id AS Id, question AS Question, correct_answer_id AS CorrectAnswerId FROM \"new-questions\" ORDER BY random() LIMIT @NumberOfQuestions";

                using (var cmd = new NpgsqlCommand(questionQuery, conn))
                {
                    cmd.Parameters.AddWithValue("NumberOfQuestions", numberOfQuestions);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var question = new QuestionModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                            randomQuestions.Add(question);
                        }
                    }
                }

                string answerQuery = "SELECT id AS Id, question_id AS QuestionId, answer AS Answer FROM \"new-answers\" WHERE question_id = @QuestionId";
                using (var cmd = new NpgsqlCommand(answerQuery, conn))
                {
                    foreach (var question in randomQuestions)
                    {
                        cmd.Parameters.AddWithValue("QuestionId", question.Id!);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                question.Answers ??= new List<AnswerModel>();
                                question.Answers.Add(new AnswerModel(reader.GetString(0), reader.GetInt32(1), reader.GetString(2)));
                            }
                        }

                        // shuffle the Answers list --> https://stackoverflow.com/questions/273313/randomize-a-listt
                        cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return randomQuestions;
    }


    public static async Task<List<OtherGroupQuestion>?> GetOtherGroupsQuestions()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://kanganquizapi1.azurewebsites.net/get5RandomQuestions");

        var response = await client.GetFromJsonAsync<List<OtherGroupQuestion>>(client.BaseAddress);

        Random random = new Random();

        if (response != null)
        {
            foreach (var question in response)
            {
                ShuffleOptions(random, question);
            }
        }

        return response;
    }

    private static void ShuffleOptions(Random random, OtherGroupQuestion question)
    {
        if (question != null && question.Options != null)
        {
            int n = question.Options.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                // Swap the Options[i] and Options[j]
                string temp = question.Options[i];
                question.Options[i] = question.Options[j];
                question.Options[j] = temp;
            }
        }
    }


}
