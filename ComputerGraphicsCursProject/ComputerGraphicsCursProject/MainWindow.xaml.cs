using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;


namespace ComputerGraphicsCursProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PictureBox form = new PictureBox();
        private double lastX=0;
        private double lastY = 0;
        private double lastZ = 0;
        private SlideStates[] states= new SlideStates[6];
        private SlideStates[] lasts = new SlideStates[6];
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += OnKeyDown;
            dataPointCount = 0;
            withMarkers = true;
            for (int i = 0; i < 6; ++i)
            {
                states[i]=new SlideStates();
                lasts[i]=new SlideStates();
            }
            //panelOfApproximation.Enabled = checkBoxOfMerkersEnabled.Checked;
            //panelOfDataPoints.Enabled = checkBoxOfMerkersEnabled.Checked;
            textBoxOfNumberOfDrawPoints.Text = Convert.ToString(40);

            dataPoints1 = new Point[3];
            Point point11 = new Point(0, 0, 0, 1); // точки дефолтные 
            Point point12 = new Point(1, -1, 0, 1);
            Point point14 = new Point(3, -1, 0, 1);

            dataPoints1[0] = point11;
            dataPoints1[1] = point12;
            dataPoints1[2] = point14;

            dataPoints2 = new Point[3];
            Point point21 = new Point(0, 0, 1, 1);
            Point point22 = new Point(1, -1, 1, 1);
            Point point24 = new Point(3, -1, 1, 1);

            dataPoints2[0] = point21;
            dataPoints2[1] = point22;
            dataPoints2[2] = point24;

            dataPoints3 = new Point[3];
            Point point31 = new Point(0, 0, 1, 1);
            Point point32 = new Point(0, 0, 0.5, 1);
            Point point34 = new Point(0, 0, 0, 1);

            dataPoints3[0] = point31;
            dataPoints3[1] = point32;
            dataPoints3[2] = point34;

            dataPoints4 = new Point[3];
            Point point41 = new Point(3, -1, 1, 1);
            Point point42 = new Point(3, -1, 0.5, 1);
            Point point44 = new Point(3, -1, 0, 1);

            dataPoints4[0] = point41;
            dataPoints4[1] = point42;
            dataPoints4[2] = point44;

            this.CalcCurves();

            mx = 0;
            my = 0;
            cx = 0;
            cy = 0;

            scale = 100;
            mashtabK = 0;

            isMouseDown = false;
        }
        void OnKeyDown(object o, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                SliderX.ValueChanged -= SliderX_OnValueChanged;
                SliderY.ValueChanged -= SliderY_OnValueChanged;
                SliderZ.ValueChanged -= SliderZ_OnValueChanged;
                lasts[dataPointCount].X=lastX;
                lasts[dataPointCount].Y = lastY;
                lasts[dataPointCount].Z = lastZ;
                states[dataPointCount].X = SliderX.Value;
                states[dataPointCount].Y = SliderY.Value;
                states[dataPointCount].Z = SliderZ.Value;
                dataPointCount = dataPointCount < 5 ? dataPointCount + 1 : 0;
                SliderX.Value = states[dataPointCount].X;
                SliderY.Value = states[dataPointCount].Y;
                SliderZ.Value = states[dataPointCount].Z;
                lastX = lasts[dataPointCount].X;
                lastY = lasts[dataPointCount].Y; 
                lastZ = lasts[dataPointCount].Z;
                SliderX.ValueChanged += SliderX_OnValueChanged;
                SliderY.ValueChanged += SliderY_OnValueChanged;
                SliderZ.ValueChanged += SliderZ_OnValueChanged;
                form.Refresh();
            }
        }
        private void HostInitialized(object sender, EventArgs e)
        {
            
            ((WindowsFormsHost)sender).Child = form;
            Host.Child.Paint += Form1_Paint;
            Host.Child.MouseDown += Form1_MouseDown;
            Host.Child.MouseMove += Form1_MouseMove;
            Host.Child.MouseWheel += Form1_MouseWheel;
            Host.Child.MouseUp += Form1_MouseUp;
            Host.Child.SizeChanged += Form1_SizeChanged;

        }

        private void CalcCurves()
        {
            bezierCurve1 = new BezierCurve(dataPoints1, int.Parse(textBoxOfNumberOfDrawPoints.Text), false);
            bezierCurve2 = new BezierCurve(dataPoints2, int.Parse(textBoxOfNumberOfDrawPoints.Text), false);
            bezierCurve3 = new BezierCurve(dataPoints3, int.Parse(textBoxOfNumberOfDrawPoints.Text), true);
            bezierCurve4 = new BezierCurve(dataPoints4, int.Parse(textBoxOfNumberOfDrawPoints.Text), true);
            kuntzSurface = new RuledSurface(bezierCurve1, bezierCurve2, bezierCurve3, bezierCurve4);
        }

        

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            double zoomLevel = (scale + mashtabK) / 1000;
            double coeff = Math.Max(e.ClipRectangle.Width, e.ClipRectangle.Height) * zoomLevel;

            ShiftMatrix sh = new ShiftMatrix(e.ClipRectangle.Width / 2f, e.ClipRectangle.Height / 2f, 0);
            ScalingMatrix sc = new ScalingMatrix(coeff, coeff, coeff);
            RotationMatrix rtx = new RotationMatrix('X', my * Math.PI / 180.0);
            RotationMatrix rty = new RotationMatrix('Y', -mx * Math.PI / 180.0);
            Matrix preobr = sh * rtx * rty * sc;

            kuntzSurface.Draw(preobr, e.Graphics, dataPointCount, withMarkers);

        }

        private void Form1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (isMouseDown)
            {
                int deltaX = e.X - cx;
                int deltaY = e.Y - cy;
                mx += deltaX;
                my += deltaY;
                cx = e.X;
                cy = e.Y;
                form.Refresh();
            }
        }

        private void Form1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mashtabK += e.Delta;
            form.Refresh();
        }

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isMouseDown = true;
            cx = e.X;
            cy = e.Y;
        }

        private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            form.Refresh();
        }

        private void buttonOfApply1_Click(object sender, EventArgs e)
        {
            this.CalcCurves();
            form.Refresh();
            //ExtensionMethods.Refresh(this);
        }
       
        // текущие координаты курсора и координаты его предыдущего положения

        int mx, my, cx, cy;

        // масштаб

        float scale;
        double mashtabK;

        Point[] dataPoints1;
        Point[] dataPoints2;
        Point[] dataPoints3;
        Point[] dataPoints4;
        int dataPointCount;

        bool isMouseDown;

        BezierCurve bezierCurve1;
        BezierCurve bezierCurve2;
        BezierCurve bezierCurve3;
        BezierCurve bezierCurve4;


        RuledSurface kuntzSurface;

        // LinearSurface linearSurface;
        bool withMarkers;
        
        private void SliderX_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double delta = SliderX.Value - lastX;
            if (dataPointCount < 3) dataPoints1[dataPointCount].x += delta;
            if (dataPointCount > 2 && dataPointCount < 6) dataPoints2[dataPointCount - 3].x += delta;
            if (dataPointCount > 5 && dataPointCount < 9) dataPoints3[dataPointCount - 6].x += delta;
            if (dataPointCount > 8 && dataPointCount < 12) dataPoints4[dataPointCount - 9].x += delta;

            if (dataPointCount == 0) dataPoints3[dataPointCount + 2].x += delta;
            if (dataPointCount == 2) dataPoints4[dataPointCount].x += delta;
            if (dataPointCount == 5) dataPoints4[dataPointCount - 5].x += delta;
            if (dataPointCount == 3) dataPoints3[dataPointCount - 3].x += delta;

            if (dataPointCount == 8) dataPoints1[dataPointCount - 8].x += delta;
            if (dataPointCount == 6) dataPoints2[dataPointCount - 6].x += delta;
            if (dataPointCount == 9) dataPoints2[dataPointCount - 7].x += delta;

            if (dataPointCount == 11) dataPoints1[dataPointCount - 9].x += delta;

            lastX = SliderX.Value;
            CalcCurves();
            form.Refresh();
        }

        private void SliderY_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double delta = SliderY.Value - lastY;
            if (dataPointCount < 3) dataPoints1[dataPointCount].y += delta;
            if (dataPointCount > 2 && dataPointCount < 6) dataPoints2[dataPointCount - 3].y +=delta;
            if (dataPointCount > 5 && dataPointCount < 9) dataPoints3[dataPointCount - 6].y +=delta;
            if (dataPointCount > 8 && dataPointCount < 12) dataPoints4[dataPointCount - 9].y +=delta;

            if (dataPointCount == 0) dataPoints3[dataPointCount + 2].y +=delta;
            if (dataPointCount == 2) dataPoints4[dataPointCount].y +=delta;
            if (dataPointCount == 5) dataPoints4[dataPointCount - 5].y +=delta;
            if (dataPointCount == 3) dataPoints3[dataPointCount - 3].y +=delta;

            if (dataPointCount == 8) dataPoints1[dataPointCount - 8].y +=delta;
            if (dataPointCount == 6) dataPoints2[dataPointCount - 6].y +=delta;
            if (dataPointCount == 9) dataPoints2[dataPointCount - 7].y +=delta;

            if (dataPointCount == 11) dataPoints1[dataPointCount - 9].y +=delta;

            lastY = SliderY.Value;
            CalcCurves();
            form.Refresh();
        }

        private void SliderZ_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double delta = SliderZ.Value - lastZ;
            if (dataPointCount < 3) dataPoints1[dataPointCount].z +=delta;
            if (dataPointCount > 2 && dataPointCount < 6) dataPoints2[dataPointCount - 3].z +=delta;
            if (dataPointCount > 5 && dataPointCount < 9) dataPoints3[dataPointCount - 6].z +=delta;
            if (dataPointCount > 8 && dataPointCount < 12) dataPoints4[dataPointCount - 9].z +=delta;

            if (dataPointCount == 0) dataPoints3[dataPointCount + 2].z +=delta;
            if (dataPointCount == 2) dataPoints4[dataPointCount].z +=delta;
            if (dataPointCount == 5) dataPoints4[dataPointCount - 5].z +=delta;
            if (dataPointCount == 3) dataPoints3[dataPointCount - 3].z +=delta;

            if (dataPointCount == 8) dataPoints1[dataPointCount - 8].z +=delta;
            if (dataPointCount == 6) dataPoints2[dataPointCount - 6].z +=delta;
            if (dataPointCount == 9) dataPoints2[dataPointCount - 7].z +=delta;

            if (dataPointCount == 11) dataPoints1[dataPointCount - 9].z +=delta;

            lastZ = SliderZ.Value;
            CalcCurves();
            form.Refresh();
        }
    }
}