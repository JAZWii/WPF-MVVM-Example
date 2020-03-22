using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;

namespace WpfMvvmExample
{
    class ChartData
    {
        public string Title { get; set; }
        public IList<ScatterPoint> Points { get; set; }
        public IList<DataPoint> FitPoints { get; set; }
        public double CoefficientA { get; set; } = 0;
        public double CoefficientB { get; set; } = 0;
        public bool Enabled { get; set; } = false;
    }
}
