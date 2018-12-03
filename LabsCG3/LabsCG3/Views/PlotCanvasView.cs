using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LabsCG3.DTO;

namespace LabsCG3.Views
{
    public partial class PlotCanvasView : Canvas
    {
        private static readonly double[,] TransferMatrix =
        {
            {Math.Sqrt(3d / 6), 0, -Math.Sqrt(3d / 6)},
            {Math.Sqrt(1d / 6), 2 / Math.Sqrt(6), Math.Sqrt(1d / 6)},
            {Math.Sqrt(2d / 6), -Math.Sqrt(2d / 6), Math.Sqrt(2d / 6)}
        };

        private static readonly Point3D Observer = new Point3D(1, -1, 1);
        private static readonly Polygon[] Edges = new Polygon[6];

        private static readonly Point3D ObserverReflection = new Point3D(-1, 1, -1);
        private static readonly Polygon[] EdgesReflection = new Polygon[6];

        private static readonly Point3D Light = new Point3D(1,-1,1);
        private static readonly Point3D LightReflection = new Point3D(-1, 1, -1);

        private static double coefficientF=0.5;
        private static double coefficientS=0.5;

        private static List<Point3D> points;
        private static List<Color> colors = new List<Color>();
        
        private static void ItemSourcePropertyChanged(IReadOnlyCollection<Point3D> points3D)
        {
            if (points3D!=null){points = points3D.ToList();
            FillingLines(points3D.ToList(), Edges, Observer, Light);
            FillingLines(points3D.ToList().Select(x => new Point3D(x.X * -1, x.Y * -1, x.Z * -1)).ToList(),
                EdgesReflection, ObserverReflection, LightReflection);
            }
        }

        private static void CoefficientFPropertyChanged(double coefficientFParam)
        {
            coefficientF = coefficientFParam;
            ItemSourcePropertyChanged(points);
        }

        private static void CoefficientSPropertyChanged(double coefficientSParam)
        {
            coefficientS = coefficientSParam;
            ItemSourcePropertyChanged(points);
        }

        public PlotCanvasView()
        {
            if (double.IsNaN(Height) || double.IsNaN(Width))
            {
                Height = 500d;
                Width = 500d;
            }

            for (int i = 0; i < 6; i++)
            {
                colors.Add(new Color());
            }
            for (var i = 0; i < Edges.Length; ++i) Edges[i] = new Polygon();
            for (var i = 0; i < EdgesReflection.Length; ++i) EdgesReflection[i] = new Polygon();

            DrawPlot();
        }

        private static void FillingLines(List<Point3D> points3D, Polygon[] polygons,
            Point3D observer, Point3D lightVector)
        {
            var edgeIndexes = Enumerable.Repeat(1, 6).ToArray();
            edgeIndexes[5]--;
            DeletingLines(points3D, observer, edgeIndexes, lightVector);

            for (var i = 0; i < 6; i++)
            {
                polygons[i].Fill = edgeIndexes[i] > 0 ? new SolidColorBrush(colors[i]) : Brushes.Transparent;
                polygons[i].Stroke= edgeIndexes[i] > 0 ? new SolidColorBrush(colors[i]) : Brushes.Transparent;
            }

            var points2D = points3D.Select(TransferTo2D).ToArray();
            for (var i = 0; i < 4; ++i)
            {
                polygons[i].Points = new PointCollection(4)
                {
                    new Point(points2D[i].X, points2D[i].Y),
                    new Point(points2D[(i + 1) % 4].X, points2D[(i + 1) % 4].Y),
                    new Point(points2D[(i + 1) % 4 + 4].X, points2D[(i + 1) % 4 + 4].Y),
                    new Point(points2D[i + 4].X, points2D[i + 4].Y)
                };

                polygons[4].Points = new PointCollection(4)
                {
                    new Point(points2D[4].X, points2D[4].Y),
                    new Point(points2D[5].X, points2D[5].Y),
                    new Point(points2D[6].X, points2D[6].Y),
                    new Point(points2D[7].X, points2D[7].Y)
                };

                polygons[5].Points = new PointCollection(4)
                {
                    new Point(points2D[0].X, points2D[0].Y),
                    new Point(points2D[1].X, points2D[1].Y),
                    new Point(points2D[2].X, points2D[2].Y),
                    new Point(points2D[3].X, points2D[3].Y)
                };
            }
        }

