// This script generates BCrypt hashes for the seed users.csv file.
// Run: dotnet script GenerateHashes.csx  (or just run it with dotnet-script)
// Passwords: admin123, student123, faculty123

using BCrypt.Net;
Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("admin123"));
Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("student123"));
Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("faculty123"));
