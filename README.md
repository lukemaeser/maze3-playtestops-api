# MAZE3 PlayTestOps API

MAZE3 PlayTestOps API is a lightweight ASP.NET Core Web API for organizing game builds, playtest sessions, bug reports, and feedback across MAZE3 Studios games, prototypes, and gameplay systems in development.

The API replaces scattered notes, messages, spreadsheets, and informal bug reports with structured, SQL-backed playtest records. Each recorded session can be connected to the exact build that was tested, the issues that were discovered, and the feedback that should inform the next iteration.

## Current Status

**Local SQL-backed API operational**

The current version includes:

- ASP.NET Core Minimal API endpoints
- Entity Framework Core persistence
- SQL Server LocalDB for local development
- Relationships between builds, sessions, bugs, and feedback
- Realistic MAZE3 Studios seed data
- CRUD operations for all four core record types
- Basic validation and error responses
- Workflow-focused query endpoints
- OpenAPI documentation and Swagger UI
- Saved VS Code REST Client requests
- Structured logging for create, update, and delete operations
- Environment-specific application configuration

Cloud deployment, Azure SQL, and GitHub Actions CI/CD are planned but are not yet implemented.

## Why This Project Exists

Game development produces valuable feedback, but that feedback is only useful when it can be found, compared, and acted on later.

A single game build can generate questions such as:

- Which version and branch were tested?
- Who performed the playtest?
- Which platform was used?
- What bugs were discovered?
- How severe was each issue?
- What design or usability feedback came from the session?
- Which problems remain unresolved?
- What changed between builds?

MAZE3 PlayTestOps API provides a focused backend for recording and retrieving that information without introducing a large production-management platform too early.

## Core Workflow

The API models the playtesting process as four connected record types:

```text
GameBuild
└── PlaytestSession
    ├── BugReport
    └── FeedbackNote
```

- A **GameBuild** represents a specific project version or prototype build.
- A **PlaytestSession** records one testing event performed against a build.
- A **BugReport** records a defect discovered during a session.
- A **FeedbackNote** records design, usability, performance, or player feedback from a session.

This structure makes it possible to trace bugs and feedback back to the exact build and testing session where they were observed.

## Use Cases

- Track project versions, branches, release notes, and build dates
- Record playtest sessions against specific builds
- Store tester, platform, date, and session information
- Capture bugs with severity, status, descriptions, and reproduction steps
- Store design and usability feedback by category and rating
- Retrieve every session associated with a build
- Retrieve every bug associated with a session
- Review unresolved issues
- Filter feedback by category
- Support future data exports, dashboards, Unity tools, and playtest forms

## Features

### Persistent SQL Storage

Entity Framework Core connects the API to SQL Server LocalDB during local development. Records remain available after the application stops and restarts.

### Complete CRUD Operations

The four core record types support:

- `GET` to read records
- `POST` to create records
- `PUT` to update records
- `DELETE` to remove records

### Relational Data Model

Playtest sessions reference game builds. Bug reports and feedback notes reference playtest sessions.

### Validation and Error Responses

The API performs basic request validation and returns clear HTTP responses:

- `200 OK` for successful reads and updates
- `201 Created` for successful record creation
- `204 No Content` for successful deletion
- `400 Bad Request` for invalid input
- `404 Not Found` when a requested record does not exist

### Workflow Queries

Additional endpoints support common playtest-review tasks:

- Get all sessions for a build
- Get all bugs for a session
- Get unresolved bugs
- Get feedback by category

### API Documentation

The project generates an OpenAPI document and provides Swagger UI during local development for interactive endpoint review.

### Repeatable API Testing

The repository includes `api-tests.http`, which can be run through the VS Code REST Client extension.

The saved requests include:

- GET requests for all four record types
- A complete GameBuild POST → PUT → DELETE test flow
- Reusable variables for the local API address
- Response-based ID reuse between dependent requests

### Structured Application Logging

Successful create, update, and delete operations generate structured informational logs.

Examples include:

```text
Created game build 3 for project MAZE3 REST Client Test
Updated bug report 2 with status Fixed
Deleted feedback note 4
```

These logs provide useful operational context without logging full request bodies, connection strings, or sensitive information.

## Tech Stack

### Implemented

- C#
- .NET
- ASP.NET Core Minimal APIs
- Entity Framework Core
- SQL Server LocalDB
- LINQ
- REST API design
- OpenAPI
- Swagger UI
- VS Code REST Client
- Structured ASP.NET Core logging
- Git and GitHub

### Planned

- Azure SQL
- Azure App Service
- GitHub Actions CI/CD
- Production environment configuration
- Data export to CSV or JSON
- Optional Python playtest summary
- Optional Unity Editor integration or web dashboard

## Repository Structure

```text
maze3-playtestops-api/
├── Maze3.PlayTestOps.Api/
│   ├── Data/
│   │   ├── PlayTestOpsDbContext.cs
│   │   └── SeedData.cs
│   ├── Migrations/
│   ├── Models/
│   │   ├── GameBuild.cs
│   │   ├── PlaytestSession.cs
│   │   ├── BugReport.cs
│   │   └── FeedbackNote.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
├── api-tests.http
├── Maze3.PlayTestOps.sln
└── README.md
```

