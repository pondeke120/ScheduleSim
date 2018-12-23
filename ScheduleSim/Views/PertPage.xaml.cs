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
    /// Interaction logic for PertPage.xaml
    /// </summary>
    public partial class PertPage : Page
    {
        private PertPageViewModel viewModel;

        public PertPage(PertPageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.viewModel = viewModel;
        }

        private void dataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            this.viewModel.AddEdgeCommand.Execute(new[] { sender, e });
        }

        private void Process_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.ProcessChangeCommand.Execute(new object[] { sender, e });
        }

        private void Function_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.FunctionChangeCommand.Execute(new object[] { sender, e });
        }

        private void Task_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.TaskChangeCommand.Execute(new object[] { sender, e });
        }

        private void INode_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.viewModel.SrcNodeChangeCommand.Execute(new object[] { sender, e });
        }

        private void JNode_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.viewModel.DstNodeChangeCommand.Execute(new object[] { sender, e });
        }

        private void Task_DropDownOpened(object sender, EventArgs e)
        {
            this.viewModel.TaskSelectionSourceFilterCommand.Execute(new object[] { sender, this.viewModel });
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            // オーバーフローパネルを非表示にする
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }
    }
}
