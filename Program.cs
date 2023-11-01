using DotNetEnv;
  
Env.Load();
var frontEndUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["Access-Control-Allow-Origin"] = frontEndUrl;

    if (HttpMethods.IsOptions(ctx.Request.Method))
    {
        ctx.Response.Headers["Access-Control-Allow-Headers"] = "*";
        ctx.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE";

        await ctx.Response.CompleteAsync();
        return;
    }

    await next();
});

// Old testing apis.
app.MapGet("/get-answers", () => AnswerRepository.GetAnswers());
app.MapGet("/get-questions", () => QuestionRepository.GetAllQuestions());
app.MapGet("/get-questions-with-answers", () => QuestionsWithAnswersRepository.GetQuestionsWithAnswers());
app.MapGet("/get-random-question", () => RandomQuestion.GetRandomQuestion());
app.MapGet("/get-new-random-question", () => QuestionRepository.GetARandomQuestion());
app.MapGet("/correct-answer-{id}", (int id) => QuestionRepository.GetCorrectAnswerId(id));
app.MapPost("/add-results", (ResultsModel ResultsData) => ResultsRepository.InsertResults(ResultsData.UserId, ResultsData.CorrectAnswers, ResultsData.TotalAnswers));

// Fetch the leaderboard.
app.MapGet("/get-leaderboard", () => UserRepository.GetTopUsers(5));

// Make sure username exists.
app.MapPost("/get-validation", (UsernameModel data) => UserRepository.GetUserExist(data.Username));

// Get questions.
app.MapGet("/get-random-questions", () => QuestionRepository.GetRandomQuestions(5));

//Check if answers are correct.
app.MapPost("/check-answers", (CheckWithUser data) => CheckAnswer.CheckAnswers(data.CheckAnswers, data.UserId));

// Update user data after a test.
app.MapPost("/update-user-data", (UpdateUserModel data) => UserRepository.UpdateUserStats(data.Id, data.CorrectAnswers));

// Post request to show the past 5 results in the dashboard.
app.MapPost("/get-past-results", (UserIdModel data) => ResultsRepository.GetPastResults(data.UserId, 5));

app.Run();