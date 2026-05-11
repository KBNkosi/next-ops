# Job Search Command Center — V1 Product Design

## 1. Product Purpose

### Problem
Job seekers struggle to stay consistent with:
- job applications
- networking
- follow-ups

Because:
- tracking is scattered
- visibility is poor
- follow-ups are missed

### V1 Goal
> Help the user answer: “What needs my attention today?”

---

## 2. Target User

### Primary User
Junior / graduate tech job seeker who:
- applies to jobs regularly
- reaches out to recruiters / developers
- struggles with consistency and follow-ups
- uses spreadsheets, notes, or memory

---

## 3. Core Entities

---

## 3.1 Application

Represents a job opportunity.

### Properties
- Id
- CompanyName
- RoleTitle
- Source
- JobLink
- Status
- DateApplied
- FollowUpDate
- Notes
- CreatedAt
- UpdatedAt

### Status Values
- Saved
- Applied
- FollowUpNeeded
- Interviewing
- Rejected
- Offer
- Closed

### Status Meaning

#### Saved
The user has saved the opportunity but has not applied yet.

#### Applied
The user has submitted the application and is waiting for a response or next step.

#### FollowUpNeeded
The application has already been submitted, and the follow-up date has arrived or passed.

This status means the application requires user attention.

#### Interviewing
The user is actively in the interview process.

#### Rejected
The application was rejected.

#### Offer
The user received an offer.

#### Closed
The application is no longer being tracked actively.

### Business Rules
- CompanyName is required.
- RoleTitle is required.
- Status is required.
- New applications default to `Saved` unless explicitly created as already applied.
- If Status is `Applied`, `DateApplied` must exist.
- If Status is not `Applied`, `DateApplied` should be null unless the status represents a post-application state such as `FollowUpNeeded`, `Interviewing`, `Rejected`, or `Offer`.
- `DateApplied` should be set when the application first moves to `Applied`.
- `DateApplied` should not be edited through the general update endpoint.
- If an application moves from `Saved` to `Applied`, the system sets `DateApplied`.
- If an application moves from `Applied` to `FollowUpNeeded`, `DateApplied` remains unchanged.
- If `FollowUpDate` is today or earlier and Status is `Applied`, the application should be treated as needing follow-up.
- In V1, `FollowUpNeeded` can be set manually through the status update endpoint.
- Later, the dashboard or scheduled logic may automatically classify `Applied` applications with overdue follow-up dates as needing follow-up.
- Rejected, Offer, and Closed applications are excluded from active follow-up tracking.
- `UpdatedAt` changes whenever the application is edited or its workflow status changes.

### General Update Rules
The general application update endpoint is for descriptive fields only.

Editable through general update:
- CompanyName
- RoleTitle
- Source
- JobLink
- FollowUpDate
- Notes

Not editable through general update:
- Id
- Status
- DateApplied
- CreatedAt
- UpdatedAt

Workflow fields must be changed through action-specific endpoints.

---

## 3.2 Contact

Represents a person such as a recruiter, developer, hiring manager, mentor, or referral contact.

### Properties
- Id
- Name
- Company
- Role
- Platform
- ProfileLink
- RelationshipStatus
- LastContactedDate
- NextFollowUpDate
- Notes
- CreatedAt
- UpdatedAt

### Platform Values
- LinkedIn
- Email
- WhatsApp
- Referral
- Other

### Relationship Status Values
- NotContacted
- Contacted
- Responded
- WarmConnection
- NoResponse
- DoNotContact

### Business Rules
- Name is required.
- Platform is required.
- `LastContactedDate` can be null if the user has not contacted the person yet.
- `NextFollowUpDate` can be null if no follow-up is planned.
- If `NextFollowUpDate` is today or earlier, the contact appears in the Today View.
- Contacts with `DoNotContact` status are excluded from follow-ups.
- `UpdatedAt` changes whenever the contact is edited or relationship status changes.

### General Update Rules
The general contact update endpoint is for descriptive fields only.

Editable through general update:
- Name
- Company
- Role
- Platform
- ProfileLink
- LastContactedDate
- NextFollowUpDate
- Notes

Not editable through general update:
- Id
- RelationshipStatus
- CreatedAt
- UpdatedAt

Workflow fields must be changed through action-specific endpoints.

---

## 3.3 FollowUp

Represents a task or action the user needs to complete.

### Properties
- Id
- Title
- DueDate
- Completed
- CompletedAt
- FollowUpType
- ApplicationId (nullable)
- ContactId (nullable)
- Notes
- Outcome
- CreatedAt
- UpdatedAt

