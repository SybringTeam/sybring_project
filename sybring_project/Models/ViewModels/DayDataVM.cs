﻿using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    
    public class DayDataVM
    {
        public DateTime Date { get; set; }
        //public DateTime Schedule { get; set; } 
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
        public ICollection<ProjectTimeReport> ProjectHistories { get; set; }
        public ICollection<User> Users { get; set; }

        public bool IsRedDay { get; set; }

    }

}
