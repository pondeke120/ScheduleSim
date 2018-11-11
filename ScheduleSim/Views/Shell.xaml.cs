using Prism.Ioc;
using Prism.Regions;
using ScheduleSim.ViewModels;
using System.Windows;

namespace ScheduleSim.Views
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : Window
    {
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
            //view discovery
            regionManager.RegisterViewWithRegion("Menu", typeof(Menu));
            regionManager.RegisterViewWithRegion("Sidebar", typeof(Sidebar));
            //regionManager.RegisterViewWithRegion("Main", typeof(ProjectSettingPage));

            this.Loaded += Shell_Loaded;

        }

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            var mainRegion = regionManager.Regions["Main"];
            mainRegion.Add(container.Resolve<ProjectSettingPage>());
            mainRegion.Add(container.Resolve<MemberPage>());
            mainRegion.Add(container.Resolve<WbsPage>());
            mainRegion.Add(container.Resolve<ProcessDependencyPage>());
            mainRegion.Add(container.Resolve<FunctionDependencyPage>());
            mainRegion.Add(container.Resolve<PertPage>());
            mainRegion.Add(container.Resolve<EntireSchedulePage>());
            mainRegion.Add(container.Resolve<PlanValuePage>());
            mainRegion.Add(container.Resolve<GanttChartGraphPage>());
            mainRegion.Add(container.Resolve<ActivityScheduleGraphPage>());
            mainRegion.Add(container.Resolve<PertGraphPage>());
        }
    }
}
