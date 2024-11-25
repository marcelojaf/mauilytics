using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Plugin.Firebase.Analytics;
using Plugin.Firebase.Crashlytics;

#if IOS
using Plugin.Firebase.Core.Platforms.iOS;
#elif ANDROID
using Plugin.Firebase.Core.Platforms.Android;
#endif

namespace Mauilytics;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterFirebaseServices();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    /// <summary>
    /// Registers Firebase services and initializes them based on the platform
    /// </summary>
    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {
        builder.ConfigureLifecycleEvents(events => {
#if IOS
            events.AddiOS(iOS => iOS.WillFinishLaunching((_,__) => {
                    CrossFirebase.Initialize();
                    SetupCrashlytics();
                    return false;
            }));
#elif ANDROID
            events.AddAndroid(android => android.OnCreate((activity, _) => {
                    CrossFirebase.Initialize(activity);
                    SetupCrashlytics();
            }));
#endif
        });

        builder.Services.AddSingleton(_ => CrossFirebaseCrashlytics.Current);
        builder.Services.AddSingleton(_ => CrossFirebaseAnalytics.Current);

        return builder;
    }

    /// <summary>
    /// Sets up Crashlytics configuration and error handlers
    /// </summary>
    private static void SetupCrashlytics()
    {
        // Enable crash collection
        CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
        
        // Configure handler for unhandled exceptions
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var exception = args.ExceptionObject as Exception;
            if (exception != null)
            {
                CrossFirebaseCrashlytics.Current.RecordException(exception);
            }
        };
        
        // Configure handler for unobserved task exceptions
        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            CrossFirebaseCrashlytics.Current.RecordException(args.Exception);
            args.SetObserved(); // Prevents process termination
        };
        
        // Set custom keys and values
        CrossFirebaseCrashlytics.Current.SetCustomKey(FirebaseConstants.CrashlyticsKeys.Environment, "production");
        CrossFirebaseCrashlytics.Current.SetUserId("mytestinguser123");
        
        // Log Crashlytics initialization
        CrossFirebaseCrashlytics.Current.Log("Crashlytics initialized");
    }
}