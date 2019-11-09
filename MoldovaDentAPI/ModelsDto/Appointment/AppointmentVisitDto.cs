using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoldovaDentAPI.ModelsDto.Appointment
{
    public class AppointmentVisitDto
    {
        public DateTime TimeOfVisit { get; set; }
        public int? ProcedureId { get; set; }
        public string ProcedureName { get; set; }
        public TimeSpan? AdjustedDuration { get; set; }
        public decimal? AdjustedPrice { get; set; }
    }
}
