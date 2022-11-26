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

            NotebookDataGrid.SelectedCellsChanged += NotebookDataGrid_SelectedCellsChanged;
        }

        private void NotebookDataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            NotebookCalendar.SelectedDates.Clear();

            if (NotebookDataGrid.SelectedItem is Task task)
            {
                ShortDescriptionTextBlock.Text = task.ShortDescription;
                CompletedCheckBox.IsChecked = task.IsCompleted;
                CreationDateTextBlock.Text = task.CreationDate;
                FinishToTextBlock.Text = task.FinishTo;
                PriorityTextBlock.Text = task.Priority.ToString();
                TypeOfTaskTextBlock.Text = task.TypeOfTask.ToString();
                DetailedDescriptionTextBlock.Text = task.DetailedDescription;
                DateRangeTextBlock.Text = task.DateRange != null ? task.DateRange.ToString() : "-";

                if (task.DateRange != null)
                    HelpService.MarkDateRangeInCalendar(NotebookCalendar, task.DateRange, task.FinishTo);
                else if (DateTime.TryParse(task.FinishTo, out DateTime finishToDate))
                    NotebookCalendar.SelectedDate = finishToDate.Date;
            }
            else
            {
                ShortDescriptionTextBlock.Text = string.Empty;
                CompletedCheckBox.IsChecked = false;
                CreationDateTextBlock.Text = string.Empty;
                FinishToTextBlock.Text = string.Empty;
                PriorityTextBlock.Text = string.Empty;
                TypeOfTaskTextBlock.Text = string.Empty;
                DetailedDescriptionTextBlock.Text = string.Empty;
                DateRangeTextBlock.Text = string.Empty;
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
            if (newTask.CanCreate)
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
