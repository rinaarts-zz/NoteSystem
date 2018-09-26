using System;
using System.Collections.Generic;
using System.Text;

namespace NoteSystem
{
    class Note
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public string Content { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? Done { get; set; }

        public override bool Equals(object obj)
        {
            Note other = obj as Note;
            return other != null && other.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        
    }
}
