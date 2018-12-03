using System.Collections.Generic;
using LabsCG3.DTO;
using LabsCG3.Models;

namespace LabsCG3.ViewModels
{
    public class PlotDrawingViewModel : BaseViewModel
    {
        private List<Point3D> lastPoints = Obelisk.Points;
        private List<Point3D> points = Obelisk.Points;

        private double lastXAxisAngle;
        private double lastYAxisAngle;
        private double lastZAxisAngle;
        
        private double scale = 1;
        private double xAxisAngle = Constans.LinearPi;
        private double yAxisAngle = Constans.LinearPi;
        private double zAxisAngle = Constans.LinearPi;

         
        private double width = 30;
        private double height = 100;

        private double coefficientF = 0.5;
        private double coefficientS = 0.5;

        public List<Point3D> Points
        {
            get => points;
            set
            {
                points = value;
                OnPropertyChanged();
            }
        }
        public double CoefficientS
        {
            get => coefficientS;
            set
            {
                coefficientS = value;
                OnPropertyChanged();
            }
        }

        public double CoefficientF
        {
            get => coefficientF;
            set
            {
                coefficientF = value;
                OnPropertyChanged();
            }
        }

        public double XAxisAngle
        {
            get => xAxisAngle;
            set
            {
                lastXAxisAngle = xAxisAngle;
                xAxisAngle = value;
                OnPropertyChanged();
                XAngleRotation();
                lastPoints = points;
            }
        }

        public double YAxisAngle
        {
            get => yAxisAngle;
            set
            {
                lastYAxisAngle = yAxisAngle;
                yAxisAngle = value;
                OnPropertyChanged();
                YAngleRotation();
                lastPoints = points;
            }
        }

        public double ZAxisAngle
        {
            get => zAxisAngle;
            set
            {
                lastZAxisAngle = zAxisAngle;
                zAxisAngle = value;
                OnPropertyChanged();
                ZAngleRotation();
                lastPoints = points;
            }
        }

        public double Scale
        {
            get => scale;
            set
            {
                scale = value;
                Scaling();
                OnPropertyChanged();
                
            }
        }

        public double Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged();
                WidthChange();
            }
        }
        
        public double Height
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged();
                HeightChange();
            }
        }

        private void HeightChange()
        {
            var calculatePoints = PlotDrawing.Approximating(height,width, xAxisAngle, yAxisAngle, zAxisAngle, scale);
            Points = calculatePoints;
        }
        
        private void WidthChange()
        {
            var calculatePoints = PlotDrawing.Approximating(height, width, xAxisAngle,yAxisAngle,zAxisAngle,scale);
            Points = calculatePoints;
        }

        private void Scaling()
        {
            var calculatePoints = PlotDrawing.Scaling(lastPoints, scale);
            Points = calculatePoints;
        }

        private void XAngleRotation()
        {
            var calculatePoints = PlotDrawing.XAxisRotating(points, xAxisAngle - lastXAxisAngle);
            Points = calculatePoints;
        }

        private void YAngleRotation()
        {
            var calculatePoints = PlotDrawing.YAxisRotating(points, yAxisAngle - lastYAxisAngle);
            Points = calculatePoints;
        }

        private void ZAngleRotation()
        {
            var calculatePoints = PlotDrawing.ZAxisRotating(points, zAxisAngle - lastZAxisAngle);
            Points = calculatePoints;
        }
    }
}