namespace Marketplace.Common.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Переводит первый символ в строке к нижнему регистру, если строка не пустая
    /// </summary>
    public static string SetFirstLetterToLowerCase(this string value)
    {
        if (!string.IsNullOrWhiteSpace(value) && char.IsUpper(value[0]))
            return value.Length == 1 ? char.ToLower(value[0]).ToString() : char.ToLower(value[0]) + value[1..];

        return value;
    }
}