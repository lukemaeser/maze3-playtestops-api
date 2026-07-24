# MAZE3 PlayTestOps API

MAZE3 PlayTestOps API is a lightweight ASP.NET Core Web API built for MAZE3 Studios to organize game builds, playtest sessions, bug reports, and feedback across games, prototypes, and gameplay systems in development.

The API keeps playtest information in one structured place instead of spreading it across notes, messages, spreadsheets, and separate bug reports. Each recorded playtest should make the next build easier to review and improve.

## Project Purpose

Game development produces valuable feedback, but that feedback is only useful when it can be found, compared, and acted on later.

A single build can generate important questions:

- Which version was tested?
- Who tested it?
- What platform was used?
- What bugs were found?
- How severe was each issue?
- What design or usability feedback came from the session?
- What changed between builds?
- Were important issues resolved before the next build?

MAZE3 PlayTestOps API provides a focused structure for recording that information without introducing a large production-management system too early.

## Use Cases

- Track game builds, versions, branches, and release notes
- Record playtest sessions for specific builds and platforms
- Review bugs reported during a playtest
- Compare feedback across sessions
- Track issue severity and status
- Confirm whether critical problems were addressed before the next build
- Support future data exports, Unity tools, dashboards, and external playtest forms

## Current Features

- Four core playtest record types:
  - Game builds
  - Playtest sessions
  - Bug reports
  - Feedback notes
- Create, read, update, and delete operations for all four record types
- Basic required-field validation
- Clear `400 Bad Request` responses for invalid input
- Clear `404 Not Found` responses for missing records
- OpenAPI JSON documentation
- Local testing through browser and PowerShell requests
- In-memory storage for the current development version

## Tech Stack

### Current

- C#
- ASP.NET Core Web API
- Minimal APIs
- OpenAPI
- In-memory data storage
- Git and GitHub

### Planned

- Entity Framework Core
- SQL Server for local persistence
- Azure SQL
- Azure App Service
- GitHub Actions CI/CD
- Swagger UI or another API testing and documentation interface

## Current Status

The local in-memory API version is functional.

CRUD endpoints, basic validation, error responses, OpenAPI documentation, local setup instructions, endpoint documentation, and sample request bodies are working.

The current data store is temporary, so all records reset when the application stops. The next major milestone is replacing the in-memory collections with Entity Framework Core and a SQL-backed database.

## How to Run Locally

### Requirements

- .NET SDK
- Git, or the ability to download the repository
- Windows PowerShell, Command Prompt, or another terminal

### Steps

1. Clone the repository:

```powershell
git clone https://github.com/lukemaeser/maze3-playtestops-api.git
```

2. Open the repository folder:

```powershell
cd maze3-playtestops-api
```

3. Run the API:

```powershell
dotnet run --project Maze3.PlayTestOps.Api --launch-profile https
```

4. Check the terminal output for the local URLs.

The application should display addresses similar to:

```text
https://localhost:7136
http://localhost:5284
```

The exact port may differ. Use the address shown in the terminal.

5. Open the game builds endpoint in a browser:

```text
https://localhost:7136/api/gamebuilds
```

6. Open the OpenAPI JSON document:

```text
https://localhost:7136/openapi/v1.json
```

If the HTTPS development certificate is not trusted, run:

```powershell
dotnet dev-certs https --trust
```

> **Note:** The current version uses in-memory storage. Data resets whenever the API stops.

## Current Endpoints

### Game Builds

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/gamebuilds` | Get all game builds |
| `GET` | `/api/gamebuilds/{id}` | Get one game build by ID |
| `POST` | `/api/gamebuilds` | Create a game build |
| `PUT` | `/api/gamebuilds/{id}` | Update a game build |
| `DELETE` | `/api/gamebuilds/{id}` | Delete a game build |

### Playtest Sessions

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/sessions` | Get all playtest sessions |
| `GET` | `/api/sessions/{id}` | Get one playtest session by ID |
| `POST` | `/api/sessions` | Create a playtest session |
| `PUT` | `/api/sessions/{id}` | Update a playtest session |
| `DELETE` | `/api/sessions/{id}` | Delete a playtest session |

