#  Enterprise Notification Delivery Platform

A centralized notification delivery platform that provides a unified API for sending and managing notifications across multiple channels, including **Email, SMS, and Push Notifications**.

The platform is designed with scalability, reliability, and maintainability in mind, following **Clean Architecture** principles and asynchronous background processing.

##  Features

-  Send notifications through multiple channels:
  - Email
  - SMS
  - Push Notifications
-  Notification request management
-  Asynchronous notification processing using a queue
-  Background notification worker
-  Notification templates
-  Notification provider abstraction
-  Operational dashboards and notification statistics
-  Channel-based notification summaries
-  Request validation and error handling
-  Unit testing
-  SQL Server database with Entity Framework Core
-  Firebase Cloud Messaging integration for push notifications
-  SendGrid integration for email notifications

## 🏗️ Architecture

The project follows **Clean Architecture** principles, separating responsibilities into different layers:

```text
├── API
├── Application
├── Domain
├── Infrastructure
└── Tests
API

Responsible for HTTP endpoints, controllers, request handling, and API configuration.

Application

Contains business logic, DTOs, interfaces, services, and application-level abstractions.

Domain

Contains the core entities, enums, and domain models.

Infrastructure

Responsible for database access, external notification providers, queue implementation, and infrastructure services.

Tests

Contains unit tests covering the application's main components and business logic.

 Notification Processing

The platform uses an asynchronous queue-based architecture to process notifications efficiently.

Client
   ↓
Notification API
   ↓
Notification Queue
   ↓
Background Worker
   ↓
Notification Provider
   ↓
Email / SMS / Push

The queue allows notification requests to be processed asynchronously without blocking the API request.

A background worker continuously monitors the queue and processes pending notification requests.

 Unit Testing

The project includes unit tests for important application components and business scenarios.

The tests are organized in a separate test project and use mocking where required to isolate dependencies and verify application behavior.

 Database

The application uses:

SQL Server
Entity Framework Core
EF Core Migrations

The database manages notifications, notification templates, notification providers, delivery attempts, and notification history.

A database creation script is also included in the repository.

 Operational Dashboard

The platform provides operational dashboards for monitoring notification delivery.

Notification Summary
Total Notifications
Pending
Processing
Delivered
Failed
Success Rate
Failure Rate
Channel Summary

Notifications are grouped by:

Email
SMS
Push
🛠️ Technologies
C#
ASP.NET Core Web API
Entity Framework Core
SQL Server
Clean Architecture
Background Services
System.Threading.Channels
Unit Testing
Swagger / OpenAPI
Firebase Cloud Messaging
SendGrid
Team

This project was developed by:

Mohammed Islam
Marwan Mamdouh
Jana Mostafa
 Project Status

Completed as part of an Enterprise Notification Delivery Platform assessment.
