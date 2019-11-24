using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class Pert : INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id))); }
        }

        private int _srcNodeCd;
        public int SrcNodeCd
        {
            get { return _srcNodeCd; }
            set { _srcNodeCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SrcNodeCd))); }
        }

        private int _dstNodeCd;
        public int DstNodeCd
        {
            get { return _dstNodeCd; }
            set { _dstNodeCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DstNodeCd))); }
        }

        private int? _taskCd;
        public int? TaskCd
        {
            get { return _taskCd; }
            set { _taskCd = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskCd))); }
        }

        private bool _isCritical;
        public bool IsCritical
        {
            get { return _isCritical; }
            set { _isCritical = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCritical))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
