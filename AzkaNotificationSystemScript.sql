CREATE DATABASE EnterpriseNotificationPlatform;
GO

USE EnterpriseNotificationPlatform;
GO

-------------------------------------------------
-- Notification Templates
-------------------------------------------------

CREATE TABLE NotificationTemplates
(
    TemplateId INT IDENTITY(1,1) PRIMARY KEY,

    TemplateName NVARCHAR(150) NOT NULL,

    NotificationType NVARCHAR(50) NOT NULL,

    Subject NVARCHAR(250),

    Body NVARCHAR(MAX) NOT NULL,

    Language NVARCHAR(20) NOT NULL,

    Version INT NOT NULL DEFAULT 1,

    Status NVARCHAR(20) NOT NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

-------------------------------------------------
-- Notification Providers
-------------------------------------------------

CREATE TABLE NotificationProviders
(
    ProviderId INT IDENTITY PRIMARY KEY,

    ProviderName NVARCHAR(100) NOT NULL,

    Channel NVARCHAR(20) NOT NULL,

    IsActive BIT NOT NULL DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

-------------------------------------------------
-- Notifications
-------------------------------------------------

CREATE TABLE Notifications
(
    NotificationId UNIQUEIDENTIFIER PRIMARY KEY,

    TemplateId INT NULL,

    ProviderId INT NULL,

    NotificationType NVARCHAR(50) NOT NULL,

    Recipient NVARCHAR(250) NOT NULL,

    Channel NVARCHAR(20) NOT NULL,

    Subject NVARCHAR(250),

    Body NVARCHAR(MAX) NOT NULL,

    Priority NVARCHAR(20) NOT NULL,

    Status NVARCHAR(30) NOT NULL,

    CorrelationId UNIQUEIDENTIFIER NOT NULL UNIQUE,

    RequestedBy NVARCHAR(100),

    RequestedAt DATETIME2 NOT NULL,

    QueueTime DATETIME2 NULL,

    ProcessingTime DATETIME2 NULL,

    DeliveryTime DATETIME2 NULL,

    RetryCount INT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Notifications_Template
        FOREIGN KEY(TemplateId)
        REFERENCES NotificationTemplates(TemplateId),

    CONSTRAINT FK_Notifications_Provider
        FOREIGN KEY(ProviderId)
        REFERENCES NotificationProviders(ProviderId)
);

-------------------------------------------------
-- Delivery Attempts
-------------------------------------------------

CREATE TABLE DeliveryAttempts
(
    AttemptId INT IDENTITY PRIMARY KEY,

    NotificationId UNIQUEIDENTIFIER NOT NULL,

    AttemptNumber INT NOT NULL,

    StartedAt DATETIME2 NOT NULL,

    CompletedAt DATETIME2,

    Result NVARCHAR(20) NOT NULL,

    ResponseTimeMs INT,

    ProviderResponse NVARCHAR(MAX),

    CONSTRAINT FK_DeliveryAttempts_Notifications
        FOREIGN KEY(NotificationId)
        REFERENCES Notifications(NotificationId)
);
GO

-------------------------------------------------
-- Notification History
-------------------------------------------------

CREATE TABLE NotificationHistory
(
    HistoryId INT IDENTITY PRIMARY KEY,

    NotificationId UNIQUEIDENTIFIER NOT NULL,

    OldStatus NVARCHAR(30),

    NewStatus NVARCHAR(30) NOT NULL,

    ChangedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    Remarks NVARCHAR(500),

    ChangedBy NVARCHAR(100),

    CONSTRAINT FK_History_Notifications
        FOREIGN KEY(NotificationId)
        REFERENCES Notifications(NotificationId)
);
GO