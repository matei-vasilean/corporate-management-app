namespace final_project.Pages
{
    [Obsolete]
    public partial class LoginPage : ContentPage
    {
        private Entry usernameEntry;
        private Entry passwordEntry;
        private Button loginButton;

        public LoginPage()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            var signInLabel = new Label
            {
                Text = "SIGN IN",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 0, 20)
            };

            usernameEntry = new Entry
            {
                Placeholder = "Username",
                HeightRequest = 60
            };
            usernameEntry.Completed += (s, e) => passwordEntry.Focus();

            passwordEntry = new Entry
            {
                Placeholder = "Password",
                HeightRequest = 60
            };
            passwordEntry.Completed += OnPasswordEntryCompleted;

            loginButton = new Button
            {
                Text = "Login",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(20),
                HeightRequest = 45
            };
            loginButton.Clicked += OnLoginButtonClicked;

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = { signInLabel, usernameEntry, passwordEntry, loginButton }
            };
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > 0)
            {
                usernameEntry.WidthRequest = width * 0.7;
                passwordEntry.WidthRequest = width * 0.7;
                loginButton.WidthRequest = width * 0.45;
            }
        }

        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            Login();
        }

        private void OnPasswordEntryCompleted(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            if (username == "admin" && password == "pass")
            {
                Application.Current.MainPage = new MainPage("admin");
            }
            else if (username == "user" && password == "pass")
            {
                Application.Current.MainPage = new MainPage("user");
            }
            else
            {
                DisplayAlert("Login Failed", "Invalid username or password", "OK");
                usernameEntry.Text = "";
                passwordEntry.Text = "";
            }
        }
    }
}