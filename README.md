# Societies Management System

A Windows desktop application built with C# .NET for managing residential/housing societies. It allows admins and users to manage society members, handle registrations, and streamline society operations.


## Features

- User Registration & Login
- Role-based Authentication (Admin / User)
- Member Management
- Society Management
- SQL Server Database Integration


## Technologies Used

| Technology | Version |
|---|---|
| C# .NET | 10.0 |
| Windows Forms | .NET 10 |
| SQL Server | Express |
| Microsoft.Data.SqlClient | 7.0 |


## Prerequisites

Make sure you have the following installed:

- [.NET SDK 10.0+](https://dotnet.microsoft.com/en-us/download)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/ssms/download-sql-server-management-studio-ssms)
- [Visual Studio Code](https://code.visualstudio.com/) with C# Dev Kit extension

---

## Database Setup

1. Open SSMS and connect to your SQL Server
2. Create a new database named:

SocietiesDataBase

3. Open the provided `.sql` script file
4. Select `SocietiesDataBase` from the dropdown
5. Press F5 to execute and create all tables


## Configuration

Open the project and find the connection string (in `App.config` or `Services` folder) and update it with your server name:

```json
"Server=YOUR_SERVER_NAME\\SQLEXPRESS;Database=SocietiesDataBase;Trusted_Connection=True;TrustServerCertificate=True;"
```

Replace `YOUR_SERVER_NAME` with your actual PC/server name. For example:
```
Server=DESKTOP-QI6H2EA\\SQLEXPRESS01;Database=SocietiesDataBase;Trusted_Connection=True;TrustServerCertificate=True;
```


## Running the Project

1. Clone the repository:

git clone https://github.com/kashan-X/Society_Management_System.git


2. Navigate to the project folder:

cd Society_Management_System


3. Restore dependencies:

dotnet restore


4. Run the project:

dotnet run


## Project Structure

SocietiesManagementSystem/
├── Forms/
│   ├── LoginForm.cs
│   ├── RegisterForm.cs
│   └── ...
├── Services/
│   ├── AuthService.cs
│   └── ...
├── Models/
│   └── User.cs
├── bin/
├── obj/
└── README.md


This project is for educational purposes.
