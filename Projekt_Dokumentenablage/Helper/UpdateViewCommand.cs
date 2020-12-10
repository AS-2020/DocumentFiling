using Projekt_Dokumentenablage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projekt_Dokumentenablage.Helper
{
    public class UpdateViewCommand : ICommand
    {
        private ActualMainVM viewModel;

        public event EventHandler CanExecuteChanged;

        public UpdateViewCommand(ActualMainVM viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == "Start")
            {
                viewModel.SelectedViewModel = new MainVM();
            }
        }
    }
}
