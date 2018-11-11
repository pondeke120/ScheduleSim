using Microsoft.Practices.Unity;
using Prism.Ioc;
using Prism.Regions;
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
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar : UserControl
    {
        private IContainerExtension container;
        private IRegionManager regionManager;
        private SidebarViewModel viewModel;

        public Sidebar(IContainerExtension container, IRegionManager regionManager, SidebarViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.container = container;
            this.regionManager = regionManager;
            this.viewModel = viewModel;

            this.Loaded += Sidebar_Loaded;
        }

        private void Sidebar_Loaded(object sender, RoutedEventArgs e)
        {
            BindingViews(this.viewModel.Items);
        }

        private void Sidebar_Click(object sender, RoutedEventArgs e)
        {
            //activate view a
            var selectedView = this.viewModel?.SelectedItem?.BindingView;
            if (selectedView != null)
            {
                regionManager.Regions["Main"].Activate(selectedView);
            }
        }

        private void BindingViews(List<SidebarItemViewModel> items)
        {
            if (items == null) return;
            foreach (var vm in items)
            {
                try
                {
                    var type = Type.GetType($"{this.GetType().Namespace}.{vm.ViewName}");
                    vm.BindingView = container.Resolve(type);
                }
                catch (Exception)
                {
                    Console.Error.WriteLine($"ビューの名前解決に失敗:{vm.Name} -> {vm.ViewName}");
                }
                BindingViews(vm.Children);
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //activate view a
            
            var selectedView = (e.NewValue as SidebarItemViewModel)?.BindingView;
            if (selectedView != null)
            {
                regionManager.Regions["Main"].Activate(selectedView);
            }
        }
    }
}
