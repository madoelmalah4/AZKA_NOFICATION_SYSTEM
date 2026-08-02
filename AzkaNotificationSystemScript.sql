CREATE TABLE NotificationProviders
(
    ProviderId UNIQUEIDENTIFIER PRIMARY KEY,
    ProviderName NVARCHAR(150) NOT NULL,
    Channel INT NOT NULL,
    IsActive BIT NOT NULL
);

CREATE TABLE NotificationTemplates
(
    TemplateId UNIQUEIDENTIFIER PRIMARY KEY,
    TemplateName NVARCHAR(200) NOT NULL,
    Channel INT NOT NULL,
    Subject NVARCHAR(998),
    Body NVARCHAR(MAX) NOT NULL,
    Language NVARCHAR(10) NOT NULL,
    Version INT NOT NULL,
    Status NVARCHAR(20) NOT NULL
);

CREATE TABLE Notifications
(
    NotificationId UNIQUEIDENTIFIER PRIMARY KEY,
    NotificationType NVARCHAR(100) NOT NULL,
    Recipient NVARCHAR(320) NOT NULL,
    Channel INT NOT NULL,
    Subject NVARCHAR(998),
    Body NVARCHAR(MAX) NOT NULL,
    Status INT NOT NULL,
    CorrelationId UNIQUEIDENTIFIER NOT NULL,
    RequestedAt DATETIME2 NOT NULL,
    ApplicationName NVARCHAR(100)
);

CREATE TABLE NotificationHistories
(
    HistoryId UNIQUEIDENTIFIER PRIMARY KEY,
    NotificationId UNIQUEIDENTIFIER NOT NULL,
    Status INT NOT NULL,
    ChangedAt DATETIME2 NOT NULL,
    Remarks NVARCHAR(1000),

    FOREIGN KEY (NotificationId)
        REFERENCES Notifications(NotificationId)
);

CREATE TABLE DeliveryAttempts
(
    AttemptId UNIQUEIDENTIFIER PRIMARY KEY,
    NotificationId UNIQUEIDENTIFIER NOT NULL,
    AttemptNumber INT NOT NULL,
    StartedAt DATETIME2 NOT NULL,
    CompletedAt DATETIME2,
    Result NVARCHAR(50),
    ProviderResponse NVARCHAR(MAX),

    FOREIGN KEY (NotificationId)
        REFERENCES Notifications(NotificationId)
);
