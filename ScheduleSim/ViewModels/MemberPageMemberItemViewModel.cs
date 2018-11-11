using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class MemberPageMemberItemViewModel : BindableBase
    {
        private int? _no;
        public int? No
        {
            get { return _no; }
            set { SetProperty(ref _no, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private DateTime? _joinDate;
        public DateTime? JoinDate
        {
            get { return _joinDate; }
            set { SetProperty(ref _joinDate, value); }
        }

        private DateTime? _leaveDate;
        public DateTime? LeaveDate
        {
            get { return _leaveDate; }
            set { SetProperty(ref _leaveDate, value); }
        }
    }
}
