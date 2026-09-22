using System;

namespace TollManagement.Models
{
    public class TollPlaza
    {
        public int TollPlazaId { get; set; }

        public string PlazaName { get; set; }

        public string Location { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}