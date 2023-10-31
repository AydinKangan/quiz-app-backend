using Npgsql;
using DotNetEnv;
public class DbConnection
{
    public static NpgsqlConnection GetDbConnection()
    {


        Env.Load();
        var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
        var password = Environment.GetEnvironmentVariable("SUPABASE_PASSWORD");
        return new NpgsqlConnection($"User Id=postgres;Password={password};Server={url};Port=5432;Database=postgres");
    }
}
