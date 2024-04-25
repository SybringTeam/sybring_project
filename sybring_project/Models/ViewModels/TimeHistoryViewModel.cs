﻿using Microsoft.AspNetCore.Mvc.Rendering;
using sybring_project.Models.Db;

namespace sybring_project.Models.ViewModels
{
    public class TimeHistoryViewModel
    {
        public SelectList UserList { get; set; }
        public SelectList DateRanges { get; set; }
        public IEnumerable<TimeHistory> TimeHistories { get; set; }
    }
}