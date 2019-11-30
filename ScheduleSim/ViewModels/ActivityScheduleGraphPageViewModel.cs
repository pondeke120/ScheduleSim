using LiveCharts;
using Prism.Mvvm;
using ScheduleSim.Core.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class ActivityScheduleGraphPageViewModel : BindableBase
    {
        private AppContext appContext;

        private GraphData _graphData;
        public GraphData GraphData { get { return _graphData; } set { _graphData = value; RaisePropertyChanged("GraphData"); } }

        public ICommand DrawCommand { get; private set; }

        public ActivityScheduleGraphPageViewModel(
            AppContext appContext,
            ICommand drawCommand)
        {
            this.appContext = appContext;
            this.DrawCommand = drawCommand;
        }
    }
    
    public class GraphData
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
