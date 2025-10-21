using CMCS.Data;
using CMCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    public class ManagerController : Controller
    {
        private readonly AppDbContext _db;
        public ManagerController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Approved()
        {
            var claims = await _db.Claims
               
                .Include(c => c.Documents)
                .Where(c => c.Status == ClaimStatus.Approved && !c.IsPaid)
                .ToListAsync();

            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveAndPay(int id)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.IsPaid = true;
            claim.Status = ClaimStatus.Approved;
            await _db.SaveChangesAsync();

            return RedirectToAction("Approved");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var claim = await _db.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;
           
            await _db.SaveChangesAsync();

            return RedirectToAction("Approved");
        }
    }
}
