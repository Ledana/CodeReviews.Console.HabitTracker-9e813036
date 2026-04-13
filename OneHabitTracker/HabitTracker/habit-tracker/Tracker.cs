using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace habit_tracker
{
    public class Tracker
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        public static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View All Records.");
                Console.WriteLine("Type 2 to Insert Record.");
                Console.WriteLine("Type 3 to Delete Record.");
                Console.WriteLine("Type 4 to Update Record.");
                Console.WriteLine("----------------------------------\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        Insert();
                        break;
                    case "3":
                        Delete();
                        break;
                    case "4":
                        Update();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;

                }
            }
        }

        private static void Update()
        {
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to update or type 0 to retun to main menu. \n\n");

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM habits WHERE Id = '{recordId}')";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id '{recordId}' doesn't exist.\n\n");
                connection.Close();
                Update();
            }
            string title = GetTitleInput("Please put the name of the habit");
            string date = GetDateInput();
            string unit = GetUnitInput("Please put the unit");
            int quantity = GetNumberInput("\n\nPlease insert number of units (no decimals allowed)\n\n");

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE habits SET Name = '{title}', Date = '{date}', Quantity = '{quantity}', Unit = '{unit}' WHERE Id = '{recordId}'";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        private static void Delete()
        {
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to return to main menu. \n\n");

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM habits WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                Delete();
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

            GetUserInput();
        }

        private static void GetAllRecords()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM habits";
            List<Habit> tableData = [];

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new Habit
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(3),
                            Unit = reader.GetString(4)
                        }
                    );
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }
            connection.Close();

            Console.WriteLine("-------------------------------\n");
            foreach (var item in tableData)
            {
                Console.WriteLine($"Id: {item.Id} | Name: {item.Name} | Date: {item.Date.ToString("dd-MM-yy")} | Quantity: {item.Quantity} | Unit: {item.Unit}");
            }
            Console.WriteLine("-------------------------------\n");
        }

        private static void Insert()
        {
            string title = GetTitleInput("Please put the name of the habit");
            string date = GetDateInput();
            string unit = GetUnitInput("Please put the unit");
            int quantity = GetNumberInput("\n\nPlease insert number of units (no decimals allowed)\n\n");
            
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO habits(Name, Date, Quantity, Unit) VALUES('{title}', '{date}', {quantity}, '{unit}')";
            tableCmd.ExecuteNonQuery();
            connection.Close();
            //GetUserInput();
        }

        public static string GetTitleInput(string message)
        {
            Console.WriteLine(message);

            return Console.ReadLine();
        }
        public static string GetUnitInput(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);

            string numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();

            while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again\n\n");
                numberInput = Console.ReadLine();
            }
            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }

        public static string GetDateInput()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.\n\n");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main menu or try again.\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }
    }
}
