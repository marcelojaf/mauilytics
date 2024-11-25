using Plugin.Firebase.Analytics;

namespace Mauilytics;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the navigation back to the main page
    /// </summary>
    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
        CrossFirebaseAnalytics.Current.LogEvent("navigated_from_about");
    }
}