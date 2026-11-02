/*
 * Uppgift 8
SQLi-Labbet (Den sårbara koden). Skapa een C#-konsolapplikation i Visual Studio.
Skriv en metod som tar en söksträng (e-post) via Console.ReadLine() och skickar den
till databasen med en osäker, strängkonkaternerad ADO.NET-fråga.
*/

using Microsoft.Data.SqlClient;

string connectionString =
    @"Server=(localdb)\MSSQLLocalDB;Database=FailSafeIT_Local;Integrated Security=True;";

using var connection = new SqlConnection(connectionString);

connection.Open();
Console.WriteLine();
Console.WriteLine("Ansluten till FailSafeIT_Local!");
Console.WriteLine();
/* TEST AV DB CONN 
using var command = new SqlCommand("SELECT COUNT(*) FROM dbo.Users;", connection);
Console.WriteLine($"Antal användare i databasen: {command.ExecuteScalar()}");
*/

Console.WriteLine("Ange e-postadress: ");
string email = Console.ReadLine() ?? string.Empty;
SearchUserByEmail(connection, email);

static void SearchUserByEmail(SqlConnection connection, string email)
{
    // Avsiktligt osäker SQL-fråga för uppgift 8
    using var command = new SqlCommand(
        $"SELECT * FROM dbo.Users WHERE Email = '{email}';",
        connection);

    using var reader = command.ExecuteReader();

    bool found = false;

    while(reader.Read())
    {
        found = true;
        Console.WriteLine();
        Console.WriteLine("Användare hittad!");
        Console.WriteLine($"Username: {reader["Username"]}");
        Console.WriteLine($"E-post: {reader["Email"]}");
        Console.WriteLine();
    }

    if(!found)
    {
        Console.WriteLine("Ingen användare hittades.");
    }
}