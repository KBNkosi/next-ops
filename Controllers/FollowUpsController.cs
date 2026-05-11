using Microsoft.AspNetCore.Mvc;
using JobCommandCenter.Models;
using JobCommandCenter.Enums;

namespace JobCommandCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowUpsController : ControllerBase
    {
        // Temporary in-memory storage
       private static List<FollowUp> _followUps = new List<FollowUp>
       {
           new FollowUp
           {
               Id = 1,
               Title = "Technical Recruiter",
               DueDate = new DateTime(2026, 5, 12),
               FollowUpType = FollowUpType.ApplicationFollowUp

           },
       };

       // 1. CREATE: api/followups
       [HttpPost]
       public ActionResult<FollowUp> CreateFollowUp(FollowUp newFollowUp)
        {
            newFollowUp.Id = _followUps.Any() ? _followUps.Max(f => f.Id) + 1 : 1;

            _followUps.Add(newFollowUp);

            return CreatedAtAction(nameof(GetFollowUp), new {id = newFollowUp.Id}, newFollowUp);
        }

        // 2. GET ALL: api/followups
        [HttpGet]
        public ActionResult<List<FollowUp>> GetAll()
        {
            return Ok(_followUps);
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