using System.Text.RegularExpressions;
using NewsNet.Domain.Common;

namespace NewsNet.Domain.ValueObjects;

public sealed class Slug : IEquatable<Slug>
{
    private static readonly Regex KebabRegex =
        new(@"^[a-z0-9]+(-[a-z0-9]+)*$", RegexOptions.Compiled);

    public string Value { get; }

    private Slug(string value) => Value = value;

    public static Slug Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("slug.empty", "Slug cannot be empty.");

        if (value.Length > 160)
            throw new DomainException("slug.length", "Slug length must be ≤ 160.");

        if (!KebabRegex.IsMatch(value))
            throw new DomainException("slug.format", "Slug must be kebab-case (^[a-z0-9]+(-[a-z0-9]+)*$).");

        return new Slug(value);
    }

    public override string ToString() => Value;

    public bool Equals(Slug? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is Slug s && Equals(s);
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);
}
