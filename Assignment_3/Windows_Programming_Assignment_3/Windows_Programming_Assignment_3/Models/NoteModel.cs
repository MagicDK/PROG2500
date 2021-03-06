using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_Programming_Assignment_3.Models
{
    public class NoteModel
    {
        public string Title { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }

        public NoteModel(string title, string note)
        {
            this.Title = title;
            this.Note = note;
            this.IsDeleted = false;
        }
    }
}
