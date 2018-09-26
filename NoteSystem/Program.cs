using System;

namespace NoteSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            RunFaultyDemo();
            RunCorrectDemo();
            Console.ReadLine();
        }

        private static void RunFaultyDemo()
        {
            Console.WriteLine("Faulty Implementation Demo:");
            Console.WriteLine("--------------------------");
            RunDemo(new FaultyNotesManager());
        }

        private static void RunCorrectDemo()
        {
            Console.WriteLine("Correct Implementation Demo:");
            Console.WriteLine("----------------------------");
            RunDemo(new CorrectNotesManager());
        }

        private static void RunDemo(NotesManager notesManager)
        {
            CompareLogger.Active = false;
            CreateDemoData(notesManager);
            PrintNotesStatus(notesManager);
            MarkTaskAsDone(notesManager, 5);
            MarkTaskAsDone(notesManager, 1);
        }

        private static void CreateDemoData(NotesManager notesManager)
        {
            DateTime tomorrow = DateTime.Now.Date.AddDays(1);
            DateTime nextWeek = DateTime.Now.Date.AddDays(7);
            notesManager.CreateNote("Task 1", new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0));
            notesManager.CreateNote("Task 2", new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0));
            notesManager.CreateNote("Task 3", new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 10, 0, 0));
            notesManager.CreateNote("Task 4", new DateTime(nextWeek.Year, nextWeek.Month, nextWeek.Day, 10, 0, 0));
            notesManager.CreateNote("Task 5", new DateTime(nextWeek.Year, nextWeek.Month, nextWeek.Day, 9, 0, 0));
        }

        private static void PrintNotesStatus(NotesManager notesManager)
        {
            Console.WriteLine("tasks:");
            foreach (var task in notesManager.Tasks)
            {
                Console.WriteLine($"{task.Id}\t{task.DueDate.Value.ToShortDateString()} {task.DueDate.Value.ToShortTimeString()}");
            }
            Console.WriteLine("notes:");
            foreach (var task in notesManager.Notes)
            {
                Console.WriteLine($"{task.Id}\t{task.DueDate.Value.ToShortDateString()} {task.DueDate.Value.ToShortTimeString()}");
            }
            Console.WriteLine();
        }

        private static void MarkTaskAsDone(NotesManager notesManager, int noteId)
        {
            Console.WriteLine($"Mark task {noteId} as done:");
            CompareLogger.Active = true;
            var success = notesManager.MarkDone(noteId);
            CompareLogger.Active = false;
            PrintNotesStatus(notesManager);
            if (!success)
            {
                Console.WriteLine($"Oops! Task {noteId} was not removed!");
                Console.WriteLine();
            }
        }
    }
}
