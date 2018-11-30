using AutoMapper;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Mappers
{
    public class ProcessDependencyPageProfile : Profile
    {
        public ProcessDependencyPageProfile()
        {
            // ViewModel -> Model
            CreateMap<ProcessDependencyPageDependencyItemViewModel, ProcessDependency>()
                .ForMember(d => d.OrgProcessCd, o => o.MapFrom(s => s.SrcProcessId))
                .ForMember(d => d.DstProcessCd, o => o.MapFrom(s => s.DstProcessId))
                .ForMember(d => d.DependencyType, o => o.MapFrom(s => s.DependencyType));
            CreateMap<ProcessDependencyPageProcessItemViewModel, Process>()
                .ForMember(d => d.ProcessCd, o => o.MapFrom(s => s.ProcessId))
                .ForMember(d => d.ProcessName, o => o.MapFrom(s => s.ProcessName));
            CreateMap<ProcessDependencyPageDependencyTypeItemViewModel, DependencyType>()
                .ForMember(d => d.DependencyTypeCd, o => o.MapFrom(s => s.DependencyType))
                .ForMember(d => d.DependencyName, o => o.MapFrom(s => s.DependencyTypeName));

            // Model -> ViewModel
            CreateMap<ProcessDependency, ProcessDependencyPageDependencyItemViewModel>()
                .ForMember(d => d.SrcProcessId, o => o.MapFrom(s => s.OrgProcessCd))
                .ForMember(d => d.DstProcessId, o => o.MapFrom(s => s.DstProcessCd))
                .ForMember(d => d.DependencyType, o => o.MapFrom(s => s.DependencyType));
            CreateMap<Process, ProcessDependencyPageProcessItemViewModel>()
                .ForMember(d => d.DataContext, o => o.MapFrom(s => s));
            CreateMap<DependencyType, ProcessDependencyPageDependencyTypeItemViewModel>()
                .ForMember(d => d.DataContext, o => o.MapFrom(s => s));
        }
    }
}
