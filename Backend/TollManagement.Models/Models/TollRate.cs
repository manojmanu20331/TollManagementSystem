using System;

namespace TollManagement.Models
{
    public class TollRate
    {
        public int TollRateId { get; set; }

        public int TollPlazaId { get; set; }

        public string VehicleType { get; set; }

        public decimal Amount { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}