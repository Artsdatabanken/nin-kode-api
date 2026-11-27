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
using NiN3.Core.Models.DTOs;
using NiN3.Core.Models.DTOs.search;
using NiN3.Core.Models.Enums;
using NiN3.Infrastructure.Services;
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
    [InlineData("v3", "3")]
    [InlineData("v3.0", "3.0")]
    [InlineData("v3.0-v2026", "3.0-v2026")]
    [InlineData("V3.0-V2026", "3.0-v2026")]
    public void ParserRecognizesPrefixedVersion3Routes(string routeVersion, string expectedVersion)
    {
        var parser = new LegacyApiVersionParser();

        Assert.True(parser.TryParse(routeVersion, out var version));
        Assert.Equal(ApiVersionParser.Default.Parse(expectedVersion), version);
        Assert.Equal(version, parser.Parse(routeVersion));
        Assert.False(parser.TryParse("v2.1b", out _));
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
    [InlineData("v3.0-v2026", "v3.0-v2026")]
    public async Task SwaggerIncludesTheCorrectVersionedPaths(string group, string routeVersion)
    {
        using var factory = CreateFactory(new Mock<ICodeService>());
        using var client = factory.CreateClient();

        using var response = await client.GetAsync($"/swagger/{group}/swagger.json");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var paths = json.RootElement.GetProperty("paths");
        var expectedPath = group.StartsWith("v3.0", StringComparison.Ordinal) ? $"/{routeVersion}/typer/allekoder" : $"/{routeVersion}/koder/allekoder";
        Assert.True(paths.TryGetProperty(expectedPath, out var operation));
        Assert.True(operation.TryGetProperty("get", out _));
        Assert.False(paths.TryGetProperty("/", out _));
        if (group == "v2.1b")
        {
            Assert.Equal("2.1-b", json.RootElement.GetProperty("info").GetProperty("version").GetString());
            Assert.Contains("deprecated", json.RootElement.GetProperty("info").GetProperty("description").GetString());
        }
        else if (group.StartsWith("v3.0", StringComparison.Ordinal))
        {
            Assert.True(paths.TryGetProperty($"/{routeVersion}/variabler/allekoder", out _));
            Assert.True(paths.TryGetProperty($"/{routeVersion}/rapporter/kodeoversikt", out _));
            Assert.True(paths.TryGetProperty($"/{routeVersion}/soek/enkeltSoek", out _));
        }
    }

    [Theory]
    [InlineData("3")]
    [InlineData("3.0")]
    [InlineData("3.0-v2026")]
    public async Task VersionedTypeRouteUsesExistingDataVersion(string routeVersion)
    {
        var types = new Mock<ITypeApiService>();
        types.Setup(x => x.AllCodesAsync("3.0", It.IsAny<string>())).ReturnsAsync(new VersjonDto());
        using var factory = CreateFactory(new Mock<ICodeService>(), types);
        using var client = factory.CreateClient();

        using var response = await client.GetAsync($"/v{routeVersion}/typer/allekoder");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        types.Verify(x => x.AllCodesAsync("3.0", It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [InlineData("3.0")]
    [InlineData("3.0-v2026")]
    public async Task VersionedRoutesReachEachVersion3Controller(string routeVersion)
    {
        var reports = new Mock<IRapportService>();
        reports.Setup(x => x.MakeKodeoversiktCSV("3.0", ";")).Returns("test");
        var variables = new Mock<IVariabelApiService>();
        variables.Setup(x => x.AllCodes("3.0")).Returns(new VersjonDto());
        var search = new Mock<ISearchService>();
        search.Setup(x => x.SimpleSearch("test", It.IsAny<KlasseEnum>(), It.IsAny<SearchMethodEnum>()))
            .Returns(new List<SearchResultDto>());
        using var baseFactory = CreateFactory(new Mock<ICodeService>());
        using var factory = baseFactory.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
        {
            services.RemoveAll<IRapportService>();
            services.AddSingleton(reports.Object);
            services.RemoveAll<IVariabelApiService>();
            services.AddSingleton(variables.Object);
            services.RemoveAll<ISearchService>();
            services.AddSingleton(search.Object);
        }));
        using var client = factory.CreateClient();

        using var reportResponse = await client.GetAsync($"/v{routeVersion}/rapporter/kodeoversikt");
        using var variableResponse = await client.GetAsync($"/v{routeVersion}/variabler/allekoder");
        using var searchResponse = await client.GetAsync($"/v{routeVersion}/soek/enkeltSoek?searchTerm=test&klasseEnum=0&searchMethodEnum=0");

        Assert.Equal(HttpStatusCode.OK, reportResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, variableResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, searchResponse.StatusCode);
        reports.Verify(x => x.MakeKodeoversiktCSV("3.0", ";"), Times.Once);
        variables.Verify(x => x.AllCodes("3.0"), Times.Once);
        search.Verify(x => x.SimpleSearch("test", It.IsAny<KlasseEnum>(), It.IsAny<SearchMethodEnum>()), Times.Once);
    }

    [Fact]
    public async Task SwaggerUiListsBothHistoricalAndNewVersions()
    {
        using var factory = CreateFactory(new Mock<ICodeService>());
        using var client = factory.CreateClient();

        var script = await client.GetStringAsync("/swagger/index.js");

        Assert.Contains("/swagger/v2.1b/swagger.json", script);
        Assert.Contains("/swagger/v3.0-v2026/swagger.json", script);
    }

    [Theory]
    [InlineData("/3.0/typer/allekoder", HttpStatusCode.NotFound)]
    [InlineData("/3.0-v2026/typer/allekoder", HttpStatusCode.NotFound)]
    [InlineData("/v3.0v2026/typer/allekoder", HttpStatusCode.BadRequest)]
    public async Task Version3RoutesRequireThePublishedSpelling(string path, HttpStatusCode expectedStatus)
    {
        using var factory = CreateFactory(new Mock<ICodeService>());
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(path);

        Assert.Equal(expectedStatus, response.StatusCode);
    }

    private static WebApplicationFactory<Program> CreateFactory(Mock<ICodeService> codes, Mock<ITypeApiService>? types = null)
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
                if (types != null)
                {
                    services.RemoveAll<ITypeApiService>();
                    services.AddSingleton(types.Object);
                }
            });
        });
    }
}
