//// See https://aka.ms/new-console-template for more information
////int[] marks = new int[3];
////void student_details()
////{
////    Console.WriteLine("Enter your name: ");
////    string name = Console.ReadLine();

////}
////int[] take_std_details()
////{

////    Console.WriteLine("Enter all 3 subjects marks: ");
////    for (int i = 0; i < 3; i++)
////    marks[i] = Convert.ToInt32(Console.ReadLine());
////    return marks;

////}
////float average_marks( int no_of_sub)
////{
////    return total_marks(marks) / no_of_sub;
////}
////float total_marks(int[] marks)
////{
////    float sum = 0;
////    for(int i = 0; i < marks.Length; i++)
////        sum+= marks[i];
////    return sum;
////}

//using ConsoleApp2;

//float sum = 0, avg=0;
//int[] marks = new int[3];

//Student student = new Student("Gopal Singh");
//Student std= new Student();

//std.Name = "Mantu";
//Console.WriteLine(std.Name);
////student.student_details();
////marks = student.take_std_details();
////sum = student.total_marks(marks);
////Console.WriteLine($"Total Marks is: {sum}");
////avg = student.average_marks(3);
////Console.WriteLine($"Average mark is:{avg}");
////Console.WriteLine($"College is: {Student.college}");

//student.walk();
//Human human = new Human();
//human.Name = "Dharmesh";
//Console.WriteLine(std.Name);
//Console.WriteLine(student.Name);
//Console.WriteLine(student.age);
//student.eat(human.name);
//student.eat(student.Name);




using ConsoleApp2;

float sum = 0, avg = 0;
int[] marks = new int[3];

// Using different constructors
Student student = new Student("Gopal Singh");
Student std = new Student();
Student studentWithId = new Student(1, "Karthik", "Shashi");

std.Name = "Mantu";
Console.WriteLine(std.Name);

// Testing the base constructor calls
Console.WriteLine($"Student ID: {studentWithId.Id}"); // From Person class
Console.WriteLine($"Person Name: {studentWithId.Name}"); // From Person class  
Console.WriteLine($"Student Name: {studentWithId.Name}"); // From Student class

student.walk();
Human human = new Human();
human.Name = "Dharmesh";
Console.WriteLine(std.Name);
Console.WriteLine(student.Name);
Console.WriteLine(student.age); // From Human class via inheritance
student.eat(human.Name);
student.eat(student.Name);

Person person = new Person();
person.Name = "Karthik";
person.dance(person.Name);
human.dance(human.Name);