using DailyNotebookApp.Models;
using System;
using System.Windows.Controls;

namespace DailyNotebookApp.Services
{
    public class CheckNAssignService
    {
        public static string CheckNAssignFinishTo(DateTime? finishToDate, double? finishToHour, double? finishToMinutes)
        {
            if (finishToDate != null)
            {
                var result = finishToDate.Value;

                if (finishToHour != null)
                {
                    result = result.AddHours(finishToHour.Value);

                    if (finishToMinutes != null)
                    {
                        result = result.AddMinutes(finishToMinutes.Value);
                    }
                }

                return HelpService.FormatDateTimeOutput(result);
            }

            return "-";
        }

        public static DateRange CheckNAssignDateRange(DateTime? start, DateTime? end)
        {
            if (start != null && end != null)
            {
                return new DateRange(start.Value, end.Value);
            }
            return null;
        }
    }
}
