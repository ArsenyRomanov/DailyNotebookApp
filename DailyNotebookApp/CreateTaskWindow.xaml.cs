using DailyNotebookApp.Models;
using DailyNotebookApp.Services;
using System;
using System.Windows;

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
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void СreateButton_Click(object sender, RoutedEventArgs e)
        {
            NewTask.ShortDescription = ShortDescriptionTextBox.Text;
            NewTask.IsCompleted = false;
            NewTask.CreationDate = CreationDateTextBlock.Text;
            NewTask.FinishToDate = CheckNAssignService.CheckNAssignFinishTo(FinishToDatePicker, FinishToHoursTextBox, FinishToMinutesTextBox);
            NewTask.Priority = (PriorityEnum)PriorityComboBox.SelectedItem;
            NewTask.TypeOfTask = (TypeOfTaskEnum)TypeOfTaskComboBox.SelectedItem;
            NewTask.DetailedDescription = DetailedDescriptionTextBox.Text;
            NewTask.DateRange = new DateRange(DateRangeFirstDateDatePicker.SelectedDate.Value, DateRangeLastDateDatePicker.SelectedDate.Value);

            Close();
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
