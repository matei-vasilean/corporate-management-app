using final_project.Models;
using final_project.Services;

namespace final_project.Pages
{
    public partial class ModifyEmployeePage : ContentPage
    {
        private EmployeeManager employeeManager;

        public delegate void EmployeeModificationHandler(object sender, string username, string fullName, Department department, Position position, DateTime dateOfBirth);
        public event EmployeeModificationHandler EmployeeModified;
        public event EventHandler<string> EmployeeDeleted;

        Picker employeePicker;
        Entry fullNameEntry;
        Picker departmentPicker;
        Picker positionPicker;
        DatePicker dateOfBirthPicker;
        Button modifyButton;
        Button deleteButton;

        public ModifyEmployeePage(EmployeeManager employeeManager)
        {
            this.employeeManager = employeeManager;

            employeePicker = new Picker
            {
                Title = "Select Employee",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40,
            };
            foreach (var usernameEmployee in employeeManager.GetEmployees())
            {
                employeePicker.Items.Add(usernameEmployee.Username);
            }

            fullNameEntry = new Entry
            {
                Placeholder = "Enter full name",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40,
            };

            departmentPicker = new Picker
            {
                Title = "Select department",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40
            };
            departmentPicker.ItemsSource = Enum.GetValues(typeof(Department)).Cast<Department>().ToList();

            positionPicker = new Picker
            {
                Title = "Select position",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40
            };
            positionPicker.ItemsSource = Enum.GetValues(typeof(Position)).Cast<Position>().ToList();

            dateOfBirthPicker = new DatePicker
            {
                Format = "D",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40
            };

            modifyButton = new Button
            {
                Text = "Modify",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(20),
                WidthRequest = 150,
                HeightRequest = 40
            };
            modifyButton.Clicked += OnModifyClicked;

            deleteButton = new Button
            {
                Text = "Delete",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(20),
                WidthRequest = 150,
                HeightRequest = 40,
                BackgroundColor = Colors.Red
            };
            deleteButton.Clicked += OnDeleteClicked;

            employeePicker.SelectedIndexChanged += EmployeePicker_SelectedIndexChanged;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    employeePicker,
                    fullNameEntry,
                    departmentPicker,
                    positionPicker,
                    dateOfBirthPicker,
                    modifyButton,
                    deleteButton
                }
            };
        }

        private void EmployeePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = employeePicker.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < employeeManager.GetEmployees().Count)
            {
                var selectedEmployee = employeeManager.GetEmployees()[selectedIndex];
                fullNameEntry.Text = selectedEmployee.FullName;
                departmentPicker.SelectedItem = selectedEmployee.Department;
                positionPicker.SelectedItem = selectedEmployee.Position;
                dateOfBirthPicker.Date = selectedEmployee.DateOfBirth;
            }
        }

        private async void OnModifyClicked(object sender, EventArgs e)
        {
            if (employeePicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select an employee.", "OK");
                return;
            }

            string selectedUsername = employeePicker.SelectedItem.ToString();
            EmployeeModified?.Invoke(this, selectedUsername, fullNameEntry.Text, (Department)departmentPicker.SelectedItem, (Position)positionPicker.SelectedItem, dateOfBirthPicker.Date);
            await Navigation.PopModalAsync();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (employeePicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select an employee.", "OK");
                return;
            }

            string selectedUsername = employeePicker.SelectedItem.ToString();

            bool result = await DisplayAlert("Confirmation", "Are you sure you want to delete this employee?", "Yes", "No");
            if (result)
            {
                EmployeeDeleted?.Invoke(this, selectedUsername);
                await Navigation.PopModalAsync();
            }
        }
    }
}