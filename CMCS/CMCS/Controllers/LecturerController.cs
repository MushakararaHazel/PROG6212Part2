using Microsoft.AspNetCore.Mvc;
using CMCS.Models;
using CMCS.Models.ViewModels;
using CMCS.Services;


namespace CMCS.Controllers
{
    public class LecturerController : Controller
    {
        private readonly IClaimService _service;
        public LecturerController(IClaimService service) => _service = service;

        public IActionResult Submit() => View(new SubmitClaimVm());

        [HttpPost, ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> Submit(SubmitClaimVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            try
            {
                var claim = new Claim
                {
                    LecturerName = vm.LecturerName,
                    LecturerId = vm.LecturerId,
                    Month = vm.Month,
                    HoursWorked = vm.HoursWorked,
                    HourlyRate = vm.HourlyRate,
                    Notes = vm.Notes,
                    Status = ClaimStatus.Pending
                };

                await _service.SubmitClaimAsync(claim, vm.FileUploads);
                TempData["Success"] = "Claim submitted successfully!";
                return RedirectToAction("Track");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Track(string? searchLecturerId = null)
        {
            var claims = await _service.GetMyClaimsAsync();

            if (!string.IsNullOrEmpty(searchLecturerId))
            {
                claims = claims
                    .Where(c => c.LecturerId.Contains(searchLecturerId, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(claims);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var claim = await _service.GetClaimByIdAsync(id);
            if (claim == null) return NotFound();

            await _service.DeleteClaimAsync(id);
            TempData["Success"] = "Claim deleted successfully.";
            return RedirectToAction("Track");
        }

    }
}

