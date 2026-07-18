using final_project.Pages;

namespace final_project
{
    [Obsolete]
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }
    }
}