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
Help the user answer:

> “What needs my attention today?”

---

# 2. Target User

Junior / graduate tech job seeker who:
- applies to jobs regularly
- reaches out to recruiters, developers, or hiring managers
- struggles with consistency and follow-ups
- currently tracks things using spreadsheets, notes, memory, or scattered tools

---

# 3. V1 Scope

## Included
- Track job applications
- Track professional contacts
- Track follow-up tasks
- Show a daily dashboard / Today View
- Enforce workflow rules
- Use DTOs for API contracts
- Use services for business logic
- Use in-memory storage initially
- Move to database persistence later

## Out of Scope
- Authentication
- AI features
- Email sending
- Notifications
- LinkedIn integration
- Payments
- Team features
- Frontend frameworks
- Advanced analytics

---

# 4. Core Entities

---

# 4.1 Application

Represents a job opportunity.

## Properties
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

## Status Values
- Saved
- Applied
- FollowUpNeeded
- Interviewing
- Rejected
- Offer
- Closed

## Business Rules
- CompanyName is required.
- RoleTitle is required.
- Source is required.
- New applications default to `Saved` if no status is provided.
- `FollowUpNeeded` should not be used as an initial create status.
- `FollowUpNeeded` represents an active application whose follow-up date has arrived or passed.
- `DateApplied` represents the date the application was submitted.
- If status is `Applied`, `Interviewing`, `Rejected`, `Offer`, or `Closed`, `DateApplied` should exist.
- If status is `Saved`, `DateApplied` should be null.
- If an application moves to `Applied` and `DateApplied` is null, the system may set `DateApplied` to the current date.
- `Rejected`, `Offer`, and `Closed` applications are excluded from active follow-up tracking.
- `UpdatedAt` changes whenever the application is modified.

---

# 4.2 Contact

Represents a professional contact.

## Properties
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

## Platform Values
- LinkedIn
- Email
- WhatsApp
- Referral
- Other

## Relationship Status Values
- NotContacted
- Contacted
- Responded
- WarmConnection
- NoResponse
- DoNotContact

## Business Rules
- Name is required.
- Platform is required.
- New contacts default to `NotContacted`.
- Contacts with `DoNotContact` status are excluded from follow-ups.
- If `NextFollowUpDate` is today or earlier, the contact appears in Today View.
- `LastContactedDate` should be null until contact is actually made.
- `UpdatedAt` changes whenever the contact is modified.

---

# 4.3 FollowUp

Represents a task or action.

## Properties
- Id
- Title
- DueDate
- Completed
- CompletedAt
- FollowUpType
- ApplicationId
- ContactId
- Notes
- Outcome
- CreatedAt
- UpdatedAt

## FollowUpType Values
- ApplicationFollowUp
- ContactFollowUp
- InterviewFollowUp
- Reconnect
- GeneralTask

## Outcome Values
- None
- NoResponse
- Responded
- Positive
- Negative
- Rescheduled

## Business Rules
- Title is required.
- DueDate is required.
- A follow-up can link to an Application, Contact, both, or neither.
- New follow-ups default to `Completed = false`.
- If `Completed = true`, `CompletedAt` must be set.
- If `Completed = false`, `CompletedAt` should be null.
- Completed follow-ups do not appear in Today View.
- A follow-up is overdue when `DueDate` is before today and `Completed = false`.
- A follow-up is due today when `DueDate` is today and `Completed = false`.
- `UpdatedAt` changes whenever the follow-up is modified.

---

# 5. API Design

The API separates:
- descriptive data updates
- workflow/state changes

## API Rules
- `POST` creates resources.
- `GET` reads resources.
- `PUT` updates editable descriptive fields.
- `PATCH` performs workflow/state changes.
- `DELETE` removes resources.

---

# 5.1 Applications API

## Routes
- `POST /api/applications`
- `GET /api/applications`
- `GET /api/applications/{id}`
- `PUT /api/applications/{id}`
- `PATCH /api/applications/{id}/status`
- `DELETE /api/applications/{id}`

## CreateApplicationRequest
Fields allowed on create:
- CompanyName
- RoleTitle
- Source
- JobLink
- Status
- DateApplied
- FollowUpDate
- Notes

## UpdateApplicationRequest
Editable descriptive fields:
- CompanyName
- RoleTitle
- Source
- JobLink
- FollowUpDate
- Notes

### Excluded from General Update
- Id
- Status
- DateApplied
- CreatedAt
- UpdatedAt

## UpdateApplicationStatusRequest
Workflow fields:
- Status
- DateApplied (only if needed for status transition)

## ApplicationResponse
Fields returned:
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

---

# 5.2 Contacts API

## Routes
- `POST /api/contacts`
- `GET /api/contacts`
- `GET /api/contacts/{id}`
- `PUT /api/contacts/{id}`
- `PATCH /api/contacts/{id}/relationship-status`
- `DELETE /api/contacts/{id}`

## CreateContactRequest
Fields allowed on create:
- Name
- Company
- Role
- Platform
- ProfileLink
- NextFollowUpDate
- Notes

## UpdateContactRequest
Editable descriptive fields:
- Name
- Company
- Role
- Platform
- ProfileLink
- NextFollowUpDate
- Notes

### Excluded from General Update
- Id
- RelationshipStatus
- LastContactedDate
- CreatedAt
- UpdatedAt

## UpdateRelationshipStatusRequest
Workflow fields:
- RelationshipStatus

## ContactResponse
Fields returned:
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

---

# 5.3 FollowUps API

## Routes
- `POST /api/followups`
- `GET /api/followups`
- `GET /api/followups/{id}`
- `PUT /api/followups/{id}`
- `PATCH /api/followups/{id}/complete`
- `DELETE /api/followups/{id}`

