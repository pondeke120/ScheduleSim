using ScheduleSim.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class FunctionDependency : INotifyPropertyChanged
    {
        private int? _orgFunctionCd;
        public int? OrgFunctionCd
        {
            get { return _orgFunctionCd; }
            set { _orgFunctionCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrgFunctionCd))); }
        }

        private int? _dstFunctionCd;
        public int? DstFunctionCd
        {
            get { return _dstFunctionCd; }
            set { _dstFunctionCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DstFunctionCd))); }
        }

        private DependencyTypes? _dependencyTypeCd;
        public DependencyTypes? DependencyTypeCd
        {
            get { return _dependencyTypeCd; }
            set { _dependencyTypeCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DependencyTypeCd))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
