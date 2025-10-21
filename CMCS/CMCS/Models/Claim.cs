using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class Claim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Month { get; set; }

      
        public decimal HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }

        public string Notes { get; set; } = string.Empty;

        public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public ICollection<SupportingDocument>? Documents { get; set; }

        public string LecturerName { get; set; } = string.Empty;
        public string LecturerId { get; set; } = string.Empty;

       
        public bool IsPaid { get; set; } = false;

    }
}

