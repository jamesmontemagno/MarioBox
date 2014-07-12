using System;
using System.Collections.Generic;

namespace MarioBox
{
	public class ARCard
	{
		static Random random = new Random();
		public ARCard ()
		{
			Scale = (float)Math.Max(random.NextDouble(),.5);
			X = (float)random.NextDouble();
			Y = (float)random.NextDouble();
		}
		public string Name {get;set;}
		public float Rotation {get;set;}
		public float Scale {get;set;}
		public float X {get;set;}
		public float Y {get;set;}
		public int Z {get;set;}


		public static ARCard[] GetAllARCards()
		{
			return new ARCard[] {
				new ARCard{
					Name = "bowser",
				},
				new ARCard{
					Name = "goomba",
				},
				new ARCard {
					Name = "koopatroopa",
				},
				new ARCard {
					Name = "luigi",
				},
				new ARCard {
					Name = "mario",
				},
				new ARCard {
					Name = "peach",
				}
			};
		}
	}
}

