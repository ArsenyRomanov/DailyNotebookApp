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

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreationDateTextBlock.Text = HelpService.FormatDateTimeOutput();
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
