using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

try
{
    string action = args.Length == 0 ? "list" : args[0].ToLowerInvariant();

    switch (action)
    {
        case "init":
        {
            using var connection = new SqlConnection(LabDb.CreateConnectionString("master"));
            connection.Open();
            string path = Path.Combine(AppContext.BaseDirectory, "sql", "01-init.sql");
            RunScript(connection, path, printResults: false);
            Console.WriteLine("База CollegeAdo готова.");
            break;
        }
        case "count":
        {
            using var connection = new SqlConnection(LabDb.ConnectionString);
            using var command = new SqlCommand("SELECT COUNT(*) FROM dbo.Students;", connection);
            connection.Open();
            int count = (int)command.ExecuteScalar()!;
            Console.WriteLine($"Количество студентов: {count}");
            break;
        }
        case "list":
        {
            string? group = args.Length > 1 ? args[1] : null;
            if (group is { Length: > 20 })
            {
                throw new ArgumentException("Группа должна содержать не более 20 символов.");
            }

            using var connection = new SqlConnection(LabDb.ConnectionString);
            using var command = new SqlCommand(
                "SELECT StudentId, FullName, GroupName, Email FROM dbo.Students " +
                "WHERE (@group IS NULL OR GroupName = @group) ORDER BY StudentId;",
                connection);
            command.Parameters.Add("@group", SqlDbType.NVarChar, 20).Value =
                group is null ? DBNull.Value : (object)group;
            connection.Open();
            using var reader = command.ExecuteReader();
            bool found = false;
            while (reader.Read())
            {
                found = true;
                string email = reader.IsDBNull(3) ? "не указана" : reader.GetString(3);
                Console.WriteLine(
                    $"{reader.GetInt32(0)}: {reader.GetString(1)}, {reader.GetString(2)}, почта: {email}");
            }

            if (!found)
            {
                Console.WriteLine("Студенты не найдены.");
            }
            break;
        }
        case "sql" when args.Length == 2:
        {
            using var connection = new SqlConnection(LabDb.ConnectionString);
            connection.Open();
            RunScript(connection, args[1], printResults: true);
            break;
        }
        default:
            throw new ArgumentException("Команды: init | count | list [группа] | sql путь-к-файлу.sql");
    }
}
catch (Exception ex) when (ex is SqlException or InvalidOperationException or ArgumentException or IOException)
{
    Console.Error.WriteLine($"Ошибка: {ex.Message}");
    Environment.ExitCode = 1;
}

static void RunScript(SqlConnection connection, string path, bool printResults)
{
    string script = File.ReadAllText(path);
    // Учебный разделитель GO на отдельной строке. GO с числом повторов не поддерживается.
    string[] batches = Regex.Split(script, @"(?im)^\s*GO\s*(?:--[^\r\n]*)?$");

    foreach (string batch in batches)
    {
        if (string.IsNullOrWhiteSpace(batch))
        {
            continue;
        }

        using var command = new SqlCommand(batch, connection) { CommandTimeout = 60 };
        using var reader = command.ExecuteReader();
        do
        {
            if (printResults && reader.FieldCount > 0)
            {
                var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName);
                Console.WriteLine(string.Join(" | ", names));
            }

            while (reader.Read())
            {
                if (printResults)
                {
                    var values = Enumerable.Range(0, reader.FieldCount).Select(index =>
                        reader.IsDBNull(index)
                            ? "NULL"
                            : Convert.ToString(reader.GetValue(index), CultureInfo.InvariantCulture));
                    Console.WriteLine(string.Join(" | ", values));
                }
            }
        }
        while (reader.NextResult());
    }
}
