This is a RESTFul API named Code Server API built using the C# Language and .NET Core 3.1. 
It is capable of running on multiple platforms efficiently.

.NET Core 3.1 is the latest version on the Core platform released by windows. 
I tried as much as possible to break down the API into several modules for better core quality and readability. 
The Code Server API is using the multi layered architecture.

CodeServer API which is the point of access for our application. Houses our controllers. 
CodeServer.Business this is where the business logic and services are implemented. Houses our services and business interfaces.
CodeServer.Core which is the application’s foundation, it will hold our entities and our models which are crucial.
CodeServer.Data For persisting our data, where we will connect with data providers (SQL Server Express) and also our repositories and Unit of work.
CodeServer.Tests where we conduct our unit and integration tests.

PostmanCollections Folder contains the postman test scripts.

Some of the practices I implemented are 

1.  Create an application in separated projects to make it decoupled from each module.
2.  Implement Repository and Unit of Work pattern.
3.  Use Entity Framework Core for data persistence. Utilized the Code first approach.
4.  Add AutoMapper for mapping view models(Data Transfer Objects) into API resources.
5.  Add Swagger to have a friendly API interface.
6.  Add Unit and Integration Tests.
7.  A sample postman collection file provided, that can be used to verify API functionality.
8.  Used MSSQL LocalDB.
9.  Docker Containers. So there is a choice of either running up the dockerfile or running with IIS Express.
10. implemented serilog for structured logging.

Tests used Moq and XUnit
API is resource-based
API has proper HTTP verb and statuses
API has integration tests covering all cases leading to different HTTP statuses
Business logic handles all database schema validations
Business logic handles all entity class validations
If the entity class requires a field not to be blank, then that constraint should be validated at business layer
PATCH API does not accidentally update any fields

To Run On Docker Container, Default connection in API appsettings has to be changed to Server Name/IP address
