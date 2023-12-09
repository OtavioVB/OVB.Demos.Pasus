using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OVB.Demos.Eschody.Libraries.Observability.Trace.Facilitators;

public static class ObservabilityFacilitator
{
    public static string EndpointKey = "Endpoint";
    public static string HttpMethodKey = "HttpMethod";
    public static string CorrelationIdKey = "CorrelationId";
    public static string ExecutionUserKey = "ExecutionUser";
    public static string SourcePlatformKey = "SourcePlatform";
    public static string StatusCodeKey = "StatusCode";
    public static string IdempotencyKey = "IdempotencyKey";
    public static string HasUsedIdempotencyCache = "HasUsedIdempotencyCache";
    public static string RemoteHostKey = "RemoteHost";
    public static string RemoteAddrKey = "RemoteAddress";
    public static string HttpForwardedForKey = "HttpForwardedFor";
}
