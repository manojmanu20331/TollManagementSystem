using System;

namespace TollManagement.Models
{
    public class TollTransaction
    {
        public int TransactionId { get; set; }

        public int VehicleId { get; set; }

        public int TollPlazaId { get; set; }

        public int TollRateId { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}