using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Human
    {
        public string name;
        public int age = 65;
        public string Name {
            get {
                return name;
            }
            set { 
                name = value;
            }
        }
        public void eat(string name)
        {
            Console.WriteLine($"{name} Can eat");
        }

        public virtual void dance(string name)
        {
            Console.WriteLine($"{name} can dance");
        }
    }
}
