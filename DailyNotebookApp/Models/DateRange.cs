﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DailyNotebookApp.Models
{
    public class DateRange : INotifyDataErrorInfo
    {
		private DateTime? start;
        private DateTime? end;
        public bool HasErrors => propertyErrors.Any();

        private readonly Dictionary<string, List<string>> propertyErrors = new Dictionary<string, List<string>>();

        public DateTime? Start
        { 
            get { return start; }
            set
            {
                start = value;
                RemoveError(nameof(Start));
                if (start == null && End != null)
                    AddError(nameof(Start), "End date in range specified, specify the start date");
                if (start >= End)
                    AddError(nameof(Start), "Start date in range cannot be later than the end date");
            }
        }

		public DateTime? End
        { 
            get { return end; }
            set
            {
                end = value;
                RemoveError(nameof(End));
                if (end == null && Start != null)
                    AddError(nameof(End), "Start date in range specified, specify the end date");
                if (end <= Start)
                    AddError(nameof(End), "End date in range cannot be earlier than the start date");
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public DateRange() { }

        public DateRange(DateTime start, DateTime end)
		{
			Start = start;
			End = end;
		}

		public bool Contains(DateTime dateTime)
		{
			return (start <= dateTime) && (dateTime <= end);
		}

        public override string ToString()
        {
			return Start.Value.ToShortDateString() + " - " + End.Value.ToShortDateString();
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
}
