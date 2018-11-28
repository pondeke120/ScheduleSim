using AutoMapper;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Utility;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.WbsPage
{
    public class AddTaskCommand : ICommand
    {
        private AppContext appContext;
        private IMapper mapper;
        private IIDGenerator taskIdGen;

        public AddTaskCommand(
            AppContext appContext,
            IIDGenerator taskIdGen,
            IMapper mapper)
        {
            this.appContext = appContext;
            this.taskIdGen = taskIdGen;
            this.mapper = mapper;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var sender = (parameter as object[])[0];
            var args = (parameter as object[])[1] as AddingNewItemEventArgs;

            var newTask = new Task()
            {
                TaskCd = this.taskIdGen.CreateNewId()
            };

            this.appContext.Tasks.Add(newTask);
            args.NewItem = this.mapper.Map<WbsPageTaskItemViewModel>(newTask);
        }
    }
}