using DailyNotebookApp.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DailyNotebookApp.Services
{
    public class SubtasksControlService
    {
        public static void AddSubtaskControls(Grid subtasksGrid, EventHandler<SelectionChangedEventArgs> selectedDateChanged, BindingList<Subtask> subtasks, DateRange dateRange)
        {
            subtasksGrid.RowDefinitions.Add(new RowDefinition());
            int rowCount = subtasksGrid.RowDefinitions.Count;

            var newSubtask = new Subtask(dateRange)
            {
                OrdinalNumber = rowCount
            };
            subtasks.Add(newSubtask);

            var newControls = GetControls(rowCount);

            var newOrdinalNumberTextBlock = newControls.OrdinalNumberTextBlock;
            var newDateDatePicker = newControls.DateDatePicker;
            var newDescriptionTextBox = newControls.DescriptionTextBox;

            newDescriptionTextBox.SetBinding(TextBox.TextProperty, AddBinding(subtasks[rowCount - 1], "Description"));
            newDateDatePicker.SetBinding(DatePicker.SelectedDateProperty, AddBinding(subtasks[rowCount - 1], "Date"));

            newDateDatePicker.SelectedDateChanged += selectedDateChanged;

            Grid.SetRow(newOrdinalNumberTextBlock, rowCount - 1);
            Grid.SetRow(newDateDatePicker, rowCount - 1);
            Grid.SetRow(newDescriptionTextBox, rowCount - 1);
            subtasksGrid.Children.Add(newOrdinalNumberTextBlock);
            subtasksGrid.Children.Add(newDateDatePicker);
            subtasksGrid.Children.Add(newDescriptionTextBox);
        }

        private static Binding AddBinding(Subtask source, string propertyName)
        {
            return new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }

        private static (TextBlock OrdinalNumberTextBlock, DatePicker DateDatePicker, TextBox DescriptionTextBox) GetControls(int index)
        {
            var ordinalNumberTextBlock = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5, 5, 0, 0),
                Text = index.ToString() + ".",
                Style = (Style)Application.Current.FindResource("MaterialDesignBody1TextBlock")
            };

            var dateDatePicker = new DatePicker
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = 100,
                IsTodayHighlighted = true,
                FontSize = 16,
                Margin = new Thickness(25, 5, 0, 0)
            };

            var descriptionTextBox = new TextBox
            {
                Margin = new Thickness(130, 5, 5, 0),
                FontSize = 16
            };

            return (ordinalNumberTextBlock, dateDatePicker, descriptionTextBox);
        }
    }
}
