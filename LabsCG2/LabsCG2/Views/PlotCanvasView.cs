using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LabsCG2.DTO;

namespace LabsCG2.Views
{
    public class PlotCanvasView : Canvas
    {
        private static readonly double[,] TransferMatrix =
        {
            {Math.Sqrt(3d / 6), 0, -Math.Sqrt(3d / 6)},
            {Math.Sqrt(1d / 6), 2 / Math.Sqrt(6), Math.Sqrt(1d / 6)},
            {Math.Sqrt(2d / 6), -Math.Sqrt(2d / 6), Math.Sqrt(2d / 6)}
        };

        private static readonly Point3D IsometricObserver = new Point3D(250, -250, 250);
        private static readonly Line[] IsometricLines = new Line[12];
        private static readonly int[] IsometricLineIndexes = new int[12];

        private static readonly Point3D OrthographicObserver = new Point3D(0, -0, 2500);
        private static readonly Line[] OrthographicLines = new Line[12];
        private static readonly int[] OrthographicLineIndexes = new int[12];

        private static readonly Line[] CoordinateAxis = new Line[2];

        private static readonly TextBlock IsometricBlock = new TextBlock
        {
            FontSize = 23,
            Text = "Изометрическая проекция:"
        };

        private static readonly TextBlock OrthographicBlock = new TextBlock
        {
            FontSize = 23,
            Text= "Ортографическая проекция:"
        };

        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(List<Point3D>), typeof(PlotCanvasView),
                new FrameworkPropertyMetadata(ItemSourcePropertyChangedDependency));

        public PlotCanvasView()
        {
            if (double.IsNaN(Height) || double.IsNaN(Width))
            {
                Height = 500d;
                Width = 500d;
            }

            for (var i = 0; i < IsometricLines.Length; ++i) IsometricLines[i] = new Line();
            for (var i = 0; i < OrthographicLines.Length; ++i) OrthographicLines[i] = new Line();
            CoordinateAxis[0] = new Line
            {
                X1 = 0,
                X2 = 500,
                Y1 = 625,
                Y2 = 625,
                Stroke = Brushes.Lavender
            };
            CoordinateAxis[1] = new Line
            {
                X1 = 250,
                X2 = 250,
                Y1 = 450,
                Y2 = 800,
                Stroke = Brushes.Lavender
            };

            DrawPlot();
        }

        public List<Point3D> ItemsSource
        {
            get => (List<Point3D>) GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }

        private static void ItemSourcePropertyChanged(IReadOnlyCollection<Point3D> points3D)
        {
            FillingLines(points3D.ToList(), IsometricTransferTo2D, IsometricLines, IsometricLineIndexes,
                IsometricObserver);
            FillingLines(points3D.ToList(), OrthographicTransferTo2D, OrthographicLines, OrthographicLineIndexes,
                OrthographicObserver);
        }

        private static void ItemSourcePropertyChangedDependency(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            ItemSourcePropertyChanged((List<Point3D>) eventArgs.NewValue);
        }

        private static void FillingLines(List<Point3D> points3D, TransferTo2D transferring, Line[] lines,
            int[] lineIndexes, Point3D observer)
        {
            lineIndexes = Enumerable.Repeat(2, 12).ToArray();
            DeletingLines(points3D, observer, lineIndexes);
            for (var i = 0; i < 12; i++) lines[i].Stroke = lineIndexes[i] > 0 ? Brushes.MediumVioletRed : Brushes.Transparent;

            var points2D = points3D.Select(x => transferring(x)).ToArray();
            for (var i = 0; i < 4; ++i)
            {
                lines[i].X1 = points2D[i].Coordinates[0];
                lines[i].Y1 = points2D[i].Coordinates[1];
                lines[i].X2 = points2D[i + 4].Coordinates[0];
                lines[i].Y2 = points2D[i + 4].Coordinates[1];

                lines[i + 4].X1 = points2D[i].Coordinates[0];
                lines[i + 4].Y1 = points2D[i].Coordinates[1];
                lines[i + 4].X2 = points2D[(i + 1) % 4].Coordinates[0];
                lines[i + 4].Y2 = points2D[(i + 1) % 4].Coordinates[1];

                lines[i + 8].X1 = points2D[i + 4].Coordinates[0];
                lines[i + 8].Y1 = points2D[i + 4].Coordinates[1];
                lines[i + 8].X2 = points2D[(i + 1) % 4 + 4].Coordinates[0];
                lines[i + 8].Y2 = points2D[(i + 1) % 4 + 4].Coordinates[1];
            }
        }
        
        private static void DeletingLines(List<Point3D> points3D, Point3D observer, int[] lineIndexes)
        {
            Point3D a;
            Point3D b;
            for (var i = 0; i < 4; i++)
            {
                a = VectorMath.VectorCoordinates(points3D[i], points3D[(i + 1) % 4]);
                b = VectorMath.VectorCoordinates(points3D[i], points3D[i + 4]);
                if (VectorMath.Multiplying(VectorMath.VectorMultiplying(b, a), observer) < 0)
                {
                    lineIndexes[i]--;
                    lineIndexes[i + 4]--;
                    lineIndexes[i + 8]--;
                    lineIndexes[(i + 1) % 4]--;
                }
            }

            a = VectorMath.VectorCoordinates(points3D[0], points3D[1]);
            b = VectorMath.VectorCoordinates(points3D[0], points3D[3]);

            if (VectorMath.Multiplying(VectorMath.VectorMultiplying(a, b), observer) < 0)
            {
                lineIndexes[4]--;
                lineIndexes[5]--;
                lineIndexes[6]--;
                lineIndexes[7]--;
            }

            a = VectorMath.VectorCoordinates(points3D[6], points3D[5]);
            b = VectorMath.VectorCoordinates(points3D[6], points3D[7]);

            if (VectorMath.Multiplying(VectorMath.VectorMultiplying(a, b), observer) < 0)
            {
                lineIndexes[8]--;
                lineIndexes[9]--;
                lineIndexes[10]--;
                lineIndexes[11]--;
            }
        }

        private static Point2D IsometricTransferTo2D(Point3D point3D)
        {
            var result = new Point2D(0, 0);
            var enumerable = Enumerable.Range(0, 3).ToList();
            result.Coordinates[0] = enumerable.Select(x => TransferMatrix[0, x] * point3D.Coordinates[x]).Sum() + 250;
            result.Coordinates[1] = enumerable.Select(x => TransferMatrix[1, x] * point3D.Coordinates[x]).Sum() + 250;
            return result;
        }

        private static Point2D OrthographicTransferTo2D(Point3D point3D)
        {
            var result = new Point2D(0, 0)
            {
                Coordinates = {[0] = point3D.Coordinates[0] + 250, [1] = point3D.Coordinates[1] + 625}
            };
            return result;
        }

        private void DrawPlot()
        {
            SetLeft(IsometricBlock, 10);
            SetTop(IsometricBlock,10);
            SetLeft(OrthographicBlock, 10);
            SetTop(OrthographicBlock, 400);
            Children.Add(IsometricBlock);
            Children.Add(OrthographicBlock);
            foreach (var line in IsometricLines) Children.Add(line);

            foreach (var line in OrthographicLines) Children.Add(line);

            foreach (var line in CoordinateAxis) Children.Add(line);
        }

        private delegate Point2D TransferTo2D(Point3D point);
    }
}