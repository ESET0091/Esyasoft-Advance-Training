//using Microsoft.EntityFrameworkCore;
//using SchoolManagementSystem.Entities;

//namespace SchoolManagementSystem.Data
//{
//    public class SchoolDbContext : DbContext
//    {
//        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
//        {
//        }

//        // DbSets represent tables in the database
//        public DbSet<Student> Students { get; set; }
//        public DbSet<Course> Courses { get; set; }
//        public DbSet<Enrollment> Enrollments { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // Configure entities using Fluent API
//            ConfigureStudentEntity(modelBuilder);
//            ConfigureCourseEntity(modelBuilder);
//            ConfigureEnrollmentEntity(modelBuilder);

//            // Seed initial data
//            SeedData(modelBuilder);
//        }

//        private void ConfigureStudentEntity(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Student>(entity =>
//            {
//                // Primary Key
//                entity.HasKey(s => s.StudentId);

//                // Properties configuration
//                entity.Property(s => s.FirstName)
//                    .IsRequired()
//                    .HasMaxLength(100);

//                entity.Property(s => s.LastName)
//                    .IsRequired()
//                    .HasMaxLength(100);

//                entity.Property(s => s.Email)
//                    .IsRequired()
//                    .HasMaxLength(150);

//                entity.Property(s => s.DateOfBirth)
//                    .IsRequired();

//                entity.Property(s => s.EnrollmentDate)
//                    .IsRequired();

//                // Index for better query performance
//                entity.HasIndex(s => s.Email)
//                    .IsUnique();

//                // Table name
//                entity.ToTable("Students");
//            });
//        }

//        private void ConfigureCourseEntity(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Course>(entity =>
//            {
//                // Primary Key
//                entity.HasKey(c => c.CourseId);

//                // Properties configuration
//                entity.Property(c => c.CourseCode)
//                    .IsRequired()
//                    .HasMaxLength(20);

//                entity.Property(c => c.CourseName)
//                    .IsRequired()
//                    .HasMaxLength(150);

//                entity.Property(c => c.Description)
//                    .HasMaxLength(500);

//                entity.Property(c => c.Credits)
//                    .IsRequired();

//                // Unique constraint on CourseCode
//                entity.HasIndex(c => c.CourseCode)
//                    .IsUnique();

//                // Table name
//                entity.ToTable("Courses");
//            });
//        }

//        private void ConfigureEnrollmentEntity(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Enrollment>(entity =>
//            {
//                // Primary Key
//                entity.HasKey(e => e.EnrollmentId);

//                // Foreign Key relationships
//                entity.HasOne(e => e.Student)
//                    .WithMany(s => s.Enrollments)
//                    .HasForeignKey(e => e.StudentId)
//                    .OnDelete(DeleteBehavior.Cascade);

//                entity.HasOne(e => e.Course)
//                    .WithMany(c => c.Enrollments)
//                    .HasForeignKey(e => e.CourseId)
//                    .OnDelete(DeleteBehavior.Cascade);

//                // Properties configuration
//                entity.Property(e => e.EnrollmentDate)
//                    .IsRequired();

//                entity.Property(e => e.Grade)
//                    .HasMaxLength(2);

//                // Composite unique constraint - a student can't enroll in same course multiple times
//                entity.HasIndex(e => new { e.StudentId, e.CourseId })
//                    .IsUnique();

//                // Table name
//                entity.ToTable("Enrollments");
//            });
//        }

//        private void SeedData(ModelBuilder modelBuilder)
//        {
//            // Seed Students
//            var students = new[]
//            {
//                new Student {
//                    StudentId = 1,
//                    FirstName = "John",
//                    LastName = "Doe",
//                    Email = "john.doe@school.com",
//                    DateOfBirth = new DateTime(2000, 1, 15),
//                    EnrollmentDate = new DateTime(2023, 9, 1)
//                },
//                new Student {
//                    StudentId = 2,
//                    FirstName = "Jane",
//                    LastName = "Smith",
//                    Email = "jane.smith@school.com",
//                    DateOfBirth = new DateTime(1999, 5, 20),
//                    EnrollmentDate = new DateTime(2023, 9, 1)
//                },
//                new Student {
//                    StudentId = 3,
//                    FirstName = "Mike",
//                    LastName = "Johnson",
//                    Email = "mike.johnson@school.com",
//                    DateOfBirth = new DateTime(2001, 8, 10),
//                    EnrollmentDate = new DateTime(2023, 9, 1)
//                }
//            };

