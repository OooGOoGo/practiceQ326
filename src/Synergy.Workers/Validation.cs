namespace Synergy.Workers;

internal static class Validation
{
    public const int MinHireYear = 1950;

    public static string RequireText(string? value, string what)
    {
        var text = value?.Trim();
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException($"{what}: значение не может быть пустым.");
        return text;
    }
}