        private static void DeletingLines(List<Point3D> points3D, Point3D observer, int[] edgeIndexes, Point3D lightVector)
        {
            Point3D a;
            Point3D b;
            Point3D norm;
            Point3D unitNorm;
            double normLength;
            double normMultLight;
            Point3D abVector;
            Point3D reflectionVector;
            double cos;
            double d = 0;
            double K = 1;
            double If = 1;
            double Ii = 20;
            double Ks = 0.8;
            double Ip;
            double colorCode;
            double observerLength = Math.Sqrt(observer.Sum(x => x * x));
            Point3D unitObserver= new Point3D(observer.X/observerLength, observer.Y / observerLength, observer.Y / observerLength);

            double lightLength = Math.Sqrt(lightVector.Sum(x => x * x));
            Point3D unitLight = new Point3D(lightVector.X/lightLength, lightVector.Y / lightLength, lightVector.Z / lightLength);

            


            for (var i = 0; i < 4; i++)
            {
                a = VectorMath.VectorCoordinates(points3D[i], points3D[(i + 1) % 4]);
                b = VectorMath.VectorCoordinates(points3D[i], points3D[i + 4]);

                norm = VectorMath.VectorMultiplying(b, a);
                normLength = Math.Sqrt(norm.Sum(x => x * x));
                unitNorm = new Point3D(norm.X/normLength, norm.Y / normLength, norm.Z / normLength);

                normMultLight = VectorMath.Multiplying(unitLight,unitNorm);
                abVector = new Point3D(unitNorm.X * 2 * normMultLight, unitNorm.Y * 2 * normMultLight, unitNorm.Z * 2 * normMultLight);

                reflectionVector = VectorMath.VectorSubstract(unitLight, abVector);
                cos = VectorMath.Multiplying(reflectionVector, unitObserver);

                Ip =If*coefficientF+Ii* (coefficientS * normMultLight + Ks * cos * cos) / (d + K);
                Ip *= Math.Sign(Ip);
                if (Ip > 1)
                {
                    colors[i] = Color.FromRgb(Convert.ToByte(255 / Ip), Convert.ToByte(126/Ip), Convert.ToByte(147/Ip));
                }
                
                if (VectorMath.Multiplying(norm, observer) < 0)
                {
                    edgeIndexes[i]--;
                }

            }
           

            a = VectorMath.VectorCoordinates(points3D[6], points3D[5]);
            b = VectorMath.VectorCoordinates(points3D[6], points3D[7]);
            norm = VectorMath.VectorMultiplying(a, b);
            normLength = Math.Sqrt(norm.Sum(x => x * x));
            unitNorm = new Point3D(norm.X / normLength, norm.Y / normLength, norm.Z / normLength);

            normMultLight = VectorMath.Multiplying(unitLight, unitNorm);
            abVector = new Point3D(unitNorm.X * 2 * normMultLight, unitNorm.Y * 2 * normMultLight, unitNorm.Z * 2 * normMultLight);

            reflectionVector = VectorMath.VectorSubstract(unitLight, abVector);
            cos = VectorMath.Multiplying(reflectionVector, unitObserver);

            Ip = If * coefficientF + Ii * (coefficientS * normMultLight + Ks * cos * cos) / (d + K);
            Ip *= Math.Sign(Ip);
            if (Ip > 1)
            {
                colors[4] = Color.FromRgb(Convert.ToByte(255 / Ip), Convert.ToByte(126 / Ip), Convert.ToByte(147 / Ip));
            }
            
            if (VectorMath.Multiplying(VectorMath.VectorMultiplying(a, b), observer) < 0)
            {
                edgeIndexes[4]--;
            }

        }


        private static Point2D TransferTo2D(Point3D point3D)
        {
            var result = new Point2D(0, 0);

            var i = 0;
            foreach (var coords in point3D)
            {
                result.X += TransferMatrix[0, i] * coords;
                result.Y += TransferMatrix[1, i] * coords;
                i++;
            }

            result.X += 250;
            result.Y += 250;

            return result;
        }

        private void DrawPlot()
        {
            foreach (var edge in Edges) Children.Add(edge);
            foreach (var edge in EdgesReflection) Children.Add(edge);
        }
    }
}