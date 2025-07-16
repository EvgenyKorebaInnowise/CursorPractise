using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoNotetaker.Api.Data;

namespace VideoNotetaker.Api.Tests.Integration
{
    public class TestApiFactory : WebApplicationFactory<Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPart>
    {
        private readonly string _testDbName;
        private readonly string _connectionString;

        public TestApiFactory()
        {
            _testDbName = $"VideoNotetakerTestDb_{Guid.NewGuid():N}";
            _connectionString = $"Server=localhost;Database={_testDbName};Trusted_Connection=True;TrustServerCertificate=True;";
            EnsureTestDatabaseCreated();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var dict = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = _connectionString
                };
                config.AddInMemoryCollection(dict!);
            });

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(_connectionString));

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            });
        }

        private void EnsureTestDatabaseCreated()
        {
            using var connection = new SqlConnection("Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;");
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $"IF DB_ID('{_testDbName}') IS NULL CREATE DATABASE [{_testDbName}];";
            command.ExecuteNonQuery();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            using var connection = new SqlConnection("Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;");
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $"ALTER DATABASE [{_testDbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{_testDbName}];";
            command.ExecuteNonQuery();
        }
    }
} 