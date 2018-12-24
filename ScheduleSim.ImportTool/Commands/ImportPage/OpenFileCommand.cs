using AutoMapper;
using Microsoft.Win32;
using ScheduleSim.Core.BusinessLogics.WPF.ImportTool;
using ScheduleSim.Core.IO.WPF.ImportTool;
using ScheduleSim.ImportTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScheduleSim.ImportTool.Commands.ImportPage
{
    public class OpenFileCommand : ICommand
    {
        private IOpenFileBusinessLogic businessLogic;
        private IMapper mapper;

        public event EventHandler CanExecuteChanged;

        public OpenFileCommand(
            IOpenFileBusinessLogic businessLogic,
            IMapper mapper)
        {
            this.businessLogic = businessLogic;
            this.mapper = mapper;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as ImportPageViewModel;
            
            var ofd = new OpenFileDialog();

            // 入力がある場合初期設定ディレクトリに設定する
            if (string.IsNullOrEmpty(viewModel.ImportFile) == false)
            {
                try
                {
                    var directory = Path.GetDirectoryName(viewModel.ImportFile);
                    ofd.InitialDirectory = directory;
                }
                catch (Exception)
                {
                    // 不正な入力
                }
            }
            ofd.Filter = "Excelファイル形式(*.xlsx, *.xlsm)|*.xlsx;*.xlsm|CSVファイル形式(*.csv)|*.csv|All Files (*.*)|*.*";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == false)
            {
                return;
            }

            var input = new OpenFileInput();
            input.FilePath = ofd.FileName;
            // ファイル名だけは先に更新
            viewModel.ImportFile = ofd.FileName;

            try
            {
                var output = this.businessLogic.Execute(input);
                viewModel.TaskItems = this.mapper.Map<ObservableCollection<ImportPageTaskItemViewModel>>(output.TaskItems);
            }
            catch (Exception ex)
            {
                // エラーメッセージを表示
                MessageBox.Show(ex.Message, "読み込みエラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Application.Current.Dispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
        }
    }
}
