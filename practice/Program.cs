using Microsoft.Data.SqlClient;

// 1. Подключаемся к готовой учебной базе.
using var connection = new SqlConnection(LabDb.ConnectionString);
connection.Open();

// 2. Готовим запрос: получить имя и группу каждого студента.
string sql = "SELECT FullName, GroupName FROM dbo.Students ORDER BY StudentId;";
using var command = new SqlCommand(sql, connection);

// 3. Выполняем запрос.
using var reader = command.ExecuteReader();

// 4. Читаем строки результата по одной.
while (reader.Read())
{
    string name = reader.GetString(0);
    string group = reader.GetString(1);

    Console.WriteLine(name + " — " + group);
}
