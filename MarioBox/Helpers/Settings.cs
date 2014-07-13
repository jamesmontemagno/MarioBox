// Helpers/Settings.cs
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace MarioBox
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string ColorKey = "color";
		private static readonly int ColorDefault = 1;

		#endregion


		public static int Color {
			get {
				return AppSettings.GetValueOrDefault (ColorKey, ColorDefault);
			}
			set {
				//if value has changed then save it!
				if (AppSettings.AddOrUpdateValue (ColorKey, value))
					AppSettings.Save ();
			}
		}
	}
}