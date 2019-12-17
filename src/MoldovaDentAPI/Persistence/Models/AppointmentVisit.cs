using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoldovaDentAPI.Persistence.Models
{
    public class AppointmentVisit
    {
        [Required]
        public int Id { get; set; }
        public DateTime TimeOfVisit { get; set; }
        public int? ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public TimeSpan AdjustedDuration { get; set; }
        public decimal AdjustedPrice { get; set; }

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
    }
}
