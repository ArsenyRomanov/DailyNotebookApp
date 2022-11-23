using DailyNotebookApp.Services;
using System;
using System.ComponentModel;

namespace DailyNotebookApp.Models
{
    public class Task : INotifyPropertyChanged
    {
        private bool isCompleted;
        private string shortDescription;
        private string creationDate;
        private string finishToDate;

        public string CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = HelpService.FormatDateTimeOutput(value);
            }
        }

        public string FinishToDate
        {
            get { return finishToDate; }
            set
            {
                finishToDate = HelpService.FormatDateTimeOutput(value);
            }
        }

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
            CreationDate = HelpService.FormatDateTimeOutput();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
