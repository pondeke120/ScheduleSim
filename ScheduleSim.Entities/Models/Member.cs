using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Entities.Models
{
    public class Member : INotifyPropertyChanged
    {
        private int _memberCd;
        public int MemberCd
        {
            get { return _memberCd; }
            set { _memberCd = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberCd))); }
        }

        private string _memberName;
        public string MemberName
        {
            get { return _memberName; }
            set { _memberName = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MemberName))); }
        }

        private DateTime? _joinDate;
        public DateTime? JoinDate
        {
            get { return _joinDate; }
            set { _joinDate = value;  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(JoinDate))); }
        }

        private DateTime? _leaveDate;
        public DateTime? LeaveDate
        {
            get { return _leaveDate; }
            set { _leaveDate = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeaveDate))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
