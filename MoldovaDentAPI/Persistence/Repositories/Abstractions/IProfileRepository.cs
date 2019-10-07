using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoldovaDentAPI.Models;
using MoldovaDentAPI.Persistence.Models;

namespace MoldovaDentAPI.Persistence.Repositories.Abstractions
{
    public interface IProfileRepository
    {
        Profile GetProfileByEmail(string email);

        Profile CreateProfile(Profile profile);

        Profile GetProfileById(int id);
    }
}
