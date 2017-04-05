using System;

namespace FieldWorkerBot.Domain
{
    public class Inspection
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public DateTime? InspectionDate { get; set; }
        public int StatusCode { get; set; }
    }
}
