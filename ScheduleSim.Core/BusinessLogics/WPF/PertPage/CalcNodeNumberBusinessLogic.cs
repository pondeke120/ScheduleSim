using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScheduleSim.Core.IO.WPF.PertPage;
using ScheduleSim.Entities.Models;
using ScheduleSim.Entities.Enum;
using ScheduleSim.Core.Utility;

namespace ScheduleSim.Core.BusinessLogics.WPF.PertPage
{
    public class CalcNodeNumberBusinessLogic : ICalcNodeNumberBusinessLogic
    {
        private const int MAX_SLICING_COUNT = 32768;
        private IIDGenerator pertIdGen;

        public CalcNodeNumberBusinessLogic(
            IIDGenerator pertIdGen)
        {
            this.pertIdGen = pertIdGen;
        }
        public CalcNodeNumberOutput Execute(CalcNodeNumberInput input)
        {
            //変数を設定
            var result = new CalcNodeNumberOutput() { PertEdges = Enumerable.Empty<Pert>() };
            var funcDependencies = input.FunctionDependencies.Where(x => x.DependencyTypeCd.HasValue && x.DstFunctionCd.HasValue && x.OrgFunctionCd.HasValue).ToArray();
            var procDependencies = input.ProcessDependencies.Where(x => x.DependencyType.HasValue && x.DstProcessCd.HasValue && x.OrgProcessCd.HasValue).ToArray();
            var tasks = input.Tasks.ToArray();
            int cnt = 0;

            var dependencyMap = new Dictionary<Task, IList<IList<KeyValuePair<Task, DependencyTypes>>>>();
            var dependencyList = new Dictionary<Task, IEnumerable<KeyValuePair<Task, DependencyTypes>>>();
            foreach (var task in tasks)
            {
                var fromTasks = new List<KeyValuePair<Task, DependencyTypes>>();
                // 機能依存から依存元タスクを導出
                var fromFunctionIds = funcDependencies.Where(x => x.DstFunctionCd == task.FunctionCd)
                                                                    //&& (x.DependencyTypeCd == Entities.Enum.DependencyTypes.FinishStartDependency
                                                                    //    || x.DependencyTypeCd == Entities.Enum.DependencyTypes.FinishFinishDependency))
                                                      .ToDictionary(x => x.OrgFunctionCd.Value, x => x.DependencyTypeCd.Value);
                var fromTasksByFunctionDependency = fromFunctionIds.SelectMany(x => tasks.Where(y => y.FunctionCd == x.Key && task.ProcessCd == y.ProcessCd).Select(y => new KeyValuePair<Task, Entities.Enum.DependencyTypes>(y, x.Value)));


                // 工程依存から依存元タスクを導出
                var fromProcessIds = procDependencies.Where(x => x.DstProcessCd == task.ProcessCd)
                                                                    //&& (x.DependencyType == Entities.Enum.DependencyTypes.FinishStartDependency
                                                                    //    || x.DependencyType == Entities.Enum.DependencyTypes.FinishFinishDependency))
                                                        .ToDictionary(x => x.OrgProcessCd.Value, x => x.DependencyType.Value);
                var fromTasksByProcessDependency = fromProcessIds.SelectMany(x => tasks.Where(y => y.ProcessCd == x.Key && task.FunctionCd == y.FunctionCd).Select(y => new KeyValuePair<Task, Entities.Enum.DependencyTypes>(y, x.Value)));

                fromTasks.AddRange(fromTasksByFunctionDependency.Concat(fromTasksByProcessDependency));

                dependencyList.Add(task, fromTasks);
            }

            // 次のスライスを演算子結果を受理
            var nextResult = DoSlicing(tasks, funcDependencies, procDependencies, dependencyList, cnt++);

            // 結果をマージ
            foreach (var task in tasks)
            {
                dependencyMap[task] = new List<IList<KeyValuePair<Task, DependencyTypes>>>(){
                                            dependencyList[task].ToList()
                                      }.Concat(nextResult[task].Select(x => x)).ToList();
            }

            // 順番に番号を付けていく
            var allocationList = new Dictionary<Task, Pert>();
            var resultEdges = new List<Pert>();
            var createdDstNode = new Dictionary<Task, int>(); 
            var currentNodeNumber = 2;
            var createNumber = 0;
            foreach (var taskGraph in dependencyMap)
            {
                var task = taskGraph.Key;
                createNumber = 0;

                // 一次スライス(直前のタスク)のタスクから順に検索する
                var beforeNode = null as int?;
                var mergeEdgeList =  new List<Pert>();
                var dependencyTasks = new List<KeyValuePair<Task, DependencyTypes>>();
                foreach (var dependencyNodes in taskGraph.Value)
                {
                    var maybeFromTasks = dependencyNodes.Where(x => x.Value == DependencyTypes.FinishStartDependency || x.Value == DependencyTypes.FinishFinishDependency).ToArray();
                    foreach (var maybeFrom in maybeFromTasks)
                    {
                        // ESまたはEEの場合は該当タスクの終端番号を開始番号とする
                        // 2つ以上EE ESの関係がある場合、次数の低いスライスのタスクに高い次数のタスクがEE ESに含まれていないか調べる。
                        // 見つかった場合その関係は無視してよい。無視できない場合はマージ用ノードを作成し、依存ノードとマージノードの間に関係を作成する
                        if (beforeNode.HasValue == false)
                        {
                            beforeNode = allocationList[maybeFrom.Key].DstNodeCd;
                        }
                        else
                        {
                            if (dependencyTasks.SelectMany(x => dependencyMap[x.Key].SelectMany(y => y.Select(z => z)))
                                               .Where(x => x.Value == DependencyTypes.FinishStartDependency || x.Value == DependencyTypes.FinishFinishDependency)
                                               .Select(x => x.Key)
                                               .Contains(maybeFrom.Key) == false)
                            {
                                // マージ用ノードを作成
                                if (mergeEdgeList.Count == 0)
                                {
                                    // 最初に検出したエッジの宛先をマージ用ノードにする
                                    var firstTaskPert = allocationList[dependencyTasks.First().Key];
                                    mergeEdgeList.Add(new Pert()
                                    {
                                        Id = this.pertIdGen.CreateNewId(),
                                        SrcNodeCd = firstTaskPert.DstNodeCd,
                                        DstNodeCd = currentNodeNumber,
                                        TaskCd = null,
                                    });
                                }

                                // 依存元タスクとマージ用ノードをつなぐエッジ
                                var maybeFromPert = null as Pert;
                                var dstNodeNumber = 0;
                                if (allocationList.ContainsKey(maybeFrom.Key))
                                {
                                    maybeFromPert = allocationList[maybeFrom.Key];
                                    dstNodeNumber = maybeFromPert.DstNodeCd;
                                }
                                else
                                {
                                    if (createdDstNode.ContainsKey(maybeFrom.Key) == false)
                                    {
                                        createNumber++;
                                        createdDstNode.Add(maybeFrom.Key, currentNodeNumber + createNumber);
                                    }
                                    dstNodeNumber = createdDstNode[maybeFrom.Key];
                                }

                                mergeEdgeList.Add(new Pert()
                                {
                                    Id = this.pertIdGen.CreateNewId(),
                                    SrcNodeCd = dstNodeNumber,
                                    DstNodeCd = currentNodeNumber,
                                    TaskCd = null,
                                });

                                beforeNode = currentNodeNumber;
                            }       
                        }

                        dependencyTasks.Add(maybeFrom);
                    }
                }

                if (mergeEdgeList.Count > 0)
                {
                    currentNodeNumber += createNumber + 1;
                    resultEdges.AddRange(mergeEdgeList);
                }
                
                var pert = new Pert()
                {
                    Id = this.pertIdGen.CreateNewId(),
                    SrcNodeCd = beforeNode ?? 1,
                    DstNodeCd = createdDstNode.ContainsKey(task) ? createdDstNode[task] : currentNodeNumber,
                    TaskCd = task.TaskCd,
                };

                if (createdDstNode.ContainsKey(task) == false)
                    currentNodeNumber++;
                allocationList.Add(task, pert);
                resultEdges.Add(pert);
            }

            result.PertEdges = resultEdges;

            return result;
        }

