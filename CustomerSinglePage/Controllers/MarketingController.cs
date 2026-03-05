using CustomerSinglePage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerSinglePage.Controllers
{
    public class MarketingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MarketingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Save (Insert/Update)
        [HttpPost]
        public async Task<IActionResult> Save(Marketing marketing)
        {
            if (marketing.MarketingId > 0)
            {
                var existing = await _context.Marketings.FindAsync(marketing.MarketingId);
                if (existing != null)
                {
                    existing.Name = marketing.Name;
                    existing.Address = marketing.Address;
                    existing.Phone = marketing.Phone;
                    existing.Color = marketing.Color;   // 👈 ADD
                }
            }
            else
            {
                // যদি user color select না করে, auto random দিবে
                if (string.IsNullOrEmpty(marketing.Color))
                {
                    marketing.Color = String.Format("#{0:X6}", new Random().Next(0x1000000));
                }

                _context.Marketings.Add(marketing);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // Get single Marketing
        [HttpGet]
        public async Task<IActionResult> GetMarketing(int id)
        {
            var marketing = await _context.Marketings.FindAsync(id);
            if (marketing == null) return NotFound();
            return Json(marketing);
        }

        // Delete Marketing
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marketing = await _context.Marketings.FindAsync(id);
            if (marketing == null) return NotFound();

            // Optional: delete related Dealers if needed
            var dealers = _context.Dealers.Where(d => d.MarketingId == id);
            _context.Dealers.RemoveRange(dealers);

            _context.Marketings.Remove(marketing);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // GetAll Marketing
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var marketings = await _context.Marketings
                                .Select(m => new
                                {
                                    m.MarketingId,
                                    m.Name,
                                    m.Address,
                                    m.Phone,
                                    m.Color     // 👈 ADD
                                })
                                .ToListAsync();

            return Json(marketings);
        }
    }
}