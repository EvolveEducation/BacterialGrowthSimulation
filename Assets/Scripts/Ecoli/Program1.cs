using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EcoliPlay
{

    class Program
    {
        static void Main(string[] args)
        {
            PetriDish dish = new PetriDish(1, 2);
            dish.TimeToGrow();

        }
    }
}
