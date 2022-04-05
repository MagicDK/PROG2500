using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows_Programming_Assignment_3.Models;

namespace Windows_Programming_Assignment_3.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NoteModel> Notes { get; set; }
        public List<NoteModel> FilteredNotes = new List<NoteModel>();
        private NoteModel _selectedNote { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public Commands.AboutCommand AboutCommand;
        public Commands.AddCommand AddCommand;
        public Commands.SaveCommand SaveCommand;
        public Commands.EditCommand EditCommand;
        public Commands.DeleteCommand DeleteCommand;
        public Commands.ExitCommand ExitCommand;
        public StorageFolder SaveDirectory;
        private bool _readOnly { get; set; }
        private string _filter;
        public string SelectedNoteDescription { get; set; }
        public string SelectedNoteTitle { get; set; }
        public bool ReadOnly
        {
            get { return _readOnly; }

            set
            {
                _readOnly = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReadOnly"));
            }
        }
        public NoteModel SelectedNote
        {
            get { return _selectedNote; }
            set
            {
                _selectedNote = value;
                ReadOnly = true;
                if (value == null)
                {
                    SelectedNoteTitle = "Untitled Note";
                    SelectedNoteDescription = "";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNote"));
                }
                else
                {
                    ReadOnly = true;
                    SelectedNoteDescription = _selectedNote.Note;
                    SelectedNoteTitle = _selectedNote.Title;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNoteDescription"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNoteTitle"));
                // Commands
                SaveCommand.trigger_CanExecuteChanged();
                EditCommand.trigger_CanExecuteChanged();
                DeleteCommand.trigger_CanExecuteChanged();
            }
        }

        public NoteViewModel()
        {
            AboutCommand = new Commands.AboutCommand();
            AddCommand = new Commands.AddCommand(this);
            SaveCommand = new Commands.SaveCommand(this);
            EditCommand = new Commands.EditCommand(this);
            DeleteCommand = new Commands.DeleteCommand(this);
            ExitCommand = new Commands.ExitCommand(this);

            Notes = new ObservableCollection<NoteModel>();
            AddExistingNotes(ApplicationData.Current.LocalFolder);

            ReadOnly = true;
            PerformFiltering();
        }
        public async void AddExistingNotes(StorageFolder noteFolder)
        {
            bool folderFound = false;
            try
            {
                foreach (StorageFolder folder in await noteFolder.GetFoldersAsync())
                {
                    if (folder.Name == "Notes")
                    {
                        folderFound = true;
                        SaveDirectory = folder;
                        foreach (var file in await folder.GetFilesAsync())
                        {
                            NoteModel note = new NoteModel(file.Name.Split(".")[0], await FileIO.ReadTextAsync(file));
                            Notes.Add(note);
                            FilteredNotes.Add(note);
                        }
                        break;
                    }
                }
                if (folderFound == false)
                {
                    Debug.WriteLine("Note folder not found");
                    SaveDirectory = await noteFolder.CreateFolderAsync("Notes", CreationCollisionOption.FailIfExists);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An IO error has occurred");
            }
        }

        public async void Save()
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNoteDescription"));
                SelectedNote.Note = SelectedNoteDescription;
                foreach (StorageFile file in await SaveDirectory.GetFilesAsync())
                {
                    if (file.Name == SelectedNote.Title + ".txt")
                    {
                        await FileIO.WriteTextAsync(file, SelectedNoteDescription);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error has occured whilst trying to save edit.");
            }

        }

        public async void Delete(NoteModel deletedNote)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNoteTitle"));
                foreach (StorageFile file in await SaveDirectory.GetFilesAsync())
                {
                    Debug.WriteLine(file.Path);
                    if (file.Name == deletedNote.Title + ".txt")
                    {
                        await file.DeleteAsync();
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Unable to delete note file. Try checking file extension, and ensuring it is a txt file.");
            }
        }

        public async void NewNote()
        {
            NewNoteDialog nnd = new NewNoteDialog();
            ContentDialogResult result = await nnd.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                foreach (NoteModel note in Notes)
                {
                    if (note.Title == nnd.NewNoteTitle)
                    {
                        ContentDialog invalidDialog = new ContentDialog()
                        {
                            Title = "Invalid name provided: note name cannot match another.",
                            Content = "Please try again.",
                            PrimaryButtonText = "OK"
                        };
                        await invalidDialog.ShowAsync();
                        return;
                    }
                }
                //Save notes here
                String filename = nnd.NewNoteTitle + ".txt";
                try
                {
                    StorageFile NoteFile = await SaveDirectory.CreateFileAsync(filename,
                        CreationCollisionOption.ReplaceExisting);

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNoteDescription"));

                    await FileIO.AppendTextAsync(NoteFile, SelectedNoteDescription);

                    NoteModel NewNote = new NoteModel(nnd.NewNoteTitle, SelectedNoteDescription);
                    Notes.Add(NewNote);
                    FilteredNotes.Add(NewNote);
                    SelectedNote = NewNote;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedNote"));

                    ReadOnly = true;
                    SaveCommand.trigger_CanExecuteChanged();
                    EditCommand.trigger_CanExecuteChanged();
                    Debug.WriteLine("New note created.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("An error has occurred when trying to save to file, make sure characters provided are valid Windows characters.");
                }
            }
        }

        public string Filter
        {
            // *FILTER FUNCTIONALITY PROVIDED BY: GEOFF GILLESPIE*
            get { return _filter; }
            set
            {
                if (value == _filter) { return; }
                _filter = value;
                PerformFiltering();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
            }
        }

        private void PerformFiltering()
        {
            // *FILTER FUNCTIONALITY PROVIDED BY: GEOFF GILLESPIE*
            if (_filter == null)
            {
                _filter = "";
            }
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();
            var result =
                FilteredNotes.Where(n => n.Title.ToLowerInvariant()
                .Contains(lowerCaseFilter) && n.IsDeleted == false)
                .ToList();
            var toRemove = Notes.Except(result).ToList();
            foreach (var x in toRemove)
            {
                Notes.Remove(x);
            }
            var resultCount = result.Count;
            for (int i = 0; i < resultCount; i++)
            {
                var resultItem = result[i];
                if (i + 1 > Notes.Count || !Notes[i].Equals(resultItem))
                {
                    Notes.Insert(i, resultItem);
                }
            }
        }
    }
}
