using System;

using System.ComponentModel.DataAnnotations;

namespace MoldovaDentAPI.Persistence.Models
{
    public class Procedure
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? StandartDuration { get; set; }
        public decimal? StandartPrice { get; set; }
    }
}
