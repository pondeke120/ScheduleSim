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
using ScheduleSim.Core.Extensions;
using ScheduleSim.Entities.Models;
using System.Linq;

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

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var appContext = Container.Resolve<AppContext>();

            var procIdGen = Container.Resolve<IIDGenerator>("ProcessIdGen");
            appContext.Processes.Clear();
            appContext.Processes.AddRange(Enumerable.Range(0, 20).Select(i => new Process() {ProcessCd = procIdGen.CreateNewId(), ProcessName = string.Empty }).ToArray());

            var funcIdGen = Container.Resolve<IIDGenerator>("FunctionIdGen");
            appContext.Functions.Clear();
            appContext.Functions.AddRange(Enumerable.Range(0, 20).Select(i => new Function() {  FunctionCd = funcIdGen.CreateNewId(), FunctionName = string.Empty }).ToArray());

            var holidayIdGen = Container.Resolve<IIDGenerator>("HolidayIdGen");
            appContext.Holidays.Clear();
            appContext.Holidays.AddRange(Enumerable.Range(0, 20).Select(i => new Holiday() { HolidayCd = holidayIdGen.CreateNewId(), HolidayDate = null, HolidayName = string.Empty }).ToArray());

            appContext.WeekDays.Clear();
            appContext.WeekDays.AddRange(Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().Select(x => new WeekDay() { WeekdayCd = x, HolidayFlg = false, WeekdayName = Enum.GetName(typeof(DayOfWeek), x) }));
            
            appContext.Members.Clear();
            
            appContext.Tasks.Clear();

            appContext.DependencyTypes.Clear();
            appContext.DependencyTypes.AddRange(
                Enum.GetValues(typeof(Entities.Enum.DependencyTypes))
                .Cast<Entities.Enum.DependencyTypes>()
                .Select(x => new DependencyType() { DependencyTypeCd = x, DependencyName = Enum.GetName(typeof(Entities.Enum.DependencyTypes), x) }));

            appContext.ProcessDependencies.Clear();
            appContext.FunctionDependencies.Clear();

            appContext.PertEdges.Clear();
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
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<ICommand>("ProjectSettingPage.ProcessChangeCommand"),
                    new ResolvedParameter<ICommand>("ProjectSettingPage.FunctionChangeCommand"),
                    new ResolvedParameter<ICommand>("ProjectSettingPage.PeriodChangeCommand"),
                    new ResolvedParameter<ICommand>("ProjectSettingPage.HolidayChangeCommand"),
                    new ResolvedParameter<ICommand>("ProjectSettingPage.WeekDayChangeCommand"),
                    new ResolvedParameter<IMapper>()
                ));
            Container.RegisterType<MemberPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<ICommand>("MemberPage.AddMemberCommand"),
                    new ResolvedParameter<ICommand>("MemberPage.DeleteMemberCommand"),
                    new ResolvedParameter<ICommand>("MemberPage.NameChangeCommand"),
                    new ResolvedParameter<ICommand>("MemberPage.JoinDateChangeCommand"),
                    new ResolvedParameter<ICommand>("MemberPage.LeaveDateChangeCommand"),
                    new ResolvedParameter<IMapper>()
                ));
            Container.RegisterType<WbsPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<IMapper>(),
                    new ResolvedParameter<ICommand>("WbsPage.AddTaskCommand"),
                    new ResolvedParameter<ICommand>("WbsPage.DeleteTaskCommand"),
                    new ResolvedParameter<ICommand>("WbsPage.ProcessChangeCommand"),
                    new ResolvedParameter<ICommand>("WbsPage.FunctionChangeCommand"),
                    new ResolvedParameter<ICommand>("WbsPage.TaskNameChangeCommand"),
                    new ResolvedParameter<ICommand>("WbsPage.PlanValueChangeCommand"),
                    new ResolvedParameter<ICommand>("WbsPage.StartDateChangeCommand"),
                    new ResolvedParameter<ICommand>("WbsPage.EndDateChangeCommand"),
                    new ResolvedParameter<ICommand>("WbsPage.AssignMemberChangeCommand")
                ));
            Container.RegisterType<ProcessDependencyPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<IMapper>(),
                    new ResolvedParameter<ICommand>("ProcessDependencyPage.AddDependencyCommand"),
                    new ResolvedParameter<ICommand>("ProcessDependencyPage.DeleteDependencyCommand"),
                    new ResolvedParameter<ICommand>("ProcessDependencyPage.SrcProcessChangeCommand"),
                    new ResolvedParameter<ICommand>("ProcessDependencyPage.DstProcessChangeCommand"),
                    new ResolvedParameter<ICommand>("ProcessDependencyPage.DependencyTypeChangeCommand")
                ));
            Container.RegisterType<FunctionDependencyPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<IMapper>(),
                    new ResolvedParameter<ICommand>("FunctionDependencyPage.AddDependencyCommand"),
                    new ResolvedParameter<ICommand>("FunctionDependencyPage.DeleteDependencyCommand"),
                    new ResolvedParameter<ICommand>("FunctionDependencyPage.SrcFunctionChangeCommand"),
                    new ResolvedParameter<ICommand>("FunctionDependencyPage.DstFunctionChangeCommand"),
                    new ResolvedParameter<ICommand>("FunctionDependencyPage.DependencyTypeChangeCommand")
                ));
            Container.RegisterType<PertPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<IMapper>(),
                    new ResolvedParameter<ICommand>("PertPage.AddPertEdgeCommand"),
                    new ResolvedParameter<ICommand>("PertPage.DeleteEdgeCommand"),
                    new ResolvedParameter<ICommand>("PertPage.ProcessChangeCommand"),
                    new ResolvedParameter<ICommand>("PertPage.FunctionChangeCommand"),
                    new ResolvedParameter<ICommand>("PertPage.TaskChangeCommand"),
                    new ResolvedParameter<ICommand>("PertPage.SrcNodeChangeCommand"),
                    new ResolvedParameter<ICommand>("PertPage.DstNodeChangeCommand")
                ));

            // Register Commands
            Container.RegisterType<ICommand, Commands.Menu.CreateNewProjectCommand>("Menu.CreateNewProjectCommand");
            Container.RegisterType<ICommand, Commands.Menu.OpenFileCommand>("Menu.OpenFileCommand",
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<Core.BusinessLogics.WPF.Menu.IOpenFileBusinessLogic>(),
                    new ResolvedParameter<FunctionDependencyPageViewModel>(),
                    new ResolvedParameter<WbsPageViewModel>(),
                    new ResolvedParameter<PertPageViewModel>(),
                    new ResolvedParameter<ShellViewModel>(),
                    new ResolvedParameter<IIDGenerator>("MemberIdGen"),
                    new ResolvedParameter<IIDGenerator>("TaskIdGen"),
                    new ResolvedParameter<IIDGenerator>("PertIdGen"),
                    new ResolvedParameter<IMapper>(),
                    new ResolvedParameter<IDispatcher>()
                ));
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
                    new ResolvedParameter<PertPageViewModel>(),
                    new ResolvedParameter<ShellViewModel>(),
                    new ResolvedParameter<IMapper>()
                ));
            Container.RegisterType<ICommand, Commands.Menu.SaveAsCommand>("Menu.SaveAsCommand");
            Container.RegisterType<ICommand, Commands.Menu.ImportXlsxCommand>("Menu.ImportXlsxCommand");
            Container.RegisterType<ICommand, Commands.Menu.ExportGanttChartCommand>("Menu.ExportGanttChartCommand");
            Container.RegisterType<ICommand, Commands.Menu.ExportPertGraphCommand>("Menu.ExportPertGraphCommand");
            Container.RegisterType<ICommand, Commands.ProjectSettingPage.ProcessChangeCommand>("ProjectSettingPage.ProcessChangeCommand");
            Container.RegisterType<ICommand, Commands.ProjectSettingPage.FunctionChangeCommand>("ProjectSettingPage.FunctionChangeCommand");
            Container.RegisterType<ICommand, Commands.ProjectSettingPage.PeriodChangeCommand>("ProjectSettingPage.PeriodChangeCommand");
            Container.RegisterType<ICommand, Commands.ProjectSettingPage.HolidayChangeCommand>("ProjectSettingPage.HolidayChangeCommand");
            Container.RegisterType<ICommand, Commands.ProjectSettingPage.WeekDayChangeCommand>("ProjectSettingPage.WeekDayChangeCommand");
            Container.RegisterType<ICommand, Commands.MemberPage.AddMemberCommand>("MemberPage.AddMemberCommand",
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<IIDGenerator>("MemberIdGen"),
                    new ResolvedParameter<IMapper>()
                ));
            Container.RegisterType<ICommand, Commands.MemberPage.DeleteMemberCommand>("MemberPage.DeleteMemberCommand");
            Container.RegisterType<ICommand, Commands.MemberPage.NameChangeCommand>("MemberPage.NameChangeCommand");
            Container.RegisterType<ICommand, Commands.MemberPage.JoinDateChangeCommand>("MemberPage.JoinDateChangeCommand");
            Container.RegisterType<ICommand, Commands.MemberPage.LeaveDateChangeCommand>("MemberPage.LeaveDateChangeCommand");
            Container.RegisterType<ICommand, Commands.WbsPage.AddTaskCommand>("WbsPage.AddTaskCommand",
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<IIDGenerator>("TaskIdGen"),
                    new ResolvedParameter<IMapper>()
                ));
            Container.RegisterType<ICommand, Commands.WbsPage.DeleteTaskCommand>("WbsPage.DeleteTaskCommand");
            Container.RegisterType<ICommand, Commands.WbsPage.ProcessChangeCommand>("WbsPage.ProcessChangeCommand");
            Container.RegisterType<ICommand, Commands.WbsPage.FunctionChangeCommand>("WbsPage.FunctionChangeCommand");
            Container.RegisterType<ICommand, Commands.WbsPage.TaskNameChangeCommand>("WbsPage.TaskNameChangeCommand");
            Container.RegisterType<ICommand, Commands.WbsPage.PlanValueChangeCommand>("WbsPage.PlanValueChangeCommand");
            Container.RegisterType<ICommand, Commands.WbsPage.StartDateChangeCommand>("WbsPage.StartDateChangeCommand");
            Container.RegisterType<ICommand, Commands.WbsPage.EndDateChangeCommand>("WbsPage.EndDateChangeCommand");
            Container.RegisterType<ICommand, Commands.WbsPage.AssignMemberChangeCommand>("WbsPage.AssignMemberChangeCommand");
            Container.RegisterType<ICommand, Commands.ProcessDependencyPage.AddDependencyCommand>("ProcessDependencyPage.AddDependencyCommand");
            Container.RegisterType<ICommand, Commands.ProcessDependencyPage.DeleteDependencyCommand>("ProcessDependencyPage.DeleteDependencyCommand");
            Container.RegisterType<ICommand, Commands.ProcessDependencyPage.SrcProcessChangeCommand>("ProcessDependencyPage.SrcProcessChangeCommand");
            Container.RegisterType<ICommand, Commands.ProcessDependencyPage.DstProcessChangeCommand>("ProcessDependencyPage.DstProcessChangeCommand");
            Container.RegisterType<ICommand, Commands.ProcessDependencyPage.DependencyTypeChangeCommand>("ProcessDependencyPage.DependencyTypeChangeCommand");
            Container.RegisterType<ICommand, Commands.FunctionDependencyPage.AddDependencyCommand>("FunctionDependencyPage.AddDependencyCommand");
            Container.RegisterType<ICommand, Commands.FunctionDependencyPage.DeleteDependencyCommand>("FunctionDependencyPage.DeleteDependencyCommand");
            Container.RegisterType<ICommand, Commands.FunctionDependencyPage.SrcFunctionChangeCommand>("FunctionDependencyPage.SrcFunctionChangeCommand");
            Container.RegisterType<ICommand, Commands.FunctionDependencyPage.DstFunctionChangeCommand>("FunctionDependencyPage.DstFunctionChangeCommand");
            Container.RegisterType<ICommand, Commands.FunctionDependencyPage.DependencyTypeChangeCommand>("FunctionDependencyPage.DependencyTypeChangeCommand");
            Container.RegisterType<ICommand, Commands.PertPage.AddPertEdgeCommand>("PertPage.AddPertEdgeCommand",
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<IIDGenerator>("PertIdGen"),
                    new ResolvedParameter<IMapper>()
                ));
            Container.RegisterType<ICommand, Commands.PertPage.DeleteEdgeCommand>("PertPage.DeleteEdgeCommand");
            Container.RegisterType<ICommand, Commands.PertPage.FunctionChangeCommand>("PertPage.FunctionChangeCommand");
            Container.RegisterType<ICommand, Commands.PertPage.ProcessChangeCommand>("PertPage.ProcessChangeCommand");
            Container.RegisterType<ICommand, Commands.PertPage.TaskChangeCommand>("PertPage.TaskChangeCommand");
            Container.RegisterType<ICommand, Commands.PertPage.SrcNodeChangeCommand>("PertPage.SrcNodeChangeCommand");
            Container.RegisterType<ICommand, Commands.PertPage.DstNodeChangeCommand>("PertPage.DstNodeChangeCommand");

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
                        cfg.AddProfile<PertPageProfile>();
                    })
                ));

            // Register BusinessLogics
            Container.RegisterType<Core.BusinessLogics.WPF.Menu.ICreateNewProjectBusinessLogic, Core.BusinessLogics.WPF.Menu.CreateNewProjectBusinessLogic>();
            Container.RegisterType<Core.BusinessLogics.WPF.Menu.IOpenFileBusinessLogic, Core.BusinessLogics.WPF.Menu.OpenFileBusinessLogic>();
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
            Container.RegisterType<Core.Service.IPertAccessService, Core.Service.PertAccessService>();

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
            Container.RegisterInstance<IIDGenerator>("PertIdGen", new IDGenerator());
        }
    }
}
