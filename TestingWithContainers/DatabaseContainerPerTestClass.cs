using Dapper;
using JasperFx.Core;
using Marten;
using Npgsql;
using Weasel.Core;
using Xunit.Abstractions;

namespace TestingWithContainers;

public class DatabaseContainerPerTestClass(DatabaseFixture fixture, ITestOutputHelper output) 
    : IClassFixture<DatabaseFixture>, IDisposable
{
    [Fact]
    public async Task Database_Can_Run_Query()
    {
        await using NpgsqlConnection connection = new(fixture.ConnectionString);
        await connection.OpenAsync();
        
        output.WriteLine("Hi! 👋");

        const int expected = 1;
        var actual = await connection.QueryFirstAsync<int>("SELECT 1");

        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task Database_Can_Select_DateTime()
    {
        await using NpgsqlConnection connection = new(fixture.ConnectionString);
        await connection.OpenAsync();
        
        var actual = await connection.QueryFirstAsync<DateTime>("SELECT NOW()");
        Assert.IsType<DateTime>(actual);
    }

    [Fact]
    public async Task Can_Store_Document_With_Marten()
    {
        await using NpgsqlConnection connection = new(fixture.ConnectionString);
        var store = DocumentStore.For(options => {
            options.Connection(fixture.ConnectionString);
            options.AutoCreateSchemaObjects = AutoCreate.All;
        });

        int id;
        {
            await using var session = store.IdentitySession();
            var person = new Person("Khalid");
            session.Store(person);
            await session.SaveChangesAsync();

            id = person.Id;
        }

        {
            await using var session = store.QuerySession();
            var person = session.Query<Person>().FindFirst(p => p.Id == id);
            Assert.NotNull(person);
        }
    }

    public void Dispose()
        => output.WriteLine(fixture.ContainerId);
}