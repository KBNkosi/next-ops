using Microsoft.AspNetCore.Mvc;
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

        // 4. UPDATE: api/applications/{id}
        [HttpPut("{id}")]
        public ActionResult<ApplicationResponse> UpdateApplication(int id, UpdateApplicationRequest request)
        {
            
             try
            {
                var response = _applicationService.UpdateApplication(id, request);
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


        // 5. UPDATE: api/applications/{id}/status
        [HttpPatch("{id}/status")]
        public ActionResult<ApplicationResponse> UpdateStatus(int id, [FromBody] UpdateApplicationStatusRequest newStatus)
        {
             try
            {
                var response = _applicationService.UpdateStatus(id, newStatus);
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


        // 6. DELETE: api/applications
        [HttpDelete("{id}")]
        public ActionResult DeleteApplication(int id)
        {
             try
            {
                _applicationService.DeleteApplication(id);
                return NoContent();
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
    }
}
