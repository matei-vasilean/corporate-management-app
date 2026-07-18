using Newtonsoft.Json;
using final_project.Models;

namespace final_project.Services
{
    public class EmployeeManager
    {
        private List<Employee> employees;
        private ObjectManager objectManager;
        private string EmployeeFilePath;

        public EmployeeManager(ObjectManager objectManager)
        {
            this.objectManager = objectManager;
            employees = new List<Employee>();
            string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            EmployeeFilePath = Path.Combine(appDataFolderPath, "employees.json");
            LoadEmployees();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void LoadEmployees()
        {
            try
            {
                if (File.Exists(EmployeeFilePath))
                {
                    string json = File.ReadAllText(EmployeeFilePath);
                    employees = JsonConvert.DeserializeObject<List<Employee>>(json);
                }
                else
                {
                    employees = new List<Employee>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading employees: " + ex.Message);
                employees = new List<Employee>();
            }
        }

        private void SaveEmployees()
        {
            try
            {
                string json = JsonConvert.SerializeObject(employees, Formatting.Indented);
                File.WriteAllText(EmployeeFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving employees: " + ex.Message);
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            SaveEmployees();
        }

        public string CreateEmployee(string fullName, Department department, Position position, DateTime dateOfBirth, DataObject dataObject)
        {
            if (employees == null)
            {
                employees = new List<Employee>();
            }

            if (employees.Any(e => e.Username == dataObject.Data))
            {
                return $"An employee with the username '{dataObject.Data}' already exists";
            }

            var employee = new Employee(dataObject, dataObject.Data, dataObject.Password, fullName, department, position, dateOfBirth);
            employees.Add(employee);
            SaveEmployees();

            return $"Employee '{dataObject.Data}' created successfully";
        }

        public List<Employee> GetEmployees()
        {
            return employees;
        }

        public (string, string) ShowEmployees()
        {
            if (employees.Count == 0)
                return ("Error", "No employees to display");

            string output = "";
            for (int i = 0; i < employees.Count; i++)
            {
                output += $"{employees[i].Username} - {employees[i].FullName} - {employees[i].Department} - {employees[i].Position}";
                if (i < employees.Count - 1)
                    output += "\n";
            }
            return ("Employees", output.TrimEnd());
        }

        public string SearchEmployees(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return "Please enter a keyword";

            List<string> foundEmployees = new List<string>();
            foreach (var employee in employees)
            {
                if (employee.Username.Contains(keyword) || employee.FullName.Contains(keyword) || employee.Department.ToString().Contains(keyword) || employee.Position.ToString().Contains(keyword))
                {
                    foundEmployees.Add($"{employee.Username} - {employee.FullName} - {employee.Department} - {employee.Position}");
                }
            }

            if (foundEmployees.Count == 0)
                return $"No employees found matching the keyword '{keyword}'";

            string output = "Found employees:\n";
            foreach (var foundEmployee in foundEmployees)
            {
                output += foundEmployee + "\n";
            }
            return output.TrimEnd();
        }

        public void PopulateDataObjectPicker(Picker picker)
        {
            picker.Items.Clear();

            var dataObjects = objectManager.GetObjects();

            foreach (var dataObject in dataObjects)
            {
                picker.Items.Add(dataObject.Data);
            }
        }

        public string ModifyEmployee(string currentUsername, string fullName, Department department, Position position, DateTime dateOfBirth)
        {
            var employee = employees.FirstOrDefault(e => e.Username == currentUsername);
            if (employee == null)
            {
                return "Employee not found";
            }

            employee.FullName = fullName;
            employee.Department = department;
            employee.Position = position;
            employee.DateOfBirth = dateOfBirth;
            SaveEmployees();

            return $"Employee '{employee}' modified successfully";
        }

        public string DeleteEmployee(string username)
        {
            var employee = employees.FirstOrDefault(e => e.Username == username);
            if (employee == null)
            {
                return "Employee not found";
            }

            employees.Remove(employee);
            SaveEmployees();

            return $"Employee '{employee}' deleted successfully";
        }
    }
}