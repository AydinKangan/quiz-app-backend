public class CheckWithUser
{
    public ToCheckModel[] CheckAnswers { get; set; }

    public int UserId { get; set; }



    public CheckWithUser(ToCheckModel[] checkAnswers, int userId)
    {
        CheckAnswers = checkAnswers;
        UserId = userId;

    }

}