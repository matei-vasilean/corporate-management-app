namespace final_project.Pages
{
    public partial class CreateObjectPage : ContentPage
    {
        public delegate void UserInputReceivedHandler(object sender, string data, string password);
        public event UserInputReceivedHandler UserInputReceived;

        Entry userInputEntry;
        Entry passwordEntry;
        Button confirmButton;

        public CreateObjectPage()
        {
            userInputEntry = new Entry
            {
                Placeholder = "Enter text",
                WidthRequest = 300,
                HeightRequest = 40,
            };

            passwordEntry = new Entry
            {
                Placeholder = "Enter password",
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
                    userInputEntry,
                    passwordEntry,
                    confirmButton
                }
            };
        }

        private async void OnConfirmClicked(object sender, EventArgs e)
        {
            UserInputReceived?.Invoke(this, userInputEntry.Text, passwordEntry.Text);
            await Navigation.PopModalAsync();
        }
    }
}