using Graphviz4Net.Graphs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class PertGraphPageNodeViewModel : INotifyPropertyChanged
    {
        private readonly Graph<PertGraphPageNodeViewModel> graph;

        public PertGraphPageNodeViewModel(Graph<PertGraphPageNodeViewModel> graph)
        {
            this.graph = graph;
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }

        public ICommand RemoveCommand
        {
            get { return new RemoveCommandImpl(this); }
        }

        private class RemoveCommandImpl : ICommand
        {
            private PertGraphPageNodeViewModel person;

            public RemoveCommandImpl(PertGraphPageNodeViewModel person)
            {
                this.person = person;
            }

            public void Execute(object parameter)
            {
                this.person.graph.RemoveVertexWithEdges(this.person);
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
