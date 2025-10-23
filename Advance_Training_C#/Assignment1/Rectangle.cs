using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    public class Rectangle : Shape
    {
        public double length, bredth;
        public Rectangle()
        {

        }
        public Rectangle(double length, double bredth)
        {
            this.length = length;
            this.bredth = bredth;
        }
        public double getArea(double length, double bredth)
        {
            return length * bredth;
        }
        static public double sum_of_areas(double area1, double area2)
        {
            return area1 + area2;
        }

        public double GetArea()
        {
            return length * bredth;
        }

        // Operator overloading for + operator to sum areas
        public static double operator +(Rectangle r1, Rectangle r2)
        {
            return r1.GetArea() + r2.GetArea();
        }

        public static double operator -(Rectangle r1, Rectangle r2)
        {
            return r1.GetArea() * r2.GetArea();
        }

        public Rectangle newRec(Rectangle r1, Rectangle r2)
        {
            Rectangle r = new Rectangle();
            r.length = r1.length + r2.length;
            r.bredth = r1.bredth + r2.bredth;
            return r;
        }

        public override void Figure()
        {
            Console.WriteLine("I am rectangle");
        }
    }
}
