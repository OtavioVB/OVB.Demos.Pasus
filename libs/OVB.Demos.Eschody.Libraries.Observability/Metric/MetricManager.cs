using OVB.Demos.Eschody.Libraries.Observability.Metric.Interfaces;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace OVB.Demos.Eschody.Libraries.Observability.Metric;

public sealed class MetricManager : IMetricManager
{
    private readonly Meter _meter;
    private readonly IDictionary<string, Counter<int>> _countersDictionary;

    public MetricManager(Meter meter)
    {
        _meter = meter;
        _countersDictionary = new Dictionary<string, Counter<int>>();
    }

    public const string COUNTER_ALREADY_EXISTS_MESSAGE = "Esse contador já existe.";
    public const string COUNTER_NOT_EXISTS = "Esse contador não existe.";

    public void CreateCounter(
        string counterName)
    {
        if (_countersDictionary.ContainsKey(counterName))
            throw new ArgumentException(
                message: COUNTER_ALREADY_EXISTS_MESSAGE);

        var createdCounter = _meter.CreateCounter<int>(
                name: counterName);
        _countersDictionary.Add(
            key: counterName,
            value: createdCounter);
    }

    public void IncrementCounter(
        string counterName, int quantity = 1, params KeyValuePair<string, object?>[] keyValuePairs)
    {
        if (!_countersDictionary.ContainsKey(counterName))
            throw new ArgumentException(
                message: COUNTER_NOT_EXISTS);

        var counter = _countersDictionary[counterName];
        counter.Add(quantity, keyValuePairs);
    }
}
