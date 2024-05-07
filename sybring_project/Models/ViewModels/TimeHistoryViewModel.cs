using Microsoft.AspNetCore.Mvc.Rendering;
using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class TimeHistoryViewModel
    {
        public SelectList UserList { get; set; }
        public virtual User? User { get; set; }
        public SelectList DateRanges { get; set; }
        public string CurrentUser { get; set; }
        public IEnumerable<TimeHistory> TimeHistories { get; set; }

        public Dictionary<string, List<TimeHistory>> TimeHistoriesByMonth { get; set; }
    }
}
