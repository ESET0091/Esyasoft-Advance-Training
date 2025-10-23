
//namespace SchoolManagementSystem
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}

/*

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Entities;
using SchoolManagementSystem.Services;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add DbContext with SQL Server
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IStudentService, StudentService>();

// Add CORS (for frontend applications)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});



List<Student> students = new List<Student>
    {
    new Student { StudentId = 4, FirstName = "Gopal", LastName = "Singh",
        Email = "Gopal@gmail.com", DateOfBirth = new DateTime(2024, 3, 15, 14, 30, 0),
        EnrollmentDate = new DateTime(2024, 3, 15, 14, 30, 0) },
    new Student { StudentId = 5, FirstName = "Mantu", LastName = "Kumar",
        Email = "Mantu@gmail.com", DateOfBirth = new DateTime(2025, 3, 15, 14, 29, 0),
        EnrollmentDate = new DateTime(2025, 3, 12, 14, 30, 0) },
    new Student { StudentId = 6, FirstName = "sahil", LastName = "khan",
        Email = "Sahil@gmail.com", DateOfBirth = new DateTime(2023, 3, 15, 14, 29, 0),
        EnrollmentDate = new DateTime(2023, 3, 12, 14, 30, 0) }
};

//foreach (var student in students)
//{
//    Console.WriteLine(student.StudentId);
//    Console.WriteLine(student.FirstName);
//}

//var allStudents = from student in students
//select student;


// Or using method syntax:
//var allStudents = students.Select(s => s);


// Order by
//var higherId = from student in students
//               where student.StudentId>4
//               orderby student.FirstName
//               select student;
//foreach(var id in higherId)
//{
//    Console.WriteLine(id.FirstName);
//    Console.WriteLine(id.LastName);
//}

//var orderedByEnrollment = from student in students
//                          orderby student.EnrollmentDate
//                          select student;

//Console.WriteLine("\n Students Ordered by Enrollment Date:");
//foreach (var student in orderedByEnrollment)
//{
//    Console.WriteLine($"Name: {student.FirstName}, Enrollment: {student.EnrollmentDate}");
//}


//var groupedById = from student in students
//                  group student by student.StudentId into studentGroup
//                  select new
//                  {
//                      StudentId = studentGroup.Key,
//                      Students = studentGroup.ToList()
//                  };

//Console.WriteLine("\n Students Grouped by ID:");
//foreach (var group in groupedById)
//{
//    Console.WriteLine($"Student ID: {group.StudentId}, Count: {group.Students.Count}");
//    foreach (var student in group.Students)
//    {
//        Console.WriteLine($"  - {student.FirstName} {student.LastName}");
//    }
//}

//var filteredStudents = from student in students
//                       where student.StudentId == 5 &&
//                             student.EnrollmentDate.Year == 2025 &&
//                             student.Email.Contains("gmail.com")
//                       select student;

//Console.WriteLine("\n Complex Filtered Students:");
//foreach (var student in filteredStudents)
//{
//    Console.WriteLine($"Name: {student.FirstName}, ID: {student.StudentId}, Email: {student.Email}");
//}


//bool hasStudentsFrom2024 = students.Any(s => s.EnrollmentDate.Year == 2024);
//bool allHaveGmail = students.All(s => s.Email.EndsWith("@gmail.com"));

//Console.WriteLine("\n Any/All Checks:");
//Console.WriteLine($"Has students from 2024: {hasStudentsFrom2024}");
//Console.WriteLine($"All students have Gmail: {allHaveGmail}");


//var firstStudent = students.FirstOrDefault();
//var specificStudent = students.FirstOrDefault(s => s.StudentId == 4);

//Console.WriteLine("\n FirstOrDefault Examples:");
//Console.WriteLine($"First Student: {firstStudent?.FirstName} {firstStudent?.LastName}");
//Console.WriteLine($"Student with ID 4: {specificStudent?.FirstName} {specificStudent?.LastName}");


// IN - Check if value exists in a list
//List<int> validStudentIds = new List<int> { 4, 5, 6 };
//var inQuery = from student in students
//              where validStudentIds.Contains(student.StudentId)
//              select student;

//Console.WriteLine("\n2. IN - Students with ID in [4,5,6]:");
//foreach (var student in inQuery)
//{
//    Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName}");
//}

//// NOT IN - Check if value doesn't exist in a list
//List<string> excludedDomains = new List<string> { "yahoo.com", "hotmail.com" };
//var notInQuery = from student in students
//                 where !excludedDomains.Any(domain => student.Email.EndsWith(domain))
//                 select student;

//Console.WriteLine("\nNOT IN - Students not using yahoo or hotmail:");
//foreach (var student in notInQuery)
//{
//    Console.WriteLine($"Name: {student.FirstName}, Email: {student.Email}");
//}


//// Subquery in WHERE clause
//var subqueryWhere = from student in students
//                    where student.StudentId == (
//                        from s in students
//                        where s.FirstName == "Gopal"
//                        select s.StudentId
//                    ).FirstOrDefault()
//                    select student;

//Console.WriteLine("\n4. SUBQUERY - Students with same ID as Gopal:");
//foreach (var student in subqueryWhere)
//{
//    Console.WriteLine($"Name: {student.FirstName}, ID: {student.StudentId}");
//}

//// Subquery with ANY
//var subqueryAny = from student in students
//                  where students.Any(s => s.StudentId == 5 && s.EnrollmentDate.Year > 2023)
//                  select student;

//Console.WriteLine("\nSUBQUERY with ANY - Students when there's any student with ID 5 enrolled after 2023:");
//foreach (var student in subqueryAny)
//{
//    Console.WriteLine($"Name: {student.FirstName}");
//}

//// Subquery to find students enrolled after the earliest enrollment
//var subqueryAggregate = from student in students
//                        where student.EnrollmentDate > (
//                            from s in students
//                            select s.EnrollmentDate
//                        ).Min()
//                        select student;

//Console.WriteLine("\nSUBQUERY Aggregate - Students enrolled after the earliest enrollment:");
//foreach (var student in subqueryAggregate)
//{
//    Console.WriteLine($"Name: {student.FirstName}, Enrollment: {student.EnrollmentDate}");
//}




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    context.Database.Migrate();
}

app.Run();

*/

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Entities;
using SchoolManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add DbContext with SQL Server
builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity Configuration
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // User settings
    options.User.RequireUniqueEmail = true;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddEntityFrameworkStores<SchoolDbContext>()
.AddDefaultTokenProviders();

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new Exception("JWT Key not configured")))
    };
});

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireTeacherRole", policy => policy.RequireRole("Teacher", "Admin"));
    options.AddPolicy("RequireStudentRole", policy => policy.RequireRole("Student", "Teacher", "Admin"));
});

// Register services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

// Authentication & Authorization MIDDLEWARE
app.UseAuthentication(); // This must come before UseAuthorization
app.UseAuthorization();

app.MapControllers();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    context.Database.Migrate();
}

app.Run();