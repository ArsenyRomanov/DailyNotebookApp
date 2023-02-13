﻿using DailyNotebookApp.Models;
using DailyNotebookApp.Services;
using System;
using System.ComponentModel;
using System.Linq;
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

            try { tasks = DataBaseIOService.LoadData(); }
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
            MainGrid.MouseDown += MainGrid_MouseDown;
        }

        private void MainGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (NotebookDataGrid.SelectedItem != null)
            {
                NotebookCalendar.SelectedDates.Clear();
                NotebookDataGrid.SelectedItem = null;
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
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                try { DataBaseIOService.UpdateAll(tasks); }
                catch (Exception exception) { MessageBox.Show(exception.Message); }
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

                try { DataBaseIOService.AddTask(newTask); }
                catch (Exception exception) { MessageBox.Show(exception.Message); }

                foreach (var task in tasks)
                    task.Subtasks.ListChanged += Tasks_ListChanged;
            }
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(NotebookDataGrid.SelectedItem is Task taskToEdit))
                return;

            var editTaskWindow = new EditTaskWindow(taskToEdit);
            editTaskWindow.ShowDialog();

            if (!editTaskWindow.EditedTask.CanCreate)
                return;

            if (taskToEdit.Coequals(editTaskWindow.EditedTask))
                return;

            var index = NotebookDataGrid.SelectedIndex;

            taskToEdit.Assign(editTaskWindow.EditedTask);

            try { DataBaseIOService.UpdateTask(taskToEdit); }
            catch (Exception exception) { MessageBox.Show(exception.Message); }

            NotebookDataGrid.ItemsSource = null;
            NotebookDataGrid.ItemsSource = tasks;
            NotebookDataGrid.SelectedIndex = index;
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(NotebookDataGrid.SelectedItem is Task taskToDelete))
                return;
            if (tasks.Count > 1)
            {
                if (NotebookDataGrid.SelectedIndex != 0)
                    NotebookDataGrid.SelectedItem = NotebookDataGrid.Items[NotebookDataGrid.SelectedIndex - 1];
                else
                    NotebookDataGrid.SelectedItem = NotebookDataGrid.Items[NotebookDataGrid.SelectedIndex + 1];
            }
            tasks.Remove(taskToDelete);

            try { DataBaseIOService.RemoveTask(taskToDelete); }
            catch (Exception exception) { MessageBox.Show(exception.Message); }
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
