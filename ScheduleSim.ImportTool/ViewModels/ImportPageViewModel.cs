using Prism.Mvvm;
using ScheduleSim.Core.Enums;
using ScheduleSim.ImportTool.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ImportTool.ViewModels
{
    public class ImportPageViewModel : BindableBase
    {
        private string _importFile;
        public string ImportFile
        {
            get { return _importFile; }
            set { SetProperty(ref _importFile, value); }
        }

        private ImportTypes _importType;
        public ImportTypes ImportType
        {
            get { return _importType; }
            set { SetProperty(ref _importType, value); }
        }

        private bool _isImportToProjectSettings;
        public bool IsImportToProjectSettings
        {
            get { return _isImportToProjectSettings; }
            set { SetProperty(ref _isImportToProjectSettings, value); }
        }

        private bool _isImportToWbs;
        public bool IsImportToWbs
        {
            get { return _isImportToWbs; }
            set { SetProperty(ref _isImportToWbs, value); }
        }

        private ObservableCollection<ImportPageTaskItemViewModel> _taskItems;
        public ObservableCollection<ImportPageTaskItemViewModel> TaskItems
        {
            get { return _taskItems; }
            set { SetProperty(ref _taskItems, value); }
        }
        
        public ICommand OpenFileCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand CompleteCommand { get; set; }

        public ImportPageViewModel(
            ICommand openFileCommand,
            ICommand closeCommand,
            ICommand completeCommand)
        {
            this.OpenFileCommand = openFileCommand;
            this.CloseCommand = closeCommand;
            this.CompleteCommand = new RelayCommand(completeCommand.Execute, completeCommand.CanExecute);

            // 画面表示時のデフォルト設定
            ImportType = ImportTypes.Addition;
            IsImportToProjectSettings = true;
            IsImportToWbs = true;
            TaskItems = new ObservableCollection<ImportPageTaskItemViewModel>();
        }
    }
}