## Data Model

### GameBuild

Represents a testable game or prototype version.

```text
Id
ProjectName
Version
Branch
BuildDate
ReleaseNotes
CreatedAt
```

### PlaytestSession

Represents one playtest performed against a specific build.

```text
Id
GameBuildId
TesterName
Platform
SessionDate
Notes
```

### BugReport

Represents a defect discovered during a playtest session.

```text
Id
PlaytestSessionId
Title
Description
Severity
Status
ReproSteps
CreatedAt
```

### FeedbackNote

Represents non-bug feedback from a playtest session.

```text
Id
PlaytestSessionId
Category
Rating
Comment
CreatedAt
```

## API Endpoints

### Game Builds

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/gamebuilds` | Get all game builds |
| `GET` | `/api/gamebuilds/{id}` | Get one game build |
| `GET` | `/api/gamebuilds/{id}/sessions` | Get all sessions for a build |
| `POST` | `/api/gamebuilds` | Create a game build |
| `PUT` | `/api/gamebuilds/{id}` | Update a game build |
| `DELETE` | `/api/gamebuilds/{id}` | Delete a game build |

### Playtest Sessions

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/sessions` | Get all playtest sessions |
| `GET` | `/api/sessions/{id}` | Get one playtest session |
| `GET` | `/api/sessions/{id}/bugs` | Get all bugs for a session |
| `POST` | `/api/sessions` | Create a playtest session |
| `PUT` | `/api/sessions/{id}` | Update a playtest session |
| `DELETE` | `/api/sessions/{id}` | Delete a playtest session |

### Bug Reports

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/bugs` | Get all bug reports |
| `GET` | `/api/bugs/unresolved` | Get bugs with unresolved statuses |
| `GET` | `/api/bugs/{id}` | Get one bug report |
| `POST` | `/api/bugs` | Create a bug report |
| `PUT` | `/api/bugs/{id}` | Update a bug report |
| `DELETE` | `/api/bugs/{id}` | Delete a bug report |

### Feedback Notes

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/feedback` | Get all feedback notes |
| `GET` | `/api/feedback/{id}` | Get one feedback note |
| `GET` | `/api/feedback/category/{category}` | Get feedback by category |
| `POST` | `/api/feedback` | Create a feedback note |
| `PUT` | `/api/feedback/{id}` | Update a feedback note |
| `DELETE` | `/api/feedback/{id}` | Delete a feedback note |

## Getting Started

### Requirements

The current local setup requires:

- Windows
- .NET SDK
- SQL Server LocalDB
- Git
- Optional: Visual Studio Code with the REST Client extension

### Clone the Repository

```powershell
git clone https://github.com/lukemaeser/maze3-playtestops-api.git
cd maze3-playtestops-api
```

### Restore Dependencies

```powershell
dotnet restore
```

### Create or Update the Local Database

Install the EF Core command-line tool once, if it is not already installed:

```powershell
dotnet tool install --global dotnet-ef
```

Apply the existing migrations:

```powershell
dotnet ef database update --project Maze3.PlayTestOps.Api
```

This creates the local `Maze3PlayTestOps` database using SQL Server LocalDB.

### Run the API

```powershell
dotnet run --project Maze3.PlayTestOps.Api --launch-profile https
```

The terminal should display local addresses similar to:

```text
https://localhost:7136
http://localhost:5284
```

The exact port may differ. Use the address displayed in the terminal.

### Trust the HTTPS Development Certificate

When required, run:

```powershell
dotnet dev-certs https --trust
```

Then restart the API.

## Swagger UI and OpenAPI

While the API is running in the Development environment, open:

```text
https://localhost:7136/swagger/index.html
```

The serialized OpenAPI document is available at:

```text
https://localhost:7136/openapi/v1.json
```

Swagger UI can be used to inspect routes, request schemas, and response definitions.

## Testing with VS Code REST Client

1. Install the **REST Client** extension by Huachao Mao.
2. Open `api-tests.http`.
3. Start the API.
4. Click **Send Request** above an individual request.
5. Review the status code and JSON response in the generated response tab.

For the saved GameBuild workflow, run requests in this order:

```text
POST
PUT
DELETE
```

The PUT and DELETE requests reuse the ID returned by the named POST request.

Typical successful responses include:

```text
GET     → 200 OK
POST    → 201 Created
PUT     → 200 OK
DELETE  → 204 No Content
```

## Example Requests

### Create a Game Build

```http
POST https://localhost:7136/api/gamebuilds
Content-Type: application/json
Accept: application/json

{
  "projectName": "It Waits in the Deep",
  "version": "0.2.0",
  "branch": "prototype/interactions",
  "buildDate": "2026-07-28T12:00:00Z",
  "releaseNotes": "Added interaction updates and revised playtest prompts."
}
```

