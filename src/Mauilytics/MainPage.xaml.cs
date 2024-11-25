using Acr.UserDialogs;
using Plugin.Firebase.Analytics;
using Plugin.Firebase.Crashlytics;

namespace Mauilytics;

public partial class MainPage : ContentPage
{
    /// <summary>
    /// Counter to track button clicks
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
    /// Handles the increment button click event and logs analytics
    /// </summary>
    private void OnIncrementClicked(object sender, EventArgs e)
    {
        count++;
        CounterLabel.Text = $"Total Count: {count}";
        
        CrossFirebaseAnalytics.Current.LogEvent(
            FirebaseConstants.Events.CounterIncremented, 
            new Dictionary<string, object> 
            { 
                { FirebaseConstants.Parameters.CountValue, count } 
            });
    }

    /// <summary>
    /// Handles navigation to the About page and logs the event
    /// </summary>
    private async void OnNavigateToAboutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AboutPage());
        CrossFirebaseAnalytics.Current.LogEvent(FirebaseConstants.Events.NavigatedToAbout);
    }

    /// <summary>
    /// Simulates an API call with random delay and logs performance metrics
    /// </summary>
    private async void OnSimulateApiCallClicked(object sender, EventArgs e)
    {
        try
        {
            UserDialogs.Instance.ShowLoading("Loading data...");
            
            int delayMs = random.Next(1500, 3001);
            await Task.Delay(delayMs);
            
            UserDialogs.Instance.HideLoading();
            
            CrossFirebaseAnalytics.Current.LogEvent(
                FirebaseConstants.Events.ApiCallCompleted, 
                new Dictionary<string, object> 
                { 
                    { FirebaseConstants.Parameters.DurationMs, delayMs } 
                });
        }
        catch (Exception ex)
        {
            UserDialogs.Instance.HideLoading();
            await DisplayAlert("Error", "Failed to simulate API call", "OK");
            
            CrossFirebaseAnalytics.Current.LogEvent(
                FirebaseConstants.Events.ApiCallError, 
                new Dictionary<string, object> 
                { 
                    { FirebaseConstants.Parameters.ErrorMessage, ex.Message } 
                });
        }
    }

    /// <summary>
    /// Generates and logs a handled exception for testing purposes
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
    /// Forces an application crash for testing purposes
    /// </summary>
    private void OnForceCrashClicked(object sender, EventArgs e)
    {
        string nullString = null;
        _ = nullString.Length; // This will cause a NullReferenceException
    }
}