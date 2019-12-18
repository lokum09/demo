## links

https://www.learnentityframeworkcore.com/configuration/fluent-api/hasdefaultvalue-method
https://entityframework.net/knowledge-base/31025967/code-first-migration--how-to-set-default-value-for-new-property-
https://www.c-sharpcorner.com/article/swagger-for-asp-net-core-api-2-2/
https://jakeydocs.readthedocs.io/en/latest/migration/webapi.html
https://www.strathweb.com/2019/01/enabling-apicontroller-globally-in-asp-net-core-2-2/
https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/
https://www.learnentityframeworkcore.com/connection-strings
https://joonasw.net/view/hsts-in-aspnet-core
https://www.tutorialspoint.com/asp.net_core/asp.net_core_routing.htm
https://www.strathweb.com/2016/06/global-route-prefix-with-asp-net-core-mvc-revisited/

# baza

create table Blog (
	[Id] BIGINT Identity(1, 1) NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(100) NOT NULL
);

create table Post (
	[Id] BIGINT Identity(1, 1) NOT NULL PRIMARY KEY,
	[Title] NVARCHAR(100) NOT NULL,
	[Content] NVARCHAR(1000) NOT NULL,
	[BlogId] BIGINT NOT NULL,
	CONSTRAINT FK_Post_Blog foreign key ([BlogId]) references Blog ([Id]) on delete cascade
);

## app

dotnet new webapi -o HelloApp

cd HelloApp

::


-- mozna nie dodawac, wtedy uzyje najnowszej wersji sdk
--dotnet new globaljson --sdk-version 2.2.109

dotnet add package Microsoft.AspNetCore.App --version 2.2.0

dotnet add package Microsoft.EntityFrameworkCore.Design --version 2.2.1

--dotnet add package Microsoft.AspNetCore.OData

dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 2.2.1

### ef

dotnet tool install --global dotnet-ef --version 3.0.0

-- dotnet ef dbcontext scaffold "Server=.;Database=HelloApp;User=sa;Password=sa;Integrated Security=False" Microsoft.EntityFrameworkCore.SqlServer -o Models -d -f

dotnet ef migrations add InitialCreate

dotnet ef database update

:: swagger (https://docs.microsoft.com/pl-pl/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.0&tabs=netcore-cli)

::dotnet add package Swashbuckle.AspNetCore --version 4.0.1

services.AddSwaggerGen(c=> { c.SwaggerDoc("v1", new Info { Title = "Employee API", Version = "V1" });  
            });   

app.UseSwagger();  
            app.UseSwaggerUI(c=> {  
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "post API V1");  
                });  


:: run in console

set ASPNETCORE_URLS=http://localhost:5050 && set ASPNETCORE_ENVIRONMENT=development && dotnet watch run

netstat -abon | findStr "127.0.0.1:5000"

## sql

SELECT TOP (1000) [Id]
      ,[Name]
      ,[CategoryId]
	  , lag(id) over (order by id) prev
	  , lead(id) over (order by id) next
	  , sum(Id) over (partition by CategoryId)
  FROM [HelloApp].[dbo].[Blog]