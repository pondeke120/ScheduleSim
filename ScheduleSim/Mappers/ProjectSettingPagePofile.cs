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
    public class ProjectSettingPagePofile : Profile
    {
        public ProjectSettingPagePofile()
        {
            // ViewModel -> Model
            CreateMap<ProjectSettingPageProcessItemViewModel, Process>()
                .ForMember(d => d.ProcessCd, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ProcessName, o => o.MapFrom(s => s.Name));
            CreateMap<ProjectSettingPageFunctionItemViewModel, Function>()
                .ForMember(d => d.FunctionCd, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FunctionName, o => o.MapFrom(s => s.Name));
            CreateMap<ProjectSettingPageHolidayItemViewModel, Holiday>()
                .ForMember(d => d.HolidayCd, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.HolidayDate, o => o.MapFrom(s => s.Date));
            CreateMap<ProjectSettingPageWeekdayItemViewModel, WeekDay>()
                .ForMember(d => d.WeekdayCd, o => o.MapFrom(s => s.DayOfWeek))
                .ForMember(d => d.HolidayFlg, o => o.MapFrom(s => s.IsCheck))
                .ForMember(d => d.WeekdayName, o => o.MapFrom(s => Enum.GetName(typeof(DayOfWeek), s.DayOfWeek)));

            // Model -> ViewModel
            CreateMap<Process, ProjectSettingPageProcessItemViewModel>()
                .ForMember(d => d.DataContext, o => o.MapFrom(s => s));
            CreateMap<Function, ProjectSettingPageFunctionItemViewModel>()
                .ForMember(d => d.DataContext, o => o.MapFrom(s => s));
            CreateMap<Holiday, ProjectSettingPageHolidayItemViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.HolidayCd))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.HolidayDate));
            CreateMap<WeekDay, ProjectSettingPageWeekdayItemViewModel>()
                .ForMember(d => d.DayOfWeek, o => o.MapFrom(s => s.WeekdayCd))
                .ForMember(d => d.IsCheck, o => o.MapFrom(s => s.HolidayFlg));
        }
    }
}
