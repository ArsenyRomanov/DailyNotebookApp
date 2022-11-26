using DailyNotebookApp.Models;
using DailyNotebookApp.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DailyNotebookApp
{
    /// <summary>
    /// Логика взаимодействия для CreateTaskWindow.xaml
    /// </summary>
    public partial class CreateTaskWindow : Window
    {
        private Task NewTask { get; set; }

        public CreateTaskWindow(Task newTask)
        {
            InitializeComponent();
            NewTask = newTask;
            DataContext = NewTask;
            DateRangeGrid.DataContext = NewTask.DateRange;

            ShortDescriptionTextBox.GotKeyboardFocus += ElementGotKeyboardFocus;
            FinishToDatePicker.GotKeyboardFocus += ElementGotKeyboardFocus;
            FinishToHoursTextBox.GotKeyboardFocus += ElementGotKeyboardFocus;
            FinishToMinutesTextBox.GotKeyboardFocus += ElementGotKeyboardFocus;
            PriorityComboBox.GotKeyboardFocus += ElementGotKeyboardFocus;
            TypeOfTaskComboBox.GotKeyboardFocus += ElementGotKeyboardFocus;
            DetailedDescriptionTextBox.GotKeyboardFocus += ElementGotKeyboardFocus;
            DateRangeFirstDatePicker.GotKeyboardFocus += ElementGotKeyboardFocus;
            DateRangeLastDatePicker.GotKeyboardFocus += ElementGotKeyboardFocus;
        }

        private void ElementGotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            HelpService.UpdateProperty(NewTask, nameof(NewTask.ShortDescription));
            HelpService.UpdateProperty(NewTask, nameof(NewTask.FinishToDate));
            HelpService.UpdateProperty(NewTask, nameof(NewTask.FinishToHour));
            HelpService.UpdateProperty(NewTask, nameof(NewTask.FinishToMinutes));
            HelpService.UpdateProperty(NewTask.DateRange, nameof(NewTask.DateRange.Start));
            HelpService.UpdateProperty(NewTask.DateRange, nameof(NewTask.DateRange.End));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void СreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ShortDescriptionTextBox.Text) && !NewTask.HasErrors)
            {
                HelpService.UpdateProperty(NewTask, nameof(NewTask.ShortDescription));
            }

            if (!NewTask.HasErrors)
            {
                NewTask.CanCreate = true;
                NewTask.IsCompleted = false;
                NewTask.FinishTo = CheckNAssignService.CheckNAssignFinishTo(NewTask.FinishToDate,
                                                                            NewTask.FinishToHour,
                                                                            NewTask.FinishToMinutes);
                NewTask.Priority = (PriorityEnum)PriorityComboBox.SelectedItem;
                NewTask.TypeOfTask = (TypeOfTaskEnum)TypeOfTaskComboBox.SelectedItem;
                NewTask.DateRange = CheckNAssignService.CheckNAssignDateRange(DateRangeFirstDatePicker.SelectedDate,
                                                                              DateRangeLastDatePicker.SelectedDate);

                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreationDateTextBlock.Text = NewTask.CreationDate;
            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(PriorityEnum));
            PriorityComboBox.SelectedItem = PriorityEnum.None;
            TypeOfTaskComboBox.ItemsSource = Enum.GetValues(typeof(TypeOfTaskEnum));
            TypeOfTaskComboBox.SelectedItem = TypeOfTaskEnum.None;
        }

        private void AdditionalInfoExpander_Expanded(object sender, RoutedEventArgs e)
        {
            Width += 422;
        }

        private void AdditionalInfoExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            Width -= 422;
        }

        private void FinishToHoursTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse((sender as TextBox).Text, out _))
                (sender as TextBox).Text = null;

            if ((sender as TextBox).Text.Length == 2)
            {
                FinishToMinutesTextBox.Focus();
                FinishToMinutesTextBox.Select(0, 0);
            }
        }

        private void FinishToMinutesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!double.TryParse((sender as TextBox).Text, out _))
                (sender as TextBox).Text = null;

            if ((sender as TextBox).Text.Length == 2)
            {
                Keyboard.ClearFocus();
            }
        }
    }
}
