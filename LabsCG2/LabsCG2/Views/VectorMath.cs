using LabsCG2.DTO;

namespace LabsCG2.Views
{
    internal static class VectorMath
    {
        public static Point3D VectorMultiplying(Point3D a, Point3D b)
        {
            var x = a.Coordinates[1] * b.Coordinates[2] - a.Coordinates[2] * b.Coordinates[1];
            var y = a.Coordinates[2] * b.Coordinates[0] - a.Coordinates[0] * b.Coordinates[2];
            var z = a.Coordinates[0] * b.Coordinates[1] - a.Coordinates[1] * b.Coordinates[0];
            return new Point3D(x, y, z);
        }

        public static Point3D VectorCoordinates(Point3D begin, Point3D end)
        {
            var x = end.Coordinates[0] - begin.Coordinates[0];
            var y = end.Coordinates[1] - begin.Coordinates[1];
            var z = end.Coordinates[2] - begin.Coordinates[2];
            return new Point3D(x, y, z);
        }

        public static double Multiplying(Point3D a, Point3D b)
        {
            return a.Coordinates[0] * b.Coordinates[0] + a.Coordinates[1] * b.Coordinates[1] + a.Coordinates[2] *
                   b.Coordinates[2];
        }
    }
}