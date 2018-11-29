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
    /// Interaction logic for WbsPage.xaml
    /// </summary>
    public partial class WbsPage : Page
    {
        private WbsPageViewModel viewModel;

        public WbsPage(WbsPageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.viewModel = viewModel;
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.StartDateChangeCommand.Execute(new[] { sender, e });
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.EndDateChangeCommand.Execute(new[] { sender, e });
        }

        private void dataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            this.viewModel.AddTaskCommand.Execute(new[] { sender, e });
        }

        private void Process_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.ProcessChangeCommand.Execute(new object[] { sender, e });
        }

        private void Function_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.FunctionChangeCommand.Execute(new object[] { sender, e });
        }

        private void TaskName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.viewModel.TaskNameChangeCommand.Execute(new object[] { sender, e });
        }

        private void PlanValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.viewModel.PlanValueChangeCommand.Execute(new object[] { sender, e });
        }

        private void AssignMember_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.AssignMemberChangeCommand.Execute(new object[] { sender, e });
        }
    }
}
