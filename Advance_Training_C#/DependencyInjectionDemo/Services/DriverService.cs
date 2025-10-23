using DependencyInjectionDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDemo.Services
{
    public class DriverService
    {

        // Constructor injection
        private readonly IVehicle _constructorVehicle;
        public DriverService(IVehicle vehicle)
        {
            _constructorVehicle = vehicle;
            Console.WriteLine($"Driver service created with {vehicle.GetVehicleType()} constructor injection");
        }

        // Property injection
        public IVehicle propertyVehicle { get; set; }
        private IVehicle _setterVehicle;
        public void SetVehicle(IVehicle vehicle)
        {
            _setterVehicle = vehicle;
            Console.WriteLine($"Vehicle set via Setter: {vehicle.GetType()}");

        }
        public void DriveConstructorVehicle()
        {
            Console.WriteLine($"\nDriving Constructor Vehicle:");
            _constructorVehicle.Start();
            _constructorVehicle.Stop();
        }

        public void DrivePropertyVehicle()
        {
            if(propertyVehicle == null)
            {
                Console.WriteLine("\n No Property Vehicle set!");
                return;
            }
            Console.WriteLine($"\nDriving Property Vehicle:");
            propertyVehicle.Start();
            propertyVehicle.Stop();
        }


        public void DriveSetterVehicle()
        {
            if (_setterVehicle == null)
            {
                Console.WriteLine("\nNo Setter Vehicle set!");
                return;
            }
            Console.WriteLine($"\nDriving Setter Vehicle:");
            _setterVehicle.Start();
            _setterVehicle.Stop();
        }

    }
}
