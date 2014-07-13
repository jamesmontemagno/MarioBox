using System;

namespace MarioBox
{
	public class ARCard
	{
		Random random = new Random();
		public ARCard ()
		{
			Scale = (float)random.Next(66, 100) / 100.0f;
			X = (float)random.Next(10, 90) / 100.0f;
			Y = (float)random.Next(20, 80) / 100.0f;
		}

		string name = string.Empty;
		public string Name {
			get { return name; }
			set { name = value.Replace (" ", string.Empty); }
		}
		public float Rotation {get;set;}
		public float Scale {get;set;}
		public float X {get;set;}
		public float Y {get;set;}
		public int Z {get;set;}
	}
}

