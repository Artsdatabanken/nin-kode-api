using Asp.Versioning;

namespace NinKode.WebApi.Helpers;

public sealed class LegacyApiVersionParser : IApiVersionParser
{
    public ApiVersion Parse(ReadOnlySpan<char> text)
    {
        if (text.Equals("2.1b", StringComparison.OrdinalIgnoreCase))
            return new ApiVersion(2, 1, "b");

        return ApiVersionParser.Default.Parse(text);
    }

    public bool TryParse(ReadOnlySpan<char> text, out ApiVersion version)
    {
        if (text.Equals("2.1b", StringComparison.OrdinalIgnoreCase))
        {
            version = new ApiVersion(2, 1, "b");
            return true;
        }

        return ApiVersionParser.Default.TryParse(text, out version);
    }
}
