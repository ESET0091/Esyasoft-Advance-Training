using DependencyInjectionDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDemo.Vehicles
{
    public class Bike : IVehicle
    {
        public void Start()
        {
            Console.WriteLine("Bike started");
        }

        public void Stop()
        {
            Console.WriteLine("Bike stopped");
        }

        public string GetVehicleType()
        {
            return "Bike";
        }
    }
}