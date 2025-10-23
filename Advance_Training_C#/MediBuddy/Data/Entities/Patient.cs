using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediBuddy.Data.Entities
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(20)]
        public string BloodGroup { get; set; }

        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        [StringLength(500)]
        public string MedicalHistory { get; set; }

        [StringLength(500)]
        public string Allergies { get; set; }

        // Navigation properties
        //public User User { get; set; }
        //public ICollection<Appointment> Appointments { get; set; }
        //public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
