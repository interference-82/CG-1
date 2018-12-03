using LabsCG3.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabsCG3.Models
{
    public class PlotDrawing
    {
        private static Point3D Multiply(double[,] matrix, Point3D point)
        {
            var result = new Point3D(0, 0, 0);

            var i = 0;

            foreach (var coords in point)
            {
                result.X += matrix[0, i] * coords;
                result.Y += matrix[1, i] * coords;
                result.Z += matrix[2, i] * coords;
                i++;
            }

            return result;
        }

        public static List<Point3D> ZAxisRotating(List<Point3D> points, double angle)
        {
            double[,] transferMatrix =
            {
                {Math.Cos(angle * Math.PI /  Constans.LinearPi), -Math.Sin(angle * Math.PI /  Constans.LinearPi), 0},
                {Math.Sin(angle * Math.PI /  Constans.LinearPi), Math.Cos(angle * Math.PI /  Constans.LinearPi), 0},
                {0, 0, 1}
            };
            
            return points.Select(x => Multiply(transferMatrix, x)).ToList();
        }

        public static List<Point3D> YAxisRotating(List<Point3D> points, double angle)
        {
            double[,] transferMatrix =
            {
                {Math.Cos(angle * Math.PI /  Constans.LinearPi), 0, Math.Sin(angle * Math.PI /  Constans.LinearPi)},
                {0, 1, 0},
                {-Math.Sin(angle * Math.PI /  Constans.LinearPi), 0, Math.Cos(angle * Math.PI /  Constans.LinearPi)}
            };
            return points.Select(x => Multiply(transferMatrix, x)).ToList();
        }

        public static List<Point3D> XAxisRotating(List<Point3D> points, double angle)
        {
            double[,] transferMatrix =
            {
                {1, 0, 0},
                {0, Math.Cos(angle * Math.PI /  Constans.LinearPi), -Math.Sin(angle * Math.PI /  Constans.LinearPi)},
                {0, Math.Sin(angle * Math.PI /  Constans.LinearPi), Math.Cos(angle * Math.PI /  Constans.LinearPi)}
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

        public static List<Point3D> Approximating(double height, double width, double xAxisAngle, double yAxisAngle, double zAxisAngle, double scale)
        {
            List<Point3D> currentPoints = new List<Point3D>(8);
            double currentY = Ellipsoid.CountingY(width, 0);
            currentPoints.Add(new Point3D(width, -currentY, 0));
            currentPoints.Add(new Point3D(-width, -currentY, 0));
            currentPoints.Add(new Point3D(-width, currentY, 0));
            currentPoints.Add(new Point3D(width, currentY, 0));

            double currentA = Math.Sqrt(Math.Pow(Ellipsoid.A, 2) * (1 - Math.Pow(height / Ellipsoid.C, 2)));
            double x = 3.0 / 5 * currentA;
            double y = Ellipsoid.CountingY(x, height);
            currentPoints.Add(new Point3D(x, -y, height));
            currentPoints.Add(new Point3D(-x, -y, height));
            currentPoints.Add(new Point3D(-x, y, height));
            currentPoints.Add(new Point3D(x, y, height));

            return Scaling(ZAxisRotating(YAxisRotating(XAxisRotating(currentPoints, xAxisAngle - Constans.LinearPi), yAxisAngle - Constans.LinearPi), zAxisAngle - Constans.LinearPi), scale);
        }
        
    }
}