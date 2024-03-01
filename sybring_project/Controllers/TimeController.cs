using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class TimeController : Controller
    {

        private readonly ITimeService _timeService;
        private readonly ApplicationDbContext _context;

        public TimeController(ApplicationDbContext context, ITimeService timeService)
        {

            _timeService = timeService;
            _context = context;


        }

        public IActionResult Index()
        {
            // Retring time history  from the database
            var timeHistories = _context.TimeHistories.ToList(); 

            return View(timeHistories);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]    
        public async Task<IActionResult> Create([Bind("DateTime")] TimeHistory timeHistory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                   
                    await _timeService.AddTimeHistoryAsync(timeHistory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                   
                    ModelState.AddModelError("", "An error occurred while saving the time history record.");
                }
            }
            
            return View(timeHistory);
        }




        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeHistory = await _context.TimeHistories.FirstOrDefaultAsync(m => m.Id == id);

            if (timeHistory == null)
            {
                return NotFound();
            }

            return View(timeHistory);
        }







    }
}
