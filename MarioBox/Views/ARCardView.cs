using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace MarioBox
{
	public class ARCardView : UIControl
	{
		UIImageView image;
		UIButton deleteButton;
		public ARCard ARCard { get; private set; }
		public Action<ARCardView> RemoveCard;
		public ARCardView (ARCard card)
		{
			ARCard = card;
			image = new UIImageView (UIImage.FromBundle (card.Name));
			deleteButton = UIButton.FromType (UIButtonType.RoundedRect);
			deleteButton.Frame = new RectangleF (0, 0, 44, 44);
			deleteButton.BackgroundColor = UIColor.Red;
			deleteButton.Hidden = true;
			deleteButton.SetImage (UIImage.FromBundle ("delete"), UIControlState.Normal);
			deleteButton.TintColor = UIColor.White;

			deleteButton.TouchUpInside += (object sender, EventArgs e) => 
			{
				PlayGroundViewModel.Instance.RemoveCardView(ARCard);
				UIView.BeginAnimations ("arcards");

				this.RemoveFromSuperview ();

				UIView.CommitAnimations ();
			};

			this.AddSubview (image);
			this.AddSubview (deleteButton);
			this.Frame = image.Frame;
		}

		public PlayGroundView CurrentPlayground {get;set;}

		public bool EditMode
		{
			get {
				return !deleteButton.Hidden;
			}
			set{
				deleteButton.Hidden = !value;
			}
		}

		public override void MovedToSuperview ()
		{
			base.MovedToSuperview ();
			if (this.Superview is PlayGroundView)
				CurrentPlayground = (PlayGroundView)this.Superview;
		}
		public override void TouchesBegan (MonoTouch.Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);
			CurrentPlayground.CurrentARCard = this;
		}

		public void Update(ARCard card, RectangleF bounds)
		{
			var transform = CGAffineTransform.MakeIdentity ();
			transform.Rotate (card.Rotation);
			transform.Scale (card.Scale, card.Scale);
			this.Transform = transform;

			var transform2 = CGAffineTransform.MakeIdentity ();
			transform2.Rotate (card.Rotation);
			transform2.Scale (1 + (1 - card.Scale), 1 + (1 - card.Scale));

			deleteButton.Transform = transform2;

			var x = bounds.Width * card.X;
			var y = bounds.Height * card.Y;
			this.Center = new PointF (x, y);

		}
		public void UpdateARCard(int Z, RectangleF bounds)
		{
			ARCard.X = Center.X / bounds.Width;
			ARCard.Y = Center.Y / bounds.Height;


			ARCard.Scale = Transform.GetScale ();
			ARCard.Rotation = Transform.GetRotation ();
		}
	}
}

