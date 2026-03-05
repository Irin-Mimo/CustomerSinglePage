using CustomerSinglePage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerSinglePage.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==============================
        // Index
        // ==============================
        public async Task<IActionResult> Index()
        {
            ViewBag.MarketingList = await _context.Marketings.ToListAsync();
            return View();
        }

        // ==============================
        // Save (Insert / Update)
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Save(Shop shop)
        {
            if (shop.Id > 0)
            {
                var existing = await _context.Shops.FindAsync(shop.Id);
                if (existing == null)
                    return NotFound();

                existing.ShopName = shop.ShopName;
                existing.OwnerName = shop.OwnerName;
                existing.Phone = shop.Phone;
                existing.Address = shop.Address;
                existing.MarketingId = shop.MarketingId;
                existing.Date = shop.Date;
            }
            else
            {
                if (shop.Date == default) // null বা 0001-01-01
                    shop.Date = DateTime.Parse(Request.Form["date"]); // input থেকে নাও
                _context.Shops.Add(shop);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // ==============================
        // Get Single Shop
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetShop(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
                return NotFound();

            return Json(new
            {
                shopId = shop.Id,
                shopName = shop.ShopName,
                ownerName = shop.OwnerName,
                phone = shop.Phone,
                address = shop.Address,
                marketingId = shop.MarketingId,
                date = shop.Date
            });
        }

        // ==============================
        // Delete
        // ==============================
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
                return NotFound();

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // ==============================
        // Pagination + Search + Sorting
        // ==============================
        [HttpGet]
        public async Task<IActionResult> GetPaged(
        string search = "",
        int page = 1,
        int pageSize = 5,
        string sortColumn = "ShopName",
        bool sortAsc = true)
        {
            var query = _context.Shops
                .Include(s => s.Marketing)
                .Select(s => new ShopDTO
                {
                    ShopId = s.Id,
                    ShopName = s.ShopName,
                    OwnerName = s.OwnerName,
                    Phone = s.Phone,
                    Address = s.Address,
                    Date = s.Date,
                    MarketingId = s.MarketingId,
                    MarketingName = s.Marketing!.Name,
                    MarketingColor = s.Marketing!.Color   // 👈 ADD THIS
                });

            // 🔎 Search
           
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();

                query = query.Where(s =>
                    s.ShopName.ToLower().Contains(search) ||
                    s.OwnerName.ToLower().Contains(search) ||
                    s.Phone.ToLower().Contains(search) ||
                    s.Address.ToLower().Contains(search) ||
                    s.MarketingName.ToLower().Contains(search)   // 👈 ADD THIS
                );
            }

            // 🔥 Sorting
            query = (sortColumn, sortAsc) switch
            {
                ("ShopName", true) => query.OrderBy(s => s.ShopName),
                ("ShopName", false) => query.OrderByDescending(s => s.ShopName),
                _ => query.OrderBy(s => s.ShopName)
            };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Json(new
            {
                data,
                currentPage = page,
                totalPages
            });
        }
    }
}