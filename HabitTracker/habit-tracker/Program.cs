using System.Globalization;
using habit_tracker;
using Microsoft.Data.Sqlite;

public class Program
{
    static string connectionString = @"Data Source=habit-Tracker.db";
    public static void Main(string[] args)
    {
        //Tracker tracker = new();
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        var tableCmd = connection.CreateCommand();

        tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Date TEXT,
        Quantity INTEGER
        )";

        tableCmd.ExecuteNonQuery();

        connection.Close();


        Tracker.GetUserInput();
    }
    
}

public class DrinkingWater
{
    public int Id {get; set;}
    public DateTime Date {get; set;}
    public int Quantity {get; set;}
}