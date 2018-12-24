using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScheduleSim.Core.IO.WPF.ImportTool;
using ScheduleSim.Core.Service;
using System.IO;
using ScheduleSim.Entities.Models;
using ScheduleSim.Core.Utility;

namespace ScheduleSim.Core.BusinessLogics.WPF.ImportTool
{
    public class CompleteBusinessLogic : ICompleteBusinessLogic
    {
        private ICsvReadService<OpenFileOutput.TaskItem> csvReadService;
        private IXlsxReadService<OpenFileOutput.TaskItem> xlsxReadService;

        public CompleteBusinessLogic(
            ICsvReadService<OpenFileOutput.TaskItem> csvReadService,
            IXlsxReadService<OpenFileOutput.TaskItem> xlsxReadService)
        {
            this.csvReadService = csvReadService;
            this.xlsxReadService = xlsxReadService;
        }

        public CompleteOutput Execute(CompleteInput input)
        {
            var output = new CompleteOutput();

            // 読み込み対象ファイルの拡張子によって処理を変える
            var ext = Path.GetExtension(input.FilePath);

            var taskItems = null as OpenFileOutput.TaskItem[];
            if (ext.Equals(".csv"))
            {
                // 全レコード変換
                taskItems = this.csvReadService.Read(input.FilePath).ToArray();
            }
            else if (ext.Equals(".xlsx"))
            {
                // 全レコード変換
                taskItems = this.xlsxReadService.Read(input.FilePath).ToArray();
            }
            output.TaskItems = taskItems;
            output.Tasks = ConvertTaskEntity(taskItems).ToArray();
            output.Processes = ConvertProcessEntity(taskItems).ToArray();
            output.Functions = ConvertFunctionEntity(taskItems).ToArray();
            output.Members = ConvertMemberEntity(taskItems).ToArray();

            return output;
        }

        private IEnumerable<Member> ConvertMemberEntity(IEnumerable<OpenFileOutput.TaskItem> taskItems)
        {
            var memberNames = taskItems.Select(x => x.Member).Distinct();

            return
                memberNames.Select(x => new Member() { MemberName = x });
        }

        private IEnumerable<Function> ConvertFunctionEntity(IEnumerable<OpenFileOutput.TaskItem> taskItems)
        {
            var functionNames = taskItems.Select(x => x.Function).Distinct();

            return
                functionNames.Select(x => new Function() { FunctionName = x });
        }

        private IEnumerable<Process> ConvertProcessEntity(IEnumerable<OpenFileOutput.TaskItem> taskItems)
        {
            var processNames = taskItems.Select(x => x.Process).Distinct();

            return
                processNames.Select(x => new Process() { ProcessName = x });
        }

        private IEnumerable<Task> ConvertTaskEntity(IEnumerable<OpenFileOutput.TaskItem> taskItems)
        {
            return
                taskItems.Select(x =>
                {
                    return
                        new Task()
                        {
                            TaskName = x.TaskName,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            PlanValue = x.PlanValue,
                            // IDは選択肢によって振りなおすためnull
                            ProcessCd = null,
                            FunctionCd = null,
                            AssignMemberCd = null
                        };
                });
        }
    }
}
