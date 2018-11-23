using Unity;
using Prism.Unity;
using System.Windows;
using ScheduleSim.Views;
using Prism.Regions;
using Unity.Lifetime;
using System.Windows.Controls;
using Unity.Injection;
using System.Windows.Input;
using ScheduleSim.ViewModels;
using ScheduleSim.Entities;
using ScheduleSim.Access;
using ScheduleSim.Entities.Repositories;
using ScheduleSim.Access.Repositories;
using ScheduleSim.Core.Contexts;
using System.IO;
using System;
using ScheduleSim.Core.Dispatcher;
using AutoMapper;
using ScheduleSim.Mappers;
using ScheduleSim.Core.Utility;

namespace ScheduleSim
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // Register Views
            Container.RegisterType<MemberPage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ProjectSettingPage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<WbsPage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ProcessDependencyPage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<FunctionDependencyPage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<PertPage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<EntireSchedulePage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<PlanValuePage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<GanttChartGraphPage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ActivityScheduleGraphPage>(new ContainerControlledLifetimeManager());
            Container.RegisterType<PertGraphPage>(new ContainerControlledLifetimeManager());

            // Register ViewModels
            Container.RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<MenuViewModel>(new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(
                    new ResolvedParameter<ICommand>("Menu.CreateNewProjectCommand"),
                    new ResolvedParameter<ICommand>("Menu.OpenFileCommand"),
                    new ResolvedParameter<ICommand>("Menu.SaveCommand"),
                    new ResolvedParameter<ICommand>("Menu.SaveAsCommand"),
                    new ResolvedParameter<ICommand>("Menu.ImportXlsxCommand"),
                    new ResolvedParameter<ICommand>("Menu.ExportGanttChartCommand"),
                    new ResolvedParameter<ICommand>("Menu.ExportPertGraphCommand")
                ));
            Container.RegisterType<ProjectSettingPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<IIDGenerator>("ProcessIdGen"),
                    new ResolvedParameter<IIDGenerator>("FunctionIdGen"),
                    new ResolvedParameter<IIDGenerator>("HolidayIdGen")
                ));
            Container.RegisterType<MemberPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<ICommand>("MemberPage.AddMemberCommand"),
                    new ResolvedParameter<ICommand>("MemberPage.DeleteMemberCommand")
                ));
            Container.RegisterType<WbsPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<ICommand>("WbsPage.DeleteTaskCommand")
                ));
            Container.RegisterType<ProcessDependencyPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<ICommand>("ProcessDependencyPage.DeleteDependencyCommand")
                ));
            Container.RegisterType<FunctionDependencyPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<ICommand>("FunctionDependencyPage.DeleteDependencyCommand")
                ));
            Container.RegisterType<PertPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<ICommand>("PertPage.DeleteEdgeCommand")
                ));

            // Register Commands
            Container.RegisterType<ICommand, Commands.Menu.CreateNewProjectCommand>("Menu.CreateNewProjectCommand");
            Container.RegisterType<ICommand, Commands.Menu.OpenFileCommand>("Menu.OpenFileCommand");
            Container.RegisterType<ICommand, Commands.Menu.SaveCommand>("Menu.SaveCommand",
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<IDispatcher>(),
                    new ResolvedParameter<Core.BusinessLogics.WPF.Menu.ISaveBusinessLogic>(),
                    new ResolvedParameter<ICommand>("Menu.SaveAsCommand"),
                    new ResolvedParameter<ProjectSettingPageViewModel>(),
                    new ResolvedParameter<MemberPageViewModel>(),
                    new ResolvedParameter<WbsPageViewModel>(),
                    new ResolvedParameter<ProcessDependencyPageViewModel>(),
                    new ResolvedParameter<FunctionDependencyPageViewModel>(),
                    new ResolvedParameter<ShellViewModel>(),
                    new ResolvedParameter<IMapper>()
                ));
            Container.RegisterType<ICommand, Commands.Menu.SaveAsCommand>("Menu.SaveAsCommand");
            Container.RegisterType<ICommand, Commands.Menu.ImportXlsxCommand>("Menu.ImportXlsxCommand");
            Container.RegisterType<ICommand, Commands.Menu.ExportGanttChartCommand>("Menu.ExportGanttChartCommand");
            Container.RegisterType<ICommand, Commands.Menu.ExportPertGraphCommand>("Menu.ExportPertGraphCommand");
            Container.RegisterType<ICommand, Commands.MemberPage.AddMemberCommand>("MemberPage.AddMemberCommand",
                new InjectionConstructor(
                    new ResolvedParameter<IIDGenerator>("MemberIdGen")
                ));
            Container.RegisterType<ICommand, Commands.MemberPage.DeleteMemberCommand>("MemberPage.DeleteMemberCommand");
            Container.RegisterType<ICommand, Commands.WbsPage.DeleteTaskCommand>("WbsPage.DeleteTaskCommand");
            Container.RegisterType<ICommand, Commands.ProcessDependencyPage.DeleteDependencyCommand>("ProcessDependencyPage.DeleteDependencyCommand");
            Container.RegisterType<ICommand, Commands.FunctionDependencyPage.DeleteDependencyCommand>("FunctionDependencyPage.DeleteDependencyCommand");
            Container.RegisterType<ICommand, Commands.PertPage.DeleteEdgeCommand>("PertPage.DeleteEdgeCommand");

            // Register Mappers
            Container.RegisterType<IMapper, Mapper>(new InjectionConstructor(new ResolvedParameter<IConfigurationProvider>()));
            Container.RegisterType<IConfigurationProvider, MapperConfiguration>(new InjectionConstructor(
                    new Action<IMapperConfigurationExpression>(cfg =>
                    {
                        cfg.AddProfile<ProjectSettingPagePofile>();
                        cfg.AddProfile<MemberPageProfile>();
                        cfg.AddProfile<WbsPageProfile>();
                        cfg.AddProfile<ProcessDependencyPageProfile>();
                        cfg.AddProfile<FunctionDependencyPageProfile>();
                    })
                ));

            // Register BusinessLogics
            Container.RegisterType<Core.BusinessLogics.WPF.Menu.ISaveBusinessLogic, Core.BusinessLogics.WPF.Menu.SaveBusinessLogic>();
            Container.RegisterType<Core.BusinessLogics.WPF.Menu.ISaveAsBusinessLogic, Core.BusinessLogics.WPF.Menu.SaveAsBusinessLogic>();

            // Register Services
            Container.RegisterType<Core.Service.IDbMigrationService, Core.Service.DbMigrationService>();
            Container.RegisterType<Core.Service.IDbFileAccessService, Core.Service.DbFileAccessService>();
            Container.RegisterType<Core.Service.IProjectSettingsAccessService, Core.Service.ProjectSettingsAccessService>();
            Container.RegisterType<Core.Service.IMemberAccessService, Core.Service.MemberAccessService>();
            Container.RegisterType<Core.Service.ITaskAccessService, Core.Service.TaskAccessService>();
            Container.RegisterType<Core.Service.IProcessDependencyAccessService, Core.Service.ProcessDependencyAccessService>();
            Container.RegisterType<Core.Service.IFunctionDependencyAccessService, Core.Service.FunctionDependencyAccessService>();

            // Register Database Provider
            Container.RegisterType<AccessConnectionFactory>(new HierarchicalLifetimeManager(), new InjectionConstructor(new Func<string>(() => Container.Resolve<AppContext>().ProjectDbFile)));
            Container.RegisterType<IDbConnectionFactory, AccessConnectionFactory>();
            Container.RegisterType<IDbVersionRepository, DbVersionRepository>();
            Container.RegisterType<IDependencyTypeRepository, DependencyTypeRepository>();
            Container.RegisterType<IFunctionDependencyRepository, FunctionDependencyRepository>();
            Container.RegisterType<IFunctionRepository, FunctionRepository>();
            Container.RegisterType<IHolidayRepository, HolidayRepository>();
            Container.RegisterType<IMemberRepository, MemberRepository>();
            Container.RegisterType<IPertRepository, PertRepository>();
            Container.RegisterType<IPrjSettingsRepository, PrjSettingsRepository>();
            Container.RegisterType<IProcessDependencyRepository, ProcessDependencyRepository>();
            Container.RegisterType<IProcessRepository, ProcessRepository>();
            Container.RegisterType<ITaskRepository, TaskRepository>();
            Container.RegisterType<IWeekdayRepository, WeekdayRepository>();

            // Register Dispatcher
            Container.RegisterType<IDispatcher, TransactionDispatcher>();

            // Register Contexts
            Container.RegisterInstance(new AppContext()
            {
                IsSaved = false,
                MasterDbFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "master.accdb"),
                ProjectFolder = Path.GetTempPath(),
                ProjectDbFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
            });

            // Register Utilities
            Container.RegisterInstance<IIDGenerator>("ProcessIdGen", new IDGenerator());
            Container.RegisterInstance<IIDGenerator>("FunctionIdGen", new IDGenerator());
            Container.RegisterInstance<IIDGenerator>("HolidayIdGen", new IDGenerator());
            Container.RegisterInstance<IIDGenerator>("MemberIdGen", new IDGenerator());
            Container.RegisterInstance<IIDGenerator>("TaskIdGen", new IDGenerator());
        }
    }
}
