
using Npgsql;

public class QuestionRepository
{
    public static List<QuestionModel> GetAllQuestions()
    {
        List<QuestionModel> questions = new List<QuestionModel>();
        using (var conn = DbConnection.GetDbConnection())
        {
            try
            {
                conn.Open();
                string query = "SELECT id AS Id, question AS Question, correct_answer_id AS CorrectAnswerId FROM \"new-questions\"";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new QuestionModel(reader.GetInt16(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return questions;
    }

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


    public static QuestionModel GetARandomQuestion()
    {
        QuestionModel randomQuestion = new QuestionModel();
        using (var conn = DbConnection.GetDbConnection())
        {
            try
            {
                conn.Open();
                string questionQuery = "SELECT id AS Id, question AS Question, correct_answer_id AS CorrectAnswerId FROM \"new-questions\" ORDER BY random() LIMIT 1";

                using (var cmd = new NpgsqlCommand(questionQuery, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            randomQuestion = new QuestionModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                        }
                    }
                }

                string questionsQuery = "SELECT id AS Id, question_id AS QuestionId, answer AS Answer FROM \"new-answers\" WHERE question_id = @QuestionId";
                using (var cmd = new NpgsqlCommand(questionsQuery, conn))
                {
                    // Replace @QuestionId with the actual value
                    cmd.CommandText = cmd.CommandText.Replace("@QuestionId", randomQuestion.Id.ToString());

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            randomQuestion.Answers ??= new List<AnswerModel>();
                            randomQuestion.Answers.Add(new AnswerModel(reader.GetString(0), reader.GetInt32(1), reader.GetString(2)));

                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        return randomQuestion;
    }
}
