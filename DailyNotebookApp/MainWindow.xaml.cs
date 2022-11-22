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
            tasks.ListChanged += TaskList_ListChanged;
        }

        private void TaskList_ListChanged(object sender, ListChangedEventArgs e)
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
    }
}
