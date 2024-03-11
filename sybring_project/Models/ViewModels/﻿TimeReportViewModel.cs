using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class TimeReportViewModel
    {
        public DateTime Date { get; set; }


        public DateTime Schedule { get; set; }


     

        // Represents many-to-many relationship with Project
        public virtual ICollection<Project> ProjectId { get; set; }

        // Represents many-to-many relationship with User
        public virtual ICollection<User> Users { get; set; }



        // Week data
        public List<DayDataVM> WeekData { get; set; }




        // Constructor to initialize WeekData
        public TimeReportViewModel()
        {
            WeekData = new List<DayDataVM>();

        }
    }
}
