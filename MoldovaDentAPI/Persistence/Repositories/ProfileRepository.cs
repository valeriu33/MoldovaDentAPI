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

        public void CreateProfile(Profile profile)
        {
            _context.Profiles.Add(profile);
            _context.SaveChanges();
        }

        public Profile GetProfileById(int id)
        {
            return _context.Profiles.SingleOrDefault(p => p.Id == id);
        }
    }
}
