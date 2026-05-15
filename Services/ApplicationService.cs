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
                JobLink = application.JobLink,
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
        // Helper function to check if ID is valid
        private void ValidateId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
        }
        // Create application method 
        public ApplicationResponse Create(CreateApplicationRequest request)
        {
            int applicationId = _applications.Any() ? _applications.Max(a => a.Id) + 1 : 1;

            var app = new Application
            {
                Id = applicationId,
                CompanyName = request.CompanyName,
                RoleTitle = request.RoleTitle,
                Platform = request.Platform,
                JobLink = request.JobLink,
                Status = request.Status,
                DateApplied = request.Status == ApplicationStatus.Saved ? null : DateTime.UtcNow,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _applications.Add(app);

            // Create follow-up if application has been applied
            if (app.DateApplied != null)
            {
                CreateApplicationFollowUp(app.CompanyName, app.RoleTitle, app.Id, app.DateApplied.Value);
            }

            return MapToResponse(app);

        }

        //  Create application follow up
        private void CreateApplicationFollowUp(string companyName, string roleTitle, int applicationId, DateTime dateApplied)
        {
            var followUp = new FollowUp
            {
                Title = $"Follow up on {companyName} - {roleTitle}",
                DueDate = dateApplied.AddDays(7),
                FollowUpType = FollowUpType.ApplicationFollowUp,
                ApplicationId = applicationId,
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
            ValidateId(id);
            var app = GetApplication(id);
            return MapToResponse(app);

        }

        // General Application Update
        public ApplicationResponse UpdateApplication(int id, UpdateApplicationRequest request)
        {
            ValidateId(id);
            var app = GetApplication(id);

            if (!string.IsNullOrEmpty(request.CompanyName))
                app.CompanyName = request.CompanyName;

            if (!string.IsNullOrEmpty(request.RoleTitle))
                app.RoleTitle = request.RoleTitle;

            if (!string.IsNullOrEmpty(request.Platform))
                app.Platform = request.Platform;

            if (!string.IsNullOrEmpty(request.JobLink))
                app.JobLink = request.JobLink;

            if (!string.IsNullOrEmpty(request.Notes))
                app.Notes = request.Notes;

            app.UpdatedAt = DateTime.UtcNow;
            return MapToResponse(app);
        }

        // Update application status
        public ApplicationResponse UpdateStatus(int id, UpdateApplicationStatusRequest newStatus)
        {
            ValidateId(id);
            var app = GetApplication(id);

            app.Status = newStatus.Status;

            if (app.Status == ApplicationStatus.Applied)
            {
                app.DateApplied = DateTime.UtcNow;
                CreateApplicationFollowUp(app.CompanyName, app.RoleTitle, app.Id, app.DateApplied.Value);
            }


            app.UpdatedAt = DateTime.UtcNow;
            return MapToResponse(app);

        }

        // Delete or Remove application
        public void DeleteApplication(int id)
        {
            ValidateId(id);
            var app = GetApplication(id);
            _applications.Remove(app);

        }
    }
}