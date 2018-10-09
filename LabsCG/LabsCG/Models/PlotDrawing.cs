namespace LabsCG.Models
{
    using LabsCG.DTO;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PlotDrawing
    {
        private const double Step = 0.5d;

        private static double CalculationY(double x, double parameter) =>
            Math.Pow(Math.Pow(parameter, 2d / 3) - Math.Pow(x, 2d / 3), 3d / 2);

        public static List<Point> CalculatePoints(double parameter)
        {
            double Y(double x) => CalculationY(x, parameter);

            var enumerable = Enumerable.Range(0, (int)(parameter / Step) + 1)
                                       .Select(x => x * Step).ToArray();

            var result = enumerable.Select(x => new Point {X = x, Y = Y(x)})
                                   .Concat(enumerable.Select(x => new Point {X = Y(x), Y = -x}))
                                   .Concat(enumerable.Select(x => new Point {X = -x, Y = -Y(x)}))
                                   .Concat(enumerable.Select(x => new Point {X = -Y(x), Y = x}));
            
            return result.ToList();
        }
    }
}