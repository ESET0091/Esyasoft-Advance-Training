using DependencyInjectionDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDemo.Vehicles
{
    public class Car : IVehicle
    {
        public void Start()
        {
            Console.WriteLine("Car started");
        }
        public void Stop()
        {
            Console.WriteLine("Car stopped");
        }
        public string GetVehicleType()
        {
            return "Car";
        }
    }
}
