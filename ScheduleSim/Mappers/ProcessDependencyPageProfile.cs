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

            // Model -> ViewModel
        }
    }
}
