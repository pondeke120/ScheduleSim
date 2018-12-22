using AutoMapper;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleSim.Mappers
{
    public class PertPageProfile : Profile
    {
        public PertPageProfile()
        {
            // ViewModel -> Model
            CreateMap<PertPageEdgeItemViewModel, Pert>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.SrcNodeCd, o => o.MapFrom(s => s.INode))
                .ForMember(d => d.DstNodeCd, o => o.MapFrom(s => s.JNode))
                .ForMember(d => d.TaskCd, o => o.MapFrom(s => s.TaskId));
            CreateMap<PertPageProcessItemViewModel, Process>()
                .ForMember(d => d.ProcessCd, o => o.MapFrom(s => s.ProcessId))
                .ForMember(d => d.ProcessName, o => o.MapFrom(s => s.ProcessName));
            CreateMap<PertPageFunctionItemViewModel, Function>()
                .ForMember(d => d.FunctionCd, o => o.MapFrom(s => s.FunctionId))
                .ForMember(d => d.FunctionName, o => o.MapFrom(s => s.FunctionName));
            CreateMap<PertPageTaskItemViewModel, Task>()
                .ForMember(d => d.TaskCd, o => o.MapFrom(s => s.TaskId))
                .ForMember(d => d.TaskName, o => o.MapFrom(s => s.TaskName));

            // Model -> ViewModel
            CreateMap<Pert, PertPageEdgeItemViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.INode, o => o.MapFrom(s => s.SrcNodeCd))
                .ForMember(d => d.JNode, o => o.MapFrom(s => s.DstNodeCd))
                .ForMember(d => d.TaskId, o => o.MapFrom(s => s.TaskCd));
            CreateMap<Process, PertPageProcessItemViewModel>()
                .ForMember(d => d.DataContext, o => o.MapFrom(s => s));
            CreateMap<Function, PertPageFunctionItemViewModel>()
                .ForMember(d => d.DataContext, o => o.MapFrom(s => s));
            CreateMap<Task, PertPageTaskItemViewModel>()
                .ForMember(d => d.DataContext, o => o.MapFrom(s => s));
        }
    }
}
