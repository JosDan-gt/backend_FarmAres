using System;
using System.Collections.Generic;

namespace MyProyect_Granja.Models
{
    public partial class TriggerDebugLog
    {
        public int Id { get; set; }
        public string? OperationType { get; set; }
        public int? OldCartonesExtras { get; set; }
        public int? NewCartonesExtras { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
