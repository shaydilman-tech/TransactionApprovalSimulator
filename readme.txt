============================================================
Transaction Approval Simulator - Setup Guide
============================================================

This document explains how to set up and run the application.

------------------------------------------------------------
1. Prerequisites
------------------------------------------------------------

Download and install:

- .NET SDK 10.0 or later: Download:	https://dotnet.microsoft.com/download
- Node.js 18 or later. Download: https://nodejs.org

------------------------------------------------------------
2. Download the application from Git:
------------------------------------------------------------
- Open the GitHub repository:
   https://github.com/shaydilman-tech/TransactionApprovalSimulator.git

- Click the green "Code" button.
- Click "Download ZIP".
- Extract the ZIP file to any folder on your computer.

------------------------------------------------------------
3. Database Setup
------------------------------------------------------------
- Download SQL Server Express from: https://www.microsoft.com/sql-server/sql-server-downloads
  Run the installer -> choose Download Media -> LocalDB -> Select Download location -> Download
  Go to the chosen location and run the SqlLocalDB.msi and complete installation.
  After installation, verify it’s available with the command:
	sqllocaldb info


- Open a terminal in this path:
   TransactionApprovalSimulator.Server/

- Run the following commands:

   dotnet restore
   dotnet tool install --global dotnet-ef
   dotnet ef database update

------------------------------------------------------------
3. Configure the Application settings
------------------------------------------------------------

The backend project is located in:
Inside TransactionApprovalSimulator.Server/ folder open appsettings.json and make sure your connection string looks like that:
  
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TransactionApprovalDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },

Otherwise, change it to that value and save the file.


------------------------------------------------------------
5. Run the Application
------------------------------------------------------------

Open a terminal in the folder:
TransactionApprovalSimulator.Server/

Run:
	dotnet run

Open another terminal in this folder:
transactionapprovalsimulator.client/

And run:
	npm install
	npm run dev

