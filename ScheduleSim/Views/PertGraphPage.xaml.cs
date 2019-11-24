using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScheduleSim.Views
{
    /// <summary>
    /// Interaction logic for PertGraphPage.xaml
    /// </summary>
    public partial class PertGraphPage : Page
    {
        private PertGraphPageViewModel viewModel;

        public PertGraphPage(PertGraphPageViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            //this.AddNewEdge.Click += AddNewEdgeClick;
            //this.AddNewPerson.Click += AddNewPersonClick;
            //this.UpdatePerson.Click += UpdatePersonClick;
        }

        //void UpdatePersonClick(object sender, RoutedEventArgs e)
        //{
        //    this.viewModel.UpdatePersonName = (string)this.UpdatePersonName.SelectedItem;
        //    this.viewModel.UpdatePerson();
        //}

        //private void AddNewPersonClick(object sender, RoutedEventArgs e)
        //{
        //    this.viewModel.CreatePerson();
        //}

        //private void AddNewEdgeClick(object sender, RoutedEventArgs e)
        //{
        //    this.viewModel.NewEdgeStart = (string)this.NewEdgeStart.SelectedItem;
        //    this.viewModel.NewEdgeEnd = (string)this.NewEdgeEnd.SelectedItem;
        //    this.viewModel.CreateEdge();
        //}
    }
}
