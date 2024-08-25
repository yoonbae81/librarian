using System.Text.RegularExpressions;

namespace Domain.ValueObjects;
public class ClassificationCode
{
    private string _code = string.Empty;

    public required string Code
    {
        init
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3 || !int.TryParse(value.Substring(0, 3), out _))
            {
                throw new ArgumentException("The first three characters of the Classification code must be a valid three-digit number.");
            }
            _code = value;
        }
        get => _code;
    }
    public string Name { get; init; } = string.Empty;
}
