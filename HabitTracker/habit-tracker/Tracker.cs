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
            //Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to update or type 0 to retun to main menu. \n\n");

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE Id = '{recordId}')";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\n\nRecord with Id '{recordId}' doesn't exist.\n\n");
                connection.Close();
                Update();
            }
            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");


            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"UPDATE drinking_water SET Date = '{date}', Quantity = '{quantity}' WHERE Id = '{recordId}'";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        private static void Delete()
        {
            //Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the Id of the record you want to delete or type 0 to retun to main menu. \n\n");

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = '{recordId}'";

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
            //Console.Clear();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT * FROM drinking_water";
            List<DrinkingWater> tableData = [];

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
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
                Console.WriteLine($"Id: {item.Id} | Date: {item.Date.ToString("dd-MM-yy")} | Quantity: {item.Quantity}");
            }
            Console.WriteLine("-------------------------------\n");
        }

        private static void Insert()
        {
            string date = GetDateInput();

            int quantity = GetNumberInput("\n\nPlease insert number of glasses or other measure of your choice (no decimals allowed)\n\n");

            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }

        internal static int GetNumberInput(string message)
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

        internal static string GetDateInput()
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
