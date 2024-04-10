using System.Data.SqlClient;
using System.Data.SQLite;

var connectionString = "Data Source=peopledb.sqlite;Version=3;";

InitializeDb(connectionString);

while (true)
{
    Console.WriteLine("Please choose an option:");
    Console.WriteLine("1 - to add a new person");
    Console.WriteLine("2 - to view all the people");
    Console.WriteLine("3 - to update a person");
    Console.WriteLine("4 - to delete a person");
    Console.WriteLine("5 - EXIT");
    Console.WriteLine();
    var input = Console.ReadLine();
    switch (input)
    {
        case "1":
            AddPerson(connectionString);
            break;
        case "2":
            ViewAll(connectionString);
            break;
        case "3":
            UpdatePerson(connectionString);
            break;
        case "4":
            DeletePerson(connectionString);
            break;
        case "5":
            Environment.Exit(0);
            break;
    }
}

static void InitializeDb(string connectionString)
{
    using var connection = new SQLiteConnection(connectionString);
    connection.Open();
    const string query = @"CREATE TABLE IF NOT EXISTS people (Id INTEGER PRIMARY KEY,Name Text, Age INTEGER)";
    var command = new SQLiteCommand(query, connection);
    command.ExecuteNonQuery();
}

static void AddPerson(string connectionString)
{
    Console.WriteLine("Please enter name:");
    var name = Console.ReadLine();
    Console.WriteLine("Please enter age:");
    var age = Convert.ToInt32(Console.ReadLine());

    using var connection = new SQLiteConnection(connectionString);
    connection.Open();
    const string query = "INSERT INTO people(Name, Age) VALUES(@Name, @Age)";
    var command = new SQLiteCommand(query, connection);
    command.Parameters.AddWithValue("@Name", name);
    command.Parameters.AddWithValue("@Age", age);
    command.ExecuteNonQuery();
    Console.WriteLine();
}

static void ViewAll(string connectionString)
{
    using var connection = new SQLiteConnection(connectionString);
    connection.Open();
    const string query = "SELECT * FROM people";
    var command = new SQLiteCommand(query, connection);
    var reader = command.ExecuteReader();

    Console.WriteLine("\nId\tName\tAge");
    while (reader.Read())
    {
        Console.WriteLine($"{reader["Id"]}\t{reader["Name"]}\t{reader["Age"]}");
    }

    Console.WriteLine("\n");
}

static void UpdatePerson(string connectionString)
{
    Console.WriteLine("Enter an Id of a person, which you want to update:");
    var id = int.Parse(Console.ReadLine()!);
    Console.WriteLine("Enter a new name:");
    var name = Console.ReadLine();
    Console.WriteLine("Enter a new age");
    var age = int.Parse(Console.ReadLine()!);

    using var connection = new SQLiteConnection(connectionString);
    connection.Open();

    const string query = "UPDATE people SET Name = @Name, Age = @Age WHERE Id = @Id";
    var command = new SQLiteCommand(query, connection);
    command.Parameters.AddWithValue("@Name", name);
    command.Parameters.AddWithValue("@Age", age);
    command.Parameters.AddWithValue("@Id", id);
    var rowsUpdated = command.ExecuteNonQuery();

    Console.WriteLine(rowsUpdated > 0 ? "Record updated successfully." : "Record not found.");
}

static void DeletePerson(string connectionString)
{
    Console.WriteLine("Enter an Id of a person you want to delete.");
    var id = int.Parse(Console.ReadLine()!);

    using var connection = new SQLiteConnection(connectionString);
    connection.Open();
    const string query = "DELETE FROM people WHERE ID = @Id";
    var command = new SQLiteCommand(query, connection);
    command.Parameters.AddWithValue("@Id", id);
    var rowsDeleted = command.ExecuteNonQuery();

    Console.WriteLine(rowsDeleted > 0 ? "Record has been deleted," : "Record not found.");
}