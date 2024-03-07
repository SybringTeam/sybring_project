using sybring_project.Models;
using System.Collections.Generic;

namespace sybring_project.ViewModels
{
    public class TimeReportViewModel
    {
        

        public string UserEmail { get; set; }


        // Week data
        public List<DayData> WeekData { get; set; }


        // Total work hours for the day
        public decimal? TotalWorkHours { get; set; } // Nullable decimal to allow for cases where user enters total hours directly


        // Constructor to initialize WeekData
        public TimeReportViewModel()
        {
            WeekData = new List<DayData>();

        }

    }
}
