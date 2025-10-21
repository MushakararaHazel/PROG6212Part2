using CMCS.Data;
using CMCS.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CMCS.Services
{
    public class ClaimService : IClaimService
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ClaimService(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<Claim> SubmitClaimAsync(Claim claim, List<IFormFile>? files, CancellationToken ct = default)
        {
            _db.Claims.Add(claim);
            await _db.SaveChangesAsync(ct);

            if (files != null && files.Count > 0)
            {
                string uploadDir = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadDir);

                foreach (var file in files)
                {
                    string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                    string filePath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await file.CopyToAsync(stream, ct);

                    var doc = new SupportingDocument
                    {
                        FileName = file.FileName,
                        FilePath = $"/uploads/{fileName}",
                        ClaimId = claim.Id
                    };
                    _db.SupportingDocuments.Add(doc);
                }

                await _db.SaveChangesAsync(ct);
            }

            return claim;
        }

        public Task<List<Claim>> GetMyClaimsAsync(CancellationToken ct = default) =>
            _db.Claims.Include(c => c.Documents)
                      .OrderByDescending(c => c.CreatedUtc)
                      .ToListAsync(ct);

        public Task<List<Claim>> GetPendingAsync(CancellationToken ct = default) =>
            _db.Claims.Include(c => c.Documents)
                      .Where(c => c.Status == ClaimStatus.Pending)
                      .OrderByDescending(c => c.CreatedUtc)
                      .ToListAsync(ct);
        public async Task<SupportingDocument?> GetDocumentAsync(int id, CancellationToken ct = default)
        {
            return await _db.SupportingDocuments.FindAsync(new object[] { id }, ct);
        }

        public async Task ApproveAsync(int id, CancellationToken ct = default)
        {
            var claim = await _db.Claims.FindAsync(new object[] { id }, ct);
            if (claim == null) return;
            claim.Status = ClaimStatus.Approved;
            await _db.SaveChangesAsync(ct);
        }

        public async Task RejectAsync(int id, CancellationToken ct = default)
        {
            var claim = await _db.Claims.FindAsync(new object[] { id }, ct);
            if (claim == null) return;
            claim.Status = ClaimStatus.Rejected;
            await _db.SaveChangesAsync(ct);
        }
        public async Task<Claim?> GetClaimByIdAsync(int id, CancellationToken ct = default)
        {
            return await _db.Claims
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task DeleteClaimAsync(int id, CancellationToken ct = default)
        {
            var claim = await _db.Claims.Include(c => c.Documents)
                                        .FirstOrDefaultAsync(c => c.Id == id, ct);

            if (claim != null)
            {
                // Optional: delete uploaded files from disk too
                if (claim.Documents != null)
                {
                    foreach (var doc in claim.Documents)
                    {
                        var filePath = Path.Combine(_env.WebRootPath, doc.FilePath);

                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                _db.Claims.Remove(claim);
                await _db.SaveChangesAsync(ct);
            }
        }

    }
}



