using DailyNotebookApp.Models;
using DailyNotebookApp.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

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
            
            foreach (var task in tasks)
                task.Subtasks.ListChanged += Tasks_ListChanged;

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

                if (task.Subtasks.Count != 0)
                    SubtasksDataGrid.ItemsSource = task.Subtasks;
                else
                    SubtasksDataGrid.ItemsSource = null;

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
                SubtasksDataGrid.ItemsSource = null;
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

                foreach (var task in tasks)
                    task.Subtasks.ListChanged += Tasks_ListChanged;
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

        private void FiltersShortDescriptionTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            NotebookDataGrid.ItemsSource = HelpService.FilterCollection(tasks, FiltersShortDescription.Text,
                                                                               FiltersFinishTo.SelectedDate,
                                                                               FiltersCreationDate.SelectedDate);
        }

        private void FiltersFinishTo_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            NotebookDataGrid.ItemsSource = HelpService.FilterCollection(tasks, FiltersShortDescription.Text,
                                                                               FiltersFinishTo.SelectedDate,
                                                                               FiltersCreationDate.SelectedDate);
        }

        private void FiltersCreationDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            NotebookDataGrid.ItemsSource = HelpService.FilterCollection(tasks, FiltersShortDescription.Text,
                                                                               FiltersFinishTo.SelectedDate,
                                                                               FiltersCreationDate.SelectedDate);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Height >= (MinHeight + FiltersCreationDate.Height) || WindowState == WindowState.Maximized)
            {
                FiltersCreationDate.Visibility = Visibility.Visible;
            }
            else
            {
                FiltersCreationDate.Visibility = Visibility.Hidden;
            }
        }
    }
}
