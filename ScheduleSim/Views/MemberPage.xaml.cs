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
    /// Interaction logic for MemberPage.xaml
    /// </summary>
    public partial class MemberPage : Page
    {
        private MemberPageViewModel viewModel;

        public MemberPage(MemberPageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            this.viewModel = viewModel;

            this.dataGrid.AddingNewItem += DataGrid_AddingNewItem;
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            this.viewModel.AddMemberCommand.Execute(e);
        }

        private void JoinDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.JoinDateChangeCommand.Execute(new[] { sender, e });
        }

        private void LeaveDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewModel.LeaveDateChangeCommand.Execute(new[] { sender, e });
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.viewModel.NameChangeCommand.Execute(new[] { sender, e });
        }
    }
}
