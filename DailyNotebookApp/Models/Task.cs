using DailyNotebookApp.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DailyNotebookApp.Models
{
    public class Task : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private string shortDescription;
        private bool isCompleted;
        private string creationDate;
        //private string finishTo;
        private DateTime? finishToDate;
        private double? finishToHour;
        private double? finishToMinutes;
        private PriorityEnum priority;
        private TypeOfTaskEnum typeOfTask;
        private string detailedDescription;
        private DateRange dateRange;

        private readonly Dictionary<string, List<string>> propertyErrors = new Dictionary<string, List<string>>();

        public bool CanCreate { get; set; } = false;

        public bool HasErrors
        {
            get
            {
                var subtasksHasErrors = false;
                if (Subtasks.Any(item => item.HasErrors))
                    subtasksHasErrors = true;
                if (!propertyErrors.Any() && DateRange != null && !DateRange.HasErrors && !subtasksHasErrors)
                    return false;
                else
                    return true;
            }
        }

        public string CreationDate { get; set; }

        public string FinishTo { get; set; }

        public DateTime? FinishToDate
        {
            get { return finishToDate; }
            set
            {
                finishToDate = value;
                DateRange.FinishToDate = finishToDate;
                RemoveError(nameof(FinishToDate));
                if (finishToDate < DateTime.Parse(CreationDate))
                    AddError(nameof(FinishToDate), "Finish to date cannot be earlier than creation date");
                if ((FinishToHour != null || FinishToMinutes != null) && FinishToDate == null)
                    AddError(nameof(FinishToDate), "Finish To time is specified, specify Finish To date");
                if ((DateRange.Start != null || DateRange.End != null) && finishToDate == null)
                    AddError(nameof(FinishToDate), "Date range is specified, specify Finish To date");
            }
        }

        public double? FinishToHour
        {
            get { return finishToHour; }
            set
            {
                finishToHour = value;
                RemoveError(nameof(FinishToHour));
                if (finishToHour < 0 || finishToHour > 24)
                    AddError(nameof(FinishToHour), "Invalid hour input");
                if (FinishToMinutes != null && finishToHour == null)
                    AddError(nameof(FinishToHour), "Finish To minutes is specified, specify Finish To hour");
            }
        }

        public double? FinishToMinutes
        {
            get { return finishToMinutes; }
            set
            {
                finishToMinutes = value;
                RemoveError(nameof(FinishToMinutes));
                if (finishToMinutes < 0 || finishToMinutes > 60)
                    AddError(nameof(FinishToMinutes), "Invalid minutes input");
            }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
            set
            {
                isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        public string ShortDescription
        {
            get { return shortDescription; }
            set
            {
                if (shortDescription == value) return;
                shortDescription = value;

                RemoveError(nameof(ShortDescription));

                if (string.IsNullOrWhiteSpace(shortDescription))
                {
                    AddError(nameof(ShortDescription), "Short description cannot be empty");
                }

                if (shortDescription.Length < 3 || shortDescription.Length > 50)
                {
                    AddError(nameof(ShortDescription), "Short description should be between 3 and 50 symbols");
                }
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

                RemoveError(nameof(DetailedDescription));

                if (detailedDescription != null && detailedDescription.Length > 210)
                {
                    AddError(nameof(DetailedDescription), "Detailed description cannot be of more than 210 symbols");
                }
            }
        }

        public DateRange DateRange { get; set; }

        public BindingList<Subtask> Subtasks { get; set; }

        public Task()
        {
            CreationDate = HelpService.FormatDateTimeOutput();
            DateRange = new DateRange(CreationDate, null, null);
            Subtasks = new BindingList<Subtask>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyErrors.TryGetValue(propertyName, out _))
                return propertyErrors[propertyName];
            return default;
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!propertyErrors.ContainsKey(propertyName))
            {
                propertyErrors.Add(propertyName, new List<string>());
            }

            propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        public void RemoveError(string propertyName)
        {
            if (propertyErrors.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
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
