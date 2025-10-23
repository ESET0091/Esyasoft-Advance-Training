using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class Person : Human
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Person() : base()
        {
            Console.WriteLine("I am default constructor of Person class");
        }
        public Person(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public void walk()
        {
            Console.WriteLine("Person can walk");
        }

        public override void dance(string name)
        {
            base.dance(name);
        }
    }
}
