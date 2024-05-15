using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using sybring_project.Repos.Interfaces;

namespace DataDelete5Year
{
    public class TimeTrigger_FiveYear_Data
    {
        private readonly ILogger _logger;
      
        private readonly ITimeService _timeService;

        public TimeTrigger_FiveYear_Data(ILoggerFactory loggerFactory, ITimeService timeService)
        {
            _logger = loggerFactory.CreateLogger<TimeTrigger_FiveYear_Data>();
            _timeService = timeService;
           
        }

      //*/5 * * * * *


        [Function("TimeTrigger_FiveYear_Data")]
        public void Run([TimerTrigger("0 0 12 * * 1", RunOnStartup = true)] TimerInfo myTimer)
        {
            try
            {
                // This triggers every Monday at 12:00 PM
                _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

                if (myTimer.ScheduleStatus is not null)
                {
                    _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
                }

                // Call the delete method via the service
                _timeService.DeleteOldData();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred during execution: {ex}");
                throw; // Rethrow the exception to ensure proper error handling by Azure Functions runtime
            }
        }


















    }
}
