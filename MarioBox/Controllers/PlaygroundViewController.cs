using System;
using MonoTouch.UIKit;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace MarioBox
{
	public class PlaygroundViewController : UIViewController
	{
		PlayGroundView PlayGroundView;
		AboutController about;
		public PlaygroundViewController ()
		{
			this.Title = "Mario Box";
		}

		UIBarButtonItem editButton, doneButton, addButton;
		UIActionSheet addAction;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
			addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, (sender, args) => {

				if(addAction == null)
				{
					addAction = new UIActionSheet("Add AR Card", 
						null, 
						"Cancel",
						null, 
						PlayGroundViewModel.AllARCards);
						addAction.Clicked += (sender2, e) => {
						if(e.ButtonIndex >= PlayGroundViewModel.AllARCards.Length)
							return;

						PlayGroundViewModel.Instance.AddCard(PlayGroundViewModel.AllARCards[e.ButtonIndex]);
						PlayGroundView.UpdateARCards (PlayGroundViewModel.Instance.ActiveARCards);
					};
				}
				if(!addAction.Visible)
					addAction.ShowFrom(addButton, true);
			});

			editButton = new UIBarButtonItem (UIBarButtonSystemItem.Edit, 
				                 (sender, args) => {
					NavigationItem.RightBarButtonItems = new UIBarButtonItem[] {
						doneButton, addButton
					};

					PlayGroundViewModel.Instance.EditMode = true;
				});

			doneButton = new UIBarButtonItem (UIBarButtonSystemItem.Done, 
				(sender, args) => {
					NavigationItem.RightBarButtonItems = new UIBarButtonItem[] {
						editButton, addButton
					};
					PlayGroundViewModel.Instance.EditMode = false;
				});

			about = new AboutController ();

			var infoBarButton = new UIBarButtonItem (UIImage.FromBundle ("settings"),
				UIBarButtonItemStyle.Plain,
				(sender, args) => {
					NavigationController.PushViewController(about, true);
			});

			var saveButton = new UIBarButtonItem (UIImage.FromBundle ("save"),
				UIBarButtonItemStyle.Plain,
				(sender, args) => {
					PlayGroundViewModel.Instance.SaveDefault();
					new UIAlertView("Saved", "You are good to go!", null, "OK").Show();
			});

			NavigationItem.RightBarButtonItems = new UIBarButtonItem[] {
				editButton, addButton
			};

			NavigationItem.LeftBarButtonItems = new UIBarButtonItem[] {
				infoBarButton, saveButton
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			PlayGroundView.Parent = this;
			PlayGroundView.UpdateARCards (PlayGroundViewModel.Instance.ActiveARCards);
			base.ViewWillAppear (animated);
			this.BecomeFirstResponder ();
			switch (Settings.Color) {
			case 0:
				View.BackgroundColor = Color.Blue;
				NavigationController.NavigationBar.BarTintColor = Color.Blue;
				break;
			case 1: 
				View.BackgroundColor = Color.DarkBlue;
				NavigationController.NavigationBar.BarTintColor = Color.DarkBlue;
				break;
			case 2:
				View.BackgroundColor = Color.Gray;
				NavigationController.NavigationBar.BarTintColor = Color.Gray;
				break;
			case 3:
				View.BackgroundColor = Color.Green;
				NavigationController.NavigationBar.BarTintColor = Color.Green;
				break;
			case 4:
				View.BackgroundColor = Color.LightGray;
				NavigationController.NavigationBar.BarTintColor = Color.LightGray;
				break;
			case 5:
				View.BackgroundColor = Color.Pink;
				NavigationController.NavigationBar.BarTintColor = Color.Pink;
				break;
			case 6: 
				View.BackgroundColor = Color.Purple;
				NavigationController.NavigationBar.BarTintColor = Color.Purple;
				break;
			};
		}
		public override void ViewDidDisappear (bool animated)
		{
			PlayGroundView.Parent = null;
			base.ViewDidDisappear (animated);
		}

		void HandleARCardsUpdated (object sender, EventArgs e)
		{
			PlayGroundView.UpdateARCards (PlayGroundViewModel.Instance.ActiveARCards);
		}

		public override void LoadView ()
		{
			View = PlayGroundView = new PlayGroundView ();
		}


		public override bool CanBecomeFirstResponder {
			get {
				return true;
			}
		}

		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);
			PlayGroundView.UpdateARCards (PlayGroundViewModel.Instance.ActiveARCards);
		}

	}
}

