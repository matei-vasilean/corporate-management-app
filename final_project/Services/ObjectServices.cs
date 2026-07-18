using Newtonsoft.Json;
using final_project.Models;

namespace final_project.Services
{
    public class ObjectManager
    {
        private List<DataObject> dataObjects;
        private string ObjectFilePath;

        const int MIN_NAME_LENGTH = 2;
        const int MAX_NAME_LENGTH = 15;
        const int MIN_PASSWORD_LENGTH = 6;

        public ObjectManager()
        {
            dataObjects = new List<DataObject>();
            string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ObjectFilePath = Path.Combine(appDataFolderPath, "dataObjects.json");
            LoadDataObjects();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void LoadDataObjects()
        {
            try
            {
                if (File.Exists(ObjectFilePath))
                {
                    string json = File.ReadAllText(ObjectFilePath);
                    dataObjects = JsonConvert.DeserializeObject<List<DataObject>>(json);
                }
                else
                {
                    dataObjects = new List<DataObject>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data objects: " + ex.Message);
                dataObjects = new List<DataObject>();
            }
        }

        private void SaveDataObjects()
        {
            try
            {
                string json = JsonConvert.SerializeObject(dataObjects, Formatting.Indented);
                File.WriteAllText(ObjectFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving data objects: " + ex.Message);
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            SaveDataObjects();
        }

        public string CreateObject(string input, string password)
        {
            string name = input?.Trim();
            if (name.Length < MIN_NAME_LENGTH || name.Length > MAX_NAME_LENGTH)
                return "Please enter a valid name";

            if (string.IsNullOrEmpty(password) || password.Length < MIN_PASSWORD_LENGTH)
                return "Please enter a valid password";

            if (dataObjects.Any(obj => obj.Data.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return $"An object with the name '{name}' already exists";

            DataObject newDataObject = new DataObject(name, password);
            dataObjects.Add(newDataObject);
            SaveDataObjects();

            return $"Object '{name}' created successfully";
        }

        public List<DataObject> GetObjects()
        {
            return dataObjects;
        }

        public (string, string) ShowObjects()
        {
            if (dataObjects.Count == 0)
                return ("Error", "No objects to display");

            string output = "";
            foreach (var dataObject in dataObjects)
            {
                output += $"Username: {dataObject.Data}, Password: {dataObject.Password}\n";
            }
            return ("Objects", output.TrimEnd());
        }

        public string SearchObjects(string input)
        {
            string keyword = input?.Trim();
            if (string.IsNullOrEmpty(keyword))
                return "Please enter a keyword";

            List<DataObject> foundObjects = new List<DataObject>();
            foreach (var dataObject in dataObjects)
            {
                if (dataObject.Contains(keyword))
                {
                    foundObjects.Add(dataObject);
                }
            }

            if (foundObjects.Count == 0)
                return $"No objects found containing the keyword '{keyword}'";

            string output = "Found objects:\n";
            foreach (var foundObject in foundObjects)
            {
                output += $"Username: {foundObject.Data}, Password: {foundObject.Password}\n";
            }
            return output.TrimEnd();
        }

        public string ModifyObject(string currentData, string newData, string newPassword)
        {
            var obj = dataObjects.FirstOrDefault(o => o.Data == currentData);
            if (obj == null)
            {
                return "Object not found";
            }

            obj.Data = newData;
            obj.Password = newPassword;
            SaveDataObjects();

            return $"Object '{obj}' modified successfully";
        }

        public string DeleteObject(string currentData)
        {
            var obj = dataObjects.FirstOrDefault(o => o.Data == currentData);
            if (obj == null)
            {
                return "Object not found";
            }

            dataObjects.Remove(obj);
            SaveDataObjects();

            return $"Object '{obj}' deleted successfully";
        }
    }
}