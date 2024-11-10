using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class User
    {
        [Key] // Marks this as the primary key
        public string UserId { get; set; } // Unique identifier from Auth0
        
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}

