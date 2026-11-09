using Microsoft.Data.SqlClient;

// Подготовка окружения преподавателя. Студентам этот код разбирать не требуется.
using (var server = new SqlConnection(LabDb.CreateConnectionString("master")))
{
    server.Open();
    using var createDatabase = new SqlCommand(
        "IF DB_ID(N'CollegeAdo') IS NULL EXEC(N'CREATE DATABASE CollegeAdo;');",
        server);
    createDatabase.CommandTimeout = 60;
    createDatabase.ExecuteNonQuery();
}

using var database = new SqlConnection(LabDb.ConnectionString);
database.Open();

string path = Path.Combine(AppContext.BaseDirectory, "sql", "01-init.sql");
string sql = File.ReadAllText(path);
using var createTables = new SqlCommand(sql, database);
createTables.CommandTimeout = 60;
createTables.ExecuteNonQuery();

Console.WriteLine("База CollegeAdo готова. Теперь выполните: dotnet run --project practice");
