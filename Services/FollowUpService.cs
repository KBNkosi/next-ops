using JobCommandCenter.Models;

namespace JobCommandCenter.Services
{
    public class FollowUpService
    {
        private static List<FollowUp> _followUps = new();

        public FollowUp Create(FollowUp followUp)
        {
            int followUpId = _followUps.Any() ? _followUps.Max(f => f.Id) + 1 : 1;
            followUp.Id = followUpId;
            _followUps.Add(followUp);
            return followUp;
        }

        
    }
}