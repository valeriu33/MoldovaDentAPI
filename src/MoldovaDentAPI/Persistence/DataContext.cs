﻿using Microsoft.EntityFrameworkCore;
using MoldovaDentAPI.Persistence.Models;

namespace MoldovaDentAPI.Persistence
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentVisit> AppointmentVisits { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Profile>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
