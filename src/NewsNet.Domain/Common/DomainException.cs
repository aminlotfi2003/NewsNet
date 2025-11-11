namespace NewsNet.Domain.Common;

public class DomainException : Exception
{
    public string Code { get; }

    public DomainException(string code, string message) : base(message)
    {
        Code = code;
    }

    public static DomainException Required(string field) =>
        new("required", $"{field} is required.");

    public static DomainException NotUnique(string field, string value) =>
        new("not_unique", $"{field} '{value}' already exists.");

    public static DomainException Invalid(string field, string reason) =>
        new("invalid", $"{field} is invalid: {reason}");
}
