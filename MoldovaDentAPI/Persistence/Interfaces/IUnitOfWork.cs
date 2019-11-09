using MoldovaDentAPI.Persistence.Models;
using MoldovaDentAPI.Persistence.Repositories;
using MoldovaDentAPI.Persistence.Repositories.Abstractions;

namespace MoldovaDentAPI.Persistence.Interfaces
{
    public interface IUnitOfWork
    {
        ProfileRepository ProfileRepository { get; }
        AppointmentRepository AppointmentRepository { get; }
        void Save();
    }
}