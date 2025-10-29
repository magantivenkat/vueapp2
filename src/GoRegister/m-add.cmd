@ECHO OFF

dotnet ef migrations add %1 --project "..\GoRegister.Migrations\GoRegister.Migrations.csproj"