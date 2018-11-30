using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
        public ObservableCollection<WeekDay> WeekDays { get; private set; } = new ObservableCollection<WeekDay>();
        public ObservableCollection<Member> Members { get; private set; } = new ObservableCollection<Member>();
        public ObservableCollection<Task> Tasks { get; private set; } = new ObservableCollection<Task>();
        public ObservableCollection<ProcessDependency> ProcessDependencies { get; private set; } = new ObservableCollection<ProcessDependency>();
        public ObservableCollection<DependencyType> DependencyTypes { get; private set; } = new ObservableCollection<DependencyType>();
    }
}
