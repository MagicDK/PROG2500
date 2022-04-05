using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Windows_Programming_Assignment_3.Commands
{
    public class SaveCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.NoteViewModel noteViewModel;
        public SaveCommand(ViewModels.NoteViewModel viewModel)
        {
            this.noteViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return (noteViewModel.SelectedNote != null || noteViewModel.SelectedNoteTitle != null) && noteViewModel.ReadOnly == false;
        }

        public void Execute(object parameter)
        {
            Debug.WriteLine("Save button clicked");
            if (noteViewModel.SelectedNote != null)
            {
                noteViewModel.ReadOnly = true;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                noteViewModel.EditCommand.trigger_CanExecuteChanged();
                noteViewModel.Save();
            }
            else
            {
                noteViewModel.NewNote();
            }

        }

        public void trigger_CanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
