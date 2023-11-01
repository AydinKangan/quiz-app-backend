public class ResultsModel
{
    public int UserId { get; set; }
    public int CorrectAnswers { get; set; }

    public int TotalAnswers { get; set; }

    public DateTime? CreatedAt { get; set; }


    public ResultsModel()
    { }

    public ResultsModel(int userId, int correctAnswers, int totalAnswers, DateTime createdAt)
    {
        UserId = userId;
        CorrectAnswers = correctAnswers;
        TotalAnswers = totalAnswers;
        CreatedAt = createdAt;
    }

}

public class PastResultsModel
{
    public int CorrectAnswers { get; set; }

    public int TotalAnswers { get; set; }

    public DateTime? CreatedAt { get; set; }


    public PastResultsModel()
    { }

    public PastResultsModel(int correctAnswers, int totalAnswers, DateTime createdAt)
    {
        CorrectAnswers = correctAnswers;
        TotalAnswers = totalAnswers;
        CreatedAt = createdAt;
    }

}