using DailyNotebookApp.Models;
using System;
using System.Windows.Controls;

namespace DailyNotebookApp.Services
{
    public class HelpService
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
            calendar.SelectedDates.AddRange(dateRange.Start, dateRange.End);
            DateTime.TryParse(finishToDate, out DateTime finishToDateTime);

            if (!dateRange.Contains(finishToDateTime))
            {
                calendar.SelectedDates.Add(finishToDateTime.Date);
            }
        }
    }
}
