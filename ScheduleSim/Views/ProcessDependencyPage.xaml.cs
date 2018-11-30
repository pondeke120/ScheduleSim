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
    /// Interaction logic for ProcessDependencyPage.xaml
    /// </summary>
    public partial class ProcessDependencyPage : Page
    {
        private ProcessDependencyPageViewModel viewModel;

        public ProcessDependencyPage(ProcessDependencyPageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.viewModel = viewModel;
        }

        private void dataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            this.viewModel.AddDependencyCommand.Execute(new[] { sender, e });
        }

        private void SrcProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.SrcProcessChangeCommand.Execute(new object[] { sender, e });
        }

        private void DstProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.DstProcessChangeCommand.Execute(new object[] { sender, e });
        }

        private void DependencyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.DependencyTypeChangeCommand.Execute(new object[] { sender, e });
        }

    }
}
