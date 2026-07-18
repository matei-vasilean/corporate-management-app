using final_project.Models;
using final_project.Services;

namespace final_project.Pages
{
    public partial class CreateEmployeePage : ContentPage
    {
        public delegate void UserInputReceivedHandler(object sender, string fullName, Department department, Position position, DateTime dateOfBirth, DataObject dataObject);
        public event UserInputReceivedHandler UserInputReceived;

        Entry fullNameEntry;
        Picker departmentPicker;
        Picker positionPicker;
        DatePicker dateOfBirthPicker;
        Picker dataObjectPicker;
        Button confirmButton;

        EmployeeManager employeeManager;
        ObjectManager objectManager;

        public CreateEmployeePage(EmployeeManager employeeManager, ObjectManager objectManager)
        {
            this.employeeManager = employeeManager;
            this.objectManager = objectManager;

            dataObjectPicker = new Picker
            {
                Title = "Select Data Object",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40,
            };

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
                HeightRequest = 40,
            };
            departmentPicker.ItemsSource = Enum.GetValues(typeof(Department)).Cast<Department>().ToList();

            positionPicker = new Picker
            {
                Title = "Select position",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40,
            };
            positionPicker.ItemsSource = Enum.GetValues(typeof(Position)).Cast<Position>().ToList();

            dateOfBirthPicker = new DatePicker
            {
                Format = "D",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40,
            };

            confirmButton = new Button
            {
                Text = "Confirm",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(20),
                WidthRequest = 150,
                HeightRequest = 40
            };
            confirmButton.Clicked += OnConfirmClicked;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    dataObjectPicker,
                    fullNameEntry,
                    departmentPicker,
                    positionPicker,
                    dateOfBirthPicker,
                    confirmButton
                }
            };

            PopulateDataObjectPicker();
        }

        private void PopulateDataObjectPicker()
        {
            employeeManager.PopulateDataObjectPicker(dataObjectPicker);
        }

        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            if (dataObjectPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "Please select a data object", "OK");
                return;
            }

            DataObject selectedDataObject = objectManager.GetObjects().FirstOrDefault(d => d.Data == dataObjectPicker.SelectedItem.ToString());

            if (selectedDataObject == null)
            {
                await DisplayAlert("Error", "Please select a valid data object", "OK");
                return;
            }

            UserInputReceived?.Invoke(this, fullNameEntry.Text, (Department)departmentPicker.SelectedItem, (Position)positionPicker.SelectedItem, dateOfBirthPicker.Date, selectedDataObject);
            await Navigation.PopModalAsync();
        }
    }
}