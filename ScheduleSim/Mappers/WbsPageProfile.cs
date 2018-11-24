using AutoMapper;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.Mappers
{
    public class WbsPageProfile : Profile
    {
        public WbsPageProfile()
        {
            // ViewModel -> Model
            CreateMap<WbsPageTaskItemViewModel, Task>()
                .ForMember(d => d.TaskCd, o => o.MapFrom(s => s.No))
                .ForMember(d => d.ProcessCd, o => o.MapFrom(s => s.ProcessId))
                .ForMember(d => d.FunctionCd, o => o.MapFrom(s => s.FunctionId))
                .ForMember(d => d.TaskName, o => o.MapFrom(s => s.TaskName))
                .ForMember(d => d.PlanValue, o => o.MapFrom(s => s.PV))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate))
                .ForMember(d => d.AssignMemberCd, o => o.MapFrom(s => s.AssignMemberId));

            // Model -> ViewModel
            CreateMap<Task, WbsPageTaskItemViewModel>()
                .ForMember(d => d.No, o => o.MapFrom(s => s.TaskCd))
                .ForMember(d => d.ProcessId, o => o.MapFrom(s => s.ProcessCd))
                .ForMember(d => d.FunctionId, o => o.MapFrom(s => s.FunctionCd))
                .ForMember(d => d.TaskName, o => o.MapFrom(s => s.TaskName))
                .ForMember(d => d.PV, o => o.MapFrom(s => s.PlanValue))
                .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate))
                .ForMember(d => d.AssignMemberId, o => o.MapFrom(s => s.AssignMemberCd));
        }
    }
}
