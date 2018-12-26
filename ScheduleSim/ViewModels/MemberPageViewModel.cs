﻿using AutoMapper;
using Prism.Mvvm;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Entities.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ICommand NameChangeCommand { get; private set; }
        public ICommand JoinDateChangeCommand { get; private set; }
        public ICommand LeaveDateChangeCommand { get; private set; }
        public ICommand InsertMemberCommand { get; private set; }
        private IMapper mapper;

        private MemberPageMemberItemViewModel _selectedMember;
        public MemberPageMemberItemViewModel SelectedMember
        {
            get { return _selectedMember; }
            set { SetProperty(ref _selectedMember, value); }
        }

        private ObservableCollection<MemberPageMemberItemViewModel> _members;
        public ObservableCollection<MemberPageMemberItemViewModel> Members
        {
            get { return _members; }
            set { SetProperty(ref _members, value); }
        }

        private IList _selectedMembers;
        public IList SelectedMembers
        {
            get { return _selectedMembers; }
            set { SetProperty(ref _selectedMembers, value); }
        }

        public MemberPageViewModel(
            AppContext appContext,
            ICommand addMemberCommand,
            ICommand deleteMemberCommand,
            ICommand nameChangeCommand,
            ICommand joinDateChangeCommand,
            ICommand leaveDateChangeCommand,
            ICommand insertMemberCommand,
            IMapper mapper)
        {
            this.AddMemberCommand = addMemberCommand;
            this.DeleteMemberCommand = deleteMemberCommand;
            this.NameChangeCommand = nameChangeCommand;
            this.JoinDateChangeCommand = joinDateChangeCommand;
            this.LeaveDateChangeCommand = leaveDateChangeCommand;
            this.InsertMemberCommand = insertMemberCommand;

            //Members = new List<MemberPageMemberItemViewModel>()
            //{
            //    new MemberPageMemberItemViewModel()
            //    {
            //        No = 1,
            //        Name = "test",
            //        JoinDate = DateTime.Now,
            //        LeaveDate = DateTime.Now
            //    }
            //};
            this.mapper = mapper;
            appContext.Members.CollectionChanged += Members_CollectionChanged;
        }

        private void Members_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var members = sender as ObservableCollection<Member>;

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset
                || e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                this.Members = this.mapper.Map<ObservableCollection<MemberPageMemberItemViewModel>>(members);
            }
        }
    }
}
