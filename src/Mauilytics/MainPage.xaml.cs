using Acr.UserDialogs;
using Plugin.Firebase.Analytics;
using Plugin.Firebase.Crashlytics;

namespace Mauilytics;

public partial class MainPage : ContentPage
{
    /// <summary>
    /// Counter to keep track of button clicks
    /// </summary>
    private int count = 0;

    /// <summary>
    /// Random number generator for API simulation
    /// </summary>
    private readonly Random random = new Random();

    public MainPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the increment button click event
    /// Updates the counter and displays the new value
    /// </summary>
    private void OnIncrementClicked(object sender, EventArgs e)
    {
        count++;
        CounterLabel.Text = $"Total Count: {count}";
        
        // Log event to Firebase
        CrossFirebaseAnalytics.Current.LogEvent("counter_incremented", 
            new Dictionary<string, object> 
            { 
                { "count_value", count } 
            });
    }

    /// <summary>
    /// Navigates to the About page
    /// </summary>
    private async void OnNavigateToAboutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AboutPage());
        
        // Log navigation event
        CrossFirebaseAnalytics.Current.LogEvent("navigated_to_about");
    }

    /// <summary>
    /// Simulates an API call with random delay and shows loading dialog
    /// </summary>
    private async void OnSimulateApiCallClicked(object sender, EventArgs e)
    {
        try
        {
            UserDialogs.Instance.ShowLoading("Loading data...");
            
            // Generate random delay between 1.5 and 3 seconds
            int delayMs = random.Next(1500, 3001);
            await Task.Delay(delayMs);
            
            UserDialogs.Instance.HideLoading();
            
            // Log API call event with duration
            CrossFirebaseAnalytics.Current.LogEvent("api_call_completed", 
                new Dictionary<string, object> 
                { 
                    { "duration_ms", delayMs } 
                });
        }
        catch (Exception ex)
        {
            UserDialogs.Instance.HideLoading();
            await DisplayAlert("Error", "Failed to simulate API call", "OK");
            CrossFirebaseAnalytics.Current.LogEvent("api_call_error", 
                new Dictionary<string, object> 
                { 
                    { "error_message", ex.Message } 
                });
        }
    }

    /// <summary>
    /// Generates a handled exception for testing purposes
    /// </summary>
    private void OnGenerateExceptionClicked(object sender, EventArgs e)
    {
        try
        {
            throw new InvalidOperationException("This is a test exception");
        }
        catch (Exception ex)
        {
            CrossFirebaseCrashlytics.Current.RecordException(ex);
            
            DisplayAlert("Exception Generated", 
                        "A test exception was generated and logged.", 
                        "OK");
        }
    }

    /// <summary>
    /// Forces the application to crash for testing purposes
    /// </summary>
    private void OnForceCrashClicked(object sender, EventArgs e)
    {
        // This will cause a null reference exception
        string nullString = null;
        _ = nullString.Length;
    }
}