using MediBuddy.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace MediBuddy.Data.Context
{
    public class AplicationDBContext : DbContext
    {
        public AplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        // public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        //public DbSet<MedicalRecord> MedicalRecords { get; set; }
        //public DbSet<Prescription> Prescriptions { get; set; }
        //public DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var dob = new DateTime(1980, 1, 1);
            var CreatedAt = new DateTime(1985, 1, 1);
            modelBuilder.Entity<User>().HasData(
               new User
               {
                   UserId = 1,
                   FirstName = "John",
                   LastName = "Smith",
                   Email = "dr.john.smith@medibuddy.com",
                   PasswordHash = "hashed_password", // In real app, use proper hashing
                   PhoneNumber = "+1234567890",
                   DateOfBirth = dob,
                   Gender = "Male",
                   Address = "123 Medical Center, Healthcare City",
                   CreatedAt = CreatedAt
               }


           );
            var startTime = new TimeSpan(9, 0, 0);
            var endTime = new TimeSpan(17, 0, 0);

            modelBuilder.Entity<Doctor>().HasData(
               new Doctor
               {
                   DoctorId = 1,
                   UserId = 1,
                   Specialization = "Cardiology",
                   LicenseNumber = "MED123456",
                   YearsOfExperience = 15,
                   Qualifications = "MD, MBBS, FCPS",
                   Department = "Cardiology",
                   IsAvailable = true,
                   ConsultationStartTime = startTime,
                   ConsultationEndTime = endTime,
                   ConsultationFee = 100.00m
               }
           );

            modelBuilder.Entity<Patient>().HasData(
       new Patient
       {
           PatientId = 1,
           UserId = 1,
           BloodGroup = "A+",
           Height = 165.5m,
           Weight = 60.0m,
           MedicalHistory = "No significant medical history",
           Allergies = "None"
       }
   );

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    AppointmentId= 1,
                    PatientId =1,
                    DoctorId =1,
                    AppointmentDate = dob,
                    AppointmentTime = startTime,
                    Status = "Cancelled",
                    Reason =  "Not Listed",
                    Symptoms = "Fever",
                    CreatedAt = dob,
                    UpdatedAt = dob

                }
                );
















            // Configure relationships and constraints
            /*
                      // User - Patient (One-to-One)
                      modelBuilder.Entity<User>()
                          .HasOne(u => u.Patient)
                          .WithOne(p => p.User)
                          .HasForeignKey<Patient>(p => p.UserId)
                          .OnDelete(DeleteBehavior.Cascade);

                      // User - Doctor (One-to-One)
                      modelBuilder.Entity<User>()
                          .HasOne(u => u.Doctor)
                          .WithOne(d => d.User)
                          .HasForeignKey<Doctor>(d => d.UserId)
                          .OnDelete(DeleteBehavior.Cascade);

                      // Patient - Appointment (One-to-Many)
                      modelBuilder.Entity<Patient>()
                          .HasMany(p => p.Appointments)
                          .WithOne(a => a.Patient)
                          .HasForeignKey(a => a.PatientId)
                          .OnDelete(DeleteBehavior.Cascade);

                      // Doctor - Appointment (One-to-Many)
                      modelBuilder.Entity<Doctor>()
                          .HasMany(d => d.Appointments)
                          .WithOne(a => a.Doctor)
                          .HasForeignKey(a => a.DoctorId)
                          .OnDelete(DeleteBehavior.Restrict);

                      // Patient - MedicalRecord (One-to-Many)
                      modelBuilder.Entity<Patient>()
                          .HasMany(p => p.MedicalRecords)
                          .WithOne(mr => mr.Patient)
                          .HasForeignKey(mr => mr.PatientId)
                          .OnDelete(DeleteBehavior.Cascade);

                      // Doctor - MedicalRecord (One-to-Many) - FIXED THIS
                      modelBuilder.Entity<Doctor>()
                          .HasMany(d => d.MedicalRecords)
                          .WithOne(mr => mr.Doctor)
                          .HasForeignKey(mr => mr.DoctorId)
                          .OnDelete(DeleteBehavior.Restrict);

                      // Appointment - MedicalRecord (One-to-One)
                      modelBuilder.Entity<Appointment>()
                          .HasOne(a => a.MedicalRecord)
                          .WithOne(mr => mr.Appointment)
                          .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId)
                          .OnDelete(DeleteBehavior.SetNull);

                      // Prescription relationships
                      modelBuilder.Entity<Prescription>()
                          .HasOne(p => p.Patient)
                          .WithMany()
                          .HasForeignKey(p => p.PatientId)
                          .OnDelete(DeleteBehavior.NoAction);

                      modelBuilder.Entity<Prescription>()
                          .HasOne(p => p.Doctor)
                          .WithMany(d => d.Prescriptions)
                          .HasForeignKey(p => p.DoctorId)
                          .OnDelete(DeleteBehavior.Restrict);

                      modelBuilder.Entity<Prescription>()
                          .HasOne(p => p.Appointment)
                          .WithMany()
                          .HasForeignKey(p => p.AppointmentId)
                          .OnDelete(DeleteBehavior.SetNull);

                      modelBuilder.Entity<PrescriptionMedicine>()
                          .HasOne(pm => pm.Prescription)
                          .WithMany(p => p.PrescriptionMedicines)
                          .HasForeignKey(pm => pm.PrescriptionId)
                          .OnDelete(DeleteBehavior.Cascade);

                      // Seed data
                      SeedData(modelBuilder);
                  }

                  private void SeedData(ModelBuilder modelBuilder)
                  {
                      var dob = new DateTime(1980, 1, 1);
                      var CreatedAt = new DateTime(1985, 1, 1);


                      // Seed initial doctors
                      modelBuilder.Entity<User>().HasData(
                          new User
                          {
                              UserId = 1,
                              FirstName = "John",
                              LastName = "Smith",
                              Email = "dr.john.smith@medibuddy.com",
                              PasswordHash = "hashed_password", // In real app, use proper hashing
                              PhoneNumber = "+1234567890",
                              DateOfBirth = dob,
                              Gender = "Male",
                              Address = "123 Medical Center, Healthcare City",
                              CreatedAt = CreatedAt
                          }
                      );

                      var startTime = new TimeSpan(9, 0, 0);
                      var endTime = new TimeSpan(17, 0, 0);

                      modelBuilder.Entity<Doctor>().HasData(
                          new Doctor
                          {
                              DoctorId = 1,
                              UserId = 1,
                              Specialization = "Cardiology",
                              LicenseNumber = "MED123456",
                              YearsOfExperience = 15,
                              Qualifications = "MD, MBBS, FCPS",
                              Department = "Cardiology",
                              IsAvailable = true,
                              ConsultationStartTime = startTime,
                              ConsultationEndTime = endTime,
                              ConsultationFee = 100.00m
                          }
                      );
            */
        }
    }
}
