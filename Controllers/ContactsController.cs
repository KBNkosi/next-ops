using Microsoft.AspNetCore.Mvc;
using JobCommandCenter.Models;
using JobCommandCenter.Enums;

namespace JobCommandCenter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        // Temporary in-memory storage
        private static List<Contact> _contacts = new List<Contact>
        {
            new Contact
            {
                Id = 1,
                Name = "John",
                Platform = "Linkedin"
            },
        };

        // 1. CREATE: api/contact
        [HttpPost]
        public ActionResult<Contact> Create(Contact newContact)
        {
            newContact.Id = _contacts.Any() ? _contacts.Max(c => c.Id) + 1 : 1;

            _contacts.Add(newContact);

            return CreatedAtAction(nameof(GetContact), new { id = newContact.Id }, newContact);
        }

        // 2.GET ALL: api/contacts
        [HttpGet]
        public ActionResult<List<Contact>> GetAll()
        {
            return Ok(_contacts);
        }

        // 3. GET ONE: api/contacts
        [HttpGet("{id}")]
        public ActionResult<Contact> GetContact(int id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            return contact == null ?  NotFound() :  Ok(contact);

        }


        // 4. UPDATE: api/contact
        [HttpPatch("{id}")]
        public ActionResult UpdateRelationshipStatus(int id, [FromBody] string newStatus)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null) return NotFound();

            if (!Enum.TryParse<RelationshipStatus>(newStatus, true, out var parsedStatus))
            {
                return BadRequest("Invalid status value");
            }

            contact.RelationshipStatus = parsedStatus;
            return NoContent();
        }

        // 5. DELETE: api/contact
        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null) return NotFound();

            _contacts.Remove(contact);
            return NoContent();
        }

    }
}