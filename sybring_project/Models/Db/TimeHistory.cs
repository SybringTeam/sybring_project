﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sybring_project.Models.Db
{
    public class TimeHistory
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public DateTime Schedule { get; set; }

        [Required]

        public TimeSpan StartWork { get; set; }

        [Required]
        public TimeSpan EndWork { get; set; }

        [Required]
        public TimeSpan StartBreak { get; set; }

        [Required]
        public TimeSpan EndBreak { get; set; }


        public decimal TotalWorkingHours { get; set; }

        [Required]
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

        // Represents many-to-many relationship with Project
        public virtual ICollection<ProjectTimeReport> ProjectHistories { get; set; }

        // Represents many-to-many relationship with User
        public virtual ICollection<User> Users { get; set; }




    }
}
