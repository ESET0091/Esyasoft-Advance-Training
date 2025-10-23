using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Assignment1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Rectangle rectangle1 = new Rectangle(19, 18);
            Rectangle rectangle2 = new Rectangle(10.6, 6.6);

            double area1 = rectangle1.getArea(rectangle1.length, rectangle1.bredth);
            double area2 = rectangle2.getArea(rectangle2.length, rectangle2.bredth);
            // Using operator overloading to sum areas
            double totalArea = rectangle1 + rectangle2;
            Console.WriteLine($"Total Area: {totalArea}");

            double totalMultArea = rectangle1 - rectangle2;
            Console.WriteLine($"Total Areas multiplication is: {totalMultArea}");

            //double sum_of_areas = area1 + area2;
            Console.WriteLine($"Area of first rectangle is {area1 }");
            Console.WriteLine($"Area of second rectangle is {area2 }");
            Console.WriteLine($"Sum of Areas is: {Rectangle.sum_of_areas(area1, area2)}");


            Circle circle = new Circle(7);
            double area = circle.CalculateArea();
            double perimeter = circle.CalculatePerimeter();
            Console.WriteLine($"Area of Circle is {area}");
            Console.WriteLine($"Perimeter of Circle is {perimeter}");

            //Shape shape = new Shape();
             

        }
    }
}
