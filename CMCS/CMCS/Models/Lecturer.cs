using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
namespace CMCS.Models
{
    public class Lecturer
    {

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        
        public List<Claim> Claims { get; set; } = new();
    }
}
