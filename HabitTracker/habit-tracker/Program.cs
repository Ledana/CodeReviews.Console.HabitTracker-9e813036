using System.Globalization;
using habit_tracker;
using Microsoft.Data.Sqlite;

public class Program
{
    static string connectionString = @"Data Source=habit-Tracker.db";
    public static void Main(string[] args)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habits (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT NOT NULL,
        Date TEXT,
        Quantity INTEGER,
        Unit Text
        )";

        tableCmd.ExecuteNonQuery();
        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM habits";
        long rowCount = (long)checkCmd.ExecuteScalar();

        if (rowCount == 0)
            SeedDatabase();

        connection.Close();

        Tracker.GetUserInput();
    }
    public static void SeedDatabase()
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO habits (name, date, quantity, unit) VALUES ('running', '04-04-26', 1, 'hours');
        INSERT INTO habits (name, date, quantity, unit) VALUES ('meditation', '04-04-26', 20, 'minutes');
        INSERT INTO habits (name, date, quantity, unit) VALUES ('coding', '04-04-26', 7, 'hours');
        INSERT INTO habits (name, date, quantity, unit) VALUES ('reading', '04-04-26', 40, 'minutes');
        ";
        command.ExecuteNonQuery();
    }
    
}
public class Habit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public string Unit { get; set; }
}