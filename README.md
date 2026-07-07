# MAZE3 PlayTestOps API

MAZE3 PlayTestOps API is a lightweight ASP.NET Core Web API built to organize playtesting across games, prototypes, and gameplay systems in development.

The API is designed to keep build notes, playtest sessions, bug reports, and feedback in one structured place instead of spreading that information across notes, messages, and spreadsheets. Each playtest should make the next build easier to review and improve.

## Project Purpose

Game development creates a lot of useful feedback, but that feedback is only valuable if it can be found, compared, and acted on later.

Each build can produce important playtest information:

* Which version was tested
* Who tested it
* What platform was used
* What bugs were found
* How severe each issue was
* What design feedback came out of the session
* What changed between builds

This project was created to make that workflow more organized and repeatable without adding a large production-management system too early.

## Use Cases

* Review bugs reported against a specific game build
* Compare feedback across playtest sessions
* Track whether critical issues were fixed before the next build
* Export playtest data for basic analysis
* Support future Unity editor tools, dashboards, or external playtest forms

## Planned Core Features

* Track game builds, versions, branches, and release notes
* Record playtest sessions by build, tester, platform, date, and notes
* Capture bug reports with severity, status, repro steps, and related session data
* Store feedback notes by category, rating, and tester comment
* Provide API documentation through OpenAPI
* Use a SQL-backed data model that can run locally and support future cloud deployment

## Tech Stack

Current:

* C#
* ASP.NET Core Web API
* OpenAPI

Planned:

* Entity Framework Core
* SQL Server / Azure SQL
* Azure App Service
* GitHub Actions
* Swagger UI or another API testing/documentation interface

## Planned Analysis Layer

A small Python analysis layer is planned for exported playtest data.

The goal is to summarize build activity, bug severity, and feedback patterns without adding analytics complexity to the core API.

## Roadmap

* V1: Core API structure, EF Core models, SQL database, CRUD endpoints, OpenAPI documentation, and sample playtest data
* V2: Azure App Service deployment, Azure SQL configuration, logging, and GitHub Actions CI/CD
* V3: Data export and Python playtest summaries
* V4: Unity editor integration or a simple web dashboard
* V5: Authentication and role-based access

## Current Status

Early V1 development.

The current version contains the initial ASP.NET Core Web API project structure and local development setup. The API runs locally and currently exposes OpenAPI JSON for development verification.