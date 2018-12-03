using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabsCG3.DTO
{
    class Pair
    {
        public int RotatingId;
        public double Angle;

        Pair(int id, double angle)
        {
            RotatingId = id;
            Angle = angle;
        }
    }
}
