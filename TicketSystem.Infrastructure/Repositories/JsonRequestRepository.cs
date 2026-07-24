using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using TicketSystem.Interfaces;
using TicketSystem.Models;

namespace TicketSystem.Infrastructure.Repositories
{
    public class JsonRequestRepository : IRequestRepository
    {
        private readonly string _filePath = "Data/requests.json";

        public JsonRequestRepository()
        {
            if (!File.Exists(_filePath))
            {
                Directory.CreateDirectory("Data");
                File.WriteAllText(_filePath, "[]");
            }
        }

        private List<UserRequest> LoadData()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<UserRequest>>(json) ?? new List<UserRequest>();
        }

        private void SaveData(List<UserRequest> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
            };

            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_filePath, json);
        }

        public IEnumerable<UserRequest> GetAll() => LoadData();

        public UserRequest GetById(Guid id) => LoadData().FirstOrDefault(r => r.Id == id);

        public void Add(UserRequest request)
        {
            var data = LoadData();
            data.Add(request);
            SaveData(data);
        }

        public void Update(UserRequest request)
        {
            var data = LoadData();
            var index = data.FindIndex(r => r.Id == request.Id);
            if (index != -1)
            {
                data[index] = request;
                SaveData(data);
            }
        }

        public void Delete(Guid id)
        {
            var data = LoadData();
            var item = data.FirstOrDefault(r => r.Id == id);
            if (item != null)
            {
                data.Remove(item);
                SaveData(data);
            }
        }
    }
}
