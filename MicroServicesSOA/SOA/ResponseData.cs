using MicroServicesSOA.SOAV;

namespace MicroServicesSOA.SOA
{
    public class ResponseData
    {
        public string? Code { get; set; } = "0";
        public string? Message { get; set; } = "Receipt";
        public string? Timespan { get; set; } = Compute.ComputeTimeSpan(new DateTime());
    }
}