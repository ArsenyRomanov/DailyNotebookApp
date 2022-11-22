using System;
using System.ComponentModel;

namespace DailyNotebookApp.Models
{
    internal class Task : INotifyPropertyChanged
    {
        private bool isCompleted;
        private string shortDescription;

        public string CreationDate { get; private set; }
        private string FinishToDate { get; set; }

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                if (isCompleted == value) return;
                isCompleted = value;
                OnPropertyChanged($"Property: {nameof(IsCompleted)}");
            }
        }

        public string ShortDescription
        {
            get { return shortDescription; }
            set
            {
                if (shortDescription == value) return;
                shortDescription = value;
                OnPropertyChanged($"Property: {nameof(ShortDescription)}");
            }
        }

        public Task()
        {
            CreationDate = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
