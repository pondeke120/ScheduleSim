using ScheduleSim.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Core.IO.WPF.ImportTool;
using System.Text.RegularExpressions;

namespace ScheduleSim.Core.Utility
{
    public class CsvToTaskItemConverter : ICsvToModelConverter<OpenFileOutput.TaskItem>
    {
        private const string DATETIME_FORMAT = @"\d{4}/\d{1,2}/\d{1,2}";
        private readonly static Regex DateTimeRegex = new Regex(DATETIME_FORMAT);
        private string processHeaderName;
        private string functionHeaderName;
        private string taskHeaderName;
        private string planValueheaderName;
        private string startDateHeaderName;
        private string endDateHeaderName;
        private int processIndex;
        private int functionIndex;
        private int taskIndex;
        private int planValueIndex;
        private int startDateIndex;
        private int endDateIndex;

        public CsvToTaskItemConverter(
            string processHeaderName,
            string functionHeaderName,
            string taskHeaderName,
            string planValueheaderName,
            string startDateHeaderName,
            string endDateHeaderName)
        {
            this.processHeaderName = processHeaderName;
            this.functionHeaderName = functionHeaderName;
            this.taskHeaderName = taskHeaderName;
            this.planValueheaderName = planValueheaderName;
            this.startDateHeaderName = startDateHeaderName;
            this.endDateHeaderName = endDateHeaderName;
        }

        /// <summary>
        /// ヘッダから読み取るべき添え字を設定
        /// </summary>
        /// <param name="header"></param>
        public void SettingHeader(string header)
        {
            // カンマでスプリット
            var split = header.Split(',').ToList();
            // インデックスの位置を設定
            this.processIndex = split.IndexOf(this.processHeaderName);
            this.functionIndex = split.IndexOf(this.functionHeaderName);
            this.taskIndex = split.IndexOf(this.taskHeaderName);
            this.planValueIndex = split.IndexOf(this.planValueheaderName);
            this.startDateIndex = split.IndexOf(this.startDateHeaderName);
            this.endDateIndex = split.IndexOf(this.endDateHeaderName);
        }

        public OpenFileOutput.TaskItem Convert(string text)
        {
            // カンマでスプリット
            var split = text.Split(',');
            // モデルを生成
            var output = new OpenFileOutput.TaskItem();
            // データ型に変換
            output.Process = split[this.processIndex];
            output.Function = split[this.functionIndex];
            output.TaskName = split[this.taskIndex];
            var planValue = 0.0;
            if (double.TryParse(split[this.planValueIndex], out planValue))
            {
                output.PlanValue = planValue;
            }
            var startDate = default(DateTime);
            var startDateMatch = DateTimeRegex.Match(split[this.startDateIndex]);
            var startDateText = startDateMatch.Success ? startDateMatch.Captures[0].ToString() : string.Empty;
            if (DateTime.TryParseExact(startDateText, "yyyy/M/d", null, System.Globalization.DateTimeStyles.None, out startDate))
            {
                output.StartDate = startDate;
            }
            var endDate = default(DateTime);
            var endDateMatch = DateTimeRegex.Match(split[this.endDateIndex]);
            var endDateText = endDateMatch.Success ? endDateMatch.Captures[0].ToString() : string.Empty;
            if (DateTime.TryParseExact(endDateText, "yyyy/M/d", null, System.Globalization.DateTimeStyles.None, out endDate))
            {
                output.EndDate = endDate;
            }

            return output;
        }

    }
}
