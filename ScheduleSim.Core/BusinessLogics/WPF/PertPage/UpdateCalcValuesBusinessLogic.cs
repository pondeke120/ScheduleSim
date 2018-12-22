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

            // 最早開始時刻を演算
            var earlistStartValues = CalcEarlistStartValues(input.Data);

            output.CalcValues = input.Data.Select(x =>
            {
                return new UpdateCalcValuesOutput.CalcValue()
                {
                    EarliestStartTime = earlistStartValues[x.Id]
                };
            });

            return output;
        }

        private IDictionary<int, double> CalcEarlistStartValues(IEnumerable<UpdateCalcValuesInput.ActivityData> data)
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
                valMap[finishEdge.Id] = CalcEarlistStartValue(finishEdge, data, valMap);
            }
            
            return valMap;
        }

        private double CalcEarlistStartValue(UpdateCalcValuesInput.ActivityData finishEdge, IEnumerable<UpdateCalcValuesInput.ActivityData> data, Dictionary<int, double> valMap)
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
                var val = CalcEarlistStartValue(x, data, valMap);
                valMap[x.Id] = val;
                return val + x.PlanValue;
            });
        }
    }
}
