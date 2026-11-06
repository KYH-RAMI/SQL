# FailSafe IT – Databasprojekt

**KYH | Systemutvecklare DevSecOps**  
**Kurs:** SQL Server & T-SQL

## Projektbeskrivning

FailSafe IT är ett databasprojekt där jag har byggt en relationsdatabas i Microsoft SQL Server för att hantera kunder, konsulter och deras uppdrag.

Syftet är att tillämpa databasdesign, normalisering, SQL-frågor och referensintegritet.

## Databasstruktur

Databasen heter `FailSafeIT_Local` och består av tre tabeller:

| Tabell | Beskrivning |
|---|---|
| Customers | Lagrar kundernas företagsuppgifter |
| Consultants | Lagrar konsulter, specialiseringar och timpriser |
| Assignments | Kopplar kunder och konsulter till uppdrag |

### Relationer

- En kund kan ha flera uppdrag.
- En konsult kan ha flera uppdrag.
- Varje uppdrag tillhör en kund och en konsult.

Relationerna hanteras med primärnycklar (`PRIMARY KEY`) och främmande nycklar (`FOREIGN KEY`).

### ER-diagram

Databasens relationer och tabellstruktur har modellerats i Lucidchart.

- [Visa ER-diagram i Lucidchart](https://lucid.app/lucidchart/0719d323-aa56-48c3-a657-e815627b835f/edit)
- [Visa ER-diagram som PDF](FailSafe_IT_ERD.pdf)

## Normalisering

Databasen är utformad enligt tredje normalformen (3NF).

- **1NF:** Varje kolumn innehåller ett atomärt värde.
- **2NF:** Alla attribut är beroende av hela primärnyckeln.
- **3NF:** Icke-nyckelattribut är inte transitivt beroende av primärnyckeln.

Normaliseringen minskar onödig dataduplicering och förbättrar databasens struktur.

## SQL-frågor

Projektet innehåller följande SQL-operationer:

| Fråga | Funktion |
|---|---|
| A | SELECT, WHERE och ORDER BY |
| B | INNER JOIN mellan tre tabeller |
| C | COUNT, GROUP BY och datumfiltrering |
| D | UPDATE av konsultens timpris |
| E | DELETE med NOT EXISTS |

UPDATE och DELETE har testats separat. I inlämningsfilen är dessa satser kommenterade för att förhindra oavsiktliga dataändringar.

## Projektfiler

- `FailSafeIT_Complete.sql` – Komplett SQL-script med tabeller, testdata och frågorna A–E.
- `FailSafe_IT_ERD.pdf` – ER-diagram över databasstrukturen.

### Körinstruktioner

1. Öppna Microsoft SQL Server Management Studio (SSMS).
2. Skapa en tom databas med namnet `FailSafeIT_Local`.
3. Öppna `FailSafeIT_Complete.sql`.
4. Kör scriptet med **F5**.

Scriptet skapar tre tabeller och lägger in 11 testposter.

**Observera:** Scriptet är avsett att köras en gång mot en tom databas. Frågorna D och E kan aktiveras separat för att demonstrera UPDATE och DELETE.

## DevSecOps-reflektion

### Sec – SQL Injection

För att skydda en webbapplikation mot SQL Injection skulle jag använda parameteriserade SQL-frågor som separerar användarinmatning från SQL-koden. På så sätt behandlas exempelvis `OR 1=1` som data i stället för att köras som ett SQL-villkor.

### Ops – Indexering

Jag använder indexering på organisationsnummer för att SQL Server snabbare ska kunna hitta specifika kunder utan att behöva söka igenom hela tabellen. Nackdelen är att skrivoperationer som INSERT, UPDATE och DELETE kan bli långsammare eftersom indexen också måste uppdateras.

I tabellen `Customers` skapas ett unikt index automatiskt genom `UNIQUE` på kolumnen `OrganizationNumber`.

## Tekniker

- Microsoft SQL Server
- SQL Server Management Studio (SSMS)
- T-SQL
- Relationsdatabaser
- ER-diagram
- Databasnormalisering (3NF)
