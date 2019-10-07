using System.Collections.Generic;
using System.Linq;
using MoldovaDentAPI.Models;
using MoldovaDentAPI.Persistence.Models;
using MoldovaDentAPI.Persistence.Repositories.Abstractions;

namespace MoldovaDentAPI.Persistence.Repositories
{
    public class ProfileRepository: IProfileRepository
    {
        private DataContext _context;

        public ProfileRepository(DataContext context)
        {
            _context = context;
        }

        public Profile GetProfileByEmail(string email)
        {
            return _context.Profiles.SingleOrDefault(p =>
                p.Email == email);
        }

        public Profile CreateProfile(Profile profile)
        {//TODO: What should CreateProfile() return?
            _context.Profiles.Add(profile);
            _context.SaveChanges();
            return profile;
        }

        public Profile GetProfileById(int id)
        {
            return _context.Profiles.SingleOrDefault(p => p.Id == id);
        }
    }
}