//            // Seed Courses
//            var courses = new[]
//            {
//                new Course {
//                    CourseId = 1,
//                    CourseCode = "MATH101",
//                    CourseName = "Mathematics",
//                    Description = "Basic Mathematics",
//                    Credits = 3
//                },
//                new Course {
//                    CourseId = 2,
//                    CourseCode = "PHY101",
//                    CourseName = "Physics",
//                    Description = "Basic Physics",
//                    Credits = 4
//                },
//                new Course {
//                    CourseId = 3,
//                    CourseCode = "CHEM101",
//                    CourseName = "Chemistry",
//                    Description = "Basic Chemistry",
//                    Credits = 4
//                },
//                new Course {
//                    CourseId = 4,
//                    CourseCode = "BIO101",
//                    CourseName = "Biology",
//                    Description = "Basic Biology",
//                    Credits = 4
//                }
//            };

//            // Seed Enrollments
//            var enrollments = new[]
//            {
//                new Enrollment {
//                    EnrollmentId = 1,
//                    StudentId = 1,
//                    CourseId = 1,
//                    EnrollmentDate = new DateTime(2023, 9, 1),
//                    Grade = "A"
//                },
//                new Enrollment {
//                    EnrollmentId = 2,
//                    StudentId = 1,
//                    CourseId = 2,
//                    EnrollmentDate = new DateTime(2023, 9, 1),
//                    Grade = "B"
//                },
//                new Enrollment {
//                    EnrollmentId = 3,
//                    StudentId = 2,
//                    CourseId = 1,
//                    EnrollmentDate = new DateTime(2023, 9, 1),
//                    Grade = "A-"
//                },
//                new Enrollment {
//                    EnrollmentId = 4,
//                    StudentId = 3,
//                    CourseId = 3,
//                    EnrollmentDate = new DateTime(2023, 9, 1),
//                    Grade = "B+"
//                },
//                new Enrollment {
//                    EnrollmentId = 5,
//                    StudentId = 2,
//                    CourseId = 4,
//                    EnrollmentDate = new DateTime(2023, 9, 1),
//                    Grade = "A"
//                }
//            };

//            modelBuilder.Entity<Student>().HasData(students);
//            modelBuilder.Entity<Course>().HasData(courses);
//            modelBuilder.Entity<Enrollment>().HasData(enrollments);
//        }
//    }
//}





// IdentityDbContext<User>

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Entities;

