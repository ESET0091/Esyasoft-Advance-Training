//using System;
//using System.ComponentModel.DataAnnotations;

//namespace MediBuddy.Data.Entities
//{
//    public class MedicalRecord
//    {
//        [Key]
//        public int MedicalRecordId { get; set; }

//        [Required]
//        public int PatientId { get; set; }

//        public int? AppointmentId { get; set; }

//        [Required]
//        public int DoctorId { get; set; }

//        public DateTime RecordDate { get; set; } = DateTime.UtcNow;

//        [StringLength(200)]
//        public string Diagnosis { get; set; }

//        public string Symptoms { get; set; }

//        public string Treatment { get; set; }

//        public string Notes { get; set; }

//        public string TestsPerformed { get; set; }

//        public string TestResults { get; set; }

//        // Navigation properties
//        //public Patient Patient { get; set; }
//        //public Doctor Doctor { get; set; } // This should reference Doctor, not User
//        //public Appointment Appointment { get; set; }
//    }
//}