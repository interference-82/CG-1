using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphicsCursProject
{
    class Point
    {
        
        public Point(double x = 0, double y = 0, double z = 0, double w = 1)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public Point(Point rhs)
        {
            this.x = rhs.x;
            this.y = rhs.y;
            this.z = rhs.z;
            this.w = rhs.w;
        }

        // операции над векторами:

        // вычитание
        static public Point operator -(Point left, Point right)
        {
            return new Point(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
        }
        //линейные операции
        static public Point operator +(Point left, Point right)
        {
            return new Point(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w);
        }

        static public Point operator *(Point left, double lyambda)
        {
            return new Point(left.x * lyambda, left.y * lyambda, left.z * lyambda, left.w * lyambda);
        }
        // присваивание
        static public Point operator ==(Point thisPoint, Point rhs)
        {
            thisPoint.x = rhs.x;
            thisPoint.y = rhs.y;
            thisPoint.z = rhs.z;
            thisPoint.w = rhs.w;
            return thisPoint;
        }

        static public Point operator !=(Point thisPoint, Point rhs)
        {
            return thisPoint;
        }
        // скалярное произведение
        static public double operator *(Point left, Point right)
        {
            return left.x * right.x + left.y * right.y + left.z * right.z;
        }
        // векторное произведение
        static public Point operator ^(Point left, Point right)
        {
            Point res = new Point();
            res.x = left.y * right.z - left.z * right.y;
            res.y = left.z * right.x - left.x * right.z;
            res.z = left.x * right.y - left.y * right.x;
            res.w = 0;

            return res;
        }

        public double Modul()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public double x, y, z, w;
    };
}

