﻿using AutoMapper;
using ScheduleSim.Core.Contexts;
using ScheduleSim.Entities.Models;
using ScheduleSim.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScheduleSim.Commands.PertPage
{
    public class AddPertEdgeCommand : ICommand
    {
        private AppContext appContext;
        private IMapper mapper;

        public AddPertEdgeCommand(
            AppContext appContext,
            IMapper mapper)
        {
            this.appContext = appContext;
            this.mapper = mapper;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var sender = (parameter as object[])[0];
            var args = (parameter as object[])[1] as AddingNewItemEventArgs;

            var newEdge = new Pert()
            {
            };

            this.appContext.PertEdges.Add(newEdge);
            args.NewItem = this.mapper.Map<PertPageEdgeItemViewModel>(newEdge);
        }
    }
}
