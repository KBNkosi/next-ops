using Microsoft.AspNetCore.Mvc;
using JobCommandCenter.Models;
using JobCommandCenter.Enums;
using JobCommandCenter.Services;
using JobCommandCenter.DTOs.Applications;

namespace JobCommandCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationService _applicationService;

        public ApplicationsController(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        // 1. CREATE: api/applications
        [HttpPost]
        public ActionResult<ApplicationResponse> Create(CreateApplicationRequest newApplication)
        {

            var response = _applicationService.Create(newApplication);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // 2. GET ALL: api/applications
        [HttpGet]
        public ActionResult<List<ApplicationResponse>> GetAll()
        {
            var response = _applicationService.GetAll();
            return Ok(response);
        }

        // 3. GET APPLICATION: api/applications/{id}
        [HttpGet("{id}")]
        public ActionResult<ApplicationResponse> GetById(int id)
        {
            try
            {
                var response = _applicationService.GetById(id);
                return Ok(response);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid application ID");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }


        // // 4. UPDATE: api/applications/{id}/status
        // [HttpPatch("{id}/status")]
        // public ActionResult UpdateStatus(int id, [FromBody] string newStatus)
        // {
        //     var app = _applications.FirstOrDefault(x => x.Id == id);
        //     if (app == null) return NotFound();

        //     if (!Enum.TryParse<ApplicationStatus>(newStatus, true, out var parsedStatus))
        //     {
        //         return BadRequest("Invalid status value");
        //     }

        //     app.Status = parsedStatus;
        //     return NoContent();
        // }


        // // 5. DELETE: api/applications
        // [HttpDelete("{id}")]
        // public ActionResult DeleteApp(int id)
        // {
        //     var app = _applications.FirstOrDefault(x => x.Id == id);
        //     if (app == null) return NotFound();

        //     _applications.Remove(app);
        //     return NoContent();
        // }
    }
}
