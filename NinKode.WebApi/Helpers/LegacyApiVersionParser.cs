using Asp.Versioning;

namespace NinKode.WebApi.Helpers;

public sealed class LegacyApiVersionParser : IApiVersionParser
{
    public ApiVersion Parse(ReadOnlySpan<char> text)
    {
        if (text.Equals("2.1b", StringComparison.OrdinalIgnoreCase))
            return new ApiVersion(2, 1, "b");

        return ApiVersionParser.Default.Parse(StripVersion3Prefix(text));
    }

    public bool TryParse(ReadOnlySpan<char> text, out ApiVersion version)
    {
        if (text.Equals("2.1b", StringComparison.OrdinalIgnoreCase))
        {
            version = new ApiVersion(2, 1, "b");
            return true;
        }

        return ApiVersionParser.Default.TryParse(StripVersion3Prefix(text), out version);
    }

    private static ReadOnlySpan<char> StripVersion3Prefix(ReadOnlySpan<char> text) =>
        text.Length > 1 && (text[0] == 'v' || text[0] == 'V') &&
        ApiVersionParser.Default.TryParse(text[1..], out var version) && version.MajorVersion == 3
            ? text[1..]
            : text;
}
