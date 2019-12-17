using System;
using System.Collections.Generic;

namespace MoldovaDentAPI.ModelsDto.Appointment
{
    public class InsertAppointmentRequestDto 
    {
        public int? ProfileId { get; set; }
        public DateTime StartDate { get; set; }
        public string Comment { get; set; }

        public ICollection<AppointmentVisitDto> AppointmentVisits { get; set; }
    }
}
