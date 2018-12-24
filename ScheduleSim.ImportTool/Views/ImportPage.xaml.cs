using ScheduleSim.ImportTool.ViewModels;
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

namespace ScheduleSim.ImportTool.Views
{
    /// <summary>
    /// Interaction logic for ImportPage.xaml
    /// </summary>
    public partial class ImportPage : UserControl
    {
        private ImportPageViewModel viewModel;

        public ImportPage(ImportPageViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;

            this.DataContext = viewModel;
        }
    }
}
