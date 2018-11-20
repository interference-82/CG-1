using System.Collections.Generic;
using LabsCG2.DTO;
using LabsCG2.Models;

namespace LabsCG2.ViewModels
{
    public class PlotDrawingViewModel : BaseViewModel
    {
        private List<Point3D> lastPoints = Obelisk.Points;
        private List<Point3D> points = Obelisk.Points;

        private double lastXAxisAngle;
        private double lastYAxisAngle;
        private double lastZAxisAngle;
        
        private double scale = 1;
        private double xAxisAngle = 180;
        private double yAxisAngle = 180;
        private double zAxisAngle = 180;

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
                OnPropertyChanged();
                Scaling();
            }
        }

        public List<Point3D> Points
        {
            get => points;
            set
            {
                points = value;
                OnPropertyChanged();
            }
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