using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using LabsCG3.DTO;

namespace LabsCG3.Views
{
     public partial class PlotCanvasView
    {
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(List<Point3D>), typeof(PlotCanvasView),
                new FrameworkPropertyMetadata(ItemSourcePropertyChangedDependency));

        public List<Point3D> ItemsSource
        {
            get => (List<Point3D>)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }
        private static void ItemSourcePropertyChangedDependency(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            ItemSourcePropertyChanged((List<Point3D>)eventArgs.NewValue);
        }

        public static readonly DependencyProperty CoefficientFDependencyProperty =
            DependencyProperty.Register("CoefficientF", typeof(double), typeof(PlotCanvasView),
                new FrameworkPropertyMetadata(CoefficientFChangedDependency));

        public double CoefficientF
        {
            get => (double)GetValue(CoefficientFDependencyProperty);
            set => SetValue(CoefficientFDependencyProperty, value);
        }

        private static void CoefficientFChangedDependency(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            CoefficientFPropertyChanged((double)eventArgs.NewValue);
        }

        public static readonly DependencyProperty CoefficientSDependencyProperty =
            DependencyProperty.Register("CoefficientS", typeof(double), typeof(PlotCanvasView),
                new FrameworkPropertyMetadata(CoefficientSChangedDependency));

        public double CoefficientS
        {
            get => (double)GetValue(CoefficientSDependencyProperty);
            set => SetValue(CoefficientSDependencyProperty, value);
        }

        private static void CoefficientSChangedDependency(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            CoefficientSPropertyChanged((double)eventArgs.NewValue);
        }

    }
}