using System.Collections.Generic;

namespace LabsCG3.DTO
{
    internal class Obelisk
    {
        public static List<Point3D> Points { get; } = new List<Point3D>
        {
            new Point3D(30, -80, 0),
            new Point3D(-30, -80, 0),
            new Point3D(-30, 80, 0),
            new Point3D(30, 80, 0),

            new Point3D(16.2, -44, 100),
            new Point3D(-16.2, -44, 100),
            new Point3D(-16.2, 44, 100),
            new Point3D(16.2, 44, 100)
            //new Point3D(100, -100, -100),
            //new Point3D(-100, -100, -100),
            //new Point3D(-100, 100, -100),
            //new Point3D(100, 100, -100),

            //new Point3D(50, -50, 100),
            //new Point3D(-50, -50, 100),
            //new Point3D(-50, 50, 100),
            //new Point3D(50, 50, 100)
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