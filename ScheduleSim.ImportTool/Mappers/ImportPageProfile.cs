using AutoMapper;
using ScheduleSim.Core.IO.WPF.ImportTool;
using ScheduleSim.ImportTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.ImportTool.Mappers
{
    public class ImportPageProfile : Profile
    {
        public ImportPageProfile()
        {
            // ViewModel -> Model
            CreateMap<ImportPageTaskItemViewModel, OpenFileOutput.TaskItem>()
                    .ForMember(d => d.Process, o => o.MapFrom(s => s.Process))
                    .ForMember(d => d.Function, o => o.MapFrom(s => s.Function))
                    .ForMember(d => d.TaskName, o => o.MapFrom(s => s.TaskName))
                    .ForMember(d => d.PlanValue, o => o.MapFrom(s => s.PlanValue))
                    .ForMember(d => d.StartDate, o => o.MapFrom(s => s.StartDate))
                    .ForMember(d => d.EndDate, o => o.MapFrom(s => s.EndDate))
                    .ForMember(d => d.Member, o => o.MapFrom(s => s.Member));

            // Model -> ViewModel
            CreateMap<OpenFileOutput.TaskItem, ImportPageTaskItemViewModel>()
                    .ForMember(d => d.DataContext, o => o.MapFrom(s => s));
        }
    }
}