### Create a Playtest Session

```http
POST https://localhost:7136/api/sessions
Content-Type: application/json
Accept: application/json

{
  "gameBuildId": 1,
  "testerName": "Internal Tester 02",
  "platform": "Windows",
  "sessionDate": "2026-07-28T13:00:00Z",
  "notes": "Tester completed the main interaction loop."
}
```

### Create a Bug Report

```http
POST https://localhost:7136/api/bugs
Content-Type: application/json
Accept: application/json

{
  "playtestSessionId": 1,
  "title": "Door prompt remains visible after interaction",
  "description": "The interaction prompt remains visible after the door opens.",
  "severity": "Medium",
  "status": "Open",
  "reproSteps": "Approach the door, open it, and step backward."
}
```

### Create a Feedback Note

```http
POST https://localhost:7136/api/feedback
Content-Type: application/json
Accept: application/json

{
  "playtestSessionId": 1,
  "category": "Gameplay",
  "rating": 4,
  "comment": "The interaction worked, but stronger visual feedback would improve clarity."
}
```

## Configuration

The API uses ASP.NET Core configuration so environment-specific values remain separate from application code.

### Shared Configuration

`appsettings.json` contains settings shared across environments, including:

- Default application log level
- ASP.NET Core framework log level
- Entity Framework database-command log level
- Allowed host configuration

### Local Development Configuration

`appsettings.Development.json` contains the local SQL Server LocalDB connection string:

```json
{
  "ConnectionStrings": {
    "PlayTestOpsDatabase": "Server=(localdb)\\MSSQLLocalDB;Database=Maze3PlayTestOps;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

`Program.cs` reads the value through the configuration system:

```csharp
builder.Configuration.GetConnectionString("PlayTestOpsDatabase")
```

This keeps the database address outside the application logic and allows each environment to supply a different connection string.

A future Azure deployment will provide its production connection string through Azure or environment configuration rather than requiring changes to the C# source code.

Passwords, production connection strings, API keys, and other secrets must not be committed to the repository.

## Logging

The API uses ASP.NET Core structured logging for successful create, update, and delete operations.

Logging is intentionally focused on useful identifiers and workflow context, such as:

- Record ID
- Project name
- Build ID
- Bug severity
- Bug status
- Feedback category

Full request bodies, credentials, and connection strings are not logged.

The default application logging level is `Information`, while repetitive ASP.NET Core and Entity Framework framework output is reduced to `Warning`.

## Seed Data

The local database includes realistic sample data for:

```text
Project: It Waits in the Deep
Version: 0.1.0
Branch: prototype/interactions
```

The sample records demonstrate how a build connects to its sessions, bugs, and feedback.

Seed initialization checks whether the records already exist before adding them, preventing the same sample data from being inserted on every application startup.

## Development Approach

The project was developed incrementally:

1. Define the four core playtest record types.
2. Implement temporary in-memory CRUD endpoints.
3. Add validation and clear error responses.
4. Confirm the API behavior before introducing database complexity.
5. Replace temporary collections with EF Core and SQL Server.
6. Add relationships and realistic seed data.
7. Add workflow-focused query endpoints.
8. Add OpenAPI, Swagger UI, and reusable REST Client requests.
9. Add structured logging and environment-based configuration.
10. Refactor the working API for maintainability before cloud deployment.

This approach keeps each stage testable and avoids adding infrastructure before the core workflow is proven.

## Scope

The current version intentionally remains focused on the central playtest workflow:

```text
Builds → Sessions → Bugs and Feedback
```

The following features are intentionally deferred:

- Authentication
- User accounts
- Role-based permissions
- Complex architectural patterns
- Dashboards
- Unity Editor integration
- Machine learning
- Microservices
- Container orchestration

These features may be considered after the core API, deployment, and automation workflow are complete.

## Roadmap

### Maintainability

- Group or reorganize endpoints
- Reduce the size of `Program.cs`
- Introduce request and response DTOs where useful
- Keep existing API behavior stable during refactoring

### Cloud Deployment

- Create an Azure App Service
- Create an Azure SQL database
- Configure the production connection string
- Deploy the API
- Confirm the deployed API and Swagger UI work
- Add the live API URL to this README

### Continuous Integration and Deployment

- Add a GitHub Actions workflow
- Restore and build the project on each push
- Add automated tests when a test project exists
- Deploy to Azure after a successful build

### Data Analysis

- Export playtest records as CSV or JSON
- Add a small Python script that summarizes:
  - Sessions by build
  - Bugs by severity
  - Feedback by category
  - Build-quality warnings

### Future Integrations

- Unity Editor tool for submitting build notes
- Lightweight web dashboard
- External playtest form
- Authentication and role-based access
- Expanded reporting

## Project Direction

MAZE3 PlayTestOps API is being developed as a practical internal tool for organizing playtesting across MAZE3 Studios projects.

The core goal is to keep build, session, bug, and feedback data structured, traceable, and easy to review so that each playtest contributes directly to the next development iteration.