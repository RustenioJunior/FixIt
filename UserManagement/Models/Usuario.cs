using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        public string? Token { get; set; }
    }
}