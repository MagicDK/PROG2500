
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows_Programming_Assignment_3;
using Windows_Programming_Assignment_3.ViewModels;
using Windows_Programming_Assignment_3.Models;
using Windows_Programming_Assignment_3.Commands;

namespace UnitTesting
{
    [TestClass]
    public class UnitTestCommandFunctionality
    {
        // Instantiate a new viewModel
        NoteViewModel viewModel = new NoteViewModel();

        //--------------------Command test #1: Add Command--------------------

        // Test the add functionailty.
        // Add command should:
        // 1. deselect any currently selected note
        // 2. clear note description
        // 3. set the selected note's title to "Untitled note"
        // 4. disable read only permissions / enable editing mode

        [TestMethod, TestCategory("AddCommand")]
        public void Test_AddCommand1()
        {
            viewModel.AddCommand.Execute(null);
            object expectedResult = null;
            object actualResult = viewModel.SelectedNote;
            Assert.AreEqual(expectedResult , actualResult);
        }

        [TestMethod, TestCategory("AddCommand")]
        public void Test_AddCommand2()
        {
            viewModel.AddCommand.Execute(null);
            var expectedResult = "";
            var actualResult = viewModel.SelectedNoteDescription;
            Assert.AreEqual(expectedResult , actualResult);
        }

        [TestMethod, TestCategory("AddCommand")]
        public void Test_AddCommand3()
        {
            viewModel.AddCommand.Execute(null);
            var expectedResult = "Untitled note";
            var actualResult = viewModel.SelectedNoteTitle;
            Assert.AreEqual(expectedResult , actualResult);
        }

        [TestMethod, TestCategory("AddCommand")]
        public void Test_AddCommand4()
        {
            viewModel.AddCommand.Execute(null);
            var expectedResult = false;
            var actualResult = viewModel.ReadOnly;
            Assert.AreEqual(expectedResult , actualResult);
        }

        //--------------------Command test #2: Delete Command--------------------

        // Test the delete functionailty.
        // Delete command should:
        // 1. set the note's IsDeleted property to true
        // 2. remove note from Notes collection
        // 3. clear note's title
        // 4. clear note's description

        [TestMethod, TestCategory("DeleteCommand")]
        public void Test_DeleteCommand1()
        {
            viewModel.SelectedNote = new NoteModel("Test title", "Test description");
            viewModel.DeleteCommand.Execute(null);
            var expectedResult = true;
            var actualResult = viewModel.SelectedNote.IsDeleted;
            Assert.AreEqual(expectedResult , actualResult);
        }

        [TestMethod, TestCategory("DeleteCommand")]
        public void Test_DeleteCommand2()
        {
            viewModel.SelectedNote = new NoteModel("Test title", "Test description");
            viewModel.Notes.Add(viewModel.SelectedNote);
            viewModel.DeleteCommand.Execute(null);
            var expectedResult = 0;
            var actualResult = viewModel.Notes.Count;
            Assert.AreEqual(expectedResult , actualResult);
        }

        [TestMethod, TestCategory("DeleteCommand")]
        public void Test_DeleteCommand3()
        {
            viewModel.DeleteCommand.Execute(null);
            var expectedResult = "";
            var actualResult = viewModel.SelectedNoteTitle;
            Assert.AreEqual(expectedResult , actualResult);
        }

        [TestMethod, TestCategory("DeleteCommand")]
        public void Test_DeleteCommand4()
        {
            viewModel.DeleteCommand.Execute(null);
            var expectedResult = "";
            var actualResult = viewModel.SelectedNoteDescription;
            Assert.AreEqual(expectedResult , actualResult);
        }

        //--------------------Command test #3: Edit Command--------------------

        // Test the edit functionailty.
        // Edit command should:
        // 1. disable read only permissions

        [TestMethod, TestCategory("EditCommand")]
        public void Test_EditCommand1()
        {
            viewModel.EditCommand.Execute(null);
            object expectedResult = false;
            object actualResult = viewModel.ReadOnly;
            Assert.AreEqual(expectedResult , actualResult);
        }

        //--------------------Command test #4: Save Command--------------------

        // Test the save functionailty.
        // Save command should:
        // 1. enable read only permissions if editing a pre-existing note
        // 2. create a new instance of a note if writing to a newly created note

        [TestMethod, TestCategory("SaveCommand")]
        public void Test_SaveCommand1()
        {
            viewModel.SaveCommand.Execute(null);
            object expectedResult = true;
            object actualResult = viewModel.ReadOnly;
            Assert.AreEqual(expectedResult , actualResult);
        }

        [TestMethod, TestCategory("SaveCommand")]
        public void Test_SaveCommand2()
        {
            viewModel.SelectedNote = new NoteModel("Test title", "Test description");
            viewModel.SaveCommand.Execute(null);
            object expectedResult = 1;
            object actualResult = viewModel.Notes.Count;
            Assert.AreEqual(expectedResult , actualResult);
        }
    }
}
