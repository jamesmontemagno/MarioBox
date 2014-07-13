using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MarioBox
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{

		// class-level declarations
		UIWindow window;
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{


			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGBA(0,0,0,0);
			UINavigationBar.Appearance.TintColor = UIColor.White;
			UINavigationBar.Appearance.SetTitleTextAttributes (new UITextAttributes () {
				Font = UIFont.FromName ("HelveticaNeue-Light", 20f),
				TextColor = UIColor.White
			});
			UIButton.Appearance.TintColor = UIColor.White;
			window.RootViewController = new UINavigationController (new PlaygroundViewController ());


			// make the window visible
			window.MakeKeyAndVisible ();

			app.ApplicationSupportsShakeToEdit = true;
			return true;
		}

		static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}

	}
}

