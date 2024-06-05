using System;
using Notifications.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Windows;

namespace Notifications.Notifies
{
    public class NotesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Note> notes;
        private Note selectedNote;
        private string newNoteContent;
        private string newNoteTitle;
		private string notesFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "notes.json");

		public ObservableCollection<Note> Notes
        {
            get { return notes; }
            set
            {
                notes = value;
                OnPropertyChanged();
            }
        }

        public Note SelectedNote
        {
            get { return selectedNote; }
            set
            {
                selectedNote = value;
                OnPropertyChanged();
            }
        }

        public string NewNoteContent
        {
            get { return newNoteContent; }
            set
            {
                newNoteContent = value;
                OnPropertyChanged();
            }
        }

        public string NewNoteTitle
        {
            get { return newNoteTitle; }
            set
            {
                newNoteTitle = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddNoteCommand { get; }
        public ICommand DeleteNoteCommand { get; }

        public NotesViewModel()
        {
            LoadNotes();
            AddNoteCommand = new RelayCommand(AddNote);
            DeleteNoteCommand = new RelayCommand(DeleteNote, CanDeleteNote);
        }

		private void AddNote()
		{
			Console.WriteLine($"NewNoteTitle: {NewNoteTitle}");
			Console.WriteLine($"NewNoteContent: {NewNoteContent}");
			DateTime CreatedAt = DateTime.Now;
			string formattedTime = CreatedAt.ToString("dd-MM-yyyy HH:mm");
			var newNote = new Note
			{
				Title = NewNoteTitle,
				Content = NewNoteContent,
				CreatedAt = formattedTime
			};



			Notes.Add(newNote);
			SaveNotes();
		}

		private void DeleteNote()
        {
            if (SelectedNote != null)
            {
                Notes.Remove(SelectedNote);
                SaveNotes();
            }
        }

        private bool CanDeleteNote()
        {
            return SelectedNote != null;
        }

		private void LoadNotes()
		{
			if (!File.Exists(notesFilePath))
			{
				Notes = new ObservableCollection<Note>();
				return;
			}

			string json = File.ReadAllText(notesFilePath);
			Notes = JsonConvert.DeserializeObject<ObservableCollection<Note>>(json);
		}

		private void SaveNotes()
		{
			string directory = Path.GetDirectoryName(notesFilePath);
            bool has_exists = Directory.Exists(directory);
            bool has_file_exists = File.Exists(notesFilePath);
			Console.WriteLine($"If dir exists: {has_exists}");
			Console.WriteLine($"If file exists: {has_file_exists}");
			if (!has_exists)
			{
				Directory.CreateDirectory(directory);
			}
            else
            {
				Console.WriteLine($"Dir path: {directory}");
			}
			if (!has_file_exists)
			{
				File.Create(notesFilePath).Close(); // Создание файла, если его нет
			}

			string json = JsonConvert.SerializeObject(Notes);
			File.WriteAllText(notesFilePath, json);

			using (StreamWriter file = File.CreateText(notesFilePath))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, Notes);
				file.Flush();
			}
            

			NewNoteTitle = string.Empty;
			NewNoteContent = string.Empty;
		}

		public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
