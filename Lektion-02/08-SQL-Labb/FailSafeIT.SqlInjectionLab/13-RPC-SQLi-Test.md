## Resultat

Jag testade att skriva SQL-kod som servernamn i mitt C#-program.

Programmet registrerade servern utan fel.

### Test i C#

![Test i konsolappen](13-RPC-Konsol.png)

### Kontroll i SQL Server

![Resultat i SSMS](13-RPC-SSMS.png)

Hela texten sparades som servernamn i databasen.
SQL-koden kördes alltså inte som en egen instruktion.

Det beror på att servernamnet skickas som en parameter
till min Stored Procedure.