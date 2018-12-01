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
    /// Interaction logic for FunctionDependencyPage.xaml
    /// </summary>
    public partial class FunctionDependencyPage : Page
    {
        private FunctionDependencyPageViewModel viewModel;

        public FunctionDependencyPage(FunctionDependencyPageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.viewModel = viewModel;
        }
        
        private void dataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            this.viewModel.AddDependencyCommand.Execute(new[] { sender, e });
        }

        private void SrcFunction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.SrcFunctionChangeCommand.Execute(new object[] { sender, e });
        }

        private void DstFunction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.DstFunctionChangeCommand.Execute(new object[] { sender, e });
        }

        private void DependencyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.DependencyTypeChangeCommand.Execute(new object[] { sender, e });
        }
    }
}
