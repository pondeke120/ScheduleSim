using Prism.Mvvm;
using ScheduleSim.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class ProcessDependencyPageDependencyItemViewModel : BindableBase
    {
        private int? _srcProcessId;
        public int? SrcProcessId
        {
            get { return _srcProcessId; }
            set { SetProperty(ref _srcProcessId, value); }
        }

        private int? _dstProcessId;
        public int? DstProcessId
        {
            get { return _dstProcessId; }
            set { SetProperty(ref _dstProcessId, value); }
        }

        private DependencyTypes? _dependencyType;
        public DependencyTypes? DependencyType
        {
            get { return _dependencyType; }
            set { SetProperty(ref _dependencyType, value); }
        }
    }
}
