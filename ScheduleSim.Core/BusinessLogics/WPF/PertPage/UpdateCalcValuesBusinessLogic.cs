using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Core.IO.WPF.PertPage;

namespace ScheduleSim.Core.BusinessLogics.WPF.PertPage
{
    public class UpdateCalcValuesBusinessLogic : IUpdateCalcValuesBusinessLogic
    {
        public UpdateCalcValuesOutput Execute(UpdateCalcValuesInput input)
        {
            var output = new UpdateCalcValuesOutput();

            // 稼働日数を演算
            var totalValueOfPeriod = CalcTotalValueOfPeriod(input.StartDate, input.EndDate, input.RestDate, input.Holidays, input.ValueOfDay);
            // 最早開始時刻を演算
            var earliestStartValues = CalcEarliestStartValues(input.Data);
            // 最遅開始時刻を演算
            var latestStartValues = CalcLatestStartValues(input.Data, totalValueOfPeriod);
            // 最早完了時刻を演算
            var earliestFinishValues = CalcEarliestFinishValues(input.Data, earliestStartValues);
            // 最遅完了時刻を演算
            var latestFinishValues = CalcLatestFinishValues(input.Data, latestStartValues);
            // トータルフロート(全余裕時間)を演算
            var totalFloats = CalcTotalFloatValues(latestStartValues, earliestStartValues);

            output.CalcValues = input.Data.Select(x =>
            {
                return new UpdateCalcValuesOutput.CalcValue()
                {
                    EarliestStartTime = earliestStartValues[x.Id],
                    LatestStartTime = latestStartValues[x.Id],
                    EarliestFinishTime = earliestFinishValues[x.Id],
                    LatestFinishTime = latestFinishValues[x.Id],
                    TotalFloat = totalFloats[x.Id]
                };
            });

            return output;
        }

        /// <summary>
        /// トータルフロート(Total Float)を演算する
        /// </summary>
        /// <param name="latestStartValues"></param>
        /// <param name="earliestStartValues"></param>
        /// <returns></returns>
        private IDictionary<int, double> CalcTotalFloatValues(IDictionary<int, double> latestStartValues, IDictionary<int, double> earliestStartValues)
        {
            return
                latestStartValues.ToDictionary(x => x.Key, x => x.Value - earliestStartValues[x.Key]);
        }

        /// <summary>
        /// 最遅終了時刻の演算
        /// </summary>
        /// <param name="data"></param>
        /// <param name="latestStartValues"></param>
        /// <returns></returns>
        private IDictionary<int, double> CalcLatestFinishValues(IEnumerable<UpdateCalcValuesInput.ActivityData> data, IDictionary<int, double> latestStartValues)
        {
            return
                data.ToDictionary(x => x.Id, x => x.PlanValue + latestStartValues[x.Id]);
        }

        /// <summary>
        /// 最早終了時刻の演算
        /// </summary>
        /// <param name="data"></param>
        /// <param name="earliestStartValues"></param>
        /// <returns></returns>
        private IDictionary<int, double> CalcEarliestFinishValues(IEnumerable<UpdateCalcValuesInput.ActivityData> data, IDictionary<int, double> earliestStartValues)
        {
            return
                data.ToDictionary(x => x.Id, x => x.PlanValue + earliestStartValues[x.Id]);
        }

        /// <summary>
        /// 期間内の労働力を調べる
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="restDate"></param>
        /// <param name="holidays"></param>
        /// <param name="valueOfDay"></param>
        /// <returns></returns>
        private double CalcTotalValueOfPeriod(DateTime startDate, DateTime endDate, IEnumerable<DayOfWeek> restDate, IEnumerable<DateTime> holidays, double valueOfDay)
        {
            // 日数を算出
            var span = endDate - startDate;
            var days = Enumerable.Range(0, 1 + (int)Math.Ceiling(span.TotalDays)).Select(i => startDate.AddDays(i)).ToArray();
            // 休日にあたる曜日を減算
            days = days.Where(x => restDate.Contains(x.DayOfWeek) == false && holidays.Contains(x) == false).ToArray();

            // 稼働日×一日当たりの生産量
            return
                days.Length * valueOfDay;
        }