        private Dictionary<Task, IList<IList<KeyValuePair<Task, DependencyTypes>>>> DoSlicing(
            Entities.Models.Task[] tasks,
            FunctionDependency[] funcDependencies,
            ProcessDependency[] procDependencies,
            Dictionary<Task, IEnumerable<KeyValuePair<Task, DependencyTypes>>> beforeDependencyList,
            int cnt)
        {
            if (cnt >= MAX_SLICING_COUNT)
                return new Dictionary<Task, IList<IList<KeyValuePair<Task, DependencyTypes>>>>();

            var dependencyMap = new Dictionary<Task, IList<IList<KeyValuePair<Task, DependencyTypes>>>>();
            var dependencyList = new Dictionary<Task, IEnumerable<KeyValuePair<Task, DependencyTypes>>>();
            foreach (var task in tasks)
            {
                var fromTaskList = beforeDependencyList[task];
                var fromTasks = new List<KeyValuePair<Task, DependencyTypes>>();
                foreach (var fromTaskAndType in fromTaskList)
                {
                    var fromTask = fromTaskAndType.Key;
                    // 機能依存から依存元タスクを導出
                    var fromFunctionIds = funcDependencies.Where(x => x.DstFunctionCd == fromTask.FunctionCd)
                                                                        //&& (x.DependencyTypeCd == Entities.Enum.DependencyTypes.FinishStartDependency
                                                                        //    || x.DependencyTypeCd == Entities.Enum.DependencyTypes.FinishFinishDependency))
                                                          .ToDictionary(x => x.OrgFunctionCd.Value, x => x.DependencyTypeCd.Value);
                    var fromTasksByFunctionDependency = fromFunctionIds.SelectMany(x => tasks.Where(y => y.FunctionCd == x.Key && fromTask.ProcessCd == y.ProcessCd).Select(y => new KeyValuePair<Task, Entities.Enum.DependencyTypes>(y, x.Value)));


                    // 工程依存から依存元タスクを導出
                    var fromProcessIds = procDependencies.Where(x => x.DstProcessCd == fromTask.ProcessCd)
                                                                        //&& (x.DependencyType == Entities.Enum.DependencyTypes.FinishStartDependency
                                                                        //    || x.DependencyType == Entities.Enum.DependencyTypes.FinishFinishDependency))
                                                            .ToDictionary(x => x.OrgProcessCd.Value, x => x.DependencyType.Value);
                    var fromTasksByProcessDependency = fromProcessIds.SelectMany(x => tasks.Where(y => y.ProcessCd == x.Key && fromTask.FunctionCd == y.FunctionCd).Select(y => new KeyValuePair<Task, Entities.Enum.DependencyTypes>(y, x.Value)));

                    fromTasks.AddRange(fromTasksByFunctionDependency.Concat(fromTasksByProcessDependency));
                }
                dependencyList.Add(task, fromTasks);
            }

            // 次のスライスを演算子結果を受理
            var nextResult = null as Dictionary<Task, IList<IList<KeyValuePair<Task, DependencyTypes>>>>;
            if (dependencyList.SelectMany(x => x.Value).Count() > 0)
            {
                nextResult = DoSlicing(tasks, funcDependencies, procDependencies, dependencyList, cnt + 1);

                // 結果をマージ
                foreach (var task in tasks)
                {
                    dependencyMap[task] = new List<IList<KeyValuePair<Task, DependencyTypes>>>(){
                                            dependencyList[task].ToList()
                                      }.Concat(nextResult[task].Select(x => x)).ToList(); ;
                }
            }
            else
            {
                // 結果をマージ
                foreach (var task in tasks)
                {
                    dependencyMap[task] = new List<IList<KeyValuePair<Task, DependencyTypes>>>(){
                                            dependencyList[task].ToList()
                                      };
                }
            }

            // 結果を返す
            return dependencyMap;
        }
    }
}
