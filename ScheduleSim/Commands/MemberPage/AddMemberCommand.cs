using AutoMapper;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Core.Utility;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.MemberPage
{
    public class AddMemberCommand : ICommand
    {
        private AppContext appContext;
        private IMapper mapper;
        private IIDGenerator memberIdGen;

        public AddMemberCommand(
            AppContext appContext,
            IIDGenerator memberIdGen,
            IMapper mapper)
        {
            this.appContext = appContext;
            this.memberIdGen = memberIdGen;
            this.mapper = mapper;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var args = parameter as AddingNewItemEventArgs;

            var newMember = new Member()
            {
                MemberCd = this.memberIdGen.CreateNewId()
            };

            this.appContext.Members.Add(newMember);
            args.NewItem = this.mapper.Map<MemberPageMemberItemViewModel>(newMember);
        }
    }
}
