using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.Contexts
{
    public class AppContext
    {
        public bool IsSaved { get; set; }
        public string MasterDbFile { get; set; }
        public string ProjectDbFile { get; set; }
        public string ProjectFolder { get; set; }
        public PrjSettings PrjSettings { get; private set; } = new PrjSettings();
        public ObservableCollection<Process> Processes { get; private set; } = new ObservableCollection<Process>();
        public ObservableCollection<Function> Functions { get; private set; } = new ObservableCollection<Function>();
        public ObservableCollection<Holiday> Holidays { get; private set; } = new ObservableCollection<Holiday>();
    }
}
