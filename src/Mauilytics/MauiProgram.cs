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

					SetupCrashlytics();
					CrossFirebase.Initialize(activity);
			}));
#endif
		});

		builder.Services.AddSingleton(_ => CrossFirebaseCrashlytics.Current);
        builder.Services.AddSingleton(_ => CrossFirebaseAnalytics.Current);

		return builder;
	}

	private static void SetupCrashlytics()
    {
        // Habilitar coleta de crashes
        CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
        
        // Configurar handler para exceções não tratadas
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var exception = args.ExceptionObject as Exception;
            if (exception != null)
            {
                CrossFirebaseCrashlytics.Current.RecordException(exception);
            }
        };
        
        // Configurar handler para exceções de Task não observadas
        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            CrossFirebaseCrashlytics.Current.RecordException(args.Exception);
            args.SetObserved(); // Previne que o processo seja terminado
        };
        
        // Registrar chaves e valores customizados (opcional)
        CrossFirebaseCrashlytics.Current.SetCustomKey("environment", "production");
        CrossFirebaseCrashlytics.Current.SetUserId("mytestinguser123");
        
        // Log que o Crashlytics está pronto
        CrossFirebaseCrashlytics.Current.Log("Crashlytics initialized");
    }
}
