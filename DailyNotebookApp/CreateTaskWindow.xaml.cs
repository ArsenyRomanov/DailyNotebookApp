using DailyNotebookApp.Models;
using DailyNotebookApp.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            NewTask.DateRange = new DateRange();
            DataContext = NewTask;

            this.Resources.Add("daterange", NewTask.DateRange);

            Binding binding = new Binding();
            binding.Source = NewTask;
            binding.Path = new PropertyPath("Start");
            DateRangeFirstDatePicker.SetBinding(DatePicker.SelectedDateProperty, binding);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewTask.CanCreate = false;
            Close();
        }

        private void СreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!NewTask.HasErrors)
            {
                NewTask.ShortDescription = ShortDescriptionTextBox.Text;
                NewTask.IsCompleted = false;
                NewTask.CreationDate = CreationDateTextBlock.Text;
                NewTask.FinishToDate = CheckNAssignService.CheckNAssignFinishTo(FinishToDatePicker, FinishToHoursTextBox, FinishToMinutesTextBox);
                NewTask.Priority = (PriorityEnum)PriorityComboBox.SelectedItem;
                NewTask.TypeOfTask = (TypeOfTaskEnum)TypeOfTaskComboBox.SelectedItem;
                NewTask.DetailedDescription = DetailedDescriptionTextBox.Text;
                NewTask.DateRange = CheckNAssignService.CheckNAssignDateRange(DateRangeFirstDatePicker.SelectedDate, DateRangeLastDatePicker.SelectedDate);

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
