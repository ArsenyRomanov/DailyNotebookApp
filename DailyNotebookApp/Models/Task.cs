using DailyNotebookApp.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace DailyNotebookApp.Models
{
    public class Task : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private string shortDescription;
        private bool isCompleted;
        private string creationDate;
        private string finishToDate;
        private PriorityEnum priority;
        private TypeOfTaskEnum typeOfTask;
        private string detailedDescription;
        private DateRange dateRange;

        public bool CanCreate { get; set; } = true;

        public bool HasErrors
        {
            get
            {
                if (!propertyErrors.Any() && DateRange != null && !DateRange.HasErrors)
                    return false;
                return true;
            }
        }

        private readonly Dictionary<string, List<string>> propertyErrors = new Dictionary<string, List<string>>();

        public string CreationDate { get; set; }

        public string FinishToDate
        {
            get { return finishToDate; }
            set
            {
                finishToDate = value;
                RemoveError(nameof(FinishToDate));
                if (string.IsNullOrWhiteSpace(finishToDate) && DateRange != null && DateRange.End != null)
                    AddError(nameof(FinishToDate), "End date in range specified, specify the Finish To Date");
            }
        }

        public bool IsCompleted { get; set; }

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

        public Task()
        {
            CreationDate = HelpService.FormatDateTimeOutput();
            DateRange = new DateRange();
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
