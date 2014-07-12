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
		public ARCard[] ARCards { get; set; }
		public PlaygroundViewController ()
		{
			this.Title = "Mario Box";

			ARCards = ARCard.GetAllARCards ();
		}
		public override void ViewWillAppear (bool animated)
		{
			PlayGroundView.Parent = this;
			PlayGroundView.UpdateARCards (ARCards);
			base.ViewWillAppear (animated);
			this.BecomeFirstResponder ();
		}
		public override void ViewDidDisappear (bool animated)
		{
			PlayGroundView.Parent = null;
			base.ViewDidDisappear (animated);
		}

		void HandleARCardsUpdated (object sender, EventArgs e)
		{
			PlayGroundView.UpdateARCards (ARCards);
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

	}

	public class PlayGroundView : UIView
	{
		UIPinchGestureRecognizer pinchGesture;
		public PlaygroundViewController Parent;
		public PlayGroundView ()
		{
			pinchGesture = new UIPinchGestureRecognizer (Scale);
			this.AddGestureRecognizer (pinchGesture);

			var rotationGesture = new UIRotationGestureRecognizer (Rotate);
			this.AddGestureRecognizer (rotationGesture);

			var panGesture = new UIPanGestureRecognizer (Move);
			this.AddGestureRecognizer (panGesture);

			this.BackgroundColor = UIColor.DarkGray;
		}
		Dictionary<ARCard, ARCardView> ARCardDictionary = new Dictionary<ARCard, ARCardView> ();
		public void UpdateARCards(ARCard[] arcards)
		{
			UIView.BeginAnimations ("arcards");
			for(int i = 0; i < arcards.Length; i ++){
				ARCard arcard = arcards[i];
				ARCardView view;
				ARCardDictionary.TryGetValue(arcard,out view);
				if (view == null){
					view = new ARCardView (arcard);
					ARCardDictionary.Add(arcard,view);
				}
				view.Update (arcard, this.Bounds);
				this.InsertSubview(view,i);
			}
			UIView.CommitAnimations ();
		}

		ARCardView currentARCard;

		public ARCardView CurrentARCard {
			get {
				if (currentARCard == null)
					currentARCard = Subviews.FirstOrDefault () as ARCardView;
				return currentARCard;
			}
			set {
				if (currentARCard == value)
					return;
				currentARCard = value;
				this.BringSubviewToFront (currentARCard);
				for (int i = 0; i < Subviews.Length; i ++) {
					var view = Subviews [i] as ARCardView;
					view.ARCard.Z = i;
				}
			}
		}

		float lastScale = 1f;

		void Scale (UIPinchGestureRecognizer gesture)
		{
			if (CurrentARCard == null)
				return;
			if (gesture.State == UIGestureRecognizerState.Began)
				lastScale = 1f;
			var scale = 1f - (lastScale - gesture.Scale);

			var transform = CurrentARCard.Transform;
			transform.Scale (scale, scale);
			CurrentARCard.Transform = transform;

			lastScale = gesture.Scale;
			if(gesture.State == UIGestureRecognizerState.Ended)
			{
				CurrentARCard.UpdateARCard(0,this.Bounds);
			}
		}

		float lastRotation = 0f;

		void Rotate (UIRotationGestureRecognizer gesture)
		{
			if (CurrentARCard == null)
				return;
			if (gesture.State == UIGestureRecognizerState.Ended) {
				lastRotation = 0;
				CurrentARCard.UpdateARCard(0,this.Bounds);
				return;
			}
			var rotation = 0 - (lastRotation - gesture.Rotation);
			var transform = CurrentARCard.Transform;
			transform.Rotate (rotation);
			CurrentARCard.Transform = transform;

			lastRotation = gesture.Rotation;
		}

		PointF initialPoint;

		void Move (UIPanGestureRecognizer gesture)
		{
			if (CurrentARCard == null)
				return;
			var point = gesture.TranslationInView (this);

			if (gesture.State == UIGestureRecognizerState.Began)
				initialPoint = CurrentARCard.Center;

			point.X += initialPoint.X;
			point.Y += initialPoint.Y;

			CurrentARCard.Center = point;
			if(gesture.State == UIGestureRecognizerState.Ended)
			{
				CurrentARCard.UpdateARCard(0,this.Bounds);
			}
		}
	}
}

