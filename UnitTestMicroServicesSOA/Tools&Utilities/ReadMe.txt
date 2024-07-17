dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json
dotnet add package Microsoft.Extensions.Configuration.FileExtensions
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design            
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Newtonsoft.Json
dotnet add package System.Data.SQLite

dotnet add package Microsoft.NET.Test.Sdk    
dotnet add package NUnit
dotnet add package NUnit3TestAdapter
dotnet add package NUnit.Analyzers
dotnet add package coverlet.collector

Add appsettings.json
Right Click File -> Properties -> Copy to Output Directory -> Copy if never
--------------------

DB Script:
----------
CREATE VIEW AppSuperLevel AS
    SELECT u.*,
           ur.*,
           r.*
      FROM AspNetUsers u,
           AspNetUserRoles ur,
           AspNetRoles r
     WHERE ur.UserId = u.Id AND 
           ur.RoleId = r.Id AND 
           u.ConcurrencyStamp IS NOT NULL AND 
           r.ConcurrencyStamp IS NOT NULL;
CREATE VIEW AppUser (
    MemberId,
    VisitDate
)
AS
    SELECT u.*,
           ur.*,
           r.*
      FROM AspNetUsers u,
           AspNetUserRoles ur,
           AspNetRoles r
     WHERE ur.UserId = u.Id AND 
           ur.RoleId = r.Id AND 
           u.ConcurrencyStamp IS NULL AND 
           r.ConcurrencyStamp IS NULL;
CREATE VIEW AppUserIdol AS
    SELECT u.*
      FROM AspNetUsers u
           LEFT JOIN
           AspNetUserRoles ur ON u.Id = ur.UserId
     WHERE ur.UserId IS NULL;

http://localhost:8080/Webservice/WebService.asmx
http://localhost:8080/RestService/Host/MakeCall

Let's create Some Micro Services SOA:
-------------------------------------

dotnet new webapi -controllers -n MicroAuthenticateApi
dotnet new webapi -controllers -n MicroAuthorizeApi