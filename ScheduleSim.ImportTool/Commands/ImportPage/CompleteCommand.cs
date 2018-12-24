using ScheduleSim.Core.BusinessLogics.WPF.ImportTool;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.IO.WPF.ImportTool;
using ScheduleSim.ImportTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ScheduleSim.Core.Enums;
using ScheduleSim.Core.Utility;
using ScheduleSim.Core.Extensions;
using System.IO;

namespace ScheduleSim.ImportTool.Commands.ImportPage
{
    public class CompleteCommand : ICommand
    {
        private AppContext appContext;
        private ICompleteBusinessLogic businessLogic;
        private IIDGenerator taskIdGenerator;
        private IIDGenerator procIdGenerator;
        private IIDGenerator funcIdGenerator;

        public event EventHandler CanExecuteChanged;

        public CompleteCommand(
            AppContext appContext,
            ICompleteBusinessLogic businessLogic,
            IIDGenerator procIdGenerator,
            IIDGenerator funcIdGenerator,
            IIDGenerator taskIdGenerator)
        {
            this.appContext = appContext;
            this.businessLogic = businessLogic;
            this.procIdGenerator = procIdGenerator;
            this.funcIdGenerator = funcIdGenerator;
            this.taskIdGenerator = taskIdGenerator;
        }

        public bool CanExecute(object parameter)
        {
            var sender = parameter as ScheduleSim.ImportTool.Views.ImportPage;
            var viewModel = sender?.DataContext as ImportPageViewModel;

            if (sender == null || viewModel == null)
                return false;

            var exists = File.Exists(viewModel.ImportFile);
            var check = viewModel.IsImportToProjectSettings || viewModel.IsImportToWbs;

            return exists && check;
        }

