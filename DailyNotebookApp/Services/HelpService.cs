using DailyNotebookApp.Models;
using System;
using System.Windows.Controls;

namespace DailyNotebookApp.Services
{
    public static class HelpService
    {
        public static string FormatDateTimeOutput()
        {
            return DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

        public static string FormatDateTimeOutput(DateTime dateTime)
        {
            return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
        }

        public static string FormatDateTimeOutput(string dateTimeString)
        {
            if (DateTime.TryParse(dateTimeString, out DateTime dateTime))
            {
                return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            }
            return "-";
        }

        public static void MarkDateRangeInCalendar(Calendar calendar, DateRange dateRange, string finishToDate)
        {
            calendar.SelectedDates.AddRange(dateRange.Start.Value, dateRange.End.Value);
            DateTime.TryParse(finishToDate, out DateTime finishToDateTime);

            if (!dateRange.Contains(finishToDateTime))
            {
                calendar.SelectedDates.Add(finishToDateTime.Date);
            }
        }

        public static void UpdateProperty(Task task, string propertyName)
        {
            var taskType = typeof(Task);
            var propertyInfo = taskType.GetProperty(propertyName);
            var prevValue = propertyInfo.GetValue(task);

            if (prevValue != null)
                propertyInfo.SetValue(task, prevValue);
            else
                propertyInfo.SetValue(task, string.Empty);
        }

        public static void UpdateProperty(DateRange dateRange, string propertyName)
        {
            var dateRangeType = typeof(DateRange);
            var propertyInfo = dateRangeType.GetProperty(propertyName);
            var prevValue = propertyInfo.GetValue(dateRange);
            propertyInfo.SetValue(dateRange, prevValue);
        }
    }
}
