using NewsNet.Domain.Abstractions;
using NewsNet.Domain.Common;
using System.Text.RegularExpressions;

namespace NewsNet.Domain.ValueObjects;

public sealed class Slug : ValueObject
{
    private static readonly Regex InvalidChars = new("[^a-z0-9\\- ]", RegexOptions.Compiled);
    private static readonly Regex MultiSpaces = new("\\s+", RegexOptions.Compiled);
    private static readonly Regex MultiDashes = new("\\-+", RegexOptions.Compiled);

    public string Value { get; }

    private Slug(string value) => Value = value;

    public static async Task<Slug> CreateAsync(
        string? raw,
        IUniqueSlugChecker uniqueChecker,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw DomainException.Required(nameof(Slug));

        var normalized = Normalize(raw);

        if (string.IsNullOrWhiteSpace(normalized))
            throw DomainException.Invalid(nameof(Slug), "Empty after normalization.");

        var isUnique = await uniqueChecker.IsUniqueAsync(normalized, ct);
        if (!isUnique)
            throw DomainException.NotUnique(nameof(Slug), normalized);

        return new Slug(normalized);
    }

    public static Slug FromExisting(string value) => new(value);

    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    // Helpers
    public static string Normalize(string input)
    {
        var s = input.Trim().ToLowerInvariant();

        s = s.Replace('‌', ' ')  // ZWNJ → space
             .Replace('–', '-')  // en-dash
             .Replace('—', '-')  // em-dash
             .Replace('ـ', ' '); // kashida

        s = InvalidChars.Replace(s, string.Empty);

        s = MultiSpaces.Replace(s, " ");

        s = s.Replace(' ', '-');

        s = MultiDashes.Replace(s, "-");

        s = s.Trim('-');

        return s;
    }
}
