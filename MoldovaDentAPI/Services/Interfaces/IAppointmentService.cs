using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoldovaDentAPI.ModelsDto.Appointment;
using MoldovaDentAPI.Persistence.Models;

namespace MoldovaDentAPI.Services.Interfaces
{
    public interface IAppointmentService
    {
        int AddAppointment(InsertAppointmentRequestDto request);
        int UpdateAppointment(UpdateAppointmentRequestDto request);
        Appointment GetAppointment(int id);
        void DeleteAppointment(int id, int profileId);
    }
}