### FollowUpType Values
- ApplicationFollowUp
- ContactFollowUp
- InterviewFollowUp
- Reconnect
- GeneralTask

### Outcome Values
- None
- NoResponse
- Responded
- Positive
- Negative
- Rescheduled

### Business Rules
- Title is required.
- DueDate is required.
- A follow-up can link to an Application, Contact, both, or none.
- If `Completed` is true, `CompletedAt` must be set.
- If `Completed` is false, `CompletedAt` must be null.
- Completed follow-ups do not appear in Today View.
- A follow-up is overdue when `DueDate` is before today and it is not completed.
- A follow-up is due today when `DueDate` is today and it is not completed.
- Marking a follow-up complete must happen through the complete endpoint.
- `UpdatedAt` changes whenever the follow-up is edited or completed.

### General Update Rules
The general follow-up update endpoint is for descriptive task fields only.

Editable through general update:
- Title
- DueDate
- FollowUpType
- ApplicationId
- ContactId
- Notes
- Outcome

Not editable through general update:
- Id
- Completed
- CompletedAt
- CreatedAt
- UpdatedAt

Workflow fields must be changed through action-specific endpoints.

---

## 4. DTO Strategy

The API should not accept the full model directly for create or update operations.

DTOs define what the client is allowed to send for a specific action.

### DTO Categories

#### Create DTOs
Used when creating new records.

Purpose:
- Accept only user-provided fields.
- Exclude system-managed fields.

System-managed fields include:
- Id
- CreatedAt
- UpdatedAt
- CompletedAt

#### General Update DTOs
Used when editing normal descriptive information.

Purpose:
- Allow safe editing.
- Exclude workflow state fields.
- Prevent clients from bypassing business rules.

#### Workflow/Action DTOs
Used for meaningful state changes.

Examples:
- Update application status
- Update contact relationship status
- Mark follow-up complete

Purpose:
- Keep business rules attached to workflow actions.
- Prevent inconsistent state.

---

## 5. Core Actions

---

## 5.1 Application Actions

- Create Application
- View Applications
- View Single Application
- Update Application Details
- Update Application Status
- Delete Application

### Application Workflow Actions

#### Update Status
Changes the application workflow state.

Examples:
- Saved → Applied
- Applied → FollowUpNeeded
- Applied → Interviewing
- Applied → Rejected
- Interviewing → Offer
- Interviewing → Rejected
- Any active status → Closed

#### FollowUpNeeded Rule
`FollowUpNeeded` should be used when:
- the application has already been submitted, and
- the user has reached or passed the planned follow-up date, and
- the application is still active.

In V1, this can be done manually by updating the status.

The dashboard should also be able to identify applications needing follow-up even if the stored status is still `Applied`.

---

## 5.2 Contact Actions

- Create Contact
- View Contacts
- View Single Contact
- Update Contact Details
- Update Relationship Status
- Delete Contact

### Contact Workflow Actions

#### Update Relationship Status
Changes the relationship stage with the contact.

Examples:
- NotContacted → Contacted
- Contacted → Responded
- Responded → WarmConnection
- Contacted → NoResponse
- Any status → DoNotContact

---

## 5.3 FollowUp Actions

- Create FollowUp
- View FollowUps
- View Single FollowUp
- Update FollowUp Details
- Mark FollowUp Complete
- Delete FollowUp

### FollowUp Workflow Actions

#### Mark Complete
Marks a follow-up as completed.

Rules:
- Set `Completed` to true.
- Set `CompletedAt` to the current date/time.
- Update `UpdatedAt`.

---

## 6. Dashboard: Today View

### Endpoint
GET /api/dashboard/today

### Returns
- DueTodayFollowUps
- OverdueFollowUps
- ApplicationsNeedingFollowUp
- ContactsNeedingFollowUp
- WeeklyActivitySummary

---

## 6.1 Dashboard Logic

### Due Today FollowUps
- DueDate is today.
- Completed is false.

### Overdue FollowUps
- DueDate is before today.
- Completed is false.

### Applications Needing FollowUp
An application appears here when:
- Status is `Applied` or `FollowUpNeeded`, and
- FollowUpDate is today or earlier, and
- the application is not Rejected, Offer, or Closed.

Important:
- `FollowUpNeeded` is a workflow status.
- The dashboard should still catch overdue applied applications even if their status has not yet been changed to `FollowUpNeeded`.

