using api.Data;
using api.Models;
using api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace api.tests
{
    public class ClientApiTests
    {
        [Fact]
        public async void Test_CreateNewClients_Success()
        {
            // Arrange 
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "ClientInMemory");
            var dbContextOptions = builder.Options;

            var dbContext = new DataContext(dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var repository = new ClientRepository(dbContext);

            var clientRequest = new Client
            {
                Id = "1",
                FirstName = "Test User",
                LastName = "Test",
                Email = "Test@mailnator.com",
                PhoneNumber = "1111111"
            };

            // Act 
            var result =  await ClientEndpointsV1.CreateClient(clientRequest, repository);

            // Assert
            Assert.Equal("CreatedResult", result.GetType().Name);
        }

        [Fact]
        public async void Test_CreateNewClients_BadRequest()
        {
            // Arrange 
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "ClientInMemory");
            var dbContextOptions = builder.Options;

            var dbContext = new DataContext(dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var repository = new ClientRepository(dbContext);

            var clientRequest = new Client
            {
                Id = "1",
                FirstName = "Test User",
                LastName = "Test",
                Email = "Test@mailnator.com",
                PhoneNumber = "1111111"
            };

            await ClientEndpointsV1.CreateClient(clientRequest, repository);

            // Act 
            var result = await ClientEndpointsV1.CreateClient(clientRequest, repository);

            // Assert
            Assert.Equal("BadRequestObjectResult", result.GetType().Name);
        }

        [Fact]
        public async void Test_UpdateClient_Success()
        {
            // Arrange 
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "ClientInMemory");
            var dbContextOptions = builder.Options;

            var dbContext = new DataContext(dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var repository = new ClientRepository(dbContext);

            var createRequest = new Client
            {
                Id = "1",
                FirstName = "Test User",
                LastName = "Test",
                Email = "Test@mailnator.com",
                PhoneNumber = "1111111"
            };

            await ClientEndpointsV1.CreateClient(createRequest, repository);

            var updateRequest = new Client
            {
                Id = "2",
                FirstName = "Test User2",
                LastName = "Test2",
                Email = "Test@mailnator.com",
                PhoneNumber = "1111111"
            };

            // Act 
            var result = await ClientEndpointsV1.UpdateClient(createRequest.Id,
                updateRequest, repository);

            // Assert
            Assert.Equal("OkObjectResult", result.GetType().Name);
        }

        [Fact]
        public async void Test_UpdateClient_BadRequest()
        {
            // Arrange 
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "ClientInMemory");
            var dbContextOptions = builder.Options;

            var dbContext = new DataContext(dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var repository = new ClientRepository(dbContext);

            var createRequest = new Client
            {
                Id = "1",
                FirstName = "Test User",
                LastName = "Test",
                Email = "Test@mailnator.com",
                PhoneNumber = "1111111"
            };

            await ClientEndpointsV1.CreateClient(createRequest, repository);

            var updateRequest = new Client
            {
                Id = "2",
                FirstName = "Test User2",
                LastName = "Test2",
                Email = "Test@mailnator.com",
                PhoneNumber = "1111111"
            };

            // Act 
            var result = await ClientEndpointsV1.UpdateClient("3",
                updateRequest, repository);

            // Assert
            Assert.Equal("BadRequestObjectResult", result.GetType().Name);
        }

        [Fact]
        public async void Test_GetClients()
        {
            // Arrange 
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "ClientInMemory");
            var dbContextOptions = builder.Options;

            var dbContext = new DataContext(dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var repository = new ClientRepository(dbContext);

            var clientRequest = new Client
            {
                Id = "1",
                FirstName = "Test User",
                LastName = "Test",
                Email = "Test@mailnator.com",
                PhoneNumber = "1111111"
            };

            await ClientEndpointsV1.CreateClient(clientRequest, repository);

            // Act 
            var result = await ClientEndpointsV1.GetClients(repository);

            // Assert
            Assert.Equal("OkObjectResult", result.GetType().Name);
        }

        [Theory]
        [InlineData("steven")]
        [InlineData("Steven")]
        [InlineData("STEVEN")]

        public async void Test_SearchClientName_Found(string expectedName)
        {
            // Arrange 
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "ClientInMemory");
            var dbContextOptions = builder.Options;

            var dbContext = new DataContext(dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var repository = new ClientRepository(dbContext);

            var clientOneRequest = new Client
            {
                Id = "1",
                FirstName = "John",
                LastName = "Stevens",
                Email = "Test@mailnator.com",
                PhoneNumber = "1111111"
            };

            var clientTwoRequest = new Client
            {
                Id = "2",
                FirstName = "Steven",
                LastName = "Smith",
                Email = "Test2@mailnator.com",
                PhoneNumber = "2222222"
            };

            await ClientEndpointsV1.CreateClient(clientOneRequest, repository);
            await ClientEndpointsV1.CreateClient(clientTwoRequest, repository);

            // Act 
            var result = await ClientEndpointsV1.SearchClients(expectedName, repository);
            
            // Assert
            Assert.Equal("OkObjectResult", result.GetType().Name);
        }

        [Theory]
        [InlineData("steven")]
        [InlineData("Steven")]
        [InlineData("STEVEN")]

        public async void Test_SearchClientName_NotFound(string expectedName)
        {
            // Arrange 
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(databaseName: "ClientInMemory");
            var dbContextOptions = builder.Options;

            var dbContext = new DataContext(dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var repository = new ClientRepository(dbContext);

            var clientOneRequest = new Client
            {
                Id = "1",
                FirstName = "John",
                LastName = "Smith",
                Email = "Test@mailnator.com",
                PhoneNumber = "1111111"
            };

            var clientTwoRequest = new Client
            {
                Id = "2",
                FirstName = "Mary",
                LastName = "Smith",
                Email = "Test2@mailnator.com",
                PhoneNumber = "2222222"
            };

            await ClientEndpointsV1.CreateClient(clientOneRequest, repository);
            await ClientEndpointsV1.CreateClient(clientTwoRequest, repository);

            // Act 
            var result = await ClientEndpointsV1.SearchClients(expectedName, repository);

            // Assert
            Assert.Equal("NotFoundObjectResult", result.GetType().Name);
        }
    }
}