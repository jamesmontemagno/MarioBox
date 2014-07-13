using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace MarioBox
{
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

		public void UpdateARCards(ARCard[] arcards)
		{
			UIView.BeginAnimations ("arcards");
			for (int i = 0; i < arcards.Length; i++) {
				ARCard arcard = arcards [i];
				var view = PlayGroundViewModel.Instance.AddOrGetCardView (arcard);
				view.EditMode = PlayGroundViewModel.Instance.EditMode;

				view.Update (arcard, this.Bounds);
				this.InsertSubview (view, i);
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

			if (scale < .4)
				scale = .4f;
			else if (scale > 3)
				scale = 3.0f;

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

