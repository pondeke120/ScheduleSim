﻿using AutoMapper;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Mappers
{
    public class MemberPageProfile : Profile
    {
        public MemberPageProfile()
        {
            // ViewModel -> Model
            CreateMap<MemberPageMemberItemViewModel, Member>()
                .ForMember(d => d.MemberCd, o => o.MapFrom(s => s.No))
                .ForMember(d => d.MemberName, o => o.MapFrom(d => d.Name))
                .ForMember(d => d.JoinDate, o => o.MapFrom(d => d.JoinDate))
                .ForMember(d => d.LeaveDate, o => o.MapFrom(d => d.LeaveDate));

            // Model -> ViewModel
        }
    }
}
