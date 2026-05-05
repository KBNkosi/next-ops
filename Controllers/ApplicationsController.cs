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

        // 1. GET ALL: api/applications
        [HttpGet]
        public ActionResult<List<Application>> GetAll()
        {
            return Ok(_applications);
        }

        // 2. CREATE: api/applications
        [HttpPost]
        public ActionResult<Application> Create(Application newApp)
        {
            newApp.Id = _applications.Max(a => a.Id) + 1;

            _applications.Add(newApp);

            return CreatedAtAction(nameof(GetAll), new { id = newApp.Id }, newApp);
        }

        // 3. UPDATE: api/applications
        [HttpPatch("id")]
        public ActionResult UpdateStatus(int id, [FromBody] string newStatus)
        {
            var app = _applications.FirstOrDefault(x => x.Id == id);
            if(app == null) return NotFound();

            app.Status = Enum.Parse<ApplicationStatus>(newStatus);
            return NoContent();
        }

        // 4. DELETE: api/applications
        [HttpDelete("id")]
        public ActionResult DeleteApp(int id)
        {
            var app = _applications.FirstOrDefault(x => x.Id == id);
            if(app == null) return NotFound();

            _applications.Remove(app);
            return NoContent();
        }
    }
} 
