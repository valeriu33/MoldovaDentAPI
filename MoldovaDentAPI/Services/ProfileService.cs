using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MoldovaDentAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoldovaDentAPI.Helpers;
using MoldovaDentAPI.ModelsDto;
using MoldovaDentAPI.Persistence.Repositories.Abstractions;
using MoldovaDentAPI.Services.Abstractions;
using Profile = MoldovaDentAPI.Persistence.Models.Profile;


namespace MoldovaDentAPI.Services
{
    public class ProfileService: IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public ProfileService(IProfileRepository profileRepository, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public ProfileAuthenticationResponseDto Authenticate(ProfileAuthenticationRequestDto profileFromUi)
        {//TODO: Validate parameters
            var profileFromDb = _profileRepository.GetProfileByEmail(profileFromUi.Email);

            if (profileFromDb == null) return null;

            if (!VerifyPasswordHash(profileFromUi.Password,
                profileFromDb.PasswordHash, profileFromDb.PasswordSalt))
            {
                return null;
            }

            var responseProfile = new ProfileAuthenticationResponseDto
            {
                Email = profileFromDb.Email
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                    {
                        new Claim(ClaimTypes.Name, profileFromDb.Id.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            responseProfile.Token = tokenHandler.WriteToken(token);

            return responseProfile;    
        }

        public void Register(ProfileAuthenticationRequestDto profileFromUi)
        {//TODO: Validate parameters
            CreatePasswordHash(profileFromUi.Password, out byte[] passwordHash,
                out byte[] passwordSalt);

            var profileForDb = new Profile()
            {
                Email = profileFromUi.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _profileRepository.CreateProfile(profileForDb);
        }

        public Profile GetProfileById(int id)
        {
            return _profileRepository.GetProfileById(id);
        }

        public ProfileEditDto Update(ProfileEditDto profile)
        {
            return null;
        }

        private static void CreatePasswordHash(string password,
            out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash =
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password,
            byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac =
                new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash =
                    hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }

                return true;
            }
        }
    }
}
