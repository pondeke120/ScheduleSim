using Graphviz4Net.Graphs;
using ScheduleSim.Core.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace ScheduleSim.ViewModels
{
    public class Arrow
    {
        public string Color { get; set; }
    }

    public class PertGraphPageViewModel : INotifyPropertyChanged
    {
        private AppContext appContext;
        public ICommand RefreshCommand { get; private set; }

        public PertGraphPageViewModel(
            AppContext appContext,
            ICommand refreshCommand)
        {
            this.appContext = appContext;
            this.RefreshCommand = refreshCommand;
            
            var graph = new Graph<PertGraphPageNodeViewModel>();
            graph.Rankdir = RankDirection.LeftToRight;

            this.Graph = graph;
            this.Graph.Changed += GraphChanged;
            this.NewNodeName = "Enter new name";
            this.UpdateNodeNewName = "Enter new name";
            
            appContext.PertEdges.CollectionChanged += PertEdges_CollectionChanged;
        }

        private void PertEdges_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if ((sender as Collection<Entities.Models.Pert>).Count > 0)
            {
                RemakeGraph();
            }
        }

        public void RemakeGraph()
        {
            var graph = new Graph<PertGraphPageNodeViewModel>();
            graph.Rankdir = RankDirection.LeftToRight;
            var nodes = new Dictionary<int, PertGraphPageNodeViewModel>();

            foreach (var edge in this.appContext.PertEdges)
            {
                var src = nodes.ContainsKey(edge.SrcNodeCd) ? nodes[edge.SrcNodeCd] : new PertGraphPageNodeViewModel(graph) { Name = edge.SrcNodeCd.ToString() };
                var dst = nodes.ContainsKey(edge.DstNodeCd) ? nodes[edge.DstNodeCd] : new PertGraphPageNodeViewModel(graph) { Name = edge.DstNodeCd.ToString() };
                nodes[edge.SrcNodeCd] = src;
                nodes[edge.DstNodeCd] = dst;
                var task = edge.TaskCd.HasValue ? this.appContext.Tasks.First(x => x.TaskCd == edge.TaskCd) : null;
                var label = string.Format(@"{0}{1}",
                            task != null && string.IsNullOrEmpty(task.TaskName) == false ? $"{task.TaskName}{Environment.NewLine}" : string.Empty,
                            task != null && task.PlanValue > 0.0 ? $"{{{task.PlanValue} H}}" : string.Empty);

                graph.RemoveVertex(src);
                graph.RemoveVertex(dst);

                graph.AddVertex(src);
                graph.AddVertex(dst);

                var color = (edge.IsCritical ? "Red" : "Black");
                graph.AddEdge(
                    new Edge<PertGraphPageNodeViewModel>(
                        src,
                        dst,
                        new Arrow() { Color = color}) {
                            Label = label,
                            Attributes = {
                                { "Color", color },
                                { "Thickness", (edge.IsCritical ? "2" : "1") },
                                {"StrokeDashArray", (task != null && task.PlanValue > 0.0 ? "" : "3.0") }
                            } });
            }
            if (this.Graph != null)
            {
                this.Graph.Changed -= GraphChanged;
            }
            this.Graph = graph;
            this.Graph.Changed += GraphChanged;
            this.RaisePropertyChanged("Graph");
        }

        public Graph<PertGraphPageNodeViewModel> Graph { get; private set; }

        private LayoutEngine _layoutEngine = LayoutEngine.Dot;
        public LayoutEngine LayoutEngine
        {
            get { return _layoutEngine; }
            set
            {
                _layoutEngine = value;
                this.RaisePropertyChanged("LayoutEngine");
            }
        }

        public string NewNodeName { get; set; }

        public string UpdateNodeName { get; set; }

        public string UpdateNodeNewName { get; set; }

        public IEnumerable<string> PersonNames
        {
            get { return this.Graph.AllVertices.Select(x => x.Name); }
        }

        public string NewEdgeStart { get; set; }

        public string NewEdgeEnd { get; set; }

        public string NewEdgeLabel { get; set; }

        public void CreateEdge()
        {
            if (string.IsNullOrWhiteSpace(this.NewEdgeStart) ||
                string.IsNullOrWhiteSpace(this.NewEdgeEnd))
            {
                return;
            }

            this.Graph.AddEdge(
                new Edge<PertGraphPageNodeViewModel>
                    (this.GetNode(this.NewEdgeStart),
                    this.GetNode(this.NewEdgeEnd))
                {
                    Label = this.NewEdgeLabel
                });
        }

        public void CreatePerson()
        {
            if (this.PersonNames.Any(x => x == this.NewNodeName))
            {
                // such a person already exists: there should be some validation message, but 
                // it is not so important in a demo
                return;
            }

            var p = new PertGraphPageNodeViewModel(this.Graph) { Name = this.NewNodeName };
            this.Graph.AddVertex(p);
        }

        public void UpdateNode()
        {
            if (string.IsNullOrWhiteSpace(this.UpdateNodeName))
            {
                return;
            }

            this.GetNode(this.UpdateNodeName).Name = this.UpdateNodeNewName;
            this.RaisePropertyChanged("NodeNames");
            this.RaisePropertyChanged("Graph");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void GraphChanged(object sender, GraphChangedArgs e)
        {
            this.RaisePropertyChanged("NodeNames");
        }

        private void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private PertGraphPageNodeViewModel GetNode(string name)
        {
            return this.Graph.AllVertices.First(x => string.CompareOrdinal(x.Name, name) == 0);
        }
    }
}
