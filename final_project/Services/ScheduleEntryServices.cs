using Newtonsoft.Json;
using final_project.Models;

namespace final_project.Services
{
    public class ScheduleEntryManager
    {
        private List<ScheduleEntry> scheduleEntries;
        private readonly string ScheduleEntryFilePath;

        public ScheduleEntryManager()
        {
            string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ScheduleEntryFilePath = Path.Combine(appDataFolderPath, "scheduleEntries.json");

            LoadScheduleEntries();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void LoadScheduleEntries()
        {
            try
            {
                if (File.Exists(ScheduleEntryFilePath))
                {
                    string json = File.ReadAllText(ScheduleEntryFilePath);
                    scheduleEntries = JsonConvert.DeserializeObject<List<ScheduleEntry>>(json) ?? new List<ScheduleEntry>();
                    Console.WriteLine("Schedule entries loaded successfully.");
                }
                else
                {
                    scheduleEntries = new List<ScheduleEntry>();
                    Console.WriteLine("No schedule entries found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading schedule entries: " + ex.Message);
                scheduleEntries = new List<ScheduleEntry>();
            }
        }

        private void SaveScheduleEntries()
        {
            try
            {
                string json = JsonConvert.SerializeObject(scheduleEntries, Formatting.Indented);
                File.WriteAllText(ScheduleEntryFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving schedule entries: " + ex.Message);
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            SaveScheduleEntries();
        }

        public List<ScheduleEntry> GetAllEntries()
        {
            return scheduleEntries.OrderBy(entry => entry.StartTime).ThenBy(entry => entry.EndTime).ToList();
        }

        public void AddEntry(ScheduleEntry entry)
        {
            try
            {
                entry.Id = scheduleEntries.Count > 0 ? scheduleEntries.Max(e => e.Id) + 1 : 1;
                scheduleEntries.Add(entry);
                SaveScheduleEntries();
                Console.WriteLine("Entry added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding schedule entry: " + ex.Message);
            }
        }

        public void UpdateEntry(ScheduleEntry entry)
        {
            try
            {
                var existingEntry = scheduleEntries.FirstOrDefault(e => e.Id == entry.Id);
                if (existingEntry != null)
                {
                    existingEntry.StartTime = entry.StartTime;
                    existingEntry.EndTime = entry.EndTime;
                    existingEntry.Description = entry.Description;
                    SaveScheduleEntries();
                    Console.WriteLine("Entry updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Entry with ID {entry.Id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating schedule entry: " + ex.Message);
            }
        }

        public void DeleteEntry(int entryId)
        {
            try
            {
                var entry = scheduleEntries.FirstOrDefault(e => e.Id == entryId);
                if (entry != null)
                {
                    scheduleEntries.Remove(entry);
                    SaveScheduleEntries();
                    Console.WriteLine("Entry deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Entry with ID {entryId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting schedule entry: " + ex.Message);
            }
        }
    }
}
