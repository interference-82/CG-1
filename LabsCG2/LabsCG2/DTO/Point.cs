namespace LabsCG2.DTO
{
    public class Point2D
    {
        public double[] Coordinates = new double[2];

        public Point2D(double x, double y)
        {
            Coordinates[0] = x;
            Coordinates[1] = y;
        }
    }

    public class Point3D
    {
        public double[] Coordinates = new double[3];

        public Point3D(double x, double y, double z)
        {
            Coordinates[0] = x;
            Coordinates[1] = y;
            Coordinates[2] = z;
        }
    }
}