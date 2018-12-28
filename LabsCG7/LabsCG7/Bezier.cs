using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LabsCG7
{
    public class Bezier
    {
        public static Polyline CountingPolyline(Point p1, Point p2, Point p3)
        {
            Polyline result = new Polyline();
            PointCollection points = new PointCollection(Enumerable.Range(0,11).Select(x=> CountingPoints(x/10.0,p1,p2,p3)));
            result.Points = points;
            result.Stroke = Brushes.White;
            return result;
        }

        public static void ChangingPolyline(Polyline polyline,Point p1, Point p2, Point p3)
        {
            double t = 0;
            for(int i=0;i<polyline.Points.Count;++i)
            {
                ChangingPoints(polyline.Points[i],t,p1,p2,p3);
                t += 0.1;
            }
        }

        private static Point CountingPoints(double t,Point p1,Point p2,Point p3)
        {
            double x = (1 - t)*(1 - t)*p1.X + 2*(1 - t)*t*p2.X + t*t*p3.X;
            double y = (1 - t) * (1 - t) * p1.Y + 2 * (1 - t) * t * p2.Y + t * t * p3.Y;
            return new Point(x,y);
        }

        private static void ChangingPoints(Point point,double t, Point p1, Point p2, Point p3)
        {
            point.X = (1 - t) * (1 - t) * p1.X + 2 * (1 - t) * t * p2.X + t * t * p3.X;
            point.Y = (1 - t) * (1 - t) * p1.Y + 2 * (1 - t) * t * p2.Y + t * t * p3.Y;
        }
    }
}
