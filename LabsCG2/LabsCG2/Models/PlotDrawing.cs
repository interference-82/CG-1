using System;
using System.Collections.Generic;
using System.Linq;
using LabsCG2.DTO;

namespace LabsCG2.Models
{
    public class PlotDrawing
    {
        private static Point3D Multiply(double[,] matrix, Point3D point)
        {
            var result = new Point3D(0, 0, 0);
            var enumerable = Enumerable.Range(0, 3).ToList();
            result.Coordinates[0] = enumerable.Select(x => matrix[0, x] * point.Coordinates[x]).Sum();
            result.Coordinates[1] = enumerable.Select(x => matrix[1, x] * point.Coordinates[x]).Sum();
            result.Coordinates[2] = enumerable.Select(x => matrix[2, x] * point.Coordinates[x]).Sum();

            return result;
        }

        public static List<Point3D> ZAxisRotating(List<Point3D> points, double angle)
        {
            double[,] transferMatrix =
            {
                {Math.Cos(angle * Math.PI / 180), -Math.Sin(angle * Math.PI / 180), 0},
                {Math.Sin(angle * Math.PI / 180), Math.Cos(angle * Math.PI / 180), 0},
                {0, 0, 1}
            };
            return points.Select(x => Multiply(transferMatrix, x)).ToList();
        }

        public static List<Point3D> YAxisRotating(List<Point3D> points, double angle)
        {
            double[,] transferMatrix =
            {
                {Math.Cos(angle * Math.PI / 180), 0, Math.Sin(angle * Math.PI / 180)},
                {0, 1, 0},
                {-Math.Sin(angle * Math.PI / 180), 0, Math.Cos(angle * Math.PI / 180)}
            };
            return points.Select(x => Multiply(transferMatrix, x)).ToList();
        }

        public static List<Point3D> XAxisRotating(List<Point3D> points, double angle)
        {
            double[,] transferMatrix =
            {
                {1, 0, 0},
                {0, Math.Cos(angle * Math.PI / 180), -Math.Sin(angle * Math.PI / 180)},
                {0, Math.Sin(angle * Math.PI / 180), Math.Cos(angle * Math.PI / 180)}
            };
            return points.Select(x => Multiply(transferMatrix, x)).ToList();
        }

        public static List<Point3D> Scaling(List<Point3D> points, double coefficient)
        {
            double[,] transferMatrix =
            {
                {coefficient, 0, 0},
                {0, coefficient, 0},
                {0, 0, coefficient}
            };
            return points.Select(x => Multiply(transferMatrix, x)).ToList();
        }
    }
}