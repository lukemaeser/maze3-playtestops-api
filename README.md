# MAZE3 PlayTestOps API

MAZE3 PlayTestOps API is a lightweight ASP.NET Core Web API built for MAZE3 Studios to organize playtesting across games, prototypes, and gameplay systems in development.

The tool keeps build notes, playtest sessions, bug reports, and feedback in one structured place instead of spreading that information across notes, messages, and spreadsheets. Each playtest should make the next build easier to review and improve.

## Project Purpose

Game development creates a lot of useful feedback, but that feedback is only valuable if it can be found, compared, and acted on later.

Each build can produce important playtest information:

- which version was tested
- who tested it
- what platform was used
- what bugs were found
- how severe each issue was
- what design feedback came out of the session
- what changed between builds

This project was created to make that workflow more organized and repeatable without adding a large production-management system too early.

## Use Cases

- Review bugs reported against a specific game build.
- Compare feedback across playtest sessions.
- Track whether critical issues were fixed before the next build.
- Export playtest data for basic analysis.
- Support future Unity editor tools, dashboards, or external playtest forms.

## Core Goals

- Track game builds, versions, branches, and release notes.
- Record playtest sessions by build, tester, platform, date, and notes.
- Capture bug reports with severity, status, repro steps, and related session data.
- Store feedback notes by category, rating, and tester comment.
- Provide a clean REST API for playtest operations.
- Include a small Python analysis layer for reviewing exported playtest data and spotting basic build-quality trends.

## Tech Stack

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- Swagger / OpenAPI
- Python for playtest data analysis

## Roadmap

- V1: Core API, SQLite database, CRUD endpoints, and Swagger documentation.
- V2: Data export and Python analysis summaries.
- V3: Unity editor integration or a simple web dashboard.
- V4: Authentication and hosted deployment.

## Current Status

Early V1 development.

The first milestone focuses on the core API structure, data models, database setup, CRUD endpoints, Swagger documentation, and sample playtest data.
