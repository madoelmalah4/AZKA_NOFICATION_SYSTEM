using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Azka.NotificationPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class M1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NotificationTemplates",
                columns: new[] { "TemplateId", "Body", "Channel", "Language", "Status", "Subject", "TemplateName", "Version" },
                values: new object[,]
                {
                    { new Guid("19b318e2-d938-4279-b892-12807759c9dd"), "<h2>Welcome to Azka!</h2><p>Your account has been successfully created. You can now log in and start using the platform.</p><p>If you have any questions, our support team is always here to help.</p><br/><p>Best regards,<br/><strong>Azka Platform Team</strong></p>", 0, "en-US", "Active", "Welcome to Azka Platform!", "UserRegistration", 2 },
                    { new Guid("1b3038b9-9fbf-41c5-8748-40cbf3d2c605"), "Please submit your meter reading for account {{AccountNumber}} by {{DueDate}}. Tap to submit.", 2, "en-US", "Active", "Meter Reading Due", "MeterReadingReminder", 1 },
                    { new Guid("1b8b55e3-5032-4715-937b-80c16267ee5f"), "Azka: New invoice {{InvoiceNumber}} generated. Amount: {{Amount}} due on {{DueDate}}. Visit azka.io/invoices to pay.", 1, "en-US", "Active", null, "InvoiceGeneration", 1 },
                    { new Guid("2fe49b2e-ddce-4096-aa40-8b88dbba57b9"), "Your verification code is {{OTP}}. Valid for 5 minutes.", 2, "en-US", "Active", "Verification Code", "OTP", 1 },
                    { new Guid("47b149ec-1319-44f0-85b1-f1556d8f94f7"), "<h2>Order Confirmed!</h2><p>Thank you for your order. We have received it and it is now being processed.</p><p>You will receive a follow-up notification once your order has been dispatched.</p><br/><p>Thank you for choosing Azka.<br/><strong>Azka Order Team</strong></p>", 0, "en-US", "Active", "Your Order Has Been Confirmed ? Azka", "OrderConfirmation", 2 },
                    { new Guid("49ffb717-49b1-47b5-a06e-f5bb50744795"), "<h2>Appointment Confirmation</h2><p>Dear customer,</p><p>A maintenance appointment has been scheduled for <strong>{{AppointmentDate}}</strong> at <strong>{{AppointmentTime}}</strong>.</p><p>Our technician, <strong>{{TechnicianName}}</strong>, will visit your address.</p><p>Best regards,<br/><strong>Azka Maintenance Team</strong></p>", 0, "en-US", "Active", "Maintenance Appointment Scheduled", "MaintenanceAppointment", 1 },
                    { new Guid("532f0b36-8034-4258-a04b-31b3036d8eb7"), "Service interruption detected for {{AffectedService}}. Tap for updates.", 2, "en-US", "Active", "Service Interruption", "ServiceInterruption", 1 },
                    { new Guid("6902fe8b-8159-4da6-a090-76a0853d67a9"), "Azka: Ticket #{{TicketId}} has been updated. Update: {{TicketUpdate}}. Visit portal for details.", 1, "en-US", "Active", null, "SupportTicketUpdate", 1 },
                    { new Guid("6caa04e4-0cf0-484a-ab07-c3fc379be45a"), "<h2>Service Interruption Notice</h2><p>Hello,</p><p>We are experiencing a temporary service interruption affecting <strong>{{AffectedService}}</strong>. Our team is actively investigating, and we expect resolution by <strong>{{EstimatedResolution}}</strong>.</p><p>We apologize for the inconvenience.</p><p>Sincerely,<br/><strong>Azka Operations Team</strong></p>", 0, "en-US", "Active", "Service Interruption Notice", "ServiceInterruption", 1 },
                    { new Guid("6f0a094e-01df-4301-aa61-0dc56964dc76"), "Azka Alert: Temporary service interruption affecting {{AffectedService}}. Expected resolution: {{EstimatedResolution}}. We apologize for the inconvenience.", 1, "en-US", "Active", null, "ServiceInterruption", 1 },
                    { new Guid("730cea46-535d-4795-9c99-e8c5d1ed982c"), "Azka: Maintenance appointment scheduled for {{AppointmentDate}} at {{AppointmentTime}} with {{TechnicianName}}. Reply to reschedule.", 1, "en-US", "Active", null, "MaintenanceAppointment", 1 },
                    { new Guid("7a94c765-c2b2-429e-9d4c-e15281158352"), "<h2>Meter Reading Reminder</h2><p>Hello,</p><p>It is time to submit your meter reading for account <strong>{{AccountNumber}}</strong>. Please submit it by <strong>{{DueDate}}</strong> to ensure accurate billing.</p><p>Best regards,<br/><strong>Azka Billing Team</strong></p>", 0, "en-US", "Active", "Reminder: Submit Your Meter Reading", "MeterReadingReminder", 1 },
                    { new Guid("8474d34b-ba0e-4cac-9973-b94dc13fb5b5"), "<h2>Transaction Failure Alert</h2><p>Dear customer,</p><p>We attempted to process a transaction for your account, but it failed. Reference: <strong>{{TransactionId}}</strong>. Reason: {{FailureReason}}.</p><p>Please update your payment method or try again.</p><p>Regards,<br/><strong>Azka Support Team</strong></p>", 0, "en-US", "Active", "Transaction Failed - Action Required", "TransactionFailure", 1 },
                    { new Guid("877d008b-4188-4f37-8780-72bd7f845aa7"), "<h2>New Invoice Ready</h2><p>Dear customer,</p><p>A new invoice <strong>{{InvoiceNumber}}</strong> has been generated for your account. Amount due: <strong>{{Amount}}</strong>. Due date: <strong>{{DueDate}}</strong>.</p><p>Please log in to your portal to pay.</p><p>Best regards,<br/><strong>Azka Billing Team</strong></p>", 0, "en-US", "Active", "New Invoice Generated - {{InvoiceNumber}}", "InvoiceGeneration", 1 },
                    { new Guid("8836c121-4384-4e72-b764-f60783aa0cef"), "Azka: Payment of {{Amount}} for invoice {{InvoiceNumber}} was successful. Thank you!", 1, "en-US", "Active", null, "PaymentConfirmation", 1 },
                    { new Guid("8b3168ba-b25d-4a2c-97a1-269a67a11dec"), "Ticket #{{TicketId}} was updated: '{{TicketUpdate}}'. Tap to view.", 2, "en-US", "Active", "Support Ticket Update", "SupportTicketUpdate", 1 },
                    { new Guid("8bfe1266-0ab8-4b6e-8da5-f052db823a1c"), "<h2>Support Ticket Update</h2><p>Hello,</p><p>Your support ticket <strong>#{{TicketId}}</strong> has been updated.</p><p><strong>Latest Update:</strong><br/>{{TicketUpdate}}</p><p>View full history or reply in your support portal.</p><p>Best regards,<br/><strong>Azka Customer Support</strong></p>", 0, "en-US", "Active", "Support Ticket Updated - #{{TicketId}}", "SupportTicketUpdate", 1 },
                    { new Guid("98a41d3f-71cf-4753-a865-398d04fc4d6c"), "Maintenance scheduled on {{AppointmentDate}} at {{AppointmentTime}}. Tap for details.", 2, "en-US", "Active", "Appointment Scheduled", "MaintenanceAppointment", 1 },
                    { new Guid("9f1f5b65-d655-42aa-b7f0-e6992ff7ddfe"), "Your payment of {{Amount}} has been processed successfully.", 2, "en-US", "Active", "Payment Confirmed", "PaymentConfirmation", 1 },
                    { new Guid("a2534032-029e-4900-bb6d-435216e776f6"), "Welcome to Azka Platform! Your account is ready. Log in at azka.io to get started.", 1, "en-US", "Active", null, "UserRegistration", 2 },
                    { new Guid("a27ec50b-bf99-4d61-8b9f-8dbcbe3a97ce"), "<h2>Password Reset</h2><p>We received a request to reset the password on your Azka account.</p><p>Please follow the instructions in your account portal to complete the reset. This link is valid for 15 minutes.</p><p>If you did not request this, you can safely ignore this email.</p><br/><p>Regards,<br/><strong>Azka Security Team</strong></p>", 0, "en-US", "Active", "Password Reset Request ? Azka Platform", "PasswordReset", 2 },
                    { new Guid("b3919f3a-95f4-4d5f-b84e-8146c75f8b17"), "Azka: Your order has been confirmed and is now being processed. Thank you for your purchase!", 1, "en-US", "Active", null, "OrderConfirmation", 2 },
                    { new Guid("c57a5edd-54ed-4ae5-b9da-b41c867ba939"), "<h2 style=\"color:red;\">System Alert</h2><p>A critical system event has been detected on the Azka platform. Please log in to the administration panel immediately to review and take action.</p><p>This is an automated alert from the Azka monitoring system.</p>", 0, "en-US", "Active", "SYSTEM ALERT ? Azka Platform", "SystemAlert", 2 },
                    { new Guid("cc94d662-a814-4d22-998f-db2251523c75"), "<h2>Your OTP</h2><p>Hello,</p><p>Your one-time password is: <strong style='font-size:24px; letter-spacing:2px; color:#0056b3;'>{{OTP}}</strong></p><p>This code is valid for 5 minutes. Do not share this code with anyone.</p><p>Best regards,<br/><strong>Azka Security Team</strong></p>", 0, "en-US", "Active", "Your One-Time Password (OTP) - Azka", "OTP", 1 },
                    { new Guid("d147a76d-11ca-4f1e-81a7-42656adf0b0b"), "Azka Reminder: Please submit your meter reading for account {{AccountNumber}} by {{DueDate}}. Visit azka.io/meter.", 1, "en-US", "Active", null, "MeterReadingReminder", 1 },
                    { new Guid("d22c639f-2d58-4236-a012-a7d49f17b5a7"), "Azka: Transaction {{TransactionId}} failed. Reason: {{FailureReason}}. Please check your account.", 1, "en-US", "Active", null, "TransactionFailure", 1 },
                    { new Guid("d63ba317-1c20-418b-8a32-a11616ec25cf"), "A password reset was requested for your account. Tap to verify.", 2, "en-US", "Active", "Password Reset Requested", "PasswordReset", 2 },
                    { new Guid("db4ad581-a1fb-4742-b7ee-f0e7f37f7365"), "Welcome to Azka Platform! Tap to open your profile and get started.", 2, "en-US", "Active", "Welcome to Azka!", "UserRegistration", 2 },
                    { new Guid("e3f885e3-c6df-470f-a156-0d5c7b5d2c3f"), "Transaction of {{Amount}} failed. Tap to review.", 2, "en-US", "Active", "Transaction Failed", "TransactionFailure", 1 },
                    { new Guid("e926f5a9-3c12-48a8-9668-3841d0fc4739"), "Invoice {{InvoiceNumber}} for {{Amount}} is now available. Due date: {{DueDate}}. Tap to view.", 2, "en-US", "Active", "New Invoice Available", "InvoiceGeneration", 1 },
                    { new Guid("f467354c-834e-48a7-8dc1-ec7b58f5c2a8"), "<h2>Payment Successful</h2><p>Hi there,</p><p>We've successfully processed your payment of <strong>{{Amount}}</strong> for invoice <strong>{{InvoiceNumber}}</strong>.</p><p>Thank you for your business!</p><p>Best regards,<br/><strong>Azka Finance Team</strong></p>", 0, "en-US", "Active", "Payment Confirmed - Thank You", "PaymentConfirmation", 1 },
                    { new Guid("f59d2754-2b8e-428f-8bf5-252bff6c4559"), "Azka: A password reset was requested for your account. Visit the platform to complete the process. Valid for 15 minutes.", 1, "en-US", "Active", null, "PasswordReset", 2 },
                    { new Guid("f8d8afff-e0b0-4a92-83e0-883f45b108b1"), "Azka: Your verification code is {{OTP}}. Valid for 5 minutes. Do not share this code.", 1, "en-US", "Active", null, "OTP", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("19b318e2-d938-4279-b892-12807759c9dd"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("1b3038b9-9fbf-41c5-8748-40cbf3d2c605"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("1b8b55e3-5032-4715-937b-80c16267ee5f"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("2fe49b2e-ddce-4096-aa40-8b88dbba57b9"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("47b149ec-1319-44f0-85b1-f1556d8f94f7"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("49ffb717-49b1-47b5-a06e-f5bb50744795"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("532f0b36-8034-4258-a04b-31b3036d8eb7"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("6902fe8b-8159-4da6-a090-76a0853d67a9"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("6caa04e4-0cf0-484a-ab07-c3fc379be45a"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("6f0a094e-01df-4301-aa61-0dc56964dc76"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("730cea46-535d-4795-9c99-e8c5d1ed982c"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("7a94c765-c2b2-429e-9d4c-e15281158352"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("8474d34b-ba0e-4cac-9973-b94dc13fb5b5"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("877d008b-4188-4f37-8780-72bd7f845aa7"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("8836c121-4384-4e72-b764-f60783aa0cef"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("8b3168ba-b25d-4a2c-97a1-269a67a11dec"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("8bfe1266-0ab8-4b6e-8da5-f052db823a1c"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("98a41d3f-71cf-4753-a865-398d04fc4d6c"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("9f1f5b65-d655-42aa-b7f0-e6992ff7ddfe"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("a2534032-029e-4900-bb6d-435216e776f6"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("a27ec50b-bf99-4d61-8b9f-8dbcbe3a97ce"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("b3919f3a-95f4-4d5f-b84e-8146c75f8b17"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("c57a5edd-54ed-4ae5-b9da-b41c867ba939"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("cc94d662-a814-4d22-998f-db2251523c75"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("d147a76d-11ca-4f1e-81a7-42656adf0b0b"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("d22c639f-2d58-4236-a012-a7d49f17b5a7"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("d63ba317-1c20-418b-8a32-a11616ec25cf"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("db4ad581-a1fb-4742-b7ee-f0e7f37f7365"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("e3f885e3-c6df-470f-a156-0d5c7b5d2c3f"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("e926f5a9-3c12-48a8-9668-3841d0fc4739"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("f467354c-834e-48a7-8dc1-ec7b58f5c2a8"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("f59d2754-2b8e-428f-8bf5-252bff6c4559"));

            migrationBuilder.DeleteData(
                table: "NotificationTemplates",
                keyColumn: "TemplateId",
                keyValue: new Guid("f8d8afff-e0b0-4a92-83e0-883f45b108b1"));
        }
    }
}
