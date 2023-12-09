using System.Diagnostics;

namespace OVB.Demos.Eschody.Libraries.Observability.Trace.Facilitators;

public static class TraceFacilitator
{
    public static Activity AppendSpanTag(this Activity activity, params KeyValuePair<string, string>[] keyValuePairs)
    {
        foreach (var keyValuePair in keyValuePairs)
        {
            activity.AddTag(
                key: keyValuePair.Key,
                value: keyValuePair.Value);
        }

        return activity;
    }
}
