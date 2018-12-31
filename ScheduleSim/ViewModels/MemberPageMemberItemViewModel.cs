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
            get { return _no; }
            set { SetProperty(ref _no, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private double? _productivity;
        public double? Productivity
        {
            get { return _productivity; }
            set { SetProperty(ref _productivity, value); }
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

        private Member _dataContext;
        public Member DataContext
        {
            get { return _dataContext; }
            set
            {
                if (_dataContext != null)
                {
                    _dataContext.PropertyChanged -= _dataContext_PropertyChanged;
                }
                SetProperty(ref _dataContext, value);
                _dataContext.PropertyChanged += _dataContext_PropertyChanged;
            }
        }

        private void _dataContext_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Member.JoinDate))
            {
                JoinDate = DataContext.JoinDate;
            }
            else if (e.PropertyName == nameof(Member.LeaveDate))
            {
                LeaveDate = DataContext.LeaveDate;
            }
            else if (e.PropertyName == nameof(Member.MemberCd))
            {
                No = DataContext.MemberCd;
            }
            else if (e.PropertyName == nameof(Member.MemberName))
            {
                Name = DataContext.MemberName;
            }
            else if (e.PropertyName == nameof(Member.Productivity))
            {
                Productivity = DataContext.Productivity;
            }
        }
    }
}
