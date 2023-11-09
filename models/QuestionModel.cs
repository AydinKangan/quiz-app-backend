public class QuestionModel
{
    public int? Id { get; set; }
    public string? Question { get; set; }
    public string? CorrectAnswerId { get; set; }
    public List<AnswerModel>? Answers { get; set; }


    public QuestionModel()
    {
        // This is required for Entity Framework
    }

    public QuestionModel(int id, string question, string correct_answer_id)
    {
        Id = id;
        Question = question;
        CorrectAnswerId = correct_answer_id;
        Answers = new List<AnswerModel>();
    }
}

public class OtherGroupQuestion
{
    public int? Id { get; set; }
    public string? Question { get; set; }
    public string? CorrectAnswerId { get; set; }
    public List<string>? Options { get; set; }


    public OtherGroupQuestion()
    {
        // This is required for Entity Framework
    }

    public OtherGroupQuestion(int id, string question, string correct_answer_id, List<string> options)
    {
        Id = id;
        Question = question;
        CorrectAnswerId = correct_answer_id;
        options = new List<string>();
    }
}