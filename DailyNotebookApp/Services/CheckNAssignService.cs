﻿using System;
using System.Windows.Controls;

namespace DailyNotebookApp.Services
{
    public class CheckNAssignService
    {
        public static string CheckNAssignFinishTo(DatePicker finishToDatePicker, TextBox finishToHoursTextBox, TextBox finishToMinutesTextBox)
        {
            var result = new DateTime();

            if (finishToDatePicker.SelectedDate != null)
            {
                result = finishToDatePicker.SelectedDate.Value;

                if (!string.IsNullOrWhiteSpace(finishToHoursTextBox.Text))
                {
                    result = result.AddHours(double.Parse(finishToHoursTextBox.Text));

                    if (!string.IsNullOrWhiteSpace(finishToMinutesTextBox.Text))
                    {
                        result = result.AddMinutes(double.Parse(finishToMinutesTextBox.Text));
                    }
                }

                return HelpService.FormatDateTimeOutput(result);
            }

            return "-";
        }
    }
}