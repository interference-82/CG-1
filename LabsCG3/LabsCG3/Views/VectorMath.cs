using LabsCG3.DTO;

namespace LabsCG3.Views
{
    internal static class VectorMath
    {
        public static Point3D VectorMultiplying(Point3D a, Point3D b)
        {
            var x = a.Y * b.Z - a.Z * b.Y;
            var y = a.Z * b.X - a.X * b.Z;
            var z = a.X * b.Y - a.Y * b.X;

            return new Point3D(x, y, z);
        }

        public static Point3D VectorCoordinates(Point3D begin, Point3D end)
        {
            var x = end.X - begin.X;
            var y = end.Y - begin.Y;
            var z = end.Z - begin.Z;

            return new Point3D(x, y, z);
        }

        public static Point3D VectorSubstract(Point3D first, Point3D second)
        {
            return new Point3D(first.X-second.X,first.Y-second.Y,first.Z-second.Z);
        }

        public static double Multiplying(Point3D a, Point3D b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
    }
}