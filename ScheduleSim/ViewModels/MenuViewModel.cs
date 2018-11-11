using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        public MenuViewModel(
            ICommand createNewProjectCommand,
            ICommand openFileCommand,
            ICommand saveCommand,
            ICommand saveAsCommand,
            ICommand importXlsxCommand,
            ICommand exportGanttChartCommand,
            ICommand exportPertGraphCommand)
        {
            this.CreateNewProjectCommand = createNewProjectCommand;
            this.OpenFileCommand = openFileCommand;
            this.SaveCommand = saveCommand;
            this.SaveAsCommand = saveAsCommand;
            this.ImportXlsxCommand = importXlsxCommand;
            this.ExportGanttChartCommand = exportGanttChartCommand;
            this.ExportPertGraphCommand = exportPertGraphCommand;
        }

        public ICommand CreateNewProjectCommand { get; private set; }
        public ICommand ExportGanttChartCommand { get; private set; }
        public ICommand ExportPertGraphCommand { get; private set; }
        public ICommand ImportXlsxCommand { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
    }
}
