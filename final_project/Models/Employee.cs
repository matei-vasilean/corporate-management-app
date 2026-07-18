namespace final_project.Models
{
    public class Employee
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string FullName { get; set; }
        public Department Department { get; set; }
        public Position Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DataObject DataObject { get; set; }

        public Employee(DataObject dataObject, string username, string password, string fullName, Department department, Position position, DateTime dateOfBirth)
        {
            Username = username;
            Password = password;
            FullName = fullName;
            Department = department;
            Position = position;
            DateOfBirth = dateOfBirth;
            DataObject = dataObject;
        }

        public override string ToString()
        {
            return Username;
        }
    }

    public enum Department
    {
        HR = 1,
        Finance = 2,
        IT = 4,
        Marketing = 8,
        Operations = 16,
        Sales = 32
    }

    public enum Position
    {
        CEO,
        Manager,
        Engineer,
        Analyst,
        Coordinator,
        Specialist,
        Accountant,
        Administrator,
        Secretary
    }
}