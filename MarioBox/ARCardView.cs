using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;

namespace MarioBox
{
	public class ARCardView : UIControl
	{
		UIImageView image;
		public ARCard ARCard { get; private set; }
		public ARCardView (ARCard card)
		{
			ARCard = card;
			image = new UIImageView (UIImage.FromBundle (card.Name));
			this.AddSubview (image);
			this.Frame = image.Frame;
		}

		public PlayGroundView CurrentPlayground {get;set;}

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