### Bug Reports

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/bugs` | Get all bug reports |
| `GET` | `/api/bugs/{id}` | Get one bug report by ID |
| `POST` | `/api/bugs` | Create a bug report |
| `PUT` | `/api/bugs/{id}` | Update a bug report |
| `DELETE` | `/api/bugs/{id}` | Delete a bug report |

### Feedback Notes

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/feedback` | Get all feedback notes |
| `GET` | `/api/feedback/{id}` | Get one feedback note by ID |
| `POST` | `/api/feedback` | Create a feedback note |
| `PUT` | `/api/feedback/{id}` | Update a feedback note |
| `DELETE` | `/api/feedback/{id}` | Delete a feedback note |

## Sample JSON Request Bodies

These examples show the JSON data that can be sent when creating records. IDs are assigned by the API and are therefore not included in the request bodies.

### Create a Game Build

`POST /api/gamebuilds`

```json
{
  "projectName": "It Waits in the Deep",
  "version": "0.0.2",
  "branch": "main",
  "buildDate": "2026-07-24T15:00:00Z",
  "releaseNotes": "Updated prototype build with interaction and feedback improvements."
}
```

### Create a Playtest Session

`POST /api/sessions`

```json
{
  "gameBuildId": 1,
  "testerName": "Internal Tester 02",
  "platform": "Windows",
  "sessionDate": "2026-07-24T16:00:00Z",
  "notes": "Tester completed the main interaction loop and noted usability feedback."
}
```

### Create a Bug Report

`POST /api/bugs`

```json
{
  "playtestSessionId": 1,
  "title": "Door prompt remains visible after interaction",
  "description": "The interaction prompt stays on screen after the player opens the door.",
  "severity": "Medium",
  "status": "Open",
  "reproSteps": "Start the build, approach the door, open the door, then step backward."
}
```

### Create a Feedback Note

`POST /api/feedback`

```json
{
  "playtestSessionId": 1,
  "category": "Gameplay",
  "rating": 4,
  "comment": "Door interaction worked, but the tester wanted stronger visual feedback."
}
```

## Validation and Error Responses

The current API performs basic validation before accepting records.

Examples include:

- Game builds require a project name and version
- Requests with missing required fields return `400 Bad Request`
- Requests for records that do not exist return `404 Not Found`
- Error responses include a simple message describing the problem

Validation will be expanded as the project adds database relationships, foreign-key checks, rating limits, and defined severity and status values.

## Development Approach

Development began with in-memory collections so the core API design could be confirmed before adding database complexity.

This incremental approach made it possible to:

- Translate the playtesting workflow into four clear data models
- Confirm the route structure and CRUD behavior
- Test HTTP methods and response codes
- Add validation and missing-record handling
- Verify the API through browser and PowerShell requests
- Document setup, endpoints, and request formats
- Establish a working baseline before introducing persistent storage

The next implementation phase will preserve the current API behavior while replacing temporary storage with Entity Framework Core and SQL-backed persistence.

## Roadmap

### Next Steps

- Add Entity Framework Core packages
- Create `PlayTestOpsDbContext`
- Configure SQL Server for local development
- Create and apply the first database migration
- Replace in-memory collections with database operations
- Retest all CRUD, validation, and error behavior

### Later V1 Work

- Add database relationships between builds, sessions, bugs, and feedback
- Add realistic MAZE3 Studios seed data
- Save reusable API test requests
- Add application logging
- Move environment-specific settings into configuration
- Improve API documentation and project organization

### Cloud and Automation

- Deploy the API to Azure App Service
- Connect the application to Azure SQL
- Add a GitHub Actions build and deployment workflow
- Document the deployed API URL and configuration process

### Future Extensions

- Export playtest data to CSV or JSON
- Add a small Python summary tool for playtest data
- Build a Unity Editor integration
- Create a simple web dashboard or external playtest form
- Add authentication, users, and role-based access