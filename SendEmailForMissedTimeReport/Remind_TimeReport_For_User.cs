using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using sybring_project.Repos.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using sybring_project.Models.Db.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;


namespace SendEmailForMissedTimeReport
{
    public class Remind_TimeReport_For_User
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        private readonly ITimeService _timeService;
        private readonly IEmailSender _emailSender;


        public Remind_TimeReport_For_User(ILoggerFactory loggerFactory, ITimeService timeService, IConfiguration configuration, IEmailSender emailSender)
        {
            _logger = loggerFactory.CreateLogger<Remind_TimeReport_For_User>();
            _timeService = timeService;
            _configuration = configuration;
            _emailSender = emailSender;

        }

        [Function("Remind_TimeReport_For_User")]
        public async Task Run([TimerTrigger("0 0 12 * * 1", RunOnStartup = true)] TimerInfo myTimer)
        { 
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            try
            {
                // Send email reminders if users forgot to fill time reports for the previous week
                var users = await _timeService.GetUsersWithoutTimeReportForPreviousWeek();
                foreach (var user in users)
                {
                    string userEmail = user.Email;
                    string subject = "Reminder: Fill out your time report";
                    string body = $"Dear {user.FirstName},\n\nThis is a friendly reminder to fill out your time report for the previous week. Please make sure to complete it at your earliest convenience.\n\nBest regards,\nThe Sybring Team";

                    await SendEmailAsync(userEmail, subject, body);
                    _logger.LogInformation($"Email reminder sent to {userEmail}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while sending email reminders: {ex.Message}");
            }
        }

        private async Task SendEmailAsync(string userEmail, string subject, string body)
        {
            try
            {
              
                await _emailSender.SendEmailAsync(userEmail, subject, body);
            }
            catch (Exception ex)
            {
                // Handle the exception
                _logger.LogError($"Failed to send email to {userEmail}: {ex.Message}");
            }


        }





    }







}
