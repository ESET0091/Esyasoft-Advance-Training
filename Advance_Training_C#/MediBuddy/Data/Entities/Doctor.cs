using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediBuddy.Data.Entities
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; }

        [StringLength(50)]
        public string LicenseNumber { get; set; }

        public int YearsOfExperience { get; set; }

        [StringLength(500)]
        public string Qualifications { get; set; }

        public string Department { get; set; }

        public bool IsAvailable { get; set; } = true;

        public TimeSpan ConsultationStartTime { get; set; }

        public TimeSpan ConsultationEndTime { get; set; }

        public decimal ConsultationFee { get; set; }

        // Navigation properties
        //public User User { get; set; }
        //public ICollection<Appointment> Appointments { get; set; }
        //public ICollection<Prescription> Prescriptions { get; set; }
        //public ICollection<MedicalRecord> MedicalRecords { get; set; } // Added this line
    }
}