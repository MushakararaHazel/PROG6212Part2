using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models.ViewModels
{
    public class SubmitClaimVm
    {
        [Required]
        public DateTime Month { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string LecturerId { get; set; } = string.Empty;

        [Required]
        public decimal HoursWorked { get; set; }

        [Required]
        public decimal HourlyRate { get; set; }

        public string Notes { get; set; } = string.Empty;

        public List<IFormFile>? FileUploads { get; set; }
    }
}