        public void Execute(object parameter)
        {
            var sender = parameter as ScheduleSim.ImportTool.Views.ImportPage;
            var viewModel = sender.DataContext as ImportPageViewModel;

            // 参照ファイルを取得
            var filePath = viewModel.ImportFile;
            // インポートのモードを取得
            var importType = viewModel.ImportType;
            // 取り込み先を選択
            var isImportToPrjSettings = viewModel.IsImportToProjectSettings;
            var isImportToWbs = viewModel.IsImportToWbs;

            var input = new CompleteInput();
            input.FilePath = filePath;
            try
            {
                var output = this.businessLogic.Execute(input);
                // AppContextに読み込み内容を反映
                UpdateContext(this.appContext, importType, isImportToPrjSettings, isImportToWbs, output);
            }
            catch (Exception ex)
            {
                // エラーメッセージを表示
                MessageBox.Show(ex.Message, "読み込みエラー", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var window = Window.GetWindow(sender);
            MessageBox.Show("取り込み完了", string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
            window.Close();
        }

        /// <summary>
        /// 読み込み内容をAppContextに反映する
        /// </summary>
        /// <param name="appContext"></param>
        /// <param name="importType"></param>
        /// <param name="isImportToPrjSettings"></param>
        /// <param name="isImportToWbs"></param>
        /// <param name="output"></param>
        private void UpdateContext(AppContext appContext, ImportTypes importType, bool isImportToPrjSettings, bool isImportToWbs, CompleteOutput output)
        {
            // インポート種別と取り込み先により適用メソッドを選択
            var applyRoutine = GetRoutine(importType, isImportToPrjSettings, isImportToWbs);

            // 適用処理を実行
            applyRoutine.Invoke(appContext, output);
        }

        /// <summary>
        /// フラグ状況によって異なる更新ルーチンを生成
        /// </summary>
        /// <param name="importType"></param>
        /// <param name="isImportToPrjSettings"></param>
        /// <param name="isImportToWbs"></param>
        /// <returns></returns>
        private Action<AppContext, CompleteOutput> GetRoutine(ImportTypes importType, bool isImportToPrjSettings, bool isImportToWbs)
        {
            var importPrjSettingAction = null as Action<AppContext, CompleteOutput>;
            var importWbsAction = null as Action<AppContext, CompleteOutput>;
            if (importType == ImportTypes.Overwrite)
            {
                importPrjSettingAction = new Action<AppContext, CompleteOutput>(OverwritePrjSettings);
                importWbsAction = new Action<AppContext, CompleteOutput>(OverriteWbs);
            }
            else if (importType == ImportTypes.Addition)
            {
                importPrjSettingAction = new Action<AppContext, CompleteOutput>(AddPrjSettings);
                importWbsAction = new Action<AppContext, CompleteOutput>(AddWbs);
            }

            return
                new Action<AppContext, CompleteOutput>((appContext, output) =>
                {
                    if (isImportToPrjSettings)
                        importPrjSettingAction.Invoke(appContext, output);
                    if (isImportToWbs)
                        importWbsAction.Invoke(appContext, output);
                });
        }

        /// <summary>
        /// プロジェクト設定(工程と機能)を上書き
        /// </summary>
        /// <param name="appContext"></param>
        /// <param name="output"></param>
        public void OverwritePrjSettings(AppContext appContext, CompleteOutput output)
        {
            // 工程と機能を削除
            appContext.Processes.Clear();
            appContext.Functions.Clear();
            procIdGenerator.SetCurrentIndex(1);
            funcIdGenerator.SetCurrentIndex(1);

            // タスクのIDをnullで初期化
            foreach (var task in appContext.Tasks)
            {
                task.ProcessCd = null;
                task.FunctionCd = null;
            }

            // 工程と機能のIDを振り直し
            foreach (var proc in output.Processes)
            {
                proc.ProcessCd = this.procIdGenerator.CreateNewId();
            }
            foreach (var func in output.Functions)
            {
                func.FunctionCd = this.funcIdGenerator.CreateNewId();
            }

            // データ更新
            appContext.Processes.AddRange(output.Processes);
            appContext.Functions.AddRange(output.Functions);
        }

        /// <summary>
        /// Wbs(作業)を上書き
        /// </summary>
        /// <param name="appContext"></param>
        /// <param name="output"></param>
        public void OverriteWbs(AppContext appContext, CompleteOutput output)
        {
            // タスクを全削除
            appContext.Tasks.Clear();
            taskIdGenerator.SetCurrentIndex(1);

            var tasks = output.TaskItems.Zip(output.Tasks, (a, b) => new { Item = a, Entity = b });

            // タスクのIDを振り直し
            foreach (var task in tasks)
            {
                var procId = appContext.Processes.FirstOrDefault(x => x.ProcessName.Equals(task.Item.Process))?.ProcessCd;
                var funcId = appContext.Functions.FirstOrDefault(x => x.FunctionName.Equals(task.Item.Function))?.FunctionCd;
                var entity = task.Entity;

                entity.TaskCd = taskIdGenerator.CreateNewId();
                entity.ProcessCd = procId;
                entity.FunctionCd = funcId;
            }

            // データ更新
            appContext.Tasks.AddRange(output.Tasks);
        }

        /// <summary>
        /// プロジェクト設定(工程と機能)を追加
        /// </summary>
        /// <param name="appContext"></param>
        /// <param name="output"></param>
        public void AddPrjSettings(AppContext appContext, CompleteOutput output)
        {
            // 名前に重複がある場合は削除
            var addProcs = output.Processes.ToList();
            foreach (var proc in appContext.Processes)
            {
                var exists = addProcs.FirstOrDefault(x => x.ProcessName.Equals(proc.ProcessName));
                if (exists != null)
                {
                    addProcs.Remove(exists);
                }
            }
            var addFuncs = output.Functions.ToList();
            foreach (var func in appContext.Functions)
            {
                var exists = addFuncs.FirstOrDefault(x => x.FunctionName.Equals(func.FunctionName));
                if (exists != null)
                {
                    addFuncs.Remove(exists);
                }
            }

            // IDを振り直し
            foreach (var proc in addProcs)
            {
                proc.ProcessCd = this.procIdGenerator.CreateNewId();
            }
            foreach (var func in addFuncs)
            {
                func.FunctionCd = this.funcIdGenerator.CreateNewId();
            }

            // データ更新
            appContext.Processes.AddRange(addProcs);
            appContext.Functions.AddRange(addFuncs);
        }

        /// <summary>
        /// WBS(作業)を追加
        /// </summary>
        /// <param name="appContext"></param>
        /// <param name="output"></param>
        public void AddWbs(AppContext appContext, CompleteOutput output)
        {
            // 名前に重複がある場合は削除
            var addTasks = output.TaskItems.Zip(output.Tasks, (a, b) => new { Item = a, Entity = b }).ToList();
            foreach (var task in appContext.Tasks)
            {
                var exists = addTasks.FirstOrDefault(x => x.Item.TaskName.Equals(task.TaskName));
                if (exists != null)
                {
                    addTasks.Remove(exists);
                }
            }

            // IDを振り直し
            foreach (var task in addTasks)
            {
                task.Entity.TaskCd = this.taskIdGenerator.CreateNewId();
                // 工程と紐づけ
                var proc = this.appContext.Processes.FirstOrDefault(x => x.ProcessName.Equals(task.Item.Process));
                if (proc != null)
                {
                    task.Entity.ProcessCd = proc.ProcessCd;
                }
                // 機能と紐づけ
                var func = this.appContext.Functions.FirstOrDefault(x => x.FunctionName.Equals(task.Item.Function));
                if (func != null)
                {
                    task.Entity.FunctionCd = func.FunctionCd;
                }
            }

            // データ更新
            appContext.Tasks.AddRange(addTasks.Select(x => x.Entity));
        }
    }
}
