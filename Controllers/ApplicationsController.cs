using Microsoft.AspNetCore.Mvc;
using JobCommandCenter.Models;
using JobCommandCenter.Enums;

namespace JobCommandCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        // Temporary in-memory storage
        private static List<Application> _applications = new List<Application>
        {
            new Application
            {
                Id = 1,
                CompanyName = "Google",
                RoleTitle = "Junior Dev",
                Status = ApplicationStatus.Applied,
            },
        };

        // 1. CREATE: api/applications
        [HttpPost]
        public ActionResult<ApplicationResponse> Create(CreateApplicationRequest newApplication)
        {
            int applicationId = _applications.Any() ? _applications.Max(a => a.Id) + 1 : 1;

            var application = new Application
            {
                Id = applicationId,
                CompanyName = newApplication.CompanyName,
                RoleTitle = newApplication.RoleTitle,
                Source = newApplication.Source,
                Status = ApplicationStatus.Saved,
                JobLink = newApplication.JobLink,
                FollowUpDate = newApplication.FollowUpDate,
                Notes = newApplication.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow

            };

            _applications.Add(application);

            return CreatedAtAction(nameof(GetApplication), new { id = application.Id },
            new ApplicationResponse
            {
                Id = application.Id,
                CompanyName = application.CompanyName,
                RoleTitle = application.RoleTitle,
                Source = application.Source,
                Status = application.Status,
                JobLink = application.JobLink,
                DateApplied = application.DateApplied,
                FollowUpDate = application.FollowUpDate,
                Notes = application.Notes,
                CreatedAt = application.CreatedAt,
                UpdatedAt = application.UpdatedAt
            }
            );
        }

        // 2. GET ALL: api/applications
        [HttpGet]
        public ActionResult<List<Application>> GetAll()
        {
            return Ok(_applications);
        }

        // 3. GET APPLICATION: api/applications/{id}
        [HttpGet("{id}")]
        public ActionResult<Application> GetApplication(int id)
        {
            var app = _applications.FirstOrDefault(x => x.Id == id);
            if (app == null) return NotFound();

            return Ok(app);
        }


        // 4. UPDATE: api/applications/{id}/status
        [HttpPatch("{id}/status")]
        public ActionResult UpdateStatus(int id, [FromBody] string newStatus)
        {
            var app = _applications.FirstOrDefault(x => x.Id == id);
            if (app == null) return NotFound();

            if (!Enum.TryParse<ApplicationStatus>(newStatus, true, out var parsedStatus))
            {
                return BadRequest("Invalid status value");
            }

            app.Status = parsedStatus;
            return NoContent();
        }


        // 5. DELETE: api/applications
        [HttpDelete("{id}")]
        public ActionResult DeleteApp(int id)
        {
            var app = _applications.FirstOrDefault(x => x.Id == id);
            if (app == null) return NotFound();

            _applications.Remove(app);
            return NoContent();
        }
    }
}
