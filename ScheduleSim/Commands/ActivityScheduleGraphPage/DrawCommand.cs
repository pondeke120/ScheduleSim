using LiveCharts;
using LiveCharts.Wpf;
using ScheduleSim.Core.Contexts;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.Commands.ActivityScheduleGraphPage
{
    public class DrawCommand : ICommand
    {
        private AppContext appContext;

        public DrawCommand(
            AppContext appContext)
        {
            this.appContext = appContext;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as ActivityScheduleGraphPageViewModel;


            // プロジェクトの期間を取得
            var span = TimeSpan.Zero as TimeSpan?;
            var startDate = this.appContext.PrjSettings.StartDate;
            var endDate = this.appContext.PrjSettings.EndDate;

            if (startDate.HasValue && endDate.HasValue)
            {
                span = new TimeSpan((endDate - startDate).Value.Ticks);
            }
            else if (this.appContext.Tasks.Any(x => x.StartDate.HasValue) && this.appContext.Tasks.Any(x => x.EndDate.HasValue))
            {
                startDate = this.appContext.Tasks.Where(x => x.StartDate.HasValue).Min(x => x.StartDate);
                endDate = this.appContext.Tasks.Where(x => x.EndDate.HasValue).Max(x => x.EndDate);
                span = new TimeSpan((endDate - startDate).Value.Ticks);
            }
            else
                return;

            // 休業日を抽出
            var weekDays = this.appContext.WeekDays.Where(x => x.HolidayFlg).Select(x => x.WeekdayCd).ToArray();
            var holidays = Enumerable.Range(0, (int)span.Value.TotalDays + 1).Select(x => startDate.Value.AddDays(x))
                                                          .Where(x => weekDays.Contains(x.DayOfWeek))
                                                          .Concat(this.appContext.Holidays.Where(x => x.HolidayDate.HasValue).Select(x => x.HolidayDate.Value))
                                                          .Distinct()
                                                          .ToArray();

            // 要員を取得
            var members = this.appContext.Members;
            var sc = new SeriesCollection();

            foreach (var member in members)
            {
                var activity = Enumerable.Range(0, (int)span.Value.TotalDays + 1).Select(x => startDate.Value.AddDays(x))
                                                                             .Where(x => holidays.Contains(x) == false)
                                                                             .ToDictionary(x => x, x => 0.0);

                // 要員ごとのタスクを取得
                var tasks = this.appContext.Tasks.Where(x => x.AssignMemberCd == member.MemberCd).ToArray();
                
                // タスクごとの作業期間から一日当たりの稼働量を算出
                foreach (var task in tasks)
                {
                    // 期間内の日数を算出
                    var taskStartDate = startDate;
                    var taskEndDate = endDate;

                    if (task.StartDate.HasValue)
                        taskStartDate = task.StartDate.Value;
                    if (task.EndDate.HasValue)
                        taskEndDate = task.EndDate.Value;

                    var taskSpan = new TimeSpan((taskEndDate - taskStartDate).Value.Ticks);

                    // 期間内に含まれる休業日を算出
                    // 期間内の稼働日数を算出
                    var activeDays = Enumerable.Range(0, (int)taskSpan.TotalDays + 1).Select(x => taskStartDate.Value.AddDays(x))
                                                                                 .Where(x => holidays.Contains(x) == false)
                                                                                 .ToArray();

                    // 日付ごとの稼働量に加算
                    var planValuePerDays = (task.PlanValue ?? 0.0) / activeDays.Count();
                    foreach (var activeDay in activeDays)
                    {
                        activity[activeDay] += planValuePerDays;
                    }
                }

                // グラフに変換
                sc.Add(new LineSeries()
                {
                    Title = member.MemberName,
                    Values = new ChartValues<double>(activity.Values)
                });
            }


            // 描画データ更新
            GraphData gd = new GraphData();

            gd.SeriesCollection = sc;
            gd.Labels = Enumerable.Range(0, (int)span.Value.TotalDays + 1).Select(i => startDate.Value.AddDays(i))
                                                                      .Where(x => holidays.Contains(x) == false)
                                                                      .Select(x => x.ToString("yyyy/MM/dd")).ToArray();
            gd.YFormatter = value => $"{value.ToString("0.0")} H";

            viewModel.GraphData = gd;
        }
    }
}
