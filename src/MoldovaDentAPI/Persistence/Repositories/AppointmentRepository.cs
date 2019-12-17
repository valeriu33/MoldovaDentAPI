using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoldovaDentAPI.Persistence.Models;
using MoldovaDentAPI.Persistence.Repositories.Abstractions;
using MoldovaDentAPI.Persistence.Repositories.Interfaces;

namespace MoldovaDentAPI.Persistence.Repositories
{
    public class AppointmentRepository : IRepository<Appointment>
    {
        private readonly DataContext context;

        public AppointmentRepository(DataContext context)
        {
            this.context = context;
        }


        public void Create(Appointment item)
        {
            context.Appointments.Add(item);
        }

        public void Delete(int id)
        {
            Appointment appointment = context.Appointments.Find(id);

            if (appointment != null)
            {
                context.Remove(appointment);
            }
        }

        public Appointment Get(int id)
        {
            return context.Appointments.Where(x => x.Id == id).Include(x => x.AppointmentVisits).SingleOrDefault();
        }

        public IEnumerable<Appointment> GetAll()
        {
            return context.Appointments;
        }

        public void Update(Appointment item)
        {
            context.Entry(item).State = EntityState.Modified;
        }
    }
}
