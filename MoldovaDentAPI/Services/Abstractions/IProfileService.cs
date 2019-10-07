using MoldovaDentAPI.Models;
using MoldovaDentAPI.ModelsDto;
using MoldovaDentAPI.Persistence.Models;

namespace MoldovaDentAPI.Services.Abstractions
{
    public interface IProfileService
    {
        ProfileAuthenticationResponseDto Authenticate(ProfileAuthenticationRequestDto profileFromUi);

        void Register(ProfileAuthenticationRequestDto profileFromUi);

        Profile GetProfileById(int id);

        ProfileEditDto Update(ProfileEditDto profile);
    }
}
