using Serilog.Events;

namespace dotnet_newrelic_log_common.Extensions;

public static class LogEventExtensions
{
    const string LinkingMetadataKey = "newrelic.linkingmetadata";

    public static IEnumerable<KeyValuePair<string, LogEventPropertyValue>> Flatten(
        this IEnumerable<KeyValuePair<string, LogEventPropertyValue>> properties,
        string keyPrefix = "",
        int depth = 2)
    {
        return depth <= 0
            ? properties
            : properties.SelectMany(p => p.Value switch
            {
                DictionaryValue value when p.Key == LinkingMetadataKey => value.Elements.Select(e =>
                    new KeyValuePair<string, LogEventPropertyValue>(e.Key.Value?.ToString()!, e.Value)),
                DictionaryValue value => value.Elements.Select(e =>
                        new KeyValuePair<string, LogEventPropertyValue>(e.Key.Value?.ToString()!, e.Value))
                    .Flatten(string.IsNullOrWhiteSpace(keyPrefix) ? p.Key : $"{keyPrefix}.{p.Key}", depth - 1),
                SequenceValue value => value.Elements.Select((e, i) =>
                        new KeyValuePair<string, LogEventPropertyValue>($"[{i}]", e))
                    .Flatten($"{keyPrefix}{p.Key}", depth - 1),
                StructureValue value => value.Properties.Select(vp =>
                        new KeyValuePair<string, LogEventPropertyValue>(vp.Name, vp.Value))
                    .Flatten(string.IsNullOrWhiteSpace(keyPrefix) ? p.Key : $"{keyPrefix}.{p.Key}", depth - 1),
                _ => new[] { p }
            });
    }
}
