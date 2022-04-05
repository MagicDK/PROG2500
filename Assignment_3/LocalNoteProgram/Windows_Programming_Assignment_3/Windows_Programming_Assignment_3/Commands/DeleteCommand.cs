using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Windows_Programming_Assignment_3.Commands
{
    public class DeleteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ViewModels.NoteViewModel noteViewModel;

        public DeleteCommand(ViewModels.NoteViewModel viewModel)
        {
            this.noteViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return noteViewModel.SelectedNote != null;
        }

        public async void Execute(object parameter)
        {
            DeleteNoteDialog dnd = new DeleteNoteDialog();
            ContentDialogResult result = await dnd.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var deletedNote = noteViewModel.SelectedNote;
                deletedNote.IsDeleted = true;
                noteViewModel.Notes.Remove(deletedNote);
                noteViewModel.SelectedNoteTitle = "";
                noteViewModel.Delete(deletedNote);
            }
        }

        public void trigger_CanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


    }
}
