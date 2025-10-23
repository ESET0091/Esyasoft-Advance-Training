using MediBuddy.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediBuddy.Data.Entities
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public int? AppointmentId { get; set; }

        public DateTime PrescriptionDate { get; set; } = DateTime.UtcNow;

        [StringLength(1000)]
        public string Instructions { get; set; }

        public DateTime? NextVisitDate { get; set; }

        // Navigation properties
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public Appointment Appointment { get; set; }
        public ICollection<PrescriptionMedicine> PrescriptionMedicines { get; set; }
    }
}

public class PrescriptionMedicine
{
    [Key]
    public int PrescriptionMedicineId { get; set; }

    [Required]
    public int PrescriptionId { get; set; }

    [Required]
    [StringLength(100)]
    public string MedicineName { get; set; }

    [StringLength(50)]
    public string Dosage { get; set; }

    [StringLength(100)]
    public string Frequency { get; set; }

    public int Duration { get; set; } // in days

    [StringLength(500)]
    public string Instructions { get; set; }

    // Navigation property
   // public Prescription Prescription { get; set; }
}
