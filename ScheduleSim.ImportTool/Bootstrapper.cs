using Unity;
using Prism.Unity;
using ScheduleSim.ImportTool.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity.Lifetime;
using ScheduleSim.ImportTool.ViewModels;
using ScheduleSim.Core.Contexts;
using System.IO;
using AutoMapper;
using ScheduleSim.ImportTool.Mappers;
using Unity.Injection;
using System.Windows.Input;
using ScheduleSim.Core.Utility;
using ScheduleSim.Core.IO.WPF.ImportTool;
using ScheduleSim.Core.Service;

namespace ScheduleSim.ImportTool
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
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            // Register Views
            Container.RegisterType<ImportPage>(new ContainerControlledLifetimeManager());

            // Register ViewModels
            Container.RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ImportPageViewModel>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new ResolvedParameter<ICommand>("ImportPage.OpenFileCommand"),
                    new ResolvedParameter<ICommand>("ImportPage.CloseCommand"),
                    new ResolvedParameter<ICommand>("ImportPage.CompleteCommand")
               ));

            // Register Commands
            Container.RegisterType<ICommand, Commands.ImportPage.OpenFileCommand>("ImportPage.OpenFileCommand");
            Container.RegisterType<ICommand, Commands.ImportPage.CloseCommand>("ImportPage.CloseCommand");
            Container.RegisterType<ICommand, Commands.ImportPage.CompleteCommand>("ImportPage.CompleteCommand",
                new InjectionConstructor(
                    new ResolvedParameter<AppContext>(),
                    new ResolvedParameter<Core.BusinessLogics.WPF.ImportTool.ICompleteBusinessLogic>(),
                    new ResolvedParameter<IIDGenerator>("ProcessIdGen"),
                    new ResolvedParameter<IIDGenerator>("FunctionIdGen"),
                    new ResolvedParameter<IIDGenerator>("TaskIdGen")
                ));

            // Register Mappers
            Container.RegisterType<IMapper, Mapper>(new InjectionConstructor(new ResolvedParameter<IConfigurationProvider>()));
            Container.RegisterType<IConfigurationProvider, MapperConfiguration>(new InjectionConstructor(
                    new Action<IMapperConfigurationExpression>(cfg =>
                    {
                        cfg.AddProfile<ImportPageProfile>();
                    })
                ));

            // Register BusinessLogics
            Container.RegisterType<Core.BusinessLogics.WPF.ImportTool.IOpenFileBusinessLogic, Core.BusinessLogics.WPF.ImportTool.OpenFileBusinessLogic>();
            Container.RegisterType<Core.BusinessLogics.WPF.ImportTool.ICompleteBusinessLogic, Core.BusinessLogics.WPF.ImportTool.CompleteBusinessLogic>();

            // Register Services
            Container.RegisterType<ICsvReadService<OpenFileOutput.TaskItem>, CsvReadService<OpenFileOutput.TaskItem>>();
            Container.RegisterType<IXlsxReadService<OpenFileOutput.TaskItem>, XlsxReadService<OpenFileOutput.TaskItem>>();

            // Register Contexts
            Container.RegisterInstance(new AppContext()
            {
                IsSaved = false,
                MasterDbFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "master.accdb"),
                ProjectFolder = Path.GetTempPath(),
                ProjectDbFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
            });

            // Register Utility
            Container.RegisterType<ICsvToModelConverter<OpenFileOutput.TaskItem>, CsvToTaskItemConverter>(
                new InjectionConstructor(
                    "工程",
                    "機能",
                    "作業",
                    "計画工数",
                    "作業開始日",
                    "作業終了日"
                ));
            Container.RegisterInstance<IIDGenerator>("ProcessIdGen", new IDGenerator());
            Container.RegisterInstance<IIDGenerator>("FunctionIdGen", new IDGenerator());
            Container.RegisterInstance<IIDGenerator>("TaskIdGen", new IDGenerator());
        }
    }
}
