using System;
using Microsoft.EntityFrameworkCore;
using MoldovaDentAPI.Persistence.Interfaces;
using MoldovaDentAPI.Persistence.Models;
using MoldovaDentAPI.Persistence.Repositories;
using MoldovaDentAPI.Persistence.Repositories.Abstractions;

namespace MoldovaDentAPI.Persistence
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
        private readonly DataContext context;
        private ProfileRepository profileRepository;
        private AppointmentRepository appointmentRepository;
        
        public ProfileRepository ProfileRepository => this.profileRepository ?? new ProfileRepository(context);
        public AppointmentRepository AppointmentRepository => this.appointmentRepository ?? new AppointmentRepository(context);

        public UnitOfWork(DataContext context)
        {
            this.context = context;
        }


        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    
}
