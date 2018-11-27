using Prism.Mvvm;
using ScheduleSim.Entities.Models;
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
            get { return _dataContext?.MemberCd; }
            set
            {
                if (_dataContext != null && value.HasValue)
                {
                    _dataContext.MemberCd = value.Value;
                }
                SetProperty(ref _no, value);
            }
        }

        private string _name;
        public string Name
        {
            get { return _dataContext?.MemberName; }
            set {
                if (_dataContext != null)
                {
                    _dataContext.MemberName = value;
                }
                SetProperty(ref _name, value);
            }
        }

        private DateTime? _joinDate;
        public DateTime? JoinDate
        {
            get { return _dataContext?.JoinDate; }
            set
            {
                if (_dataContext != null)
                {
                    _dataContext.JoinDate = value;
                }
                SetProperty(ref _joinDate, value);
            }
        }

        private DateTime? _leaveDate;
        public DateTime? LeaveDate
        {
            get { return _dataContext?.LeaveDate; }
            set
            {
                if (_dataContext != null)
                {
                    _dataContext.LeaveDate = value;
                }
                SetProperty(ref _leaveDate, value);
            }
        }

        private Member _dataContext;
        public Member DataContext
        {
            get { return _dataContext; }
            set
            {
                SetProperty(ref _dataContext, value);
            }
        }
    }
}
