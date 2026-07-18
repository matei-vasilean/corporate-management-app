using final_project.Models;

namespace final_project.Pages
{
    [Obsolete]
    public partial class SchedulingPage : ContentPage
    {
        public event EventHandler<ScheduleEntry> ScheduleUpdated;
        public event EventHandler<int> ScheduleDeleted;
        private readonly Color buttonTextColor = Color.FromRgb(255, 255, 255);
        private readonly List<ScheduleEntry> scheduleEntries;
        private ScheduleEntry scheduleEntry;
        private readonly TimePicker startTimePicker;
        private readonly TimePicker endTimePicker;
        private readonly Entry descriptionEntry;
        private readonly Picker entryPicker;

        public SchedulingPage(List<ScheduleEntry> entries)
        {
            scheduleEntry = new ScheduleEntry();
            scheduleEntries = entries;

            entryPicker = new Picker();
            foreach (var entry in entries)
            {
                entryPicker.Items.Add($"{entry.StartTime:HH:mm} - {entry.EndTime:HH:mm}: {entry.Description}");
            }
            entryPicker.SelectedIndexChanged += EntryPicker_SelectedIndexChanged;

            var startTimeLabel = new Label { Text = "Start Time:", FontSize = 18 };
            startTimePicker = new TimePicker { Time = scheduleEntry.StartTime.TimeOfDay };

            var endTimeLabel = new Label { Text = "End Time:", FontSize = 18 };
            endTimePicker = new TimePicker { Time = scheduleEntry.EndTime.TimeOfDay };

            var descriptionLabel = new Label { Text = "Description:", FontSize = 18 };
            descriptionEntry = new Entry { Text = scheduleEntry.Description };

            var saveButton = new Button { Text = "Save", BackgroundColor = Color.FromHex("#2196F3"), TextColor = buttonTextColor, CornerRadius = 8 };
            var deleteButton = new Button { Text = "Delete", BackgroundColor = Color.FromHex("#FF5722"), TextColor = buttonTextColor, CornerRadius = 8 };

            saveButton.Clicked += OnSaveButtonClicked;
            deleteButton.Clicked += OnDeleteButtonClicked;

            Content = new StackLayout
            {
                Padding = new Thickness(20),
                Spacing = 10,
                Children =
                {
                    entryPicker,
                    startTimeLabel,
                    startTimePicker,
                    endTimeLabel,
                    endTimePicker,
                    descriptionLabel,
                    descriptionEntry,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        Children = { saveButton, deleteButton }
                    }
                }
            };

            Title = "Modify Schedule Entry";
        }

        private void EntryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = entryPicker.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < scheduleEntries.Count)
            {
                scheduleEntry = scheduleEntries[selectedIndex];
                startTimePicker.Time = scheduleEntry.StartTime.TimeOfDay;
                endTimePicker.Time = scheduleEntry.EndTime.TimeOfDay;
                descriptionEntry.Text = scheduleEntry.Description;
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            scheduleEntry.StartTime = DateTime.Today.Add(startTimePicker.Time);
            scheduleEntry.EndTime = DateTime.Today.Add(endTimePicker.Time);
            scheduleEntry.Description = descriptionEntry.Text;

            ScheduleUpdated?.Invoke(this, scheduleEntry);

            await DisplayAlert("Success", "Schedule entry saved successfully.", "OK");
            await Navigation.PopModalAsync();
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this entry?", "Yes", "No");
            if (confirm)
            {
                ScheduleDeleted?.Invoke(this, scheduleEntry.Id);
                await DisplayAlert("Success", "Schedule entry deleted successfully.", "OK");
                await Navigation.PopModalAsync();
            }
        }
    }
}
