namespace sybring_project.Models.ViewModels
{
    public class DayDataVM
    {
        public string Day { get; set; }

        public DateTime Schedule { get; set; } 

        public TimeSpan StartWork { get; set; }


        public TimeSpan EndWork { get; set; }


        public TimeSpan StartBreak { get; set; }


        public TimeSpan EndBreak { get; set; }


        public decimal TotalWorkingHours { get; set; }


        public decimal WorkingHours { get; set; }

        public decimal FlexiTime { get; set; }

        public decimal MoreTime { get; set; }

        public decimal AttendanceTime { get; set; }

        public decimal AnnualLeave { get; set; }

        public decimal SickLeave { get; set; }

        public decimal LeaveOfAbsence { get; set; }

        public decimal Childcare { get; set; }

        public decimal Overtime { get; set; }


        public decimal InconvenientHours { get; set; }
        public decimal MaxRegularHoursPerDay { get; set; }

        public DayDataVM()
        {
            // Set default start time to 08:00 AM
            StartWork = TimeSpan.FromHours(8);

            // Set default end time to 17:00 PM
            EndWork = TimeSpan.FromHours(17);

            // Set default start break time to 12:00 PM
            StartBreak = TimeSpan.FromHours(12);

            // Set default end break time to 13:00 PM
            EndBreak = TimeSpan.FromHours(13);
        }



    }
}
