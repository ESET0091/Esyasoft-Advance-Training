using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace MediBuddy.Data.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [StringLength(10)]
        public string Gender { get; set; }

        public string Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        //public Patient Patient { get; set; }
        //public Doctor Doctor { get; set; }
    }
}