using System.ComponentModel.DataAnnotations;

namespace TransactionApprovalSimulator.Server.Models
{
    public class TransactionRequest
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Country { get; set; }
        [Range(0, 23)]
        public int ScheduledHour { get; set; }
        [Range(0, 59)]
        public int ScheduledMinute { get; set; }
    }
}