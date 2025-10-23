//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Metadata;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleApp2
//{
//    public class Student : Person
//    {
//        private string name;
//       public  static string college= "NITJSR";
//        public string  Name
//        {
//            get
//            {
//                return name;
//            }
//            set
//            {
//                name = value;

//            }
//        }

//        int[] marks = new int[3];
//        public Student() : base()
//        {
//            Console.WriteLine("Default Constructor");
//        }
//        public Student(string name) : base() 
//        {
//            this.name = name;
//        }
//        public void student_details()
//        {
//            Console.WriteLine("Enter your name: ");
//            string name = Console.ReadLine();

//        }

//        public int[] take_std_details()
//        {

//            Console.WriteLine("Enter all 3 subjects marks: ");
//            for (int i = 0; i < 3; i++)
//                marks[i] = Convert.ToInt32(Console.ReadLine());
//            return marks;

//        }
//        public float average_marks(int no_of_sub)
//        {
//            return total_marks(marks) / no_of_sub;
//        }
//        public float total_marks(int[] marks)
//        {
//            float sum = 0;
//            for (int i = 0; i < marks.Length; i++)
//                sum += marks[i];
//            return sum;
//        }

//    }

//}









using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Student : Person
    {
        private string name;
        public static string college = "NITJSR";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        int[] marks = new int[3];

        // Default constructor calling Person's default constructor
        public Student() : base()
        {
            Console.WriteLine("Default Constructor of Student");
        }

        // Parameterized constructor calling Person's default constructor
        public Student(string name) : base()
        {
            this.name = name;
        }

        // New constructor calling Person's parameterized constructor
        public Student(int id, string personName, string studentName) : base(id, personName)
        {
            this.name = studentName;
        }

        public void student_details()
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
        }

        public int[] take_std_details()
        {
            Console.WriteLine("Enter all 3 subjects marks: ");
            for (int i = 0; i < 3; i++)
                marks[i] = Convert.ToInt32(Console.ReadLine());
            return marks;
        }

        public float average_marks(int no_of_sub)
        {
            return total_marks(marks) / no_of_sub;
        }

        public float total_marks(int[] marks)
        {
            float sum = 0;
            for (int i = 0; i < marks.Length; i++)
                sum += marks[i];
            return sum;
        }
    }
}