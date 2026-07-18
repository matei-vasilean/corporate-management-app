namespace final_project.Models
{
    public class DataObject
    {
        public string Data { get; set; }
        public string Password { get; set; }

        public DataObject(string data, string password)
        {
            Data = data;
            Password = password;
        }

        public bool Contains(string keyword)
        {
            return Data.Contains(keyword);
        }

        public override string ToString()
        {
            return Data;
        }
    }
}