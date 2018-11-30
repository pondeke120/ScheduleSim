using ScheduleSim.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class ProcessDependency : INotifyPropertyChanged
    {
        private int? _orgProcessCd;
        public int? OrgProcessCd
        {
            get { return _orgProcessCd; }
            set { _orgProcessCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrgProcessCd))); }
        }

        private int? _dstProcessCd;
        public int? DstProcessCd
        {
            get { return _dstProcessCd; }
            set { _dstProcessCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DstProcessCd))); }
        }

        private DependencyTypes? _dependencyType;
        public DependencyTypes? DependencyType
        {
            get { return _dependencyType; }
            set { _dependencyType = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DependencyType))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
