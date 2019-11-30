using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoldovaDentAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoldovaDentAPI.Helpers;
using MoldovaDentAPI.Helpers.Exceptions;
using MoldovaDentAPI.ModelsDto;
using MoldovaDentAPI.Persistence.Interfaces;
using MoldovaDentAPI.Persistence.Repositories.Abstractions;
using MoldovaDentAPI.Persistence.Repositories.Interfaces;
using MoldovaDentAPI.Services.Interfaces;
using Profile = MoldovaDentAPI.Persistence.Models.Profile;

namespace MoldovaDentAPI.Services
{
    public class ProfileService: IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public ProfileService(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _profileRepository = unitOfWork.ProfileRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public ProfileAuthenticationResponseDto Authenticate(ProfileAuthenticationRequestDto profileFromUi)
        {
            // Validating parameters
            var invalidParams = new List<string>();
            if (!IsValidEmail(profileFromUi.Email)) // TODO: Do I need email validation on login?
                invalidParams.Add(nameof(profileFromUi.Email));

            if (invalidParams.Any()) throw new ValidationException(invalidParams);
            
            var profileFromDb = _profileRepository.GetProfileByEmail(profileFromUi.Email);

            if (profileFromDb == null)
            {
                throw new AuthenticationException();
            }

            if (profileFromDb.LockDate.HasValue &&
                profileFromDb.LockDate.Value < DateTime.Now.AddDays(1))// TODO: Magic number
            {
                throw new DomainModelException("Account locked");
            }

            if (!VerifyPasswordHash(profileFromUi.Password,
                profileFromDb.PasswordHash, profileFromDb.PasswordSalt))
            {
                _profileRepository.AddAttempt(profileFromDb.Email);
                if (profileFromDb.IncorrectAttempts >= 5)// TODO: Magic number
                {
                    _profileRepository.LockProfile(profileFromDb.Email);
                    throw new DomainModelException("Account locked");
                }
                throw new AuthenticationException();
            }
            
            // Generating response
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
        {
            var invalidParams = new List<string>();
            if (!IsValidEmail(profileFromUi.Email))
                invalidParams.Add(nameof(profileFromUi.Email));

            if (!IsValidPassword(profileFromUi.Password))
                invalidParams.Add(nameof(profileFromUi.Password));

            if (invalidParams.Any())
                throw new ValidationException(invalidParams);
            

            CreatePasswordHash(profileFromUi.Password, out byte[] passwordHash,
                out byte[] passwordSalt);

            var profileForDb = new Profile()
            {
                Email = profileFromUi.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            try
            {
                _profileRepository.CreateProfile(profileForDb);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException?.Message
                        .Contains("Email") ?? false)
                    throw new DomainModelException("Email already used");
                throw;
            }
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

        private static bool IsValidPassword(string password)
        {
            return RegexConstants.Password.IsMatch(password);
        }

        private static bool IsValidEmail(string email)
        {
            return RegexConstants.Email.IsMatch(email);
        }
    }
}
