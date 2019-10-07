using MoldovaDentAPI.ModelsDto;
using MoldovaDentAPI.Persistence.Models;

namespace MoldovaDentAPI.Helpers
{
    public class AutoMapperProfile: AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProfileAuthenticationRequestDto, Profile>();
            //CreateMap<Profile, ProfileAuthenticationResponseDto>();
        }
    }
}
