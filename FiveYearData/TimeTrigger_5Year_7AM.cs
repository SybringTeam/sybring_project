using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FiveYearData
{
    public class TimeTrigger_5Year_7AM
    {
        private readonly ILogger _logger;

        public TimeTrigger_5Year_7AM(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimeTrigger_5Year_7AM>();
        }

        [Function("TimeTrigger_5Year_7AM")]
        public void Run([TimerTrigger("0 0 12 * * 1")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
