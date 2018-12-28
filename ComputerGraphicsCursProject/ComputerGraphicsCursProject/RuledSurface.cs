using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;
using Point = ComputerGraphicsCursProject.Point;
using Graphics = System.Drawing.Graphics;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Brushes = System.Drawing.Brushes;

namespace ComputerGraphicsCursProject
{
    class RuledSurface
    {
        public RuledSurface(BezierCurve b1, BezierCurve b2, BezierCurve b3, BezierCurve b4)
        {
            this.b1 = b1;
            this.b2 = b2;
            this.b3 = b3;
            this.b4 = b4;

            //l1 = new Line(b1.DrawingPoints[0], b2.DrawingPoints[0], b1.DrawingPoints.Length);
            //l2 = new Line(b1.DrawingPoints[b1.DrawingPoints.Length - 1], b2.DrawingPoints[b1.DrawingPoints.Length - 1], b1.DrawingPoints.Length);
        }

        public void Draw(Matrix preobr, Graphics g, int dataPointCount, bool withMarkers)
        {
            Point[,] points = new Point[b1.DrawingPoints.Length, b1.DrawingPoints.Length];
            BezierCurve tempb1, tempb2, tempb3, tempb4;
            tempb1 = preobr * b1;
            tempb2 = preobr * b2;
            tempb3 = preobr * b3;
            tempb4 = preobr * b4;

            Pen pen = new Pen(Color.AliceBlue, 1f);

            double dt = 1f / (tempb1.DrawingPoints.Length - 2);

            for (int i = 0; i < tempb1.DrawingPoints.Length; i++)
            {
                for (int j = 0; j < tempb1.DrawingPoints.Length; j++)
                {
                    double t1 = i * dt;
                    double t2 = j * dt;

                    Point c0 = b2.B(t2) * (1 - t1) + b1.B(t2) * t1;
                    Point c1 = b3.B(t1) * (1 - t2) + b4.B(t1) * t2;
                    Point c2 = (b3.B(0) * (1 - t1) + b1.B(0) * t1) * (1 - t2) + (b4.B(0) * (1 - t1) + b1.B(1) * t1) * t2;

                    double x = c0.x + c1.x - c2.x;
                    double y = c0.y + c1.y - c2.y;
                    double z = c0.z + c1.z - c2.z;
                    points[i, j] = new Point(x, y, z, 1);
                }
            }

            for (int i = 0; i < tempb1.DrawingPoints.Length - 1; i++)
            {
                for (int j = 0; j < tempb1.DrawingPoints.Length - 2; j++)
                {
                    Point point1 = preobr * points[i, j];
                    Point point2 = preobr * points[i, j + 1];
                    g.DrawLine(pen, (int)point1.x, (int)point1.y, (int)point2.x, (int)point2.y);
                }
            }


            b1.Draw(preobr, g);
            b2.Draw(preobr, g);
            b3.Draw(preobr, g);
            b4.Draw(preobr, g);

            if (withMarkers)
            {
                b1.DrawMarkers(preobr, g);
                b2.DrawMarkers(preobr, g);
                b3.DrawMarkers(preobr, g);
                b4.DrawMarkers(preobr, g);
                
                if (dataPointCount != 12)
                {
                    RectangleF rectangle = new RectangleF();

                    if (dataPointCount < 3)
                    {
                        rectangle = new RectangleF((int)tempb1.DataPoints[dataPointCount].x - 5, (int)tempb1.DataPoints[dataPointCount].y - 5, (float)10, (float)10);
                    }
                    if (dataPointCount > 2 && dataPointCount < 6)
                    {
                        rectangle = new RectangleF((int)tempb2.DataPoints[dataPointCount - 3].x - 5, (int)tempb2.DataPoints[dataPointCount - 3].y - 5, (float)10, (float)10);
                    }
                    if (dataPointCount > 5 && dataPointCount < 9)
                    {
                        rectangle = new RectangleF((int)tempb3.DataPoints[dataPointCount - 6].x - 5, (int)tempb3.DataPoints[dataPointCount - 6].y - 5, (float)10, (float)10);
                    }
                    if (dataPointCount > 8 && dataPointCount < 12)
                    {
                        rectangle = new RectangleF((int)tempb4.DataPoints[dataPointCount - 9].x - 5, (int)tempb4.DataPoints[dataPointCount - 9].y - 5, (float)10, (float)10);
                    }
                    g.FillEllipse(Brushes.Yellow, rectangle);
                }
            }
        }

        BezierCurve b1, b2, b3, b4;
        //Line l1, l2;
    }
}

