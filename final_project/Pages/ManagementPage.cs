using final_project.Models;
using final_project.Services;

namespace final_project.Pages
{
    [Obsolete]
    public partial class ManagementPage : ContentPage
    {
        readonly Color buttonTextColor = Color.FromRgb(255, 255, 255);
        readonly Color buttonColor = Color.FromRgb(33, 150, 243);

        ObjectManager objectManager;
        EmployeeManager employeeManager;

        public ManagementPage()
        {
            objectManager = new ObjectManager();
            employeeManager = new EmployeeManager(objectManager);

            var welcomeLabel = new Label
            {
                Text = "MANAGEMENT",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 30, 0, 30)
            };

            var createButton = new Button
            {
                Text = "Create Objects",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.1 * Application.Current.MainPage.Width, 10, 0.05 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            var createEmployeeButton = new Button
            {
                Text = "Create Employee",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.05 * Application.Current.MainPage.Width, 10, 0.1 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            var showButton = new Button
            {
                Text = "Show Objects",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.1 * Application.Current.MainPage.Width, 10, 0.05 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            var showEmployeesButton = new Button
            {
                Text = "Show Employees",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.05 * Application.Current.MainPage.Width, 10, 0.1 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            var searchButton = new Button
            {
                Text = "Search Objects",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.1 * Application.Current.MainPage.Width, 10, 0.05 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            var searchEmployeesButton = new Button
            {
                Text = "Search Employees",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.05 * Application.Current.MainPage.Width, 10, 0.1 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            var modifyButton = new Button
            {
                Text = "Modify/Remove Objects",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.1 * Application.Current.MainPage.Width, 10, 0.05 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            var modifyEmployeeButton = new Button
            {
                Text = "Modify/Remove Employees",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.05 * Application.Current.MainPage.Width, 10, 0.1 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            var createButtonsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { createButton, createEmployeeButton }
            };

            var showButtonsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { showButton, showEmployeesButton }
            };

            var searchButtonsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { searchButton, searchEmployeesButton }
            };

            var modifyButtonsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { modifyButton, modifyEmployeeButton }
            };

            createButton.Clicked += OnCreateObjectClicked;
            createEmployeeButton.Clicked += OnCreateEmployeeClicked;
            showButton.Clicked += OnShowClicked;
            showEmployeesButton.Clicked += OnShowEmployeesClicked;
            searchButton.Clicked += OnSearchClicked;
            searchEmployeesButton.Clicked += OnSearchEmployeesClicked;
            modifyButton.Clicked += OnModifyObjectClicked;
            modifyEmployeeButton.Clicked += OnModifyEmployeeClicked;

            Content = new StackLayout
            {
                Children = {
                    welcomeLabel,
                    createButtonsLayout,
                    showButtonsLayout,
                    searchButtonsLayout,
                    modifyButtonsLayout
                }
            };
        }

        private async void OnCreateObjectClicked(object sender, EventArgs e)
        {
            var createObjectPage = new CreateObjectPage();
            createObjectPage.UserInputReceived += (s, data, password) =>
            {
                HandleObjectInput(data, password);
            };
            await Navigation.PushModalAsync(createObjectPage);
        }

        private void HandleObjectInput(string data, string password)
        {
            string returningText = objectManager.CreateObject(data, password);

            if (returningText == "Please enter a valid name")
            {
                DisplayAlert("Error", returningText, "OK");
            }
            else
            {
                DisplayAlert("Success", returningText, "OK");
            }
        }

        private async void OnCreateEmployeeClicked(object sender, EventArgs e)
        {
            var createEmployeePage = new CreateEmployeePage(employeeManager, objectManager);
            createEmployeePage.UserInputReceived += HandleEmployeeInput;
            await Navigation.PushModalAsync(createEmployeePage);
        }

        private void HandleEmployeeInput(object sender, string fullName, Department department, Models.Position position, DateTime dateOfBirth, DataObject dataObject)
        {
            string resultMessage = employeeManager.CreateEmployee(fullName, department, position, dateOfBirth, dataObject);

            if (resultMessage.StartsWith("Employee created successfully"))
            {
                DisplayAlert("Success", resultMessage, "OK");
            }
            else
            {
                DisplayAlert("Error", resultMessage, "OK");
            }
        }

        private void OnShowClicked(object sender, EventArgs e)
        {
            var (output1, output2) = objectManager.ShowObjects();
            DisplayAlert(output1, output2, "OK");
        }

        private void OnShowEmployeesClicked(object sender, EventArgs e)
        {
            var (output1, output2) = employeeManager.ShowEmployees();
            DisplayAlert(output1, output2, "OK");
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            string keyword = await DisplayPromptAsync("Search Objects", "Enter keyword:");

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                await DisplayAlert("Search completed", objectManager.SearchObjects(keyword), "OK");
            }
        }

        private async void OnSearchEmployeesClicked(object sender, EventArgs e)
        {
            string keyword = await DisplayPromptAsync("Search Employees", "Enter keyword:");

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                await DisplayAlert("Search completed", employeeManager.SearchEmployees(keyword), "OK");
            }
        }

        private async void OnModifyObjectClicked(object sender, EventArgs e)
        {
            var modifyObjectPage = new ModifyObjectPage(objectManager);
            modifyObjectPage.ObjectModified += HandleObjectModification;
            modifyObjectPage.ObjectDeleted += HandleObjectDeletion;
            await Navigation.PushModalAsync(modifyObjectPage);
        }

        private void HandleObjectModification(object sender, string currentData, string newData, string newPassword)
        {
            string resultMessage = objectManager.ModifyObject(currentData, newData, newPassword);

            if (resultMessage.Contains("successfully"))
            {
                DisplayAlert("Success", resultMessage, "OK");
            }
            else
            {
                DisplayAlert("Error", resultMessage, "OK");
            }
        }

        private void HandleObjectDeletion(object sender, string currentData)
        {
            string resultMessage = objectManager.DeleteObject(currentData);

            if (resultMessage.Contains("successfully"))
            {
                DisplayAlert("Success", resultMessage, "OK");
            }
            else
            {
                DisplayAlert("Error", resultMessage, "OK");
            }
        }

        private void OnModifyEmployeeClicked(object sender, EventArgs e)
        {
            var modifyPage = new ModifyEmployeePage(employeeManager);
            modifyPage.EmployeeModified += HandleEmployeeModification;
            modifyPage.EmployeeDeleted += HandleEmployeeDeletion;
            Navigation.PushModalAsync(modifyPage);
        }

        private void HandleEmployeeModification(object sender, string username, string fullName, Department department, Models.Position position, DateTime dateOfBirth)
        {
            string resultMessage = employeeManager.ModifyEmployee(username, fullName, department, position, dateOfBirth);

            if (resultMessage.Contains("successfully"))
            {
                DisplayAlert("Success", resultMessage, "OK");
            }
            else
            {
                DisplayAlert("Error", resultMessage, "OK");
            }
        }

        private void HandleEmployeeDeletion(object sender, string username)
        {
            string resultMessage = employeeManager.DeleteEmployee(username);

            if (resultMessage.Contains("successfully"))
            {
                DisplayAlert("Success", resultMessage, "OK");
            }
            else
            {
                DisplayAlert("Error", resultMessage, "OK");
            }
        }

        private void OnCreateInputEntryCompleted(object sender, EventArgs e)
        {
            OnCreateObjectClicked(sender, e);
        }

        private void OnSearchInputEntryCompleted(object sender, EventArgs e)
        {
            OnSearchClicked(sender, e);
        }

        private void OnSearchEmployeesInputEntryCompleted(object sender, EventArgs e)
        {
            OnSearchEmployeesClicked(sender, e);
        }
    }
}