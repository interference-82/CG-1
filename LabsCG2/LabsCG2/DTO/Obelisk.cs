using System.Collections.Generic;

namespace LabsCG2.DTO
{
    internal class Obelisk
    {
        public static List<Point3D> Points { get; } = new List<Point3D>
        {
            new Point3D(100, -100, -100),
            new Point3D(-100, -100, -100),
            new Point3D(-100, 100, -100),
            new Point3D(100, 100, -100),

            new Point3D(50, -50, 100),
            new Point3D(-50, -50, 100),
            new Point3D(-50, 50, 100),
            new Point3D(50, 50, 100)
            //new Point3D (100,-100,-100),
            //new Point3D (-100,-100,-100),
            //new Point3D (-100,100,-100),
            //new Point3D (100,100,-100),

            //new Point3D (100,-100,100),
            //new Point3D (-100,-100,100),
            //new Point3D (-100,100,100),
            //new Point3D (100,100,100)
        };
    }
}