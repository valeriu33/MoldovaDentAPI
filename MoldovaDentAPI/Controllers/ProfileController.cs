using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using MoldovaDentAPI.Helpers.Exceptions;
using MoldovaDentAPI.ModelsDto;
using MoldovaDentAPI.Services.Interfaces;

namespace MoldovaDentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(
            IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] ProfileAuthenticationRequestDto profileParam)
        {
            try
            {
                var profile = _profileService.Authenticate(profileParam);
                return Ok(profile);
            }
            catch (ValidationException validationException)
            {
                return BadRequest(new ErrorResponse(validationException.FieldNames));
            }
            catch (AuthenticationException)
            {
                return BadRequest(new ErrorResponse(
                    "Could not find any profile with this email and password"));
            }
            catch (DomainModelException domainModelException)
            {
                return BadRequest(new ErrorResponse(domainModelException.Message));
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] ProfileAuthenticationRequestDto profileParam)
        {
            try
            {
                _profileService.Register(profileParam);
                return Ok();
            }
            catch (ValidationException validationException)
            {
                return BadRequest(new ErrorResponse(validationException.FieldNames));
            }
            catch (DomainModelException domainModelException)
            {
                return BadRequest(new ErrorResponse(domainModelException.Message));
            }
        }
    }
}