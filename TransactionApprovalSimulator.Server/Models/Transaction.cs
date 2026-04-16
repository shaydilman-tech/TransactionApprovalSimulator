using System;

namespace TransactionApprovalSimulator.Server.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Country { get; set; }
        public DateTime ScheduledAt { get; set; }
        public bool Approved { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}