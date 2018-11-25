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
        public ObservableCollection<Process> Processes { get; private set; } = new ObservableCollection<Entities.Models.Process>();
    }
}
