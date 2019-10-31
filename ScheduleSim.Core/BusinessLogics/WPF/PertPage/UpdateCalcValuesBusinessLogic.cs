using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleSim.Core.IO.WPF.PertPage;
using ScheduleSim.Entities.Models;

namespace ScheduleSim.Core.BusinessLogics.WPF.PertPage
{
    public class UpdateCalcValuesBusinessLogic : IUpdateCalcValuesBusinessLogic
    {
        public UpdateCalcValuesOutput Execute(UpdateCalcValuesInput input)
        {
            var output = new UpdateCalcValuesOutput();

            // 循環依存のチェックを行う
            var circulationList = null as List<int>;
            if (ContainsCirculation(input.Data, out circulationList))
            {
                var list = string.Join(",", circulationList.ToArray());
                throw new Exception($"Contains Circulation{Environment.NewLine}{list}");
            }

            // 稼働日数を演算
            var totalValueOfPeriod = CalcTotalValueOfPeriod(input.StartDate, input.EndDate, input.RestDate, input.Holidays, input.Members);
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
            // フリーフロート(自由余裕時間)を演算
            var freeFloats = CalcFreeFloatValues(input.Data, earliestStartValues);
            // クリティカルパスを演算
            var criticalFlags = CalcCriticalPath(totalFloats);

            output.CalcValues = input.Data.Select(x =>
            {
                return new UpdateCalcValuesOutput.CalcValue()
                {
                    EdgeId = x.Id,
                    EarliestStartTime = earliestStartValues[x.Id],
                    LatestStartTime = latestStartValues[x.Id],
                    EarliestFinishTime = earliestFinishValues[x.Id],
                    LatestFinishTime = latestFinishValues[x.Id],
                    TotalFloat = totalFloats[x.Id],
                    FreeFloat = freeFloats[x.Id],
                    IsCritical = criticalFlags[x.Id]
                };
            });

            return output;
        }

        /// <summary>
        /// 循環依存のチェック
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool ContainsCirculation(IEnumerable<UpdateCalcValuesInput.ActivityData> data, out List<int> circulationList)
        {
            // ノード番号の一覧を作成する
            var nodes = new HashSet<int>();
            var srcNodes = new HashSet<int>();
            var dstNodes = new HashSet<int>();
            foreach (var edge in data)
            {
                // src = dstがあったらそこで循環してる
                if (edge.SrcNodeId == edge.DstNodeId)
                {
                    circulationList = new List<int> { edge.SrcNodeId, edge.DstNodeId };
                    return true;
                }

                nodes.Add(edge.SrcNodeId);
                nodes.Add(edge.DstNodeId);
                srcNodes.Add(edge.SrcNodeId);
                dstNodes.Add(edge.DstNodeId);
            }

            // 始点ノードを検索する
            var startNodes = nodes.Where(x => dstNodes.Contains(x) == false && srcNodes.Contains(x)).ToArray();

            // 始点ノードを深さ優先探索
            var containsCirculation = false;
            circulationList = null;
            foreach (var startNode in startNodes)
            {
                var nodeMap = nodes.ToDictionary(x => x, x => false);
                var already = new List<int>();
                containsCirculation |= DepthFirstSearch(startNode, data, nodeMap, already);
                if (containsCirculation)
                {
                    circulationList = already;
                    break;
                }
            }

            return
                containsCirculation;
        }

        /// <summary>
        /// ノード間で同じ場所をたどらないように探索する
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="nodes"></param>
        /// <param name="nodeMap"></param>
        /// <returns></returns>
        private bool DepthFirstSearch(int startNode, IEnumerable<UpdateCalcValuesInput.ActivityData> data, Dictionary<int, bool> nodeMap, List<int> already)
        {
            var ret = false;

            // 自ノードはこれで探索済み
            nodeMap[startNode] = true;
            already.Add(startNode);

            // 接続エッジを検索
            var edges = data.Where(x => x.SrcNodeId == startNode).ToArray();

            // 終点の場合探索終了
            if (edges.Length == 0)
                return false;

            // 接続先の探索済みノードがある場合は循環している
            if (edges.Any(x => already.Contains(x.DstNodeId)))
            {
                var edge = edges.FirstOrDefault(x => already.Contains(x.DstNodeId));
                already.Add(edge.DstNodeId);
                return true;
            }

            // 探索済みの接続先を除外する
            var nexts = edges.Where(x => nodeMap[x.DstNodeId] == false).ToArray();

            // 次のノードを探索
            foreach (var next in nexts)
            {
                ret |= DepthFirstSearch(next.DstNodeId, data, nodeMap, already);
                if (ret) break;
                // 接続先の探索が終わったら次の探索を始める前にパスごとの探索済みフラグをリセットする
                already.Remove(next.DstNodeId);
            }

            return ret;
        }

