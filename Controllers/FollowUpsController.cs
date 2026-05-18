using Microsoft.AspNetCore.Mvc;
using JobCommandCenter.Models;
using JobCommandCenter.Enums;
using JobCommandCenter.Services;

namespace JobCommandCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowUpsController : ControllerBase
    {
        private readonly FollowUpService _followUpService;

        public FollowUpsController(FollowUpService followUpService)
        {
            _followUpService = followUpService;
        }
      
       // 1. CREATE: api/followups
       [HttpPost]
       public ActionResult<FollowUp> CreateFollowUp(FollowUp newFollowUp)
        {
            var response = _followUpService.Create(newFollowUp);

            return CreatedAtAction(nameof(GetFollowUp), new {id = response.Id}, response);
        }

        // 2. GET ALL: api/followups
        [HttpGet]
        public ActionResult<List<FollowUp>> GetAll()
        {
             var response = _followUpService.GetAll();
            return Ok(response);
        }

        // 3. GET ONE: api/followups/{id}
        [HttpGet("{id}")]
        public ActionResult<FollowUp> GetFollowUp(int id)
        {
            var followUp = _followUps.FirstOrDefault(f => f.Id == id);
            return followUp == null ? NotFound() : Ok(followUp);
        }

        // 4. DELETE: api/followups/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteFollowUp(int id)
        {
            var followUp = _followUps.FirstOrDefault(f => f.Id == id);
            if(followUp == null)
            {
                return NotFound();
            }

            _followUps.Remove(followUp);
            return NoContent();
        }

        // 5. Patch: api/followups/{id}/complete
        // [HttpPatch("{id}/complete")]
        // public



        
    }
}