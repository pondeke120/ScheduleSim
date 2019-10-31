using ScheduleSim.Core.BusinessLogics.WPF.PertPage;
using AutoMapper;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Utility;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ScheduleSim.Core.IO.WPF.PertPage;
using System.Windows;

namespace ScheduleSim.Commands.PertPage
{
    public class CheckDependencyCommand : ICommand
    {
        private AppContext appContext;
        private ICheckDependencyBusinessLogic businessLogic;

        public CheckDependencyCommand(
            AppContext appContext,
            ICheckDependencyBusinessLogic businessLogic)
        {
            this.appContext = appContext;
            this.businessLogic = businessLogic;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as PertPageViewModel;

            // 基本データの更新
            foreach (var edge in viewModel.Edges)
            {
                var taskId = edge.TaskId;
                var task = this.appContext.Tasks.FirstOrDefault(x => x.TaskCd == taskId);
                if (task != null)
                {
                    edge.ProcessId = task.ProcessCd;
                    edge.FunctionId = task.FunctionCd;
                    edge.PV = task.PlanValue;
                }
                else
                {
                    edge.PV = 0.0;
                }
            }
            
            // 依存性に違反しているエッジを抽出
            var input = new CheckDependencyInput();
            input.ProcessDependencies = appContext.ProcessDependencies;
            input.FunctionDependencies = appContext.FunctionDependencies;
            input.Tasks = appContext.Tasks;
            input.PertEdges = appContext.PertEdges;

            var output = this.businessLogic.Execute(input);

            if (output.InvalidStartStartDependencies.Count() > 0)
            {
                MessageBox.Show(string.Format("開始日が前工程の開始日より前です。{0}{1}",
                    Environment.NewLine,
                    string.Join(Environment.NewLine, output.InvalidStartStartDependencies.Select(x => x.TaskName).ToArray())));
            }
            if (output.InvalidStartFinishDependencies.Count() > 0)
            {
                MessageBox.Show(string.Format("終了日が前工程の開始日より前です。{0}{1}",
                    Environment.NewLine,
                    string.Join(Environment.NewLine, output.InvalidStartFinishDependencies.Select(x => x.TaskName).ToArray())));
            }
            if (output.InvalidFinishStartDependencies.Count() > 0)
            {
                MessageBox.Show(string.Format("開始日が前工程の終了日より前です。{0}{1}",
                    Environment.NewLine,
                    string.Join(Environment.NewLine, output.InvalidFinishStartDependencies.Select(x => x.TaskName).ToArray())));
            }
            if (output.InvalidFinishFinishDependencies.Count() > 0)
            {
                MessageBox.Show(string.Format("終了日が前工程の終了日より前です。{0}{1}",
                    Environment.NewLine,
                    string.Join(Environment.NewLine, output.InvalidFinishFinishDependencies.Select(x => x.TaskName).ToArray())));
            }
        }
    }
}
