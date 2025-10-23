using System;
using System.ComponentModel.DataAnnotations;

namespace MediBuddy.Data.Entities
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeSpan AppointmentTime { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled

        [StringLength(500)]
        public string Reason { get; set; }

        public string Symptoms { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        //public Patient Patient { get; set; }
        //public Doctor Doctor { get; set; }
        //public MedicalRecord MedicalRecord { get; set; }
    }
}
