using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace NinKode.WebApi.Helpers.Swagger;

public sealed class VersionedApiDescriptionCompatibility : IApiDescriptionProvider
{
    private const string GeneratedGroup = "v3.0v2026";
    private const string PublicGroup = "v3.0-v2026";

    // Providers finish in reverse order, after the versioning provider has generated descriptions.
    public int Order => -1001;

    public static string PublicGroupName(string groupName) =>
        groupName == GeneratedGroup ? PublicGroup : groupName;

    public void OnProvidersExecuting(ApiDescriptionProviderContext context) { }

    public void OnProvidersExecuted(ApiDescriptionProviderContext context)
    {
        foreach (var description in context.Results)
        {
            if (description.GroupName == GeneratedGroup)
            {
                description.GroupName = PublicGroup;
                if (description.RelativePath?.StartsWith("3.0v2026/", StringComparison.Ordinal) == true)
                    description.RelativePath = PublicGroup + description.RelativePath["3.0v2026".Length..];
            }
            else if (description.GroupName == "v3.0" &&
                     description.RelativePath?.StartsWith("3.0/", StringComparison.Ordinal) == true)
            {
                description.RelativePath = "v" + description.RelativePath;
            }
        }
    }
}
