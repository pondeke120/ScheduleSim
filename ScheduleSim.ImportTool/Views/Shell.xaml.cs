using Prism.Ioc;
using Prism.Regions;
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
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
        public Shell()
        {
            InitializeComponent();
        }

        private IRegionManager regionManager;
        private IContainerExtension container;
        private ShellViewModel viewModel;

        public Shell(
            IContainerExtension container,
            IRegionManager regionManager,
            ShellViewModel viewModel)
        {
            InitializeComponent();
            this.regionManager = regionManager;
            this.container = container;
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            // タイトルを適当に設定
            this.viewModel.Title = "インポート操作ツール";
            //view discovery
            regionManager.RegisterViewWithRegion("Main", typeof(ImportPage));
        }
    }
}
