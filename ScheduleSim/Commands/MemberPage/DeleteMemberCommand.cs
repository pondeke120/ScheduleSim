﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ScheduleSim.Commands.MemberPage
{
    public class DeleteMemberCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return
                true;
        }

        public void Execute(object parameter)
        {
            MessageBox.Show("aaaa");
        }
    }
}
