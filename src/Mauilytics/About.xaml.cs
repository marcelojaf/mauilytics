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
        CrossFirebaseAnalytics.Current.LogEvent(FirebaseConstants.Events.NavigatedFromAbout);
        await Navigation.PopAsync();
    }
}