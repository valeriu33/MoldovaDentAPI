using Microsoft.EntityFrameworkCore;
using MoldovaDentAPI.Persistence.Models;

namespace MoldovaDentAPI.Persistence
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Profile> Profiles { get; set; }
    }
}
