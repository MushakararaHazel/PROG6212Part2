using CMCS.Data;
using CMCS.Models;
using CMCS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CMCS.Controllers
{
    public class CoordinatorController : Controller
    {
            private readonly AppDbContext _db;
            public CoordinatorController(AppDbContext db) => _db = db;

            public async Task<IActionResult> Pending()
            {
                var claims = await _db.Claims
                   
                    .Include(c => c.Documents)
                    .Where(c => c.Status == ClaimStatus.Pending)
                    .OrderByDescending(c => c.CreatedUtc)
                    .ToListAsync();

                return View(claims);
            }

            [HttpPost]
            public async Task<IActionResult> Approve(int id)
            {
                var claim = await _db.Claims.FindAsync(id);
                if (claim == null) return NotFound();

                claim.Status = ClaimStatus.Approved;
                await _db.SaveChangesAsync();

                return RedirectToAction("Pending");
            }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
           

            await _db.SaveChangesAsync();

            return RedirectToAction("Pending");
        }



    }
}
