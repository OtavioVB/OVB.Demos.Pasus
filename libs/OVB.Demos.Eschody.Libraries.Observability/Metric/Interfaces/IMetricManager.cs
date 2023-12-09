using OVB.Demos.Eschody.Libraries.ValueObjects;

namespace OVB.Demos.Eschody.Libraries.Observability.Metric.Interfaces;

public interface IMetricManager
{
    public void CreateCounterIfNotExists(
        string counterName);
    public void IncrementCounter(
        string counterName, AuditableInfoValueObject auditableInfo, int quantity = 1, params KeyValuePair<string, object?>[] keyValuePairs);
}
