# Longfunctie ASP.NET API — Installation & Setup Guide

This guide will help you set up the project from scratch, configure the database, and run the API.

---

## 1. Create `appsettings.json`
In the root folder, create a new `appsettings.json` file with the following content:

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=33006;database=longfunctieappdb;user=root;password=;"
  },
  "Jwt": {
    "Key": "01234567890123456789012345678901"
  },
  "AllowedHosts": "*"
}

> ⚠️ Make sure to update the `user` and `password` to match your MySQL setup.

---

## 2. Initialize the Database

1. In the root folder, you will find `DBsetup.sql`.
2. Open MySQL Workbench, phpMyAdmin, or your preferred MySQL client.
3. Execute the `DBsetup.sql` file. It will:

   - Drop the database `longfunctieappdb` if it exists.
   - Create the database `longfunctieappdb`.
   - Create the `Parents` table with:
     - `Name` as PRIMARY KEY.
     - `PinHash` and `CreatedAt` columns.
   - Create the `Children` table with:
     - `Id` as AUTO_INCREMENT PRIMARY KEY.
     - `ParentName` as FOREIGN KEY referencing `Parents(Name)` with `ON DELETE CASCADE`.
     - Optional fields: `Name`, `Age`, `Avatar`, `DoctorName`, `TreatmentType`, `TreatmentDate`.
     - `AnonymousId` as a GUID and `CreatedAt` timestamp.

> After this, the database is ready for the API.

---

## 3. Running the Project

1. Open the project in your IDE (Visual Studio, Rider, VS Code).
2. Restore NuGet packages if needed.
3. Build the solution.
4. Run the project.
5. find the postman documentation inside the other zip file.
