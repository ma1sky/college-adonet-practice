using Microsoft.Data.SqlClient;

public static class LabDb
{
    public static string ConnectionString => CreateConnectionString("CollegeAdo");

    public static string CreateConnectionString(string database)
    {
        string? password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "MSSQL_SA_PASSWORD не задан. Запустите проект в Codespaces с конфигурацией репозитория.");
        }

        return new SqlConnectionStringBuilder
        {
            DataSource = "db,1433",
            InitialCatalog = database,
            UserID = "sa",
            Password = password,
            Encrypt = true,
            TrustServerCertificate = true,
            ConnectTimeout = 15
        }.ConnectionString;
    }
}
