using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Core.IO.WPF.ImportTool;
using ScheduleSim.Core.Service;
using System.IO;

namespace ScheduleSim.Core.BusinessLogics.WPF.ImportTool
{
    public class OpenFileBusinessLogic : IOpenFileBusinessLogic
    {
        private ICsvReadService<OpenFileOutput.TaskItem> csvReadService;
        private IXlsxReadService<OpenFileOutput.TaskItem> xlsxReadService;

        public OpenFileBusinessLogic(
            ICsvReadService<OpenFileOutput.TaskItem> csvReadService,
            IXlsxReadService<OpenFileOutput.TaskItem> xlsxReadService)
        {
            this.csvReadService = csvReadService;
            this.xlsxReadService = xlsxReadService;
        }

        public OpenFileOutput Execute(OpenFileInput input)
        {
            var output = new OpenFileOutput();

            // 読み込み対象ファイルの拡張子によって処理を変える
            var ext = Path.GetExtension(input.FilePath);

            if (ext.Equals(".csv"))
            {
                // 100レコードまで変換
                output.TaskItems = this.csvReadService.Read(input.FilePath).Take(100).ToArray();
            }
            else if (ext.Equals(".xlsx"))
            {
                // 100レコードまで変換
                output.TaskItems = this.xlsxReadService.Read(input.FilePath).Take(100).ToArray();
            }

            return output;
        }
    }
}
