using Microsoft.AspNetCore.Mvc;
using JobCommandCenter.Models;
using JobCommandCenter.Enums;
using JobCommandCenter.Services;
using JobCommandCenter.DTOs.FollowUps;

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
        public ActionResult<List<FollowUpResponse>> GetAll()
        {
             var response = _followUpService.GetAll();
            return Ok(response);
        }

        // 3. GET ONE: api/followups/{id}
        [HttpGet("{id}")]
        public ActionResult<FollowUpResponse> GetFollowUp(int id)
        {
            try
            {
               var response = _followUpService.GetFollowUp(id);
               return Ok(response);
            }
             
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid follow up ID");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
           
        }

        // 4. UPDATE: api/followups/{id}
        [HttpPut("{id}")]
        public ActionResult<FollowUpResponse> UpdateFollowUp(int id, UpdateFollowUpRequest request)
        {
             try
            {
               var response = _followUpService.UpdateFollowUp(id, request);
               return Ok(response);
            }
             
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid follow up ID");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // 5. UPDATE: api/followups/{id}/complete
        [HttpPatch("{id}/complete")]
        public ActionResult<FollowUpResponse> UpdateCompleteStatus(int id, [FromBody] bool request)
        {
            try
            {
                var response = _followUpService.UpdateCompleteStatus(id, request);
                return Ok(response);
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid follow up ID");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }

        // 6. DELETE: api/followups/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteFollowUp(int id)
        {
           try
           {
             _followUpService.DeleteFollowUp(id);
             return NoContent();
           }
           catch (ArgumentOutOfRangeException)
            {
                return BadRequest("Invalid follow up ID");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

        }

        



        
    }
}