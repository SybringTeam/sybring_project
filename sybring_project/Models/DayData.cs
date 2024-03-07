using sybring_project.ViewModels;
using System;


namespace sybring_project.Models
{
    public class DayData
    {
        public string Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan LunchStart { get; set; } 
        public TimeSpan LunchEnd { get; set; } 
        public TimeSpan EndTime { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal TotalWorkHours { get; set; }
      public decimal MaxRegularHoursPerDay { get; set; } 
    }

}

