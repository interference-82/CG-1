using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LabsCG7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Point> points = new List<Point>();
        List<Ellipse> ellipses = new List<Ellipse>();
        List<Polyline> polylines = new List<Polyline>();
        private bool IsMouseUp = false;
        private int pointNumber;
        private int lastPointNumber=-1;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(this);
            //lblCoordinateInfo.Content = String.Format("X: {0}, Y: {1}", point.X, point.Y);
            if (IsMouseUp)
            {
                if (lastPointNumber < 2)
                {
                    points[pointNumber] = point;
                    Canvas.SetLeft(ellipses[pointNumber], point.X - 0.5 * Constants.R);
                    Canvas.SetTop(ellipses[pointNumber], point.Y - 0.5 * Constants.R);
                }
                else if (pointNumber == 0)
                {
                    points[0] = point;
                    Canvas.SetLeft(ellipses[0], point.X - 0.5 * Constants.R);
                    Canvas.SetTop(ellipses[0], point.Y - 0.5 * Constants.R);
                    MyCanvas.Children.Remove(polylines[0]);
                    polylines[0] = Bezier.CountingPolyline(points[0], points[1], points[2]);
                    polylines[0].MouseLeftButtonDown += UIElement_OnMouseDown;
                    MyCanvas.Children.Add(polylines[0]);
                }
                else if (pointNumber==points.Count-1)
                {
                    points[points.Count - 1] = point;
                    Canvas.SetLeft(ellipses[lastPointNumber], point.X - 0.5 * Constants.R);
                    Canvas.SetTop(ellipses[lastPointNumber], point.Y - 0.5 * Constants.R);
                    MyCanvas.Children.Remove(polylines[lastPointNumber/2-1]);
                    polylines[lastPointNumber / 2 - 1] = Bezier.CountingPolyline(points[lastPointNumber-2], points[lastPointNumber-1], points[lastPointNumber]);
                    polylines[lastPointNumber / 2 - 1].MouseLeftButtonDown += UIElement_OnMouseDown;
                    MyCanvas.Children.Add(polylines[lastPointNumber/2-1]);

                }
                else if (pointNumber % 2 == 1)
                {
                    points[pointNumber] = point;
                    Canvas.SetLeft(ellipses[pointNumber], point.X - 0.5 * Constants.R);
                    Canvas.SetTop(ellipses[pointNumber], point.Y - 0.5 * Constants.R);
                    MyCanvas.Children.Remove(polylines[pointNumber / 2]);
                    polylines[pointNumber / 2] = Bezier.CountingPolyline(points[pointNumber - 1], points[pointNumber],
                        points[pointNumber + 1]);
                    polylines[pointNumber / 2].MouseLeftButtonDown += UIElement_OnMouseDown;
                    MyCanvas.Children.Add(polylines[pointNumber / 2]);

                }
                else
                {
                    points[pointNumber] = point;
                    Canvas.SetLeft(ellipses[pointNumber], point.X - 0.5 * Constants.R);
                    Canvas.SetTop(ellipses[pointNumber], point.Y - 0.5 * Constants.R);
                    MyCanvas.Children.Remove(polylines[pointNumber / 2]);
                    MyCanvas.Children.Remove(polylines[pointNumber / 2-1]);
                    polylines[pointNumber / 2] = Bezier.CountingPolyline(points[pointNumber], points[pointNumber+1],
                        points[pointNumber + 2]);
                    polylines[pointNumber / 2 - 1] = Bezier.CountingPolyline(points[pointNumber - 2], points[pointNumber-1],
                        points[pointNumber]);
                    polylines[pointNumber / 2].MouseLeftButtonDown += UIElement_OnMouseDown;
                    polylines[pointNumber / 2-1].MouseLeftButtonDown += UIElement_OnMouseDown;
                    MyCanvas.Children.Add(polylines[pointNumber / 2]);
                    MyCanvas.Children.Add(polylines[pointNumber / 2-1]);

                }

                
            }
        }
       
        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseUp)
            {
                IsMouseUp = false;
                return;
            }
            Point position = e.GetPosition(MyCanvas);
            int i = 0;
            if (points != null)
            {
                foreach (Point point in points)
                {
                    if (Math.Abs(point.X - position.X) < Constants.R && Math.Abs(point.Y - position.Y) < Constants.R)
                    {
                        pointNumber = i;
                        IsMouseUp = true;
                        return;
                    }
                    ++i;
                }
            }

            points?.Add(position);
            lastPointNumber++;
            Ellipse currentEllipse = new Ellipse
            {
                Width = Constants.R,
                Height = Constants.R,
                Stroke = points?.Count%2==0? Brushes.DarkOrange:Brushes.White,
                Fill = Brushes.White
                
            };
            ellipses.Add(currentEllipse);
            Canvas.SetLeft(currentEllipse, position.X-0.5*Constants.R);
            Canvas.SetTop(currentEllipse,position.Y-0.5*Constants.R);
            MyCanvas.Children.Add(ellipses[ellipses.Count-1]);
            if (points?.Count%2 == 1 && points?.Count>2)
            {
                polylines.Add(Bezier.CountingPolyline(points[points.Count - 3], points[points.Count - 2],
                    points[points.Count - 1]));
                polylines[polylines.Count - 1].MouseLeftButtonDown += UIElement_OnMouseDown;
                MyCanvas.Children.Add(polylines[polylines.Count - 1]);
            }
        }

        private void MyCanvas_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (lastPointNumber > -1)
            {
                points.RemoveAt(lastPointNumber);
                MyCanvas.Children.Remove(ellipses[lastPointNumber]);
                ellipses.RemoveAt(lastPointNumber);
                if (lastPointNumber > 1 && lastPointNumber % 2 == 0)
                {
                    MyCanvas.Children.Remove(polylines[lastPointNumber/2-1]);
                    polylines.RemoveAt(polylines.Count-1);
                }
                lastPointNumber--;
            }
            
        }
    }
}
