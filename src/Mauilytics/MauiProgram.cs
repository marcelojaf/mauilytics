using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
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
                return false;
		}));
#elif ANDROID
		events.AddAndroid(android => android.OnCreate((activity, _) =>
						CrossFirebase.Initialize(activity)));
#endif
		});

		return builder;
	}
}
