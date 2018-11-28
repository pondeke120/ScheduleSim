using Prism.Mvvm;
using ScheduleSim.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ViewModels
{
    public class WbsPageMemberItemViewModel : BindableBase
    {
        public int? MemberCd
        {
            get { return _dataContext?.MemberCd; }
        }

        public string MemberName
        {
            get { return _dataContext?.MemberName; }
        }

        private Member _dataContext;
        public Member DataContext
        {
            get { return _dataContext; }
            set { SetProperty(ref _dataContext, value); }
        }
    }
}
