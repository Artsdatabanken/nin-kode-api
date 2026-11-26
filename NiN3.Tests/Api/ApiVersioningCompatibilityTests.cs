using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Asp.Versioning;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NiN.Database;
using NinKode.Common.Interfaces;
using NinKode.Common.Models.Code;
using NinKode.WebApi.Helpers;

namespace NiN3.Tests.Api;

public class ApiVersioningCompatibilityTests
{
    [Theory]
    [InlineData("2.1b")]
    [InlineData("2.1B")]
    [InlineData("2.1-b")]
    public void ParserRecognizesLegacyVersion(string text)
    {
        var parser = new LegacyApiVersionParser();

        Assert.True(parser.TryParse(text, out var version));
        Assert.Equal(new ApiVersion(2, 1, "b"), version);
        Assert.Equal(version, parser.Parse(text));
        Assert.Equal("2.1b", version.ToString("VVS"));
    }

    [Fact]
    public void ParserDelegatesOrdinaryAndInvalidVersions()
    {
        var parser = new LegacyApiVersionParser();

        Assert.Equal(new ApiVersion(2, 3), parser.Parse("2.3"));
        Assert.False(parser.TryParse("not-a-version", out _));
    }

    [Theory]
    [InlineData("2.1b", "2.1b")]
    [InlineData("2.1B", "2.1b")]
    [InlineData("2.1-b", "2.1b")]
    [InlineData("2.1", "2.1")]
    [InlineData("2.3", "2.3")]
    public async Task VersionedRoutePassesOriginalVersionToCodeService(string routeVersion, string expectedVersion)
    {
        var codes = new Mock<ICodeService>();
        codes.Setup(x => x.GetAll(It.IsAny<NiNDbContext>(), It.IsAny<string>(), It.IsAny<string>(), false))
            .Returns(Array.Empty<Codes>());
        using var factory = CreateFactory(codes);
        using var client = factory.CreateClient();

        using var response = await client.GetAsync($"/v{routeVersion}/koder/allekoder");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Empty((await response.Content.ReadFromJsonAsync<Codes[]>())!);
        codes.Verify(x => x.GetAll(It.IsAny<NiNDbContext>(), It.IsAny<string>(), expectedVersion, false), Times.Once);
    }

    [Theory]
    [InlineData("/versjoner")]
    [InlineData("/versjoner?api-version=2.1b")]
    [InlineData("/versjoner?api-version=2.3")]
    public async Task VersionQueryAndDefaultResolveExistingEndpoint(string path)
    {
        using var factory = CreateFactory(new Mock<ICodeService>());
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(path);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(new[] { "v1", "v2.1b", "v2.3" }, await response.Content.ReadFromJsonAsync<string[]>());
    }

    [Theory]
    [InlineData("/versjoner?api-version=999", "UnsupportedApiVersion")]
    [InlineData("/versjoner?api-version=bad", "InvalidApiVersion")]
    [InlineData("/versjoner?api-version=2.1b&api-version=2.3", "AmbiguousApiVersion")]
    public async Task InvalidQueriesReturnVersioningErrorObjects(string path, string errorCode)
    {
        using var factory = CreateFactory(new Mock<ICodeService>());
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(path);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(errorCode, json.RootElement.GetProperty("error").GetProperty("code").GetString());
    }

    [Theory]
    [InlineData("v1.0", "v1.0")]
    [InlineData("v2.0", "v2.0")]
    [InlineData("v2.1", "v2.1")]
    [InlineData("v2.1b", "v2.1b")]
    [InlineData("v2.2", "v2.2")]
    [InlineData("v2.3", "v2.3")]
    [InlineData("v3.0", "v3.0")]
    public async Task SwaggerIncludesTheCorrectVersionedPaths(string group, string routeVersion)
    {
        using var factory = CreateFactory(new Mock<ICodeService>());
        using var client = factory.CreateClient();

        using var response = await client.GetAsync($"/swagger/{group}/swagger.json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var paths = json.RootElement.GetProperty("paths");
        var expectedPath = group == "v3.0" ? $"/{routeVersion}/typer/allekoder" : $"/{routeVersion}/koder/allekoder";
        Assert.True(paths.TryGetProperty(expectedPath, out var operation));
        Assert.True(operation.TryGetProperty("get", out _));
        Assert.False(paths.TryGetProperty("/", out _));
        if (group == "v2.1b")
        {
            Assert.Equal("2.1-b", json.RootElement.GetProperty("info").GetProperty("version").GetString());
            Assert.Contains("deprecated", json.RootElement.GetProperty("info").GetProperty("description").GetString());
        }
    }

    private static WebApplicationFactory<Program> CreateFactory(Mock<ICodeService> codes)
    {
        var versions = new Mock<IVersionService>();
        versions.Setup(x => x.GetVersions(It.IsAny<NiNDbContext>())).Returns(new[] { "v1", "v2.1b", "v2.3" });

        return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ICodeService>();
                services.AddSingleton(codes.Object);
                services.RemoveAll<IVersionService>();
                services.AddSingleton(versions.Object);
            });
        });
    }
}
