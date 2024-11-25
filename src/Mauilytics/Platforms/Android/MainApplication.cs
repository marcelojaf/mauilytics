using Acr.UserDialogs;
using Android.App;
using Android.Runtime;
using Plugin.Firebase.Crashlytics;

namespace Mauilytics;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override void OnCreate()
	{
		base.OnCreate();
		CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
		UserDialogs.Init(this);
	}
}
