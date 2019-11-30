using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Force.DeepCloner;
using MoldovaDentAPI.Helpers.Exceptions;
using MoldovaDentAPI.ModelsDto.Appointment;
using MoldovaDentAPI.Persistence.Interfaces;
using MoldovaDentAPI.Persistence.Models;
using MoldovaDentAPI.Persistence.Repositories.Abstractions;
using MoldovaDentAPI.Persistence.Repositories.Interfaces;
using MoldovaDentAPI.Services.Interfaces;

namespace MoldovaDentAPI.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Appointment> appointmentRepository;
        private readonly IProfileRepository profileRepository;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            appointmentRepository = unitOfWork.AppointmentRepository;
            profileRepository = unitOfWork.ProfileRepository;
        }
        
        public int AddAppointment(InsertAppointmentRequestDto request)
        {
            Appointment appointment;

            ValidateRequest(request);

            ConvertDtoToDbModel(request, out appointment);
            appointmentRepository.Create(appointment);
            unitOfWork.Save();
            return appointment.Id;
        }

        public int UpdateAppointment(UpdateAppointmentRequestDto request)
        {
            Appointment appointment;

            ValidateRequest(request);
            ConvertDtoToDbModel(request as InsertAppointmentRequestDto, out var newAppointment);

            appointment = GetAppointment(request.Id);

            if (appointment.ProfileId != newAppointment.ProfileId)
            {
                throw new DomainModelException("Provided ProfileId is not corresponding Appointment record");
            }

            appointment = AdjustAppointment(appointment, newAppointment);

            appointmentRepository.Update(appointment);
            unitOfWork.Save();
            return appointment.Id;
        }

        public Appointment GetAppointment(int id)
        {
            return appointmentRepository.Get(id);
        }

        public void DeleteAppointment(int id, int profileId)
        {
            Appointment appointment = appointmentRepository.Get(id);
            if (appointment == null)
            {
                throw new DomainModelException("Appointment is not found");
            }

            if (appointment.ProfileId != profileId)
            {
                throw new DomainModelException("Provided profileId is not corresponding Appointment record");
            }

            appointmentRepository.Delete(id);
            unitOfWork.Save();
        }

        #region Private Methods

        private void ValidateRequest(UpdateAppointmentRequestDto request)
        {
            if (request.Id == 0 || request.Id < 0)
            {
                throw new DomainModelException("Appointment Id is a required field");
            }

            ValidateRequest(request as InsertAppointmentRequestDto);
        }

        private void ValidateRequest(InsertAppointmentRequestDto request)
        {
            if (request.AppointmentVisits == null || request.AppointmentVisits.Count == 0)
            {
                throw new DomainModelException("Appointment should contain at least 1 visit");
            }

            if (request.StartDate == default)
            {
                throw new DomainModelException("Appointment should contain start date/time");
            }

            foreach (var visit in request.AppointmentVisits)
            {
                if (visit.TimeOfVisit == default)
                {
                    throw new DomainModelException("Every appointment visit should contain time of visit");
                }
            }
        }
        
        private void ConvertDtoToDbModel(InsertAppointmentRequestDto dto, out Appointment dbModel)
        {
            dbModel = new Appointment()
            {
                Comment = dto.Comment,
                CreationTime = DateTime.UtcNow,
                StartDate = dto.StartDate,
                ProfileId = dto.ProfileId,
                AppointmentVisits = new List<AppointmentVisit>()
            };
            foreach (var visitDto in dto.AppointmentVisits)
            {
                AppointmentVisit visit;
                ConvertDtoToDbModel(visitDto, out visit);
                dbModel.AppointmentVisits.Add(visit);
            }
        }

        private void ConvertDtoToDbModel(AppointmentVisitDto dto, out AppointmentVisit dbModel)
        {
            dbModel = new AppointmentVisit()
            {
                ProcedureId = dto.ProcedureId,
                ProcedureName = dto.ProcedureName,
                AdjustedDuration = dto.AdjustedDuration.Value,
                AdjustedPrice = dto.AdjustedPrice.Value,
                TimeOfVisit = dto.TimeOfVisit
            };
        }

        private Appointment AdjustAppointment(Appointment oldAppointment, Appointment newAppointment)
        {
            oldAppointment.Comment = Adjust(oldAppointment.Comment, newAppointment.Comment);
            oldAppointment.StartDate = Adjust(oldAppointment.StartDate, newAppointment.StartDate);
            oldAppointment.AppointmentVisits = newAppointment.AppointmentVisits ?? oldAppointment.AppointmentVisits;
            
            oldAppointment.LastUpdatedTime = DateTime.UtcNow;

            return oldAppointment;
        }

        private T Adjust<T>(T oldObj, T newObj) where T:IEquatable<T>
        {
            if (newObj.Equals(default))
                return oldObj;
            return newObj;
        }

        #endregion
    }
}
