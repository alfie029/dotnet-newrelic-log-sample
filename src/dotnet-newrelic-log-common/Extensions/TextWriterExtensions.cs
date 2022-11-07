using System.Reflection;
using Serilog.Events;

namespace dotnet_newrelic_log_common.Extensions;

internal static class TextWriterExtensions
{
    public static void WriteObjectStart(this TextWriter output) => output.Write("{");
    public static void WriteObjectEnd(this TextWriter output) => output.Write("}");
    public static void WriteTrailingComma(this TextWriter output) => output.Write(",");

    public static void WriteAssemblyInfo(this TextWriter output) =>
        output.Write($"\"@assembly\":\"{Assembly.GetEntryAssembly()?.FullName}\"");

    public static void WriteTimestamp(this TextWriter output, string utcDateTimeString) =>
        output.Write($"\"@t\":\"{utcDateTimeString}\"");

    public static void WriteLogLevel(this TextWriter output, LogEventLevel logLevel) =>
        output.Write($"\"@l\":\"{logLevel.ToString()}\"");

    public static void WriteExceptionMessage(this TextWriter output, string? quotedTruncatedExceptionMessage) =>
        output.Write($"\"@x\":{quotedTruncatedExceptionMessage}");

    public static void WriteMessageTemplate(this TextWriter output, string quotedMessageTemplate) =>
        output.Write($"\"@mt\":{quotedMessageTemplate}");

    public static void WriteRenderedMessage(this TextWriter output, string quotedMessage) =>
        output.Write($"\"message\":{quotedMessage}");

    public static void WriteReferredProperties(this TextWriter output, IEnumerable<string> quotedRefProperties) =>
        output.Write($"\"@r\":[{string.Join(',', quotedRefProperties)}]");

    public static void WriteProperty(this TextWriter output, string propertyKey, string quotedPropertyValue) =>
        output.Write($"\"{propertyKey}\":{quotedPropertyValue}");
}