### Contacts Needing FollowUp
A contact appears here when:
- NextFollowUpDate is today or earlier, and
- RelationshipStatus is not `DoNotContact`.

### Weekly Summary
- ApplicationsCreatedThisWeek
- ContactsAddedThisWeek
- FollowUpsCompletedThisWeek
- PendingFollowUps
- OverdueFollowUps

---

## 7. API Routes

---

## 7.1 Applications

```http
POST   /api/applications
GET    /api/applications
GET    /api/applications/{id}
PUT    /api/applications/{id}
PATCH  /api/applications/{id}/status
DELETE /api/applications/{id}
```

### Route Meaning

#### POST /api/applications
Creates a new application.

#### GET /api/applications
Returns all applications.

#### GET /api/applications/{id}
Returns one application.

#### PUT /api/applications/{id}
Updates descriptive application details only.

Does not update:
- Status
- DateApplied
- CreatedAt
- UpdatedAt

#### PATCH /api/applications/{id}/status
Updates application workflow status.

This is where rules such as setting `DateApplied` when status becomes `Applied` should live.

#### DELETE /api/applications/{id}
Deletes the application.

---

## 7.2 Contacts

```http
POST   /api/contacts
GET    /api/contacts
GET    /api/contacts/{id}
PUT    /api/contacts/{id}
PATCH  /api/contacts/{id}/relationship-status
DELETE /api/contacts/{id}
```

### Route Meaning

#### PUT /api/contacts/{id}
Updates descriptive contact details only.

Does not update:
- RelationshipStatus
- CreatedAt
- UpdatedAt

#### PATCH /api/contacts/{id}/relationship-status
Updates relationship workflow state.

---

## 7.3 FollowUps

```http
POST   /api/followups
GET    /api/followups
GET    /api/followups/{id}
PUT    /api/followups/{id}
PATCH  /api/followups/{id}/complete
DELETE /api/followups/{id}
```

### Route Meaning

#### PUT /api/followups/{id}
Updates descriptive follow-up details only.

Does not update:
- Completed
- CompletedAt
- CreatedAt
- UpdatedAt

#### PATCH /api/followups/{id}/complete
Marks the follow-up as complete.

---

## 7.4 Dashboard

```http
GET /api/dashboard/today
```

Returns the daily attention view.

---

## 8. V1 Tasks

---

## 8.1 Setup
- Create ASP.NET Core Web API.
- Setup project structure.
- Enable Swagger.

---

## 8.2 Models
- Application model.
- Contact model.
- FollowUp model.
- Enums.

---

## 8.3 DTOs
- Create Application DTOs.
- Create Contact DTOs.
- Create FollowUp DTOs.
- Create update DTOs for descriptive edits.
- Create workflow/action DTOs where needed.

---

## 8.4 Application Feature
- Create Application.
- Get all applications.
- Get application by id.
- Update application details.
- Update application status.
- Enforce `DateApplied` rules.
- Delete application.
- Test in Swagger.

---

## 8.5 Contact Feature
- Create Contact.
- Get all contacts.
- Get contact by id.
- Update contact details.
- Update relationship status.
- Delete contact.
- Test in Swagger.

---

## 8.6 FollowUp Feature
- Create FollowUp.
- Get all follow-ups.
- Get follow-up by id.
- Update follow-up details.
- Mark follow-up complete.
- Enforce `CompletedAt` rules.
- Delete follow-up.
- Test in Swagger.

---

## 8.7 Dashboard Feature
- Build Today View logic.
- Show due today follow-ups.
- Show overdue follow-ups.
- Show applications needing follow-up.
- Show contacts needing follow-up.
- Show weekly activity summary.
- Test with sample data.

---

## 9. Out of Scope for V1

Do not include:
- Authentication
- AI features
- Email sending
- Notifications
- LinkedIn integration
- Analytics dashboards
- Payments
- Team features
- Frontend frameworks

---

## 10. Definition of Done

V1 is complete when:
- Applications can be created and tracked.
- Application status changes are controlled through workflow endpoints.
- `DateApplied` is correctly linked to application status.
- Contacts can be created and managed.
- Contact relationship status changes are controlled through workflow endpoints.
- Follow-ups can be created, updated, completed, and deleted.
- `CompletedAt` is correctly linked to follow-up completion.
- Dashboard shows what needs attention today.
- Data persists in a database.
- Real job search data is used.

---

## 11. Product North Star

> A daily visibility system that ensures job seekers never miss follow-ups and stay consistent.
