using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionDemo.Interfaces
{
    public interface IVehicle
    {
        void Start();
        void Stop();
        string GetVehicleType();
    }
}
