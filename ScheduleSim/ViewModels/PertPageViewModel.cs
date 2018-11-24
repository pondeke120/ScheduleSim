using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScheduleSim.ViewModels
{
    public class PertPageViewModel : BindableBase
    {
        private List<PertPageEdgeItemViewModel> _edges;
        public List<PertPageEdgeItemViewModel> Edges
        {
            get { return _edges; }
            set { SetProperty(ref _edges, value); }
        }

        public ICommand DeleteEdgeCommand { get; private set; }

        public PertPageViewModel(
            ICommand deleteEdgeCommand)
        {
            this.DeleteEdgeCommand = deleteEdgeCommand;

            Edges = new List<PertPageEdgeItemViewModel>()
            {
                new PertPageEdgeItemViewModel() { INode = 1, JNode = 2, TaskId = 0 },
                new PertPageEdgeItemViewModel() { INode = 1, JNode = 3, TaskId = 1 },
            };
        }
    }
}
