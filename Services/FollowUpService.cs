using JobCommandCenter.Models;

namespace JobCommandCenter.Services
{
    public class FollowUpService
    {
        private static List<FollowUp> _followUps = new();

        private FollowUpResponse MapToResponse(FollowUp followUp)
        {
            return new FollowUpResponse
            {
                Id = followUp.Id,
                Title = followUp.Title,
                DueDate = followUp.DueDate,
                Completed = followUp.Completed,
                CompletedAt = followUp.CompletedAt,
                FollowUpType = followUp.FollowUpType,
                Application = followUp.Application,
                ApplicationId = followUp.ApplicationId,
                Contact = followUp.Contact,
                ContactId = followUp.ContactId,
                Notes = followUp.Notes,
                Outcome = followUp.Outcome,
                CreatedAt = followUp.CreatedAt,
                UpdatedAt = followUp.UpdatedAt
            };
        }

        //Helper function to get a follow up by id
        private FollowUp GetById(int id)
        {
            var followUp = _followUps.FirstOrDefault(f => f.Id == id);
            if (followUp == null)
               throw new KeyNotFoundException($"follow up {id} not found");
            
            return followUp;
        }

        // Create a follow up
        public FollowUp Create(FollowUp followUp)
        {
            int followUpId = _followUps.Any() ? _followUps.Max(f => f.Id) + 1 : 1;
            followUp.Id = followUpId;
            _followUps.Add(followUp);
            return followUp;
        }

        // Get all follow ups
        public List<FollowUpResponse> GetAll()
        {
            return _followUps.Select(MapToResponse).ToList();
        }

        // Get a follow up
        public FollowUpResponse GetFollowUp(int id)
        {
            
        }



        
    }
}