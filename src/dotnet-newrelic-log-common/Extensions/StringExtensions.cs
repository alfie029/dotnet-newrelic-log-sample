using System.Globalization;
using System.Text;

namespace dotnet_newrelic_log_common.Extensions;

internal static class StringExtensions
{
    public static int SizeBytes(this string? value) => value == null ? 0 : Encoding.UTF8.GetByteCount(value);

    public static string? TruncateUnicodeStringByLength(this string? value, int maxLength)
    {
        if (maxLength < 0)
        {
            throw new ArgumentOutOfRangeException(
                $"maxLength must be positive.  value: {value}  maxLength: {maxLength}");
        }

        if (value?.Length <= maxLength)
        {
            return value;
        }

        var info = new StringInfo(value!);
        return info.LengthInTextElements <= maxLength ? value : info.SubstringByTextElements(0, maxLength);
    }

    public static string? TruncateUnicodeStringByBytes(this string? value, uint maxBytes)
    {
        value.TruncateUnicodeStringByBytes(maxBytes, out string? resultValue);
        return resultValue;
    }

    public static bool TruncateUnicodeStringByBytes(this string? value, uint maxBytes, out string? resultValue)
    {
        if (maxBytes == 0U)
        {
            resultValue = string.Empty;
            return value?.Length > 0;
        }

        resultValue = value;
        if (string.IsNullOrEmpty(value) || Encoding.UTF8.GetByteCount(value) <= maxBytes)
        {
            return false;
        }

        byte[] bytes = new byte[(int)maxBytes];
        char[] charArray = value.ToCharArray();
        try
        {
            Encoding.UTF8.GetEncoder().Convert(charArray, 0, charArray.Length, bytes, 0, (int)maxBytes, true,
                out var charsUsed, out int _, out bool _);
            resultValue = new string(charArray, 0, charsUsed);
            return true;
        }
        catch (ArgumentException)
        {
            resultValue = string.Empty;
            return value.Length > 0;
        }
    }
}
