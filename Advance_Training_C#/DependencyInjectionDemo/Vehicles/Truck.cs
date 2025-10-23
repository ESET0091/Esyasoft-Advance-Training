using DependencyInjectionDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDemo.Vehicles
{
    public class Truck : IVehicle
    {
        public void Start()
        {
            Console.WriteLine("Truck started");
        }

        public void Stop()
        {
            Console.WriteLine("Truck stopped");
        }

        public string GetVehicleType()
        {
            return "Truck";
        }
    }
}
