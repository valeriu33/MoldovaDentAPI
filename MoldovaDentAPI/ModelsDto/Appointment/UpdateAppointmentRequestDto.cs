using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoldovaDentAPI.ModelsDto.Appointment
{
    public class UpdateAppointmentRequestDto: InsertAppointmentRequestDto
    {
        public int Id { get; set; }
    }
}
