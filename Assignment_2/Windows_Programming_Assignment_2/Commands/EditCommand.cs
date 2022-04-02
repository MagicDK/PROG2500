using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Windows_Programming_Assignment_2.Commands
{
    public class EditCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.NoteViewModel noteViewModel;
        public EditCommand(ViewModels.NoteViewModel viewModel)
        {
            this.noteViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return noteViewModel.SelectedNote != null && noteViewModel.ReadOnly == true;
        }

        public void Execute(object parameter)
        {
            Debug.WriteLine("Edit button clicked");
            noteViewModel.ReadOnly = false;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            noteViewModel.SaveCommand.trigger_CanExecuteChanged();
        }

        public void trigger_CanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
