using ScheduleSim.ViewModels;
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

namespace ScheduleSim.Views
{
    /// <summary>
    /// Interaction logic for ProjectSettingPage.xaml
    /// </summary>
    public partial class ProjectSettingPage : Page
    {
        private ProjectSettingPageViewModel viewModel;

        public ProjectSettingPage(ProjectSettingPageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.viewModel = viewModel;
        }

        private void Process_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.viewModel.ProcessChangeCommand.Execute(new[] { sender, e});
        }

        private void Function_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.viewModel.FunctionChangeCommand.Execute(new[] { sender, e });
        }

        private void Period_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.PeriodChangeCommand.Execute(new[] { sender, e });
        }
    }
}
