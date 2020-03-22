using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.ComponentModel;

namespace WpfMvvmExample
{
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        private ChartData chartData;

        public string Title
        {
            get { return chartData.Title; }
            set
            {
                if (chartData.Title != value)
                {
                    chartData.Title = value;
                    OnPropertyChange("Title");
                }
            }
        }
        public IList<ScatterPoint> Points
        {
            get { return chartData.Points; }
            set
            {
                if (chartData.Points != value)
                {
                    chartData.Points = value;
                    OnPropertyChange("Points");
                }
            }
        }
        public IList<DataPoint> FitPoints
        {
            get { return chartData.FitPoints; }
            set
            {
                if (chartData.FitPoints != value)
                {
                    chartData.FitPoints = value;
                    OnPropertyChange("FitPoints");
                }
            }
        }
        public double CoefficientA
        {
            get { return chartData.CoefficientA; }
            set
            {
                if (chartData.CoefficientA != value)
                {
                    chartData.CoefficientA = value;
                    OnPropertyChange("CoefficientA");
                }
            }
        }
        public double CoefficientB
        {
            get { return chartData.CoefficientB; }
            set
            {
                if (chartData.CoefficientB != value)
                {
                    chartData.CoefficientB = value;
                    OnPropertyChange("CoefficientB");
                }
            }
        }
        public bool Enabled
        {
            get { return chartData.Enabled; }
            set
            {
                if (chartData.Enabled != value)
                {
                    chartData.Enabled = value;
                    OnPropertyChange("Enabled");
                }
            }
        }

        public MainViewModel()
        {
            chartData = new ChartData
            {
                Title = "Fitting Graph",
                Points = new List<ScatterPoint>()
            };
        }

        public MainViewModel(string title, IList<ScatterPoint> points)
        {
            chartData = new ChartData
            {
                Title = title,
                Points = points
            };
        }

        public MainViewModel(string title, IList<ScatterPoint> points, List<DataPoint> fitPoints, double coefficientA, double coefficientB)
        {
            chartData = new ChartData
            {
                Title = title,
                Points = points,
                FitPoints = fitPoints,
                CoefficientA = coefficientA,
                CoefficientB = coefficientB,
                Enabled = true
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
