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

### 3.1 Application

Represents a job opportunity.

#### Properties
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

#### Status Values
- Saved
- Applied
- FollowUpNeeded
- Interviewing
- Rejected
- Offer
- Closed

#### Business Rules
- CompanyName is required
- RoleTitle is required
- Status is required
- If Status is Applied, DateApplied should be provided
- If FollowUpDate is before today and Status is Applied, the application appears in the overdue list
- Rejected/Closed applications are excluded from active tracking

---

### 3.2 Contact

Represents a person (recruiter, dev, hiring manager, etc.)

#### Properties
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

#### Platform Values
- LinkedIn
- Email
- WhatsApp
- Referral
- Other

#### Relationship Status Values
- NotContacted
- Contacted
- Responded
- WarmConnection
- NoResponse
- DoNotContact

#### Business Rules
- Name is required
- Platform is required
- If NextFollowUpDate is today or earlier, the contact appears in the Today View
- Contacts with DoNotContact status are excluded from follow-ups

---

### 3.3 FollowUp

Represents a task/action.

#### Properties
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

#### FollowUpType Values
- ApplicationFollowUp
- ContactFollowUp
- InterviewFollowUp
- Reconnect
- GeneralTask

#### Outcome Values
- None
- NoResponse
- Responded
- Positive
- Negative
- Rescheduled

#### Business Rules
- Title is required
- DueDate is required
- Can link to Application, Contact, both, or none
- If Completed is true, CompletedAt must be set
- Completed follow-ups do not appear in Today View
- A follow-up is Overdue when the DueDate is before today and it is not completed
- A follow-up is DueToday when the DueDate is today and it is not completed

---

## 4. Core Actions

---

### Application Actions
- Create Application
- View Applications
- Update Status
- Delete Application

---

### Contact Actions
- Create Contact
- View Contacts
- Update Relationship Status
- Delete Contact

---

### FollowUp Actions
- Create FollowUp
- View FollowUps
- Mark FollowUp Complete
- Delete FollowUp

---

## 5. Dashboard (Today View)

### Endpoint
GET /api/dashboard/today

### Returns
- DueTodayFollowUps
- OverdueFollowUps
- ApplicationsNeedingFollowUp
- ContactsNeedingFollowUp
- WeeklyActivitySummary

---

### Logic

#### Due Today FollowUps
- DueDate is today
- Completed is false

#### Overdue FollowUps
- DueDate is before today
- Completed is false

#### Applications Needing FollowUp
- Status is either Applied or FollowUpNeeded
- FollowUpDate is today or earlier
- Exclude Rejected, Offer, Closed

#### Contacts Needing FollowUp
- NextFollowUpDate is today or earlier
- RelationshipStatus is not DoNotContact

#### Weekly Summary
- ApplicationsCreatedThisWeek
- ContactsAddedThisWeek
- FollowUpsCompletedThisWeek
- PendingFollowUps
- OverdueFollowUps

---

## 6. API Routes

### Applications
- POST   /api/applications
- GET    /api/applications
- GET    /api/applications/{id}
- PUT    /api/applications/{id}
- DELETE /api/applications/{id}

---

### Contacts
- POST   /api/contacts
- GET    /api/contacts
- GET    /api/contacts/{id}
- PUT    /api/contacts/{id}
- DELETE /api/contacts/{id}

---

### FollowUps
- POST   /api/followups
- GET    /api/followups
- GET    /api/followups/{id}
- PUT    /api/followups/{id}
- DELETE /api/followups/{id}
- PATCH  /api/followups/{id}/complete

---

### Dashboard
- GET /api/dashboard/today

---

## 7. V1 Tasks

### Setup
- Create ASP.NET Core Web API
- Setup project structure
- Enable Swagger

---

### Models
- Application model
- Contact model
- FollowUp model
- Enums

---

### Application Feature
- Create DTOs
- CRUD endpoints
- Test in Swagger

---

### Contact Feature
- Create DTOs
- CRUD endpoints
- Test in Swagger

---

### FollowUp Feature
- Create DTOs
- CRUD endpoints
- Mark complete
- Test in Swagger

---

### Dashboard Feature
- Build Today View logic
- Combine results
- Test with sample data

---

## 8. Out of Scope (V1)

Do NOT include:

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

## 9. Definition of Done

V1 is complete when:

- Applications can be created and tracked
- Contacts can be managed
- Follow-ups can be created and completed
- Dashboard shows what needs attention today
- Data persists in database
- Real job search data is used

---

## 10. Product North Star

> A daily visibility system that ensures job seekers never miss follow-ups and stay consistent.