        /// <summary>
        /// クリティカルパスを演算する
        /// </summary>
        /// <param name="totalFloats"></param>
        /// <returns></returns>
        private IDictionary<int, bool> CalcCriticalPath(IDictionary<int, double> totalFloats)
        {
            // トータルフローの最小値を取得
            var minFloat = double.MinValue;
            if (totalFloats.Count > 0)
                minFloat = totalFloats.Values.Min();

            // トータルフローが最小値をとっているパスがクリティカルパス
            return
                totalFloats.ToDictionary(x => x.Key, x => Math.Abs(x.Value - minFloat) < 0.0001);
        }

        /// <summary>
        /// フリーフロート(自由余裕時間)を演算する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="earliestStartValues"></param>
        /// <returns></returns>
        private IDictionary<int, double> CalcFreeFloatValues(IEnumerable<UpdateCalcValuesInput.ActivityData> data, IDictionary<int, double> earliestStartValues)
        {
            // ノード番号の一覧を作成する
            var valMap = new Dictionary<int, double>();

            // edgeごとに演算
            foreach (var edge in data)
            {
                // 対象エッジの終点を始点とするエッジを取得
                var nextEdges = data.Where(x => x.SrcNodeId == edge.DstNodeId).ToArray();
                // 終点の場合はFreeFloatは0
                if (nextEdges.Length == 0)
                {
                    valMap[edge.Id] = 0.0;
                    continue;
                }

                // 接続ノードの次のエッジの最早開始時刻の最小値を求める
                var earliestNextStartTime = nextEdges.Min(x => earliestStartValues[x.Id]);
                // フリーフロート(自由余裕時間)を算出
                // 接続エッジの最早開始時刻 - (対象エッジの最早開始時刻 + 所要時間)
                valMap[edge.Id] = earliestNextStartTime - (earliestStartValues[edge.Id] + edge.PlanValue);
            }

            return valMap;
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
        private double CalcTotalValueOfPeriod(DateTime startDate, DateTime endDate, IEnumerable<DayOfWeek> restDate, IEnumerable<DateTime> holidays, IEnumerable<Member> members)
        {
            // 要員ごとに参画期間内に出せるアウトプット量を計上
            var sum = 0.0;
            foreach (var member in members)
            {
                var startDateTemp = startDate > member.JoinDate ? startDate : (member.JoinDate ?? startDate) as DateTime?;
                var endDateTemp = endDate < member.LeaveDate ? endDate : (member.LeaveDate ?? endDate) as DateTime?;
                // 日数を算出
                var span = (endDateTemp - startDateTemp).Value;
                var days = Enumerable.Range(0, 1 + (int)Math.Ceiling(span.TotalDays)).Select(i => startDateTemp.Value.AddDays(i)).ToArray();
                // 休日にあたる曜日を減算
                days = days.Where(x => restDate.Contains(x.DayOfWeek) == false && holidays.Contains(x) == false).ToArray();

                // 要員のアウトプット量を加算
                sum += days.Length * member.Productivity ?? 0.0;
            }
            // 稼働日×一日当たりの生産量
            return
                sum;
        }

        /// <summary>
        /// 最早開始時刻の演算
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IDictionary<int, double> CalcEarliestStartValues(IEnumerable<UpdateCalcValuesInput.ActivityData> data)
        {
            // ノード番号の一覧を作成する
            var valMap = data.ToDictionary(x => x.Id, x => 0.0);
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
            var valMap = data.ToDictionary(x => x.Id, x => 0.0);
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
                // 終点ノードの最遅開始時刻=MAX(直列ノードの最早開始時刻+ノード自身の作業時間)
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
