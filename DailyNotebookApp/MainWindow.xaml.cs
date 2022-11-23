using DailyNotebookApp.Models;
using DailyNotebookApp.Services;
using System;
using System.ComponentModel;
using System.Windows;

namespace DailyNotebookApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}\\tasks.json";
        private BindingList<Task> tasks;
        private FileIOService fileIOService;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fileIOService = new FileIOService(PATH);

            try
            {
                tasks = fileIOService.LoadData();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                Close();
            }

            NotebookDataGrid.ItemsSource = tasks;
            tasks.ListChanged += Tasks_ListChanged;

            NotebookDataGrid.GotMouseCapture += NotebookDataGrid_GotMouseCapture;
        }

        private void NotebookDataGrid_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (NotebookDataGrid.SelectedItem is Task)
            {
                NotebookCalendar.SelectedDates.Clear();

                var task = NotebookDataGrid.SelectedItem as Task;

                ShortDescriptionTextBlock.Text = task.ShortDescription;
                CompletedCheckBox.IsChecked = task.IsCompleted;
                CreationDateTextBlock.Text = task.CreationDate;

                FinishToTextBlock.Text = task.FinishToDate;
                if (DateTime.TryParse(task.FinishToDate, out DateTime finishToDate))
                {
                    NotebookCalendar.SelectedDate = finishToDate.Date;
                }

                PriorityTextBlock.Text = task.Priority.ToString();
                TypeOfTaskTextBlock.Text = task.TypeOfTask.ToString();
                DetailedDescriptionTextBlock.Text = task.DetailedDescription;

                if (task.DateRange != null)
                {
                    DateRangeTextBlock.Text = task.DateRange.ToString();
                    HelpService.MarkDateRangeInCalendar(NotebookCalendar, task.DateRange, task.FinishToDate);
                }
            }
        }

        private void Tasks_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded ||
                e.ListChangedType == ListChangedType.ItemDeleted ||
                e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    fileIOService.SaveData(tasks);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }
        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new Task();
            var createTaskWindow = new CreateTaskWindow(newTask);
            createTaskWindow.ShowDialog();
            if (!string.IsNullOrWhiteSpace(newTask.ShortDescription))
            {
                tasks.Add(newTask);
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(NotebookDataGrid.SelectedItem is Task taskToDelete))
                return;
            if (tasks.Count > 1)
                NotebookDataGrid.SelectedItem = NotebookDataGrid.Items[NotebookDataGrid.SelectedIndex - 1];
            tasks.Remove(taskToDelete);
        }
    }
}
