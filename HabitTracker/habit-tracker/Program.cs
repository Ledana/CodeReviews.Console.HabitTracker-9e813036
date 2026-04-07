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

        connection.Close();

        Tracker.GetUserInput();
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
public class DrinkingWater
{
    public int Id {get; set;}
    public DateTime Date {get; set;}
    public int Quantity {get; set;}
}