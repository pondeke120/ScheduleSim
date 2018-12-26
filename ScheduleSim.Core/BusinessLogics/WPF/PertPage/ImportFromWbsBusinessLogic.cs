using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Core.IO.WPF.PertPage;
using ScheduleSim.Core.Utility;
using ScheduleSim.Entities.Models;

namespace ScheduleSim.Core.BusinessLogics.WPF.PertPage
{
    public class ImportFromWbsBusinessLogic : IImportFromWbsBusinessLogic
    {
        private IIDGenerator pertIdGen;

        public ImportFromWbsBusinessLogic(
            IIDGenerator pertIdGen)
        {
            this.pertIdGen = pertIdGen;
        }

        public ImportFromWbsOutput Execute(ImportFromWbsInput input)
        {
            var output = new ImportFromWbsOutput();
            var tasks = input.Tasks;
            var perts = input.Perts;
            var existTaskIds = perts.Select(x => x.TaskCd).ToArray();

            // Pertに未登録のタスクを抽出
            var addTasks = tasks.Where(x => existTaskIds.Contains(x.TaskCd) == false).ToArray();

            // 追加するタスクの一覧を生成
            var addPerts = addTasks.Select(x => new Pert()
            {
                Id = this.pertIdGen.CreateNewId(),
                TaskCd = x.TaskCd,
                SrcNodeCd = 0,
                DstNodeCd = 0,
            });

            output.Perts = perts.Concat(addPerts).ToArray();

            return output;
        }
    }
}
