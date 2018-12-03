using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabsCG3.DTO
{
    internal class Ellipsoid
    {
        public static int A = 50;
        public static int B = 100;
        public static int C = 120;

        public static double CountingX(double Y, double Z)
        {
            return Math.Sqrt(Math.Pow(A, 2) * (1 - Math.Pow(Y / B, 2) - Math.Pow(Z / C, 2)));
        }
        public static double CountingY(double X, double Z)
        {
            return Math.Sqrt((Math.Pow(B,2)) * (1 - Math.Pow(X / A, 2) - Math.Pow(Z / C, 2)));
        }

        public static double CountingZ(double X, double Y)
        {
            return Math.Sqrt((Math.Pow(C, 2)) * (1 - Math.Pow(X / A, 2) - Math.Pow(Y / B, 2)));
        }
    }
}
