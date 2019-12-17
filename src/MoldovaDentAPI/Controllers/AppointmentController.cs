using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoldovaDentAPI.ModelsDto.Appointment;
using MoldovaDentAPI.Persistence.Models;

using MoldovaDentAPI.Services.Interfaces;

namespace MoldovaDentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IProfileService profileService;
        private readonly IAppointmentService appointmentService;

        public AppointmentController(IProfileService profileService, IAppointmentService appointmentService)
        {
            this.profileService = profileService;
            this.appointmentService = appointmentService;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult InsertAppointment([FromBody] InsertAppointmentRequestDto request)
        {
            int recordId  = 0;
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claims = identity?.Claims;
                var profileClaim = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (profileClaim != null && int.TryParse(profileClaim.Value, out var profileId))
                {
                    var profile = profileService.GetProfileById(profileId);
                    request.ProfileId = profile?.Id;
                }

                recordId = appointmentService.AddAppointment(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok(recordId);
        }

        [HttpPost]
        [Authorize]
        [Route("Edit")]
        public IActionResult UpdateAppointment([FromBody] UpdateAppointmentRequestDto request)
        {
            int recordId = 0;
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claims = identity?.Claims;
                var profileClaim = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (profileClaim != null && int.TryParse(profileClaim.Value, out var profileId))
                {
                    var profile = profileService.GetProfileById(profileId);
                    request.ProfileId = profile?.Id;
                }

                recordId = appointmentService.UpdateAppointment(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok(recordId);
        }

        [HttpGet]
        [Route("get/{id}")]
        public IActionResult GetAppointment(int id)
        {
            Appointment appointment;
            try
            {
                appointment = appointmentService.GetAppointment(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return Ok(appointment);
        }

        [HttpGet]
        [Authorize]
        [Route("delete/{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            try
            {
                Profile profile=null;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IEnumerable<Claim> claims = identity?.Claims;
                var profileClaim = claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name);

                if (profileClaim != null && int.TryParse(profileClaim.Value, out var profileId))
                {
                    profile = profileService.GetProfileById(profileId);
                }

                appointmentService.DeleteAppointment(id, profile?.Id ?? 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return Ok();
        }
    }
}
