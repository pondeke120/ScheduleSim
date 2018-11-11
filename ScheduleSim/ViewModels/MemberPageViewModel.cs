using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class MemberPageViewModel : BindableBase
    {
        public ICommand AddMemberCommand { get; private set; }
        public ICommand DeleteMemberCommand { get; private set; }

        private List<MemberPageMemberItemViewModel> _members;
        public List<MemberPageMemberItemViewModel> Members
        {
            get { return _members; }
            set { SetProperty(ref _members, value); }
        }

        public MemberPageViewModel(
            ICommand addMemberCommand,
            ICommand deleteMemberCommand)
        {
            this.AddMemberCommand = addMemberCommand;
            this.DeleteMemberCommand = deleteMemberCommand;

            Members = new List<MemberPageMemberItemViewModel>()
            {
                new MemberPageMemberItemViewModel()
                {
                    No = 1,
                    Name = "test",
                    JoinDate = DateTime.Now,
                    LeaveDate = DateTime.Now
                }
            };
        }
    }
}
