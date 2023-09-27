using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace TestingWithContainers;

public class HttpTest : IAsyncLifetime
{
    private readonly IContainer container;

    public HttpTest()
    {
        container = new ContainerBuilder()
            // Set the image for the container to "testcontainers/helloworld:1.1.0".
            .WithImage("testcontainers/helloworld:1.1.0")
            // Bind port 8080 of the container to a random port on the host.
            .WithPortBinding(8080, true)
            // Wait until the HTTP endpoint of the container is available.
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
            // Build the container configuration.
            .Build();
    }

    [Fact]
    public async Task Can_Call_Endpoint()
    {
        var httpClient = new HttpClient();

        var requestUri =
            new UriBuilder(
                Uri.UriSchemeHttp,
                container.Hostname,
                container.GetMappedPublicPort(8080),
                "uuid"
            ).Uri;

        var guid = await httpClient.GetStringAsync(requestUri);

        Assert.True(Guid.TryParse(guid, out _));
    }

    public Task InitializeAsync()
        => container.StartAsync();

    public Task DisposeAsync()
        => container.DisposeAsync().AsTask();
}