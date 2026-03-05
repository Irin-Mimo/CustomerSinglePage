using CustomerSinglePage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerSinglePage.Controllers
{
    public class DealersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DealersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index page
        public async Task<IActionResult> Index()
        {
            ViewBag.MarketingList = await _context.Marketings.ToListAsync();
            return View();
        }

        // Save (Insert/Update)
        [HttpPost]
        public async Task<IActionResult> Save(Dealer dealer)
        {
            if (dealer.DealerId > 0)
            {
                var existing = await _context.Dealers.FindAsync(dealer.DealerId);
                if (existing != null)
                {
                    existing.Name = dealer.Name;
                    existing.Address = dealer.Address;
                    existing.Phone = dealer.Phone;
                    existing.DueAmount = dealer.DueAmount;
                    existing.MarketingId = dealer.MarketingId;
                    existing.Date = dealer.Date;
                }
            }
            else
            {
                _context.Dealers.Add(dealer);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // Get single Dealer
        [HttpGet]
        public async Task<IActionResult> GetDealer(int id)
        {
            var dealer = await _context.Dealers.FindAsync(id);
            if (dealer == null) return NotFound();

            return Json(new
            {
                dealerId = dealer.DealerId,
                name = dealer.Name,
                phone = dealer.Phone,
                address = dealer.Address,
                dueAmount = dealer.DueAmount,
                marketingId = dealer.MarketingId,
                date = dealer.Date
            });
        }

        // Delete Dealer
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dealer = await _context.Dealers.FindAsync(id);
            if (dealer == null) return NotFound();

            _context.Dealers.Remove(dealer);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // Pagination + Search + Sorting
        [HttpGet]
        public async Task<IActionResult> GetPaged(string search = "", int page = 1, int pageSize = 5, string sortColumn = "Name", bool sortAsc = true)
        {
            var query = _context.Dealers.Include(d => d.Marketing)
                        .Select(d => new DealerDTO
                        {
                            DealerId = d.DealerId,
                            Name = d.Name,
                            Address = d.Address,
                            Phone = d.Phone,
                            DueAmount = d.DueAmount,
                            MarketingId = d.MarketingId,
                            MarketingName = d.Marketing.Name,
                            Date = d.Date
                        });

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.Name.Contains(search) || d.Address.Contains(search) || d.Phone.Contains(search));
            }

            // Sorting
            query = (sortColumn, sortAsc) switch
            {
                ("Name", true) => query.OrderBy(d => d.Name),
                ("Name", false) => query.OrderByDescending(d => d.Name),
                _ => query.OrderBy(d => d.Name)
            };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return Json(new
            {
                data,
                currentPage = page,
                totalPages
            });
        }
    }
}