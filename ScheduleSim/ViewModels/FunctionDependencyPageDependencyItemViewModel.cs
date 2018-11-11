using Prism.Mvvm;
using ScheduleSim.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class FunctionDependencyPageDependencyItemViewModel : BindableBase
    {
        private int? _srcFunctionId;
        public int? SrcFunctionId
        {
            get { return _srcFunctionId; }
            set { SetProperty(ref _srcFunctionId, value); }
        }

        private int? _dstFunctionId;
        public int? DstFunctionId
        {
            get { return _dstFunctionId; }
            set { SetProperty(ref _dstFunctionId, value); }
        }

        private DependencyTypes? _dependencyType;
        public DependencyTypes? DependencyType
        {
            get { return _dependencyType; }
            set { SetProperty(ref _dependencyType, value); }
        }
    }
}
