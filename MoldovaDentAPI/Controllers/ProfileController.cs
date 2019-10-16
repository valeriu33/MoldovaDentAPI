using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MoldovaDentAPI.Helpers;
using MoldovaDentAPI.Helpers.Exceptions;
using MoldovaDentAPI.Models;
using MoldovaDentAPI.ModelsDto;
using MoldovaDentAPI.Services;
using MoldovaDentAPI.Services.Abstractions;

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
        {//TODO: Add a proper error handling in all methods
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

        [Authorize]
        [HttpPost("Update")]
        public IActionResult UpdateProfile([FromBody] ProfileEditDto profile)
        {
            var user = _profileService.Update(profile);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}