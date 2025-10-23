using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    interface IShape
    {
        double CalculateArea();
        double CalculatePerimeter();
        string Name { get; }
    }
}
