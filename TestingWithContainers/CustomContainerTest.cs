using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

namespace TestingWithContainers;

public class CustomContainerTest : IAsyncLifetime
{
    private IFutureDockerImage image;
    private IContainer container;

    public async Task InitializeAsync()
    {
        image = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile("Dockerfile")
            .WithCleanUp(true)
            .Build();
        
        // create image from Dockerfile
        await image.CreateAsync();

        container = new ContainerBuilder()
            .WithImage(image)
            .WithPortBinding(80, assignRandomHostPort: true)
            // use environment variables to add configuration options
            .WithEnvironment("ASPNETCORE_URLS", "http://+:80")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPath("/")))
            .Build();
        
        // build container
        await container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await image.DisposeAsync();
        await container.DisposeAsync();
    }
    
    [Fact]
    public async Task Can_Call_Endpoint()
    {
        var httpClient = new HttpClient();

        var requestUri =
            new UriBuilder(
                Uri.UriSchemeHttp,
                container.Hostname,
                container.GetMappedPublicPort(80),
                "/"
            ).Uri;

        var expected = "Hello World!";
        var actual = await httpClient.GetStringAsync(requestUri);
        
        Assert.Equal(expected, actual);
    }
}