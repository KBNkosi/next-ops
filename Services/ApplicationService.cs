using JobCommandCenter.Models;
using JobCommandCenter.DTOs.Applications;
using JobCommandCenter.Enums;

namespace JobCommandCenter.Services
{
    public class ApplicationService
    {
        private static List<Application> _applications = new();
        private readonly FollowUpService _followUpService;

        public ApplicationService(FollowUpService followUpService)
        {
            _followUpService = followUpService;
        }

        // Helper function to map application object to ApplicationResponse Model
        private ApplicationResponse MapToResponse(Application application)
        {
            return new ApplicationResponse
            {
                Id = application.Id,
                CompanyName = application.CompanyName,
                RoleTitle = application.RoleTitle,
                Source = application.Source,
                Status = application.Status,
                DateApplied = application.DateApplied,
                Notes = application.Notes,
                CreatedAt = application.CreatedAt,
                UpdatedAt = application.UpdatedAt
            };
        }

        // Helper function to get application by id
        private Application GetApplication(int id)
        {
            var app = _applications.FirstOrDefault(a => a.Id == id);

            if (app == null)
                throw new KeyNotFoundException($"Application {id} not found");

            return app;
        }
        // Create application method 
        public ApplicationResponse Create(CreateApplicationRequest request)
        {
            int applicationId = _applications.Any() ? _applications.Max(a => a.Id) + 1 : 1;

            var application = new Application
            {
                Id = applicationId,
                CompanyName = request.CompanyName,
                RoleTitle = request.RoleTitle,
                Source = request.Source,
                Status = request.Status,
                DateApplied = request.Status == ApplicationStatus.Saved ? null : DateTime.UtcNow,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _applications.Add(application);

            // Create follow-up if application has been applied
            if (application.DateApplied != null)
            {
                CreateApplicationFollowUp(application);
            }

            return MapToResponse(application);

        }

        //  Create application follow up
        private void CreateApplicationFollowUp(Application application)
        {
            var followUp = new FollowUp
            {
                Title = $"Follow up on {application.CompanyName} - {application.RoleTitle}",
                DueDate = application.DateApplied.Value.AddDays(7),
                FollowUpType = FollowUpType.ApplicationFollowUp,
                ApplicationId = application.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _followUpService.Create(followUp);
        }

        //  Get all applications
        public List<ApplicationResponse> GetAll()
        {
            return _applications.Select(MapToResponse).ToList();
        }

        // Get a single application
        public ApplicationResponse GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            var app = GetApplication(id);
            return MapToResponse(app);

        }

        // public ApplicationResponse UpdateStatus(int id, string newStatus)
        // {
           
        // }
    }
}