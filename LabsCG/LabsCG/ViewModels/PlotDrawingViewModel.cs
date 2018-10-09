namespace LabsCG.ViewModels
{
    using LabsCG.DTO;
    using LabsCG.Models;
    using System.Collections.Generic;

    public class PlotDrawingViewModel : BaseViewModel
    {
        private List<Point> points;
        private double parameter;

        public List<Point> Points
        {
            get => points;
            set
            {
                points = value;
                OnPropertyChanged();
            }
        }

        public double Parameter
        {
            get => parameter;
            set
            {
                if (value < 0)
                    return;

                parameter = value;
                OnPropertyChanged();
                DrawPlot();
            }
        }

        private void DrawPlot()
        {
            var calculatePoints = PlotDrawing.CalculatePoints(Parameter);
            Points = calculatePoints;
        }
    }
}