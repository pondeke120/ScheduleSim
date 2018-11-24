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
    public class PertPageProfile : Profile
    {
        public PertPageProfile()
        {
            // ViewModel -> Model
            CreateMap<PertPageEdgeItemViewModel, Pert>()
                .ForMember(d => d.SrcNodeCd, o => o.MapFrom(s => s.INode))
                .ForMember(d => d.DstNodeCd, o => o.MapFrom(s => s.JNode))
                .ForMember(d => d.TaskCd, o => o.MapFrom(s => s.TaskId));

            // Model -> ViewModel
            CreateMap<Pert, PertPageEdgeItemViewModel>()
                .ForMember(d => d.INode, o => o.MapFrom(s => s.SrcNodeCd))
                .ForMember(d => d.JNode, o => o.MapFrom(s => s.DstNodeCd))
                .ForMember(d => d.TaskId, o => o.MapFrom(s => s.TaskCd));
        }
    }
}
