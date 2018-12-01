using AutoMapper;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.FunctionDependencyPage
{
    public class AddDependencyCommand : ICommand
    {
        private AppContext appContext;
        private IMapper mapper;

        public AddDependencyCommand(
            AppContext appContext,
            IMapper mapper)
        {
            this.appContext = appContext;
            this.mapper = mapper;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var args = (parameter as object[])[1] as AddingNewItemEventArgs;

            var newDependency = new FunctionDependency();

            this.appContext.FunctionDependencies.Add(newDependency);
            args.NewItem = this.mapper.Map<FunctionDependencyPageDependencyItemViewModel>(newDependency);
        }
    }
}
