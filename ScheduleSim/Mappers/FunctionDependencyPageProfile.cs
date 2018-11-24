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
    public class FunctionDependencyPageProfile : Profile
    {
        public FunctionDependencyPageProfile()
        {
            // ViewModel -> Model
            CreateMap<FunctionDependencyPageDependencyItemViewModel, FunctionDependency>()
                .ForMember(d => d.OrgFunctionCd, o => o.MapFrom(s => s.SrcFunctionId))
                .ForMember(d => d.DstFunctionCd, o => o.MapFrom(s => s.DstFunctionId))
                .ForMember(d => d.DependencyTypeCd, o => o.MapFrom(s => s.DependencyType));

            // Model -> ViewModel
            CreateMap<FunctionDependency, FunctionDependencyPageDependencyItemViewModel>()
                .ForMember(d => d.SrcFunctionId, o => o.MapFrom(s => s.OrgFunctionCd))
                .ForMember(d => d.DstFunctionId, o => o.MapFrom(s => s.DstFunctionCd))
                .ForMember(d => d.DependencyType, o => o.MapFrom(s => s.DependencyTypeCd));
        }
    }
}
