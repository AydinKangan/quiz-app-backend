public class AnswerModel
{
    public string? Id { get; set; }
    public int QuestionId { get; set; }

    public string? Answer { get; set; }

    public AnswerModel()
    {
        // This is required for Entity Framework
    }

    public AnswerModel(string id, int question_id, string answer)
    {
        Id = id;
        QuestionId = question_id;
        Answer = answer;
    }

}