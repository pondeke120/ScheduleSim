using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using ScheduleSim.ViewModels;

namespace ScheduleSim.Views
{
    /// <summary>
    /// Interaction logic for ActivityScheduleGraphPage.xaml
    /// </summary>
    public partial class ActivityScheduleGraphPage : Page
    {

        public ActivityScheduleGraphPage(ActivityScheduleGraphPageViewModel viewModel)
        {
            InitializeComponent();

            this.ViewModel = viewModel;
            this.DataContext = viewModel;
        }

        public ActivityScheduleGraphPageViewModel ViewModel { get; private set; }
    }
}
