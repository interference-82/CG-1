using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabsCG3.DTO
{
    public class TransferMatrix
    {
        public double[,] MatrixX;
        public double[,] MatrixY;
        public double[,] MatrixZ;
        public double[,] MatrixScale;

        TransferMatrix(double xAngle, double yAngle, double zAngle, double scale)
        {
            MatrixX = new[,]
            {
                {1, 0, 0},
                {0, Math.Cos(xAngle * Math.PI / Constans.LinearPi), -Math.Sin(xAngle * Math.PI / Constans.LinearPi)},
                {0, Math.Sin(xAngle * Math.PI / Constans.LinearPi), Math.Cos(xAngle * Math.PI / Constans.LinearPi)}
            };
            MatrixY = new [,]
            {
                {Math.Cos(yAngle * Math.PI /  Constans.LinearPi), 0, Math.Sin(yAngle * Math.PI /  Constans.LinearPi)},
                {0, 1, 0},
                {-Math.Sin(yAngle * Math.PI /  Constans.LinearPi), 0, Math.Cos(yAngle * Math.PI /  Constans.LinearPi)}
            };
            MatrixZ = new [,]
            {
                {Math.Cos(zAngle * Math.PI /  Constans.LinearPi), -Math.Sin(zAngle * Math.PI /  Constans.LinearPi), 0},
                {Math.Sin(zAngle * Math.PI /  Constans.LinearPi), Math.Cos(zAngle * Math.PI /  Constans.LinearPi), 0},
                {0, 0, 1}
            };
            MatrixScale = new double[,]
            {
                {scale, 0, 0},
                {0, scale, 0},
                {0, 0, scale}
            };
        }
    }
}
