using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMCS.Models
{
    public class SupportingDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty; 

        [ForeignKey("Claim")]
        public int ClaimId { get; set; }

        public Claim? Claim { get; set; }
    }
}
