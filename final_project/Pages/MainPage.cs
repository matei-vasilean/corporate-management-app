using final_project.Models;
using final_project.Services;
using final_project.Converters;
using System.Collections.ObjectModel;

namespace final_project.Pages
{
    [Obsolete]
    public partial class MainPage : ContentPage
    {
        private readonly Color buttonTextColor = Color.FromRgb(255, 255, 255);
        private readonly Color buttonColor = Color.FromRgb(33, 150, 243);
        private readonly ObservableCollection<ScheduleEntry> scheduleEntries;
        private readonly ScheduleEntryManager scheduleEntryManager;

        public MainPage(string userType)
        {
            scheduleEntryManager = new ScheduleEntryManager();
            scheduleEntries = new ObservableCollection<ScheduleEntry>(scheduleEntryManager.GetAllEntries());

            var window = new Window
            { };

            var welcomeLabel = new Label
            {
                Text = "WELCOME",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 30, 0, 30)
            };

            var scheduleListView = new ListView
            {
                ItemsSource = scheduleEntries,
                ItemTemplate = new DataTemplate(() =>
                {
                    var timeRangeLabel = new Label
                    {
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        HorizontalOptions = LayoutOptions.Start
                    };

                    var startTimeBinding = new Binding("StartTime");
                    var endTimeBinding = new Binding("EndTime");

                    timeRangeLabel.SetBinding(Label.TextProperty, new MultiBinding
                    {
                        Bindings = { startTimeBinding, endTimeBinding },
                        Converter = new TimeRangeConverter()
                    });

                    var descriptionLabel = new Label
                    {
                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                        HorizontalOptions = LayoutOptions.Start
                    };
                    descriptionLabel.SetBinding(Label.TextProperty, "Description");

                    var viewCell = new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new Label { Text = "\t\t" },
                                timeRangeLabel,
                                new Label { Text = "\t\t\t\t" },
                                descriptionLabel
                            }
                        }
                    };

                    return viewCell;
                })
            };

            scheduleListView.ItemSelected += (sender, args) =>
            {
                if (sender is ListView listView && listView.SelectedItem != null)
                {
                    object selectedItem = listView.SelectedItem;
                    listView.SelectedItem = null;
                }
            };

            var schedulingButton = new Button
            {
                Text = "Scheduling",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.1 * Application.Current.MainPage.Width, 10, 0.05 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            var managementButton = new Button
            {
                Text = "Management",
                BackgroundColor = buttonColor,
                TextColor = buttonTextColor,
                CornerRadius = 5,
                Margin = new Thickness(0.05 * Application.Current.MainPage.Width, 10, 0.1 * Application.Current.MainPage.Width, 20),
                WidthRequest = 0.35 * Application.Current.MainPage.Width,
                HeightRequest = 80
            };

            schedulingButton.Clicked += OnSchedulingButtonClicked;
            managementButton.Clicked += OnManagementButtonClicked;

            var menuButtonsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { schedulingButton, managementButton }
            };

            Content = new StackLayout
            {
                Children =
                {
                    welcomeLabel,
                    scheduleListView,
                    menuButtonsLayout
                }
            };

            if (userType == "user")
            {
                menuButtonsLayout.IsVisible = false;
            }
        }

        private async void OnSchedulingButtonClicked(object sender, EventArgs e)
        {
            var schedulingPage = new SchedulingPage(scheduleEntryManager.GetAllEntries());
            schedulingPage.ScheduleUpdated += OnScheduleUpdated;
            schedulingPage.ScheduleDeleted += OnScheduleDeleted;
            await Navigation.PushModalAsync(schedulingPage);
        }

        private async void OnManagementButtonClicked(object sender, EventArgs e)
        {
            var managementPage = new ManagementPage();
            await Navigation.PushModalAsync(managementPage);
        }

        private void OnScheduleUpdated(object sender, ScheduleEntry entry)
        {
            if (entry.Id == 0)
            {
                scheduleEntryManager.AddEntry(entry);
            }
            else
            {
                scheduleEntryManager.UpdateEntry(entry);
            }

            RefreshScheduleEntries();
        }

        private void OnScheduleDeleted(object sender, int entryId)
        {
            scheduleEntryManager.DeleteEntry(entryId);
            RefreshScheduleEntries();
        }

        private void RefreshScheduleEntries()
        {
            scheduleEntries.Clear();
            foreach (var e in scheduleEntryManager.GetAllEntries())
            {
                scheduleEntries.Add(e);
            }
        }
    }
}
