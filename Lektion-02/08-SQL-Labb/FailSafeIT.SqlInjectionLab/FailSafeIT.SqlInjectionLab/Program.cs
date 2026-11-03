/*
 * Lektion 2 – Uppgift 10
 * Den tidigare SQL Injection-sårbarheten har åtgärdats
 * genom att använda SqlParameter i stället för att
 * lägga användarens text direkt i SQL-frågan.
 */
using System.Data;
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

// Uppgift 10 – Sök användare
// Console.Write("Ange e-postadress: ");
// string email = Console.ReadLine() ?? string.Empty;
// SearchUserByEmail(connection, email);

// Uppgift 13 – Testa serverregistrering
Console.Write("Ange servernamn: ");
string serverName = Console.ReadLine() ?? string.Empty;


//RegisterServer(connection, serverName);

static void SearchUserByEmail(SqlConnection connection, string email)
{
    // Kontrollerad SQL-fråga för uppgift 10
    using var command = new SqlCommand(
        "SELECT * FROM dbo.Users WHERE Email = @Email;",
        connection);

    // Alternativ: AddWithValue(), men här anger jag datatyp och längd själv.

    command.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = email;

    // command.Parameters.AddWithValue("@Email", email);

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

static void RegisterServer(SqlConnection connection, string serverName)
{
    using var command = new SqlCommand(
      "dbo.usp_RegisterServer", connection);

    command.CommandType = CommandType.StoredProcedure;

    command.Parameters.Add("@ServerName", SqlDbType.NVarChar, 100)
    .Value = serverName;
    command.Parameters.Add("@IpAddress", SqlDbType.VarChar, 15)
    .Value = "192.168.1.100";
    command.Parameters.Add("@Environment", SqlDbType.NVarChar, 50)
    .Value = "Production";
    command.Parameters.Add("@ManagedByUserId", SqlDbType.Int)
    .Value = 1;
    command.ExecuteNonQuery();
    Console.WriteLine("Servern har registrerats!");

}