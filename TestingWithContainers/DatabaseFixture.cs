using TestContainers.Container.Abstractions.Hosting;
using TestContainers.Container.Database.Hosting;
using TestContainers.Container.Database.PostgreSql;

namespace TestingWithContainers;

// ReSharper disable once ClassNeverInstantiated.Global
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer container = 
        new ContainerBuilder<PostgreSqlContainer>()
            .ConfigureDatabaseConfiguration("jetbrains", "dotUltimate", "tools")
            .Build();
    
    public string ConnectionString => container.GetConnectionString();
    public string ContainerId => $"{container.ContainerId}";

    public Task InitializeAsync() 
        => container.StartAsync();

    public Task DisposeAsync() 
        => container.StopAsync();
}