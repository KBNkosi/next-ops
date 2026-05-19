using JobCommandCenter.Models;
using JobCommandCenter.DTOs.FollowUps;

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
                ApplicationId = followUp.ApplicationId,
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

        // Helper function to check if ID is valid
         private void ValidateId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
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
            ValidateId(id);
            var followUp = GetById(id);
            return MapToResponse(followUp);
        }

       // General update
       public FollowUpResponse UpdateFollowUp(int id, UpdateFollowUpRequest request)
       {
          ValidateId(id);
          var followUp = GetById(id);

          if(!string.IsNullOrEmpty(request.Title))
            followUp.Title = request.Title;

          if(request.DueDate != null)
            followUp.DueDate = request.DueDate.Value;

          if(request.FollowUpType != null)
            followUp.FollowUpType = request.FollowUpType.Value;

          if(request.ApplicationId != null)
            ValidateId(request.ApplicationId.Value);
            followUp.ApplicationId = request.ApplicationId;

          if(request.ContactId != null)
            ValidateId(request.ContactId.Value);
            followUp.ContactId = request.ContactId;

          if(!string.IsNullOrEmpty(request.Notes))
            followUp.Notes = request.Notes;

          followUp.UpdatedAt = DateTime.UtcNow;

          return MapToResponse(followUp);         
                
       }

       // Update follow up complete status
       public FollowUpResponse UpdateCompleteStatus(int id, bool status)
       {
          ValidateId(id);
          var followUp = GetById(id);

          followUp.Completed = status;
          followUp.CompletedAt = DateTime.UtcNow;

          return MapToResponse(followUp);

       }

        // Delete follow up
       public void DeleteFollowUp(int id)
       {
          ValidateId(id);
          var followUp = GetById(id);

          _followUps.Remove(followUp);
       }

        
    }
}