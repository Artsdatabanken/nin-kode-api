using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace NinKode.WebApi.Filters
{
    public class NotFoundTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        public NotFoundTelemetryProcessor(ITelemetryProcessor next)
        {
            Next = next;
        }
        public void Process(ITelemetry item)
        {
            if (item is RequestTelemetry request && request.ResponseCode == "404")
            {
                return;
            }

            if (item is DependencyTelemetry dependency && dependency.ResultCode == "404")
            {
                return;
            }
            Next.Process(item);
        }
    }
}