using DailyNotebookApp.Models;
using DailyNotebookApp.Services;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interop;

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
            DateRangeFirstDatePicker.GotKeyboardFocus += ElementGotKeyboardFocus;
            DateRangeLastDatePicker.GotKeyboardFocus += ElementGotKeyboardFocus;
        }

        private void ElementGotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            HelpService.UpdateProperty(NewTask, nameof(NewTask.ShortDescription));
            HelpService.UpdateProperty(NewTask, nameof(NewTask.FinishToDate));
            HelpService.UpdateProperty(NewTask.DateRange, nameof(NewTask.DateRange.Start));
            HelpService.UpdateProperty(NewTask.DateRange, nameof(NewTask.DateRange.End));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewTask.CanCreate = false;
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
                NewTask.ShortDescription = ShortDescriptionTextBox.Text;
                NewTask.IsCompleted = false;
                NewTask.CreationDate = CreationDateTextBlock.Text;
                NewTask.FinishToDate = CheckNAssignService.CheckNAssignFinishTo(FinishToDatePicker,
                                                                                FinishToHoursTextBox,
                                                                                FinishToMinutesTextBox);
                NewTask.Priority = (PriorityEnum)PriorityComboBox.SelectedItem;
                NewTask.TypeOfTask = (TypeOfTaskEnum)TypeOfTaskComboBox.SelectedItem;
                NewTask.DetailedDescription = DetailedDescriptionTextBox.Text;
                NewTask.DateRange = CheckNAssignService.CheckNAssignDateRange(DateRangeFirstDatePicker.SelectedDate,
                                                                              DateRangeLastDatePicker.SelectedDate);

                Close();
            }
            else
                NewTask.CanCreate = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreationDateTextBlock.Text = HelpService.FormatDateTimeOutput();
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
    }
}
