using DocMaster.UI;
using Microsoft.Data.Sqlite;
public static class Program
{
    public static void InitializeDatabase()
    {
        // Путь к файлу БД (можно изменить)
        string dbPath = Path.Combine(Environment.CurrentDirectory, "DocMaster.db");

        using var connection = new SqliteConnection($"Data Source={dbPath}");
        connection.Open();

        // Создаем таблицу, если ее нет
        var command = connection.CreateCommand();
        command.CommandText = @"
        CREATE TABLE IF NOT EXISTS Documents (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Format TEXT NOT NULL,
            Content TEXT NOT NULL,
            CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
        )";
        command.ExecuteNonQuery();
    }
    public static void Main(string[] args)
    {
        InitializeDatabase();
        App app = new App();
        app.Run();
    }
}