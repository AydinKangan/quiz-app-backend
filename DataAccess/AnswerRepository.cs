using Npgsql;

public class AnswerRepository
{
    public static List<AnswerModel> GetAnswers()
{
    List<AnswerModel> answers = new List<AnswerModel>();
    using (var conn = DbConnection.GetDbConnection())
    {
        try
        {
            conn.Open();
            string query = "SELECT id AS Id, question_id AS QuestionId, answer AS Answer FROM \"new-answers\"";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        answers.Add(new AnswerModel(reader.GetString(0), reader.GetInt32(1), reader.GetString(2)));
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    return answers;

}
}
