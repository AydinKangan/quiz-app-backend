public class CheckedModel
{
    public string? Question { get; set; }

    public bool IsCorrect { get; set; }
    public CheckedModel()
    {
        // This is required for Entity Framework
    }

    public CheckedModel(string question, bool isCorrect)
    {
        Question = question;
        IsCorrect = isCorrect;
    }

}