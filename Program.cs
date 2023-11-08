using DotNetEnv;

Env.Load();
var frontEndUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// ----- Look at potential cleaner CORS implementation
app.Use(async (ctx, next) =>
{
    ctx.Response.Headers["Access-Control-Allow-Origin"] = frontEndUrl;

    if (HttpMethods.IsOptions(ctx.Request.Method))
    {
        ctx.Response.Headers["Access-Control-Allow-Headers"] = "*";
        ctx.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE";  //-- Get rid of PUT and DELETE ??

        await ctx.Response.CompleteAsync();
        return;
    }

    await next();
});
// --------------------------------------------------------------

// Fetch the leaderboard.
app.MapGet("/get-leaderboard", () => UserRepository.GetTopUsers(5));

// Make sure username exists.
app.MapPost("/get-validation", (UsernameModel data) => UserRepository.GetUserExist(data.Username));

// Get questions.  Gets 5 questions in one request.  
app.MapGet("/get-random-questions", () => QuestionRepository.GetRandomQuestions(5));

//Check if answers are correct.  Returns all 5 questions and results
app.MapPost("/check-answers", (CheckWithUser data) => CheckAnswer.CheckAnswers(data.CheckAnswers, data.UserId));

// Update user data after a quiz.
app.MapPost("/update-user-data", (UpdateUserModel data) => UserRepository.UpdateUserStats(data.Id, data.CorrectAnswers));

// Post request to show the past 5 results in the dashboard.
app.MapPost("/get-past-results", (UserIdModel data) => ResultsRepository.GetPastResults(data.UserId, 5));

app.Run();
