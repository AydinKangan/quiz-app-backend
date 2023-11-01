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


// Old testing apis.  -- Comment out/get rid of unused endpoints
app.MapGet("/get-answers", () => AnswerRepository.GetAnswers());
app.MapGet("/get-questions", () => QuestionRepository.GetAllQuestions());
app.MapGet("/get-questions-with-answers", () => QuestionsWithAnswersRepository.GetQuestionsWithAnswers());
app.MapGet("/get-random-question", () => RandomQuestion.GetRandomQuestion());
app.MapGet("/get-new-random-question", () => QuestionRepository.GetARandomQuestion());
app.MapGet("/correct-answer-{id}", (int id) => QuestionRepository.GetCorrectAnswerId(id));

// Get Leaderboard, the top 5 users by correct answers
app.MapGet("/get-leaderboard", () => UserRepository.GetTopUsers(5));

// Check username exists.
app.MapPost("/get-validation", (UsernameModel user) => UserRepository.GetUserExist(user.Username));

// Get questions.  Gets 5 questions in one request.  
app.MapGet("/get-random-questions", () => QuestionRepository.GetRandomQuestions(5));

//Check if answers are correct.  Returns all 5 questions and results
app.MapPost("/check-answers", (CheckWithUser data) => CheckAnswer.CheckAnswers(data.CheckAnswers, data.UserId));

// Update user data after a quiz.
app.MapPost("/update-user-data", (UpdateUserModel data) => UserRepository.UpdateUserStats(data.Id, data.CorrectAnswers));

app.Run();
