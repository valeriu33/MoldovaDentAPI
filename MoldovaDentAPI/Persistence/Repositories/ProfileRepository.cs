using System;
using System.Collections.Generic;
using System.Linq;
using MoldovaDentAPI.Persistence.Models;
using MoldovaDentAPI.Persistence.Repositories.Abstractions;
using MoldovaDentAPI.Persistence.Repositories.Interfaces;

namespace MoldovaDentAPI.Persistence.Repositories
{
    public class ProfileRepository: IProfileRepository
    {
        private readonly DataContext _context;

        public ProfileRepository(DataContext context)
        {
            _context = context;
        }

        public Profile GetProfileByEmail(string email)
        {
            return _context.Profiles.SingleOrDefault(p =>
                p.Email == email);
        }

        public void CreateProfile(Profile profile)
        {
            _context.Profiles.Add(profile);
            _context.SaveChanges();
        }

        public Profile GetProfileById(int id)
        {
            return _context.Profiles.SingleOrDefault(p => p.Id == id);
        }

        public void AddAttempt(string email)
        {
            var profile = _context.Profiles.Single(p => p.Email == email);
            profile.IncorrectAttempts++;
            _context.SaveChanges();
        }
        public void LockProfile(string email)
        {
            var profile = _context.Profiles.Single(p => p.Email == email);
            profile.LockDate = DateTime.Now;
            _context.SaveChanges();
        }
        public void UnlockProfile(string email)
        {
            var profile = _context.Profiles.Single(p => p.Email == email);
            profile.LockDate = null;
            _context.SaveChanges();
        }
    }
}