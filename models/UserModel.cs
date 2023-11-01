public class UserModel
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public int CorrectAnswers { get; set; }

    public int Attempts { get; set; }


    public UserModel()
    { }

    public UserModel(int id, string username, int correctAnswers, int attempts)
    {
        Id = id;
        Username = username;
        CorrectAnswers = correctAnswers;
        Attempts = attempts;
    }

}

public class UsernameModel
{
    public string Username { get; set; }



    public UsernameModel(string username)
    {
        Username = username;
    }

}

public class UpdateUserModel
{
    public int Id { get; set; }
    public int CorrectAnswers { get; set; }



    public UpdateUserModel(int id, int correctAnswers)
    {
        Id = id;
        CorrectAnswers = correctAnswers;
    }

}

public class TopUserModel
{
    public string? Username { get; set; }
    public int CorrectAnswers { get; set; }

    public int Attempts { get; set; }


    public TopUserModel()
    { }

    public TopUserModel(string username, int correctAnswers, int attemps)
    {
        Username = username;
        CorrectAnswers = correctAnswers;
        Attempts = attemps;
    }
}

public class UserIdModel
{
    public int UserId { get; set; }



    public UserIdModel(int userId)
    {
        UserId = userId;
    }

}