using Azka.NotificationPlatform.Domain.Entities;
using Azka.NotificationPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Azka.NotificationPlatform.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core Fluent API mapping for <see cref="NotificationTemplate"/> (FR-3).
/// </summary>
internal sealed class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
    public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        builder.ToTable("NotificationTemplates");

        builder.HasKey(t => t.TemplateId);

        builder.Property(t => t.TemplateId)
               .ValueGeneratedNever();

        builder.Property(t => t.TemplateName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.Channel)
               .IsRequired()
               .HasConversion<int>();

        builder.Property(t => t.Subject)
               .HasMaxLength(998);

        builder.Property(t => t.Body)
               .IsRequired()
               .HasColumnType("nvarchar(max)");

        builder.Property(t => t.Language)
               .IsRequired()
               .HasMaxLength(10); // BCP-47 tags are short

        builder.Property(t => t.Version)
               .IsRequired();

        builder.Property(t => t.Status)
               .IsRequired()
               .HasMaxLength(20); // "Active", "Inactive", "Archived"

        // Composite index supports the active-template resolution query
        builder.HasIndex(t => new { t.Channel, t.Language, t.Status, t.Version })
               .HasDatabaseName("IX_NotificationTemplates_Channel_Language_Status_Version");

        builder.HasData(
            new NotificationTemplate(
                Guid.Parse("19b318e2-d938-4279-b892-12807759c9dd"),
                "UserRegistration",
                NotificationChannel.Email,
                "<h2>Welcome to Azka!</h2><p>Your account has been successfully created. You can now log in and start using the platform.</p><p>If you have any questions, our support team is always here to help.</p><br/><p>Best regards,<br/><strong>Azka Platform Team</strong></p>",
                "en-US",
                2,
                "Welcome to Azka Platform!"
            ),
            new NotificationTemplate(
                Guid.Parse("a2534032-029e-4900-bb6d-435216e776f6"),
                "UserRegistration",
                NotificationChannel.SMS,
                "Welcome to Azka Platform! Your account is ready. Log in at azka.io to get started.",
                "en-US",
                2,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("db4ad581-a1fb-4742-b7ee-f0e7f37f7365"),
                "UserRegistration",
                NotificationChannel.Push,
                "Welcome to Azka Platform! Tap to open your profile and get started.",
                "en-US",
                2,
                "Welcome to Azka!"
            ),
            new NotificationTemplate(
                Guid.Parse("a27ec50b-bf99-4d61-8b9f-8dbcbe3a97ce"),
                "PasswordReset",
                NotificationChannel.Email,
                "<h2>Password Reset</h2><p>We received a request to reset the password on your Azka account.</p><p>Please follow the instructions in your account portal to complete the reset. This link is valid for 15 minutes.</p><p>If you did not request this, you can safely ignore this email.</p><br/><p>Regards,<br/><strong>Azka Security Team</strong></p>",
                "en-US",
                2,
                "Password Reset Request ? Azka Platform"
            ),
            new NotificationTemplate(
                Guid.Parse("f59d2754-2b8e-428f-8bf5-252bff6c4559"),
                "PasswordReset",
                NotificationChannel.SMS,
                "Azka: A password reset was requested for your account. Visit the platform to complete the process. Valid for 15 minutes.",
                "en-US",
                2,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("d63ba317-1c20-418b-8a32-a11616ec25cf"),
                "PasswordReset",
                NotificationChannel.Push,
                "A password reset was requested for your account. Tap to verify.",
                "en-US",
                2,
                "Password Reset Requested"
            ),
            new NotificationTemplate(
                Guid.Parse("47b149ec-1319-44f0-85b1-f1556d8f94f7"),
                "OrderConfirmation",
                NotificationChannel.Email,
                "<h2>Order Confirmed!</h2><p>Thank you for your order. We have received it and it is now being processed.</p><p>You will receive a follow-up notification once your order has been dispatched.</p><br/><p>Thank you for choosing Azka.<br/><strong>Azka Order Team</strong></p>",
                "en-US",
                2,
                "Your Order Has Been Confirmed ? Azka"
            ),
            new NotificationTemplate(
                Guid.Parse("b3919f3a-95f4-4d5f-b84e-8146c75f8b17"),
                "OrderConfirmation",
                NotificationChannel.SMS,
                "Azka: Your order has been confirmed and is now being processed. Thank you for your purchase!",
                "en-US",
                2,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("c57a5edd-54ed-4ae5-b9da-b41c867ba939"),
                "SystemAlert",
                NotificationChannel.Email,
                "<h2 style=\"color:red;\">System Alert</h2><p>A critical system event has been detected on the Azka platform. Please log in to the administration panel immediately to review and take action.</p><p>This is an automated alert from the Azka monitoring system.</p>",
                "en-US",
                2,
                "SYSTEM ALERT ? Azka Platform"
            ),
            new NotificationTemplate(
                Guid.Parse("f467354c-834e-48a7-8dc1-ec7b58f5c2a8"),
                "PaymentConfirmation",
                NotificationChannel.Email,
                "<h2>Payment Successful</h2><p>Hi there,</p><p>We've successfully processed your payment of <strong>{{Amount}}</strong> for invoice <strong>{{InvoiceNumber}}</strong>.</p><p>Thank you for your business!</p><p>Best regards,<br/><strong>Azka Finance Team</strong></p>",
                "en-US",
                1,
                "Payment Confirmed - Thank You"
            ),
            new NotificationTemplate(
                Guid.Parse("8836c121-4384-4e72-b764-f60783aa0cef"),
                "PaymentConfirmation",
                NotificationChannel.SMS,
                "Azka: Payment of {{Amount}} for invoice {{InvoiceNumber}} was successful. Thank you!",
                "en-US",
                1,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("9f1f5b65-d655-42aa-b7f0-e6992ff7ddfe"),
                "PaymentConfirmation",
                NotificationChannel.Push,
                "Your payment of {{Amount}} has been processed successfully.",
                "en-US",
                1,
                "Payment Confirmed"
            ),
            new NotificationTemplate(
                Guid.Parse("8474d34b-ba0e-4cac-9973-b94dc13fb5b5"),
                "TransactionFailure",
                NotificationChannel.Email,
                "<h2>Transaction Failure Alert</h2><p>Dear customer,</p><p>We attempted to process a transaction for your account, but it failed. Reference: <strong>{{TransactionId}}</strong>. Reason: {{FailureReason}}.</p><p>Please update your payment method or try again.</p><p>Regards,<br/><strong>Azka Support Team</strong></p>",
                "en-US",
                1,
                "Transaction Failed - Action Required"
            ),
            new NotificationTemplate(
                Guid.Parse("d22c639f-2d58-4236-a012-a7d49f17b5a7"),
                "TransactionFailure",
                NotificationChannel.SMS,
                "Azka: Transaction {{TransactionId}} failed. Reason: {{FailureReason}}. Please check your account.",
                "en-US",
                1,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("e3f885e3-c6df-470f-a156-0d5c7b5d2c3f"),
                "TransactionFailure",
                NotificationChannel.Push,
                "Transaction of {{Amount}} failed. Tap to review.",
                "en-US",
                1,
                "Transaction Failed"
            ),
            new NotificationTemplate(
                Guid.Parse("6caa04e4-0cf0-484a-ab07-c3fc379be45a"),
                "ServiceInterruption",
                NotificationChannel.Email,
                "<h2>Service Interruption Notice</h2><p>Hello,</p><p>We are experiencing a temporary service interruption affecting <strong>{{AffectedService}}</strong>. Our team is actively investigating, and we expect resolution by <strong>{{EstimatedResolution}}</strong>.</p><p>We apologize for the inconvenience.</p><p>Sincerely,<br/><strong>Azka Operations Team</strong></p>",
                "en-US",
                1,
                "Service Interruption Notice"
            ),
            new NotificationTemplate(
                Guid.Parse("6f0a094e-01df-4301-aa61-0dc56964dc76"),
                "ServiceInterruption",
                NotificationChannel.SMS,
                "Azka Alert: Temporary service interruption affecting {{AffectedService}}. Expected resolution: {{EstimatedResolution}}. We apologize for the inconvenience.",
                "en-US",
                1,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("532f0b36-8034-4258-a04b-31b3036d8eb7"),
                "ServiceInterruption",
                NotificationChannel.Push,
                "Service interruption detected for {{AffectedService}}. Tap for updates.",
                "en-US",
                1,
                "Service Interruption"
            ),
            new NotificationTemplate(
                Guid.Parse("cc94d662-a814-4d22-998f-db2251523c75"),
                "OTP",
                NotificationChannel.Email,
                "<h2>Your OTP</h2><p>Hello,</p><p>Your one-time password is: <strong style='font-size:24px; letter-spacing:2px; color:#0056b3;'>{{OTP}}</strong></p><p>This code is valid for 5 minutes. Do not share this code with anyone.</p><p>Best regards,<br/><strong>Azka Security Team</strong></p>",
                "en-US",
                1,
                "Your One-Time Password (OTP) - Azka"
            ),
            new NotificationTemplate(
                Guid.Parse("f8d8afff-e0b0-4a92-83e0-883f45b108b1"),
                "OTP",
                NotificationChannel.SMS,
                "Azka: Your verification code is {{OTP}}. Valid for 5 minutes. Do not share this code.",
                "en-US",
                1,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("2fe49b2e-ddce-4096-aa40-8b88dbba57b9"),
                "OTP",
                NotificationChannel.Push,
                "Your verification code is {{OTP}}. Valid for 5 minutes.",
                "en-US",
                1,
                "Verification Code"
            ),
            new NotificationTemplate(
                Guid.Parse("49ffb717-49b1-47b5-a06e-f5bb50744795"),
                "MaintenanceAppointment",
                NotificationChannel.Email,
                "<h2>Appointment Confirmation</h2><p>Dear customer,</p><p>A maintenance appointment has been scheduled for <strong>{{AppointmentDate}}</strong> at <strong>{{AppointmentTime}}</strong>.</p><p>Our technician, <strong>{{TechnicianName}}</strong>, will visit your address.</p><p>Best regards,<br/><strong>Azka Maintenance Team</strong></p>",
                "en-US",
                1,
                "Maintenance Appointment Scheduled"
            ),
            new NotificationTemplate(
                Guid.Parse("730cea46-535d-4795-9c99-e8c5d1ed982c"),
                "MaintenanceAppointment",
                NotificationChannel.SMS,
                "Azka: Maintenance appointment scheduled for {{AppointmentDate}} at {{AppointmentTime}} with {{TechnicianName}}. Reply to reschedule.",
                "en-US",
                1,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("98a41d3f-71cf-4753-a865-398d04fc4d6c"),
                "MaintenanceAppointment",
                NotificationChannel.Push,
                "Maintenance scheduled on {{AppointmentDate}} at {{AppointmentTime}}. Tap for details.",
                "en-US",
                1,
                "Appointment Scheduled"
            ),
            new NotificationTemplate(
                Guid.Parse("7a94c765-c2b2-429e-9d4c-e15281158352"),
                "MeterReadingReminder",
                NotificationChannel.Email,
                "<h2>Meter Reading Reminder</h2><p>Hello,</p><p>It is time to submit your meter reading for account <strong>{{AccountNumber}}</strong>. Please submit it by <strong>{{DueDate}}</strong> to ensure accurate billing.</p><p>Best regards,<br/><strong>Azka Billing Team</strong></p>",
                "en-US",
                1,
                "Reminder: Submit Your Meter Reading"
            ),
            new NotificationTemplate(
                Guid.Parse("d147a76d-11ca-4f1e-81a7-42656adf0b0b"),
                "MeterReadingReminder",
                NotificationChannel.SMS,
                "Azka Reminder: Please submit your meter reading for account {{AccountNumber}} by {{DueDate}}. Visit azka.io/meter.",
                "en-US",
                1,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("1b3038b9-9fbf-41c5-8748-40cbf3d2c605"),
                "MeterReadingReminder",
                NotificationChannel.Push,
                "Please submit your meter reading for account {{AccountNumber}} by {{DueDate}}. Tap to submit.",
                "en-US",
                1,
                "Meter Reading Due"
            ),
            new NotificationTemplate(
                Guid.Parse("877d008b-4188-4f37-8780-72bd7f845aa7"),
                "InvoiceGeneration",
                NotificationChannel.Email,
                "<h2>New Invoice Ready</h2><p>Dear customer,</p><p>A new invoice <strong>{{InvoiceNumber}}</strong> has been generated for your account. Amount due: <strong>{{Amount}}</strong>. Due date: <strong>{{DueDate}}</strong>.</p><p>Please log in to your portal to pay.</p><p>Best regards,<br/><strong>Azka Billing Team</strong></p>",
                "en-US",
                1,
                "New Invoice Generated - {{InvoiceNumber}}"
            ),
            new NotificationTemplate(
                Guid.Parse("1b8b55e3-5032-4715-937b-80c16267ee5f"),
                "InvoiceGeneration",
                NotificationChannel.SMS,
                "Azka: New invoice {{InvoiceNumber}} generated. Amount: {{Amount}} due on {{DueDate}}. Visit azka.io/invoices to pay.",
                "en-US",
                1,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("e926f5a9-3c12-48a8-9668-3841d0fc4739"),
                "InvoiceGeneration",
                NotificationChannel.Push,
                "Invoice {{InvoiceNumber}} for {{Amount}} is now available. Due date: {{DueDate}}. Tap to view.",
                "en-US",
                1,
                "New Invoice Available"
            ),
            new NotificationTemplate(
                Guid.Parse("8bfe1266-0ab8-4b6e-8da5-f052db823a1c"),
                "SupportTicketUpdate",
                NotificationChannel.Email,
                "<h2>Support Ticket Update</h2><p>Hello,</p><p>Your support ticket <strong>#{{TicketId}}</strong> has been updated.</p><p><strong>Latest Update:</strong><br/>{{TicketUpdate}}</p><p>View full history or reply in your support portal.</p><p>Best regards,<br/><strong>Azka Customer Support</strong></p>",
                "en-US",
                1,
                "Support Ticket Updated - #{{TicketId}}"
            ),
            new NotificationTemplate(
                Guid.Parse("6902fe8b-8159-4da6-a090-76a0853d67a9"),
                "SupportTicketUpdate",
                NotificationChannel.SMS,
                "Azka: Ticket #{{TicketId}} has been updated. Update: {{TicketUpdate}}. Visit portal for details.",
                "en-US",
                1,
                null
            ),
            new NotificationTemplate(
                Guid.Parse("8b3168ba-b25d-4a2c-97a1-269a67a11dec"),
                "SupportTicketUpdate",
                NotificationChannel.Push,
                "Ticket #{{TicketId}} was updated: '{{TicketUpdate}}'. Tap to view.",
                "en-US",
                1,
                "Support Ticket Update"
            )
        );
    }
}
