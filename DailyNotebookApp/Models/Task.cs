using DailyNotebookApp.Services;
using System;
using System.ComponentModel;

namespace DailyNotebookApp.Models
{
    public class Task : INotifyPropertyChanged
    {
        private string shortDescription;
        private bool isCompleted;
        private string creationDate;
        private string finishToDate;
        private PriorityEnum priority;
        private TypeOfTaskEnum typeOfTask;
        private string detailedDescription;
        private DateRange dateRange;

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

        public PriorityEnum Priority
        {
            get { return priority; }
            set 
            {
                if (Enum.IsDefined(typeof(PriorityEnum), value))
                {
                    priority = value;
                }
            }
        }

        public TypeOfTaskEnum TypeOfTask
        {
            get { return typeOfTask; }
            set
            {
                if (Enum.IsDefined(typeof(TypeOfTaskEnum), value))
                {
                    typeOfTask = value;
                }
            }
        }

        public string DetailedDescription
        {
            get { return detailedDescription; }
            set 
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    detailedDescription = value;
                }
            }
        }

        public DateRange DateRange
        {
            get { return dateRange; }
            set { dateRange = value; }
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

    public enum PriorityEnum
    {
        None,
        Lowest,
        Low,
        Medium,
        High,
        Highest
    }

    public enum TypeOfTaskEnum
    {
        None,
        Home,
        Business,
        Study,
        Hobby,
        Entertainment,
        Meeting
    }
}