        /// <summary>
        /// 最早開始時刻の演算
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IDictionary<int, double> CalcEarliestStartValues(IEnumerable<UpdateCalcValuesInput.ActivityData> data)
        {
            // ノード番号の一覧を作成する
            var valMap = new Dictionary<int, double>();
            var nodes = new HashSet<int>();
            var srcNodes = new HashSet<int>();
            var dstNodes = new HashSet<int>();
            foreach (var edge in data)
            {
                nodes.Add(edge.SrcNodeId);
                nodes.Add(edge.DstNodeId);
                srcNodes.Add(edge.SrcNodeId);
                dstNodes.Add(edge.DstNodeId);
            }

            // 終点ノードを検索する
            var finishNodes = nodes.Where(x => srcNodes.Contains(x) == false && dstNodes.Contains(x)).ToArray();

            // 終点ノードに直列しているノードを検索
            var finishEdges = data.Where(x => finishNodes.Contains(x.DstNodeId)).ToArray();
            foreach (var finishEdge in finishEdges)
            {
                // 終点ノードの最早開始時刻=MAX(直列ノードの最早開始時刻+ノード自身の作業時間)
                valMap[finishEdge.Id] = CalcEarliestStartValue(finishEdge, data, valMap);
            }
            
            return valMap;
        }

        /// <summary>
        /// エッジごとの最早開始時刻の演算
        /// </summary>
        /// <param name="finishEdge"></param>
        /// <param name="data"></param>
        /// <param name="valMap"></param>
        /// <returns></returns>
        private double CalcEarliestStartValue(UpdateCalcValuesInput.ActivityData finishEdge, IEnumerable<UpdateCalcValuesInput.ActivityData> data, Dictionary<int, double> valMap)
        {
            // 接続エッジを検索
            var edges = data.Where(x => x.DstNodeId == finishEdge.SrcNodeId).ToArray();

            // 接続エッジが存在しない=始点の場合再帰処理を終了
            if (edges.Length == 0)
            {
                return 0.0;
            }

            return edges.Max(x =>
            {
                var val = CalcEarliestStartValue(x, data, valMap);
                valMap[x.Id] = val;
                return val + x.PlanValue;
            });
        }
        
        /// <summary>
        /// 最遅開始時刻の演算
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IDictionary<int, double> CalcLatestStartValues(IEnumerable<UpdateCalcValuesInput.ActivityData> data, double totalValueOfPeriod)
        {
            // ノード番号の一覧を作成する
            var valMap = new Dictionary<int, double>();
            var nodes = new HashSet<int>();
            var srcNodes = new HashSet<int>();
            var dstNodes = new HashSet<int>();
            foreach (var edge in data)
            {
                nodes.Add(edge.SrcNodeId);
                nodes.Add(edge.DstNodeId);
                srcNodes.Add(edge.SrcNodeId);
                dstNodes.Add(edge.DstNodeId);
            }

            // 始点ノードを検索する
            var startNodes = nodes.Where(x => dstNodes.Contains(x) == false && srcNodes.Contains(x)).ToArray();

            // 始点ノードに直列しているノードを検索
            var startEdges = data.Where(x => startNodes.Contains(x.SrcNodeId)).ToArray();
            foreach (var startEdge in startEdges)
            {
                // 終点ノードの最早開始時刻=MAX(直列ノードの最早開始時刻+ノード自身の作業時間)
                valMap[startEdge.Id] = CalcLatestStartValue(startEdge, data, valMap, totalValueOfPeriod) - startEdge.PlanValue;
            }

            return valMap;
        }

        /// <summary>
        /// エッジごとの最遅開始時刻の演算
        /// </summary>
        /// <param name="finishEdge"></param>
        /// <param name="data"></param>
        /// <param name="valMap"></param>
        /// <returns></returns>
        private double CalcLatestStartValue(UpdateCalcValuesInput.ActivityData startEdge, IEnumerable<UpdateCalcValuesInput.ActivityData> data, Dictionary<int, double> valMap, double totalValueOfPeriod)
        {
            // 接続エッジを検索
            var edges = data.Where(x => x.SrcNodeId == startEdge.DstNodeId).ToArray();

            // 接続エッジが存在しない=終点の場合再帰処理を終了
            if (edges.Length == 0)
            {
                return totalValueOfPeriod;
            }

            return edges.Min(x =>
            {
                var val = CalcLatestStartValue(x, data, valMap, totalValueOfPeriod) - x.PlanValue;
                valMap[x.Id] = val;
                return val;
            });
        }
    }
}
