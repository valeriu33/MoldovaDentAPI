using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoldovaDentAPI.Persistence.Models
{
    public class Appointment
    {
        [Required]
        public int Id { get; set; }
        public int? ProfileId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string Comment { get; set; }

        public ICollection<AppointmentVisit> AppointmentVisits { get; set; }
    }
}
