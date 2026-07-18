using final_project.Services;

namespace final_project.Pages
{
    public partial class ModifyObjectPage : ContentPage
    {
        private ObjectManager objectManager;

        public delegate void ObjectModificationHandler(object sender, string currentData, string newData, string newPassword);
        public event ObjectModificationHandler ObjectModified;
        public event EventHandler<string> ObjectDeleted;

        Picker objectPicker;
        Entry newDataEntry;
        Entry newPasswordEntry;
        Button modifyButton;
        Button deleteButton;

        public ModifyObjectPage(ObjectManager objectManager)
        {
            this.objectManager = objectManager;

            objectPicker = new Picker
            {
                Title = "Select Object",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40,
            };
            foreach (var dataObject in objectManager.GetObjects())
            {
                objectPicker.Items.Add(dataObject.Data);
            }

            newDataEntry = new Entry
            {
                Placeholder = "Enter new text",
                WidthRequest = 300,
                HeightRequest = 40,
            };

            newPasswordEntry = new Entry
            {
                Placeholder = "Enter new password",
                Margin = new Thickness(20),
                WidthRequest = 300,
                HeightRequest = 40,
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

            objectPicker.SelectedIndexChanged += ObjectPicker_SelectedIndexChanged;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    objectPicker,
                    newDataEntry,
                    newPasswordEntry,
                    modifyButton,
                    deleteButton
                }
            };
        }

        private void ObjectPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = objectPicker.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < objectManager.GetObjects().Count)
            {
                var selectedObject = objectManager.GetObjects()[selectedIndex];
                newDataEntry.Text = selectedObject.Data;
                newPasswordEntry.Text = selectedObject.Password;
            }
        }

        private async void OnModifyClicked(object sender, EventArgs e)
        {
            string currentData = objectPicker.SelectedItem as string;
            if (currentData == null)
            {
                await DisplayAlert("Error", "Please select an object.", "OK");
                return;
            }

            if (newDataEntry.Text.Length >= 2 && newPasswordEntry.Text.Length >= 8)
            {
                ObjectModified?.Invoke(this, currentData, newDataEntry.Text, newPasswordEntry.Text);
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid username and/or password", "OK");
                return;
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            string currentData = objectPicker.SelectedItem as string;
            if (currentData == null)
            {
                await DisplayAlert("Error", "Please select an object.", "OK");
                return;
            }

            bool result = await DisplayAlert("Confirmation", "Are you sure you want to delete this object?", "Yes", "No");
            if (result)
            {
                ObjectDeleted?.Invoke(this, currentData);
                await Navigation.PopModalAsync();
            }
        }
    }
}