namespace SchoolManagementSystem.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        // DbSets represent tables in the database
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entities using Fluent API
            ConfigureStudentEntity(modelBuilder);
            ConfigureCourseEntity(modelBuilder);
            ConfigureEnrollmentEntity(modelBuilder);

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void ConfigureStudentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                // Primary Key
                entity.HasKey(s => s.StudentId);

                // Properties configuration
                entity.Property(s => s.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(s => s.DateOfBirth)
                    .IsRequired();

                entity.Property(s => s.EnrollmentDate)
                    .IsRequired();

                // Index for better query performance
                entity.HasIndex(s => s.Email)
                    .IsUnique();

                // Table name
                entity.ToTable("Students");
            });
        }

        private void ConfigureCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                // Primary Key
                entity.HasKey(c => c.CourseId);

                // Properties configuration
                entity.Property(c => c.CourseCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(c => c.CourseName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(c => c.Description)
                    .HasMaxLength(500);

                entity.Property(c => c.Credits)
                    .IsRequired();

                // Unique constraint on CourseCode
                entity.HasIndex(c => c.CourseCode)
                    .IsUnique();

                // Table name
                entity.ToTable("Courses");
            });
        }

        private void ConfigureEnrollmentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>(entity =>
            {
                // Primary Key
                entity.HasKey(e => e.EnrollmentId);

                // Foreign Key relationships
                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Enrollments)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Properties configuration
                entity.Property(e => e.EnrollmentDate)
                    .IsRequired();

                entity.Property(e => e.Grade)
                    .HasMaxLength(2);

                // Composite unique constraint - a student can't enroll in same course multiple times
                entity.HasIndex(e => new { e.StudentId, e.CourseId })
                    .IsUnique();

                // Table name
                entity.ToTable("Enrollments");
            });
        }

        private void SeedData(ModelBuilder modelBuilder)
        {

            // Create Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                }
            );

            // Create Admin User
            var hasher = new PasswordHasher<User>();
            var adminUser = new User
            {
                Id = "1",
                UserName = "admin@school.com",
                NormalizedUserName = "ADMIN@SCHOOL.COM",
                Email = "admin@school.com",
                NormalizedEmail = "ADMIN@SCHOOL.COM",
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "User",
                SecurityStamp = "12345678 - 1234 - 1234 - 1234 - 123456789abc"
            };
            var hashedPassword = hasher.HashPassword(adminUser, "Admin@123");

            adminUser.PasswordHash = hashedPassword;

            modelBuilder.Entity<User>().HasData(adminUser);

            // Assign Admin Role to Admin User
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "1", RoleId = "1" }
            );

            // Seed Students
            var dob = new DateTime(2000, 1, 15);
            var Enroll = new DateTime(2023, 9, 1);
            var dob1 = new DateTime(1999, 5, 20);
            var Enroll1 = new DateTime(2023, 9, 1);
            var students = new[]
            {
                new Student {
                    StudentId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@school.com",
                    DateOfBirth = dob,
                    EnrollmentDate = Enroll
                },
                new Student {
                    StudentId = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@school.com",
                    DateOfBirth = dob1,
                    EnrollmentDate = Enroll1
                },
                new Student {
                    StudentId = 3,
                    FirstName = "Mike",
                    LastName = "Johnson",
                    Email = "mike.johnson@school.com",
                    DateOfBirth = dob1,
                    EnrollmentDate = Enroll1
                }
            };

            // Seed Courses
            var courses = new[]
            {
                new Course {
                    CourseId = 1,
                    CourseCode = "MATH101",
                    CourseName = "Mathematics",
                    Description = "Basic Mathematics",
                    Credits = 3
                },
                new Course {
                    CourseId = 2,
                    CourseCode = "PHY101",
                    CourseName = "Physics",
                    Description = "Basic Physics",
                    Credits = 4
                },
                new Course {
                    CourseId = 3,
                    CourseCode = "CHEM101",
                    CourseName = "Chemistry",
                    Description = "Basic Chemistry",
                    Credits = 4
                },
                new Course {
                    CourseId = 4,
                    CourseCode = "BIO101",
                    CourseName = "Biology",
                    Description = "Basic Biology",
                    Credits = 4
                }
            };

            // Seed Enrollments
            var enrollments = new[]
            {
                new Enrollment {
                    EnrollmentId = 1,
                    StudentId = 1,
                    CourseId = 1,
                    EnrollmentDate = Enroll,
                    Grade = "A"
                },
                new Enrollment {
                    EnrollmentId = 2,
                    StudentId = 1,
                    CourseId = 2,
                    EnrollmentDate = Enroll1,
                    Grade = "B"
                },
                new Enrollment {
                    EnrollmentId = 3,
                    StudentId = 2,
                    CourseId = 1,
                    EnrollmentDate = Enroll,
                    Grade = "A-"
                },
                new Enrollment {
                    EnrollmentId = 4,
                    StudentId = 3,
                    CourseId = 3,
                    EnrollmentDate = Enroll1,
                    Grade = "B+"
                },
                new Enrollment {
                    EnrollmentId = 5,
                    StudentId = 2,
                    CourseId = 4,
                    EnrollmentDate = Enroll,
                    Grade = "A"
                }
            };

            modelBuilder.Entity<Student>().HasData(students);
            modelBuilder.Entity<Course>().HasData(courses);
            modelBuilder.Entity<Enrollment>().HasData(enrollments);
        }
        */
    }
}