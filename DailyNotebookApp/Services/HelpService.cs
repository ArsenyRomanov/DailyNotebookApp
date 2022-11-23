using System;

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
    }
}
