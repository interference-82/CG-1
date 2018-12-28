using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Graphics = System.Drawing.Graphics;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;

namespace ComputerGraphicsCursProject
{
    class Line
    {
        public Line(Point first, Point second, int numberOfDrawPoints)
        {
            dataPoints = new Point[2];
            dataPoints[0] = first;
            dataPoints[1] = second;
            this.numberOfDrawPoints = numberOfDrawPoints;

            Invalidate();
        }

        public void Invalidate()
        {
            drawingPoints = new Point[numberOfDrawPoints + 1];
            double dt = 1f / numberOfDrawPoints;
            double t = 0f;
            for (int i = 0; i <= numberOfDrawPoints; i++)
            {
                drawingPoints[i] = L(t);
                t += dt;
            }
        }

        public Point L(double t)
        {
            double x = dataPoints[0].x + t * (dataPoints[1] - dataPoints[0]).x;
            double y = dataPoints[0].y + t * (dataPoints[1] - dataPoints[0]).y;
            double z = dataPoints[0].z + t * (dataPoints[1] - dataPoints[0]).z;
            return new Point(x, y, z, 1);
        }

        public static Line operator *(Matrix m, Line l)
        {
            Point[] dataPoints1 = new Point[2];
            for (int i = 0; i < 2; i++)
            {
                dataPoints1[i] = m * l.dataPoints[i];
            }
            return new Line(dataPoints1[0], dataPoints1[1], l.numberOfDrawPoints);
        }

        public void Draw(Matrix preobr, Graphics g)
        {
            Pen pen = new Pen(Color.Red, 2f);
            Line l;
            l = preobr * this;

            for (int i = 0; i < drawingPoints.Length - 1; i++)
            {
                g.DrawLine(pen, (int) l.drawingPoints[i].x, (int) l.drawingPoints[i].y, (int) l.drawingPoints[i + 1].x,
                    (int) l.drawingPoints[i + 1].y);
            }
        }


        private int numberOfDrawPoints;
        public Point[] dataPoints;
        public Point[] drawingPoints;
    }
}