## CreateFollowUpRequest
Fields allowed on create:
- Title
- DueDate
- FollowUpType
- ApplicationId
- ContactId
- Notes

## UpdateFollowUpRequest
Editable descriptive fields:
- Title
- DueDate
- FollowUpType
- ApplicationId
- ContactId
- Notes

### Excluded from General Update
- Id
- Completed
- CompletedAt
- Outcome
- CreatedAt
- UpdatedAt

## CompleteFollowUpRequest
Workflow fields:
- Outcome
- Notes

## FollowUpResponse
Fields returned:
- Id
- Title
- DueDate
- Completed
- CompletedAt
- FollowUpType
- ApplicationId
- ContactId
- Notes
- Outcome
- CreatedAt
- UpdatedAt

---

# 5.4 Dashboard API

## Route
- `GET /api/dashboard/today`

## Returns
- DueTodayFollowUps
- OverdueFollowUps
- ApplicationsNeedingFollowUp
- ContactsNeedingFollowUp
- WeeklyActivitySummary

## Dashboard Logic

### DueTodayFollowUps
- DueDate is today
- Completed is false

### OverdueFollowUps
- DueDate is before today
- Completed is false

### ApplicationsNeedingFollowUp
- Status is `Applied` or `FollowUpNeeded`
- FollowUpDate is today or earlier
- Exclude `Rejected`, `Offer`, and `Closed`

### ContactsNeedingFollowUp
- NextFollowUpDate is today or earlier
- RelationshipStatus is not `DoNotContact`

### WeeklyActivitySummary
- ApplicationsCreatedThisWeek
- ContactsAddedThisWeek
- FollowUpsCompletedThisWeek
- PendingFollowUps
- OverdueFollowUps

---

# 6. Architecture

## Current Architecture

```text
HTTP Request
→ Controller
→ Service
→ In-memory storage
→ Service
→ Controller
→ HTTP Response
```

---

## Layer Responsibilities

### Controllers
Controllers handle:
- HTTP requests
- model binding
- calling services
- returning HTTP responses

Controllers should not contain complex business rules.

---

### Services
Services contain:
- business rules
- workflow logic
- status transitions
- timestamp management
- dashboard calculations
- DTO mapping

---

### Models
Models represent internal system entities.

---

### DTOs
DTOs define API input/output contracts.

#### Create DTOs
Define allowed create fields.

#### Update DTOs
Define editable descriptive fields.

#### Action DTOs
Define workflow/state changes.

#### Response DTOs
Define returned response shape.

---

### In-Memory Storage
Temporary storage used during early development.

This will later be replaced with database persistence.

---

# 7. Services

---

## ApplicationService

Responsible for:
- creating applications
- getting all applications
- getting application by id
- updating descriptive fields
- updating application status
- enforcing status rules
- enforcing DateApplied rules
- deleting applications
- mapping Application → ApplicationResponse

---

## ContactService

Responsible for:
- creating contacts
- getting all contacts
- getting contact by id
- updating descriptive fields
- updating relationship status
- enforcing contact follow-up rules
- deleting contacts
- mapping Contact → ContactResponse

---

## FollowUpService

Responsible for:
- creating follow-ups
- getting all follow-ups
- getting follow-up by id
- updating descriptive fields
- marking follow-ups complete
- setting CompletedAt
- enforcing completion rules
- deleting follow-ups
- mapping FollowUp → FollowUpResponse

---

## DashboardService

Responsible for:
- finding follow-ups due today
- finding overdue follow-ups
- finding applications needing follow-up
- finding contacts needing follow-up
- calculating weekly activity summary

---

# 8. Suggested Folder Structure

```text
JobCommandCenter/
│
├── Controllers/
│   ├── ApplicationsController.cs
│   ├── ContactsController.cs
│   ├── FollowUpsController.cs
│   └── DashboardController.cs
│
├── Models/
│   ├── Application.cs
│   ├── Contact.cs
│   └── FollowUp.cs
│
├── DTOs/
│   ├── Applications/
│   ├── Contacts/
│   ├── FollowUps/
│   └── Dashboard/
│
├── Enums/
│   ├── ApplicationStatus.cs
│   ├── RelationshipStatus.cs
│   ├── FollowUpType.cs
│   └── FollowUpOutcome.cs
│
├── Services/
│   ├── ApplicationService.cs
│   ├── ContactService.cs
│   ├── FollowUpService.cs
│   └── DashboardService.cs
│
└── Program.cs
```

---

# 9. V1 Build Order

## Phase 1 — Models and Enums
- Application model
- Contact model
- FollowUp model
- Status/type/outcome enums

---

## Phase 2 — DTOs
- Create DTOs
- Update DTOs
- Workflow/action DTOs
- Response DTOs

---

## Phase 3 — Services
- ApplicationService
- ContactService
- FollowUpService
- DashboardService

---

## Phase 4 — Controllers
- Keep controllers thin
- Route requests to services
- Return HTTP responses

---

## Phase 5 — Dashboard
- Build Today View logic
- Combine application, contact, and follow-up data

---

## Phase 6 — Persistence
- Replace in-memory lists with database storage
- Introduce EF Core later
- Keep business logic in services

---

# 10. Definition of Done

V1 is complete when:
- Applications can be created, viewed, updated, status-changed, and deleted.
- Contacts can be created, viewed, updated, relationship-status-changed, and deleted.
- Follow-ups can be created, viewed, updated, completed, and deleted.
- Dashboard shows what needs attention today.
- DTOs protect workflow/system-managed fields.
- Services enforce business rules.
- In-memory version works correctly before persistence is added.
- Real job search data can be tracked safely.

---

# 11. Product North Star

> A daily visibility system that ensures job seekers never miss follow-ups and stay consistent.