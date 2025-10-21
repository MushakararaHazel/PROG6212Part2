using CMCS.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CMCS.Services
{
    public interface IClaimService
    {
        Task<Claim> SubmitClaimAsync(Claim claim, List<IFormFile>? files, CancellationToken ct = default);
        Task<List<Claim>> GetMyClaimsAsync(CancellationToken ct = default);
        Task<SupportingDocument?> GetDocumentAsync(int id, CancellationToken ct = default);

        Task<List<Claim>> GetPendingAsync(CancellationToken ct = default);
        Task ApproveAsync(int id, CancellationToken ct = default);
        Task RejectAsync(int id, CancellationToken ct = default);
        Task<Claim?> GetClaimByIdAsync(int id, CancellationToken ct = default);
        Task DeleteClaimAsync(int id, CancellationToken ct = default);
    }
}
