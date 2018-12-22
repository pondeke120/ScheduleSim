using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleSim.Core.IO.WPF.PertPage
{
    public class UpdateCalcValuesOutput
    {
        public IEnumerable<CalcValue> CalcValues { get; set; }

        public class CalcValue
        {
            /// <summary>
            /// アクティビティID
            /// </summary>
            public int EdgeId { get; set; }
            /// <summary>
            /// 最早開始時刻
            /// </summary>
            public double EarliestStartTime { get; set; }
            /// <summary>
            /// 最早終了時刻
            /// </summary>
            public double EarliestFinishTime { get; set; }
            /// <summary>
            /// 最遅開始時刻
            /// </summary>
            public double LatestStartTime { get; set; }
            /// <summary>
            /// 最遅終了時刻
            /// </summary>
            public double LatestFinishTime { get; set; }
            /// <summary>
            /// フリーフロート(自由余裕時間)
            /// </summary>
            public double FreeFloat { get; set; }
            /// <summary>
            /// トータルフロート(Total Float)
            /// </summary>
            public double TotalFloat { get; set; }
            /// <summary>
            /// クリティカルパスフラグ
            /// </summary>
            public bool IsCritical { get; set; }
        }
    }
}
