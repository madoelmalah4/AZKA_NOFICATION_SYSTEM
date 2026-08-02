# Enterprise Notification Delivery Platform

A centralized notification delivery platform that provides a unified API for sending and managing notifications across multiple channels, including Email, SMS, and Push Notifications.

The platform is designed with scalability, reliability, and maintainability in mind, following Clean Architecture principles and asynchronous background processing.

## Features

- Send notifications through Email, SMS, and Push Notifications.
- Notification request management.
- Notification templates.
- Notification provider abstraction.
- Asynchronous notification processing using a queue.
- Background worker for continuous notification processing.
- Delivery attempts and notification history tracking.
- Operational dashboards for monitoring notification status.
- Channel-based notification summaries.
- Request validation and error handling.
- Unit testing.
- SQL Server database using Entity Framework Core.
- Firebase Cloud Messaging integration for Push Notifications.
- SendGrid integration for Email Notifications.

## Architecture

The project follows Clean Architecture principles to keep the application maintainable, testable, and easy to extend.

The solution is divided into:

- API – Handles HTTP requests, controllers, and API configuration.
- Application – Contains business logic, services, DTOs, interfaces, and application abstractions.
- Domain – Contains the core entities, enums, and domain models.
- Infrastructure – Handles database access, external notification providers, queue implementation, and other infrastructure services.
- Tests – Contains unit tests for the application's components and business logic.

## Notification Processing

Notifications are processed asynchronously using a queue-based architecture.

When a notification request is received, the API adds it to the notification queue. A background worker continuously monitors the queue and processes pending notifications through the appropriate provider.

This approach prevents notification delivery from blocking the API request and allows the system to handle multiple notification requests efficiently.

## Dashboard

The platform provides operational dashboards to monitor notification delivery and system performance.

### Notification Summary

The dashboard provides:

- Total Notifications
- Pending Notifications
- Processing Notifications
- Delivered Notifications
- Failed Notifications
- Success Rate
- Failure Rate

### Channel Summary

Notifications can also be monitored based on their delivery channel:

- Email
- SMS
- Push

## Unit Testing

The project includes a dedicated unit test project covering important application components and business scenarios.

Tests use mocking where required to isolate dependencies and verify the behavior of the application independently.

## Database

The project uses SQL Server with Entity Framework Core for data persistence.

The database manages notifications, notification templates, notification providers, delivery attempts, and notification history.

The repository also includes a database script for creating the required database structure.

## Technologies

- C#
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Clean Architecture
- Background Services
- System.Threading.Channels
- Unit Testing
- Swagger / OpenAPI
- Firebase Cloud Messaging
- SendGrid

## Team

This project was developed by:

- Mohammed Islam
- Marwan Mamdouh
- Jana Mostafa

## Project Status

Completed as part of an Enterprise Notification Delivery Platform assessment.
