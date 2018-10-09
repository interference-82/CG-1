namespace LabsCG.Views
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Point = DTO.Point;

    public class PlotCanvasView : Canvas
    {
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(List<Point>), typeof(PlotCanvasView),
                                        new FrameworkPropertyMetadata(ItemSourcePropertyChangedDependency));

        private static Line osX;
        private static Line osY;

        
        private static readonly SolidColorBrush LineBrush = new SolidColorBrush(Color.FromRgb(254, 213, 47));
        private static readonly SolidColorBrush AxisBrush = new SolidColorBrush(Color.FromRgb(84, 83, 86));
        private static readonly SolidColorBrush BackgroundBrush = new SolidColorBrush(Color.FromRgb(255,255,255));

        private static readonly System.Windows.Point[] ZeroParameterPoints = 
        {
            new System.Windows.Point(249, 249),
            new System.Windows.Point(251, 249),
            new System.Windows.Point(251, 251),
            new System.Windows.Point(249, 251),
            new System.Windows.Point(249, 250),
            new System.Windows.Point(250, 250) 
        };

        private static readonly TextBlock OsTextBlockX= new TextBlock
        {
            FontSize = 23
        };

        private static readonly TextBlock OsTextBlockY = new TextBlock
        {
            FontSize = 23
        };

        public static Polyline Line = new Polyline
        {
            Stroke = LineBrush,
            StrokeThickness = 3
        };

        public PlotCanvasView()
        {
            if (double.IsNaN(Height) || double.IsNaN(Width))
            {
                Height = 500d;
                Width = 500d;
            }

            Background = BackgroundBrush;
            DrawAxis();
            DrawPlot();
        }

        public List<Point> ItemsSource
        {
            get => (List<Point>)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }

        private static void ItemSourcePropertyChanged(IReadOnlyCollection<Point> points)
        {
            var max = points.Max(x => x.X);
            var coefficient = 220 / max;
            var pointCollection =
                points.Select(
                    point => new System.Windows.Point(point.X * coefficient + 250, 250 - point.Y * coefficient));

            OsTextBlockX.Text = max.ToString(CultureInfo.InvariantCulture);
            OsTextBlockY.Text = max.ToString(CultureInfo.InvariantCulture);

            var collection = pointCollection as System.Windows.Point[] ?? pointCollection.ToArray();
            Line.Points = collection.Length == 4 ? new PointCollection(ZeroParameterPoints) : new PointCollection(collection);
        }

        private static void ItemSourcePropertyChangedDependency(DependencyObject dependencyObject,
                                                                DependencyPropertyChangedEventArgs eventArgs)
        {
            ItemSourcePropertyChanged((List<Point>)eventArgs.NewValue);
        }

        private void DrawPlot()
        {
            SetLeft(OsTextBlockY, 270);
            SetTop(OsTextBlockX, 30);
            SetRight(OsTextBlockX, 30);
            SetTop(OsTextBlockX, 270);

            Children.Add(OsTextBlockY);
            Children.Add(OsTextBlockX);
            Children.Add(Line);
        }

        private void DrawAxis()
        {
            var oX = Width / 2;
            var oY = Height / 2;

            osX = new Line
            {
                Stroke = AxisBrush,
                X1 = 0,
                Y1 = oY,
                X2 = Width,
                Y2 = oY,
                StrokeThickness = 2
            };
            osY = new Line
            {
                Stroke = AxisBrush,
                X1 = oX,
                Y1 = 0,
                X2 = oX,
                Y2 = Height,
                StrokeThickness = 2
            };
            var osLineX = new Line
            {
                Stroke = AxisBrush,
                X1 = oX-7,
                Y1 = 30,
                X2 = oX+7,
                Y2 = 30,
                StrokeThickness = 2
            };
            var osLineY = new Line
            {
                Stroke = AxisBrush,
                X1 = 470,
                Y1 = oY-7,
                X2 = 470,
                Y2 = oY+7,
                StrokeThickness = 2
            };
            var arrowX = new Polyline
            {
                Points = new PointCollection
                {
                    new System.Windows.Point(Width - 10, oY - 10),
                    new System.Windows.Point(Width, oY),
                    new System.Windows.Point(Width - 10, oY + 10)
                },
                Stroke = AxisBrush,
                StrokeThickness = 2
            };
            var arrowY = new Polyline
            {
                Points = new PointCollection
                {
                    new System.Windows.Point(oX - 10, 0 + 10),
                    new System.Windows.Point(oX, 0),
                    new System.Windows.Point(oX + 10, 0 + 10)
                },
                Stroke = AxisBrush,
                StrokeThickness = 2
            };

            Children.Add(osLineX);
            Children.Add(osLineY);
            Children.Add(arrowY);
            Children.Add(arrowX);
            Children.Add(osX);
            Children.Add(osY);
        }
    }
}