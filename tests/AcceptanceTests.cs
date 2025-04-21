using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Quantum.MonkeyPatch.Service;

namespace Quantum.MonkeyPatch.Tests;

public class AcceptanceTests
{
    [Fact]
    public async Task createQuantumObject()
    {
        var services = await MonkeyPathServices();

        var cmd = new CreateQuantumObjectCommand("Person", new List<QuantumPropertyDto>()
        {
            new(QuantumPrimitiveTypes.String,true, "first name", "نام")
        });

        await services.CreateObject(cmd);

        List<QuantumObjectViewModel> objects = await services.GetAllObjects();

        objects.Should().NotBeNullOrEmpty();
    }


    [Fact]
    public async Task createQuantumInstanceObject()
    {
        var services = await MonkeyPathServices();

        var cmd = new CreateQuantumObjectCommand("Person", new List<QuantumPropertyDto>()
        {
            new(QuantumPrimitiveTypes.String,true, "first name", "نام")
        });

        await services.CreateObject(cmd);

        List<QuantumObjectViewModel> objects = await services.GetAllObjects();

        await services.CreateObjectInstance(objects.Single().id, new CreateNewInstanceCommand
        {
            Values = new Dictionary<string, object>()
            {
                {"first name", "Masoud"}
            }
        });


        var quantumObjectInstances = await services.GetAllObjectInstances(objects.Single().id);

        quantumObjectInstances.Single()
            ["first name"]
            .Should()
            .Be(new QuantumStringInstance("Masoud"));
    }


    private static async Task<IMonkeyPatchServices> MonkeyPathServices()
    {
        var sqliteConnection = new SqliteConnection("Filename=:memory:");
        sqliteConnection.Open();

        var options = new DbContextOptionsBuilder<QuantumMonkeyPatchDbContext>()
            .UseSqlite(sqliteConnection).Options;

        QuantumMonkeyPatchDbContext context = new QuantumMonkeyPatchDbContext(options);

        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();

        IMonkeyPatchServices services = new MonkeyPatchServices(context);
        return services;
    }
}