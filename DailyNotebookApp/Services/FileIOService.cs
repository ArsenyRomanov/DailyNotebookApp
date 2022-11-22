using DailyNotebookApp.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;

namespace DailyNotebookApp.Services
{
    internal class FileIOService
    {
        private readonly string PATH;

        public FileIOService(string path)
        {
            PATH = path;
        }

        public void SaveData(BindingList<Task> tasks)
        {
            using(var sw = File.CreateText(PATH))
            {
                sw.Write(JsonConvert.SerializeObject(tasks));
            }
        }

        public BindingList<Task> LoadData()
        {
            if (!File.Exists(PATH))
            {
                File.Create(PATH).Dispose();
                return new BindingList<Task>();
            }
            using(var sr = File.OpenText(PATH))
            {
                return JsonConvert.DeserializeObject<BindingList<Task>>(sr.ReadToEnd());
            }
        }
    }
}
