using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoteSystem
{
    // Base class for NotesManager -- contains all the logic except the faulty logic for removing
    // a completed task from the tasks list
    abstract class NotesManager
    {
        private int _runningId = 1;
        protected Dictionary<int, Note> _notesById = new Dictionary<int, Note>();
        protected SortedList<NoteIndex, Note> _notes = new SortedList<NoteIndex, Note>(new NoteComparer(SortDirection.Ascending));
        protected SortedList<NoteIndex, Note> _tasks = new SortedList<NoteIndex, Note>(new NoteComparer(SortDirection.Descending));

        // A list of all notes in the system, ordered by insert date descending
        public IReadOnlyList<Note> Notes
        {
            get
            {
                return _notes.Values.ToList() as IReadOnlyList<Note>;
            }
        }

        // A list of all tasks in the system (i.e. Notes with a due date), ordered by due
        // date ascending.
        public IReadOnlyList<Note> Tasks
        {
            get
            {
                return _tasks.Values.ToList() as IReadOnlyList<Note>;
            }
        }


        // create a note with the given content. If a due date is supplied 
        // this note is considered a "task".
        public Note CreateNote(string content, DateTime? due)
        {
            var note = new Note
            {
                Id = _runningId++,
                InsertDate = DateTime.Now,
                Content = content,
                DueDate = due,
                Done = false
            };

            _notesById[note.Id] = note;
            _notes.Add(new NoteIndex { Id = note.Id, Date = note.InsertDate }, note);

            if (note.DueDate.HasValue)
            {
                _tasks.Add(new NoteIndex { Id = note.Id, Date = note.DueDate.Value }, note);
            }

            return note;
        }

        // mark the note specified by noteId as "done" and remove it
        // from the task list
        public bool MarkDone(int noteId)
        {
            Note note;
            if (_notesById.TryGetValue(noteId, out note))
            {
                note.Done = true;
                return RemoveTask(noteId);
            }
            return false;
        }

        protected abstract bool RemoveTask(int noteId);

        protected class NoteIndex
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
        }

        private enum SortDirection
        {
            Ascending,
            Descending
        }

        private class NoteComparer : IComparer<NoteIndex>
        {
            private bool Ascending { get; set; }

            public NoteComparer(SortDirection direction)
            {
                Ascending = direction == SortDirection.Ascending;
            }

            public int Compare(NoteIndex x, NoteIndex y)
            {
                if (x.Id == y.Id)
                {
                    return 0;
                }

                var first = Ascending ? x : y;
                var second = Ascending ? y : x;

                var comp = Comparer<DateTime>.Default.Compare(first.Date, second.Date);
                if (comp == 0)
                {
                    comp = Comparer<int>.Default.Compare(first.Id, second.Id);
                }
                CompareLogger.WriteLine($"Comparing: x: {x.Id}, y: {y.Id}, result: {comp}");
                return comp;
            }
        }
    }

    class FaultyNotesManager : NotesManager
    {
        protected override bool RemoveTask(int noteId)
        {
            return _tasks.Remove(new NoteIndex { Id = noteId });
        }
    }

    class CorrectNotesManager : NotesManager
    {
        protected override bool RemoveTask(int noteId)
        {
            return _tasks.Remove(new NoteIndex { Id = noteId, Date = _notesById[noteId].DueDate.Value });
        }
    }
}
