using DependencyInjectionDemo.Services;
using DependencyInjectionDemo.Vehicles;

namespace DependencyInjectionDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🚗 Vehicle DI Demo - 3 Ways of Dependency Injection");
            var car = new Car();
            var bike = new Bike();
            var truck = new Truck();
/*
            // 1- Constructor Injection Demo
            Console.WriteLine("1. CONSTRUCTOR INJECTION");
            var driver1 = new DriverService(car);
            driver1.DriveConstructorVehicle();

            var driver2 = new DriverService(bike);
            driver2.DriveConstructorVehicle();

            
            // 2. Property Injection Demo
            Console.WriteLine("\n2. PROPERTY INJECTION");
            var driver3 = new DriverService(car);
            driver3.propertyVehicle = bike;
            driver3.DrivePropertyVehicle();

            driver3.propertyVehicle = truck; // Change property at runtime
            driver3.DrivePropertyVehicle();

            //// Try without setting property
            var driver4 = new DriverService(car);
            driver4.DrivePropertyVehicle();
*/

            // 3. Setter Method Injection Demo
            Console.WriteLine("\n3. SETTER METHOD INJECTION");
            var driver5 = new DriverService(car);
            driver5.SetVehicle(bike);
            driver5.DriveSetterVehicle();

            //driver5.SetVehicle(truck);
            //driver5.DriveSetterVehicle();

            //// Try without setting
            //var driver6 = new DriverService(car);
            //driver6.DriveSetterVehicle(); // This will show error

            
        }
    }
}
