using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using MathNet.Numerics;

namespace WpfMvvmExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {

        public enum RadioOptions { Linear, Exponential, PowerFunction }

        public RadioOptions FitType { get; set; } = RadioOptions.Linear;

        private readonly MainViewModel _mainViewModel;

        IList<ScatterPoint> Points = new List<ScatterPoint>();
        double[] xdata = new double[0];
        double[] ydata = new double[0];

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _mainViewModel = new MainViewModel();

            UploadFile();
        }

        private void recalculate_Click(object sender, RoutedEventArgs e)
        {
            Tuple<List<DataPoint>, Tuple<double, double>> data = new Tuple<List<DataPoint>, Tuple<double, double>>(new List<DataPoint>(), new Tuple<double, double>(0, 0));
            switch (FitType)
            {
                case RadioOptions.Linear:
                    data = LinearFit(xdata, ydata);
                    break;
                case RadioOptions.Exponential:
                    data = ExponentialFit(xdata, ydata);
                    break;
                case RadioOptions.PowerFunction:
                    data = PowerFunctionFit(xdata, ydata);
                    break;
            }

            //update DataContext
            _mainViewModel.Points = Points;
            _mainViewModel.FitPoints = data.Item1;
            _mainViewModel.CoefficientA = data.Item2.Item1;
            _mainViewModel.CoefficientB = data.Item2.Item2;
            _mainViewModel.Enabled = true;
        }

        private void uploadFile_Click(object sender, RoutedEventArgs e)
        {
            UploadFile();
        }

        private void UploadFile()
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                try
                {
                    string[] lines = File.ReadAllLines(dialog.FileName);
                    _mainViewModel.Points.Clear();

                    Application.Current.Dispatcher.Invoke(delegate
                    {

                        Points = new List<ScatterPoint>();
                        xdata = new double[lines.Length];
                        ydata = new double[lines.Length];

                        for (int i = 0; i < lines.Length; i++)
                        {
                            var array = lines[i].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                            Points.Add(new ScatterPoint(double.Parse(array[0]), double.Parse(array[1])));
                            xdata[i] = double.Parse(array[0]);
                            ydata[i] = double.Parse(array[1]);
                        }

                        Tuple<List<DataPoint>, Tuple<double, double>> data = new Tuple<List<DataPoint>, Tuple<double, double>>(new List<DataPoint>(), new Tuple<double, double>(0, 0));
                        switch (FitType)
                        {
                            case RadioOptions.Linear:
                                data = LinearFit(xdata, ydata);
                                break;
                            case RadioOptions.Exponential:
                                data = ExponentialFit(xdata, ydata);
                                break;
                            case RadioOptions.PowerFunction:
                                data = PowerFunctionFit(xdata, ydata);
                                break;
                        }

                        //update DataContext
                        _mainViewModel.Points = Points;
                        _mainViewModel.FitPoints = data.Item1;
                        _mainViewModel.CoefficientA = data.Item2.Item1;
                        _mainViewModel.CoefficientB = data.Item2.Item2;
                        _mainViewModel.Enabled = true;
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Not a Valid File!", "Warning", MessageBoxButton.OK);
                }
            }
        }

        private static Tuple<List<DataPoint>, Tuple<double, double>> LinearFit(double[] xdata, double[] ydata)
        {
            List<DataPoint> FitPoints = new List<DataPoint>();

            Tuple<double, double> p = Fit.Line(xdata, ydata);
            double a = p.Item1;
            double b = p.Item2;

            for (int i = 0; i < xdata.Length; i++)
            {
                FitPoints.Add(new DataPoint(xdata[i], (b * xdata[i]) + a));
            }

            return new Tuple<List<DataPoint>, Tuple<double, double>>(FitPoints, new Tuple<double, double>(a, b));
        }

        private static Tuple<List<DataPoint>, Tuple<double, double>> ExponentialFit(double[] xdata, double[] ydata)
        {
            List<DataPoint> FitPoints = new List<DataPoint>();

            Tuple<double, double> p = Fit.Exponential(xdata, ydata);
            double a = p.Item1;
            double b = p.Item2;

            for (int i = 0; i < xdata.Length; i++)
            {
                FitPoints.Add(new DataPoint(xdata[i], a * Math.Exp(b * xdata[i])));
            }

            return new Tuple<List<DataPoint>, Tuple<double, double>>(FitPoints, new Tuple<double, double>(a, b));
        }

        private static Tuple<List<DataPoint>, Tuple<double, double>> PowerFunctionFit(double[] xdata, double[] ydata)
        {
            List<DataPoint> FitPoints = new List<DataPoint>();

            Tuple<double, double> p = Fit.Power(xdata, ydata);
            double a = p.Item2;
            double b = p.Item1;

            for (int i = 0; i < xdata.Length; i++)
            {
                FitPoints.Add(new DataPoint(xdata[i], a * Math.Pow(xdata[i], b)));
            }

            return new Tuple<List<DataPoint>, Tuple<double, double>>(FitPoints, new Tuple<double, double>(a, b));
        }
    }


}