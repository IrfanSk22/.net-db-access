/*

Dapper

1) Dapper is a Micro ORM (Object Relational Mapper)
2) Micro ORM maps database and .NET Objects
3) ORM - Entity Framework
4) Micro ORM - Dapper
5) Dapper is built by Stack Overflow

Advantages of Micro ORM

1) Micro ORM will usually perform better.
2) Setting up full-blown ORM framework in your project
might be too much.
3) If application makes heavy use of Stored Procedures.
4) Works with any database
5) You write your own SQL query.

Disadvantages of Micro ORM

1) Write your own SQL query.
2) Mappings can be difficult.
3) Extensive Validations.

ORM or Micro ORM?

There's not just one answer. With this, the perfect answer will be It depends.
Many projects you will worked on will have stored procedures that are needed for complex logic.
At those places, entity framework lacks the support and dapper is the way to go.
If your application is close to a Crud application with little complex calculations.
your should lean more towards entity framework, but if that has a complex logic at the database end, then
dapper or a combination of dapper and entity framework should be your choice in that also you should lean
more towards pure dapper. But again, it depends on the project and the requirements.
If efficiency is your main concern, you should close your eyes and just go with micro ORM.
    
*/

/*

// Database first approach
>> dotnet ef dbcontext scaffold "<connection_string>" Microsoft.EntityFrameworkCore.SqlServer -o Data -f --no-onconfiguring

>> dotnet ef migrations add "<migration_name>"
>> dotnet ef migrations add "<migration_name>" --context "<DbContext_Name>"

>> dotnet ef database update
>> dotnet ef database update --context "<DbContext_Name>"


> dotnet tool install -g dotnet-aspnet-codegenerator
> dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
> export PATH=$HOME/.dotnet/tools:$PATH (for MacOS & Linux) ??

>> dotnet aspnet-codegenerator controller -name CompaniesController -m Company -dc ApplicationDbContext 
--relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries -f


>> dotnet aspnet-codegenerator view ManageEnrollments Details -m ClassEnrollmentViewModel 
-outDir Views/Classes -f -udl

*/

/*

< C# />

@(3) [ DapperDemo, MagicVilla_VillaAPI ]

@(1) [ jQuery, Ajax ]

< SQL />

@(1) [ EMSCrud ]

*/
