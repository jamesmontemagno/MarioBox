using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace MarioBox
{
	public class PlayGroundViewModel
	{
		static PlayGroundViewModel viewModel;
		public static PlayGroundViewModel Instance
		{
			get { return viewModel ?? (viewModel = new PlayGroundViewModel ()); }
		}

		public Dictionary<ARCard, ARCardView> ARCardDictionary { get; set; }

		public PlayGroundViewModel ()
		{
			ARCardDictionary = new Dictionary<ARCard, ARCardView> ();
			LoadDefault ();
		}

		public void LoadDefault()
		{
			try
			{
				var documents =
					Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
				var filename = Path.Combine (documents, "defaults.json");
				var json = File.ReadAllText(filename);
				var items = JsonConvert.DeserializeObject<ARCard[]>(json);
				foreach(var item in items)
					AddCard(item);
			}
			catch(Exception ex) {
			}

			if(ActiveARCards.Length == 0)
				AddCard ("Mario");

		}

		public void SaveDefault()
		{
			try
			{

				var documents =
					Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
				var filename = Path.Combine (documents, "defaults.json");
				File.WriteAllText(filename, JsonConvert.SerializeObject(ActiveARCards));
			} catch(Exception ex) {
			}
		}

		public void AddCard(ARCard card)
		{
			ARCardDictionary.Add (card, null);
		}

		public void AddCard(string name)
		{
			ARCardDictionary.Add (new ARCard{ Name = name }, null);
		}

		public void RemoveCardView(ARCard arcard)
		{
			ARCardDictionary.Remove (arcard);
		}

		private bool editMode;
		public bool EditMode 
		{
			get { return editMode; }
			set {
				editMode = value; 
				foreach (var cardAndView in ARCardDictionary) {
					if (cardAndView.Value != null)
						cardAndView.Value.EditMode = editMode;
				}
			}
		}

		public ARCardView AddOrGetCardView(ARCard arcard)
		{
			ARCardView view;
			ARCardDictionary.TryGetValue(arcard, out view);
			if (view == null){
				view = new ARCardView (arcard);
				if (ARCardDictionary.ContainsKey (arcard))
					ARCardDictionary [arcard] = view;
				else
					ARCardDictionary.Add(arcard, view);
			}

			return view;
		}

		public ARCard[] ActiveARCards
		{
			get {
				return ARCardDictionary.Keys.ToArray ();
			}
		}



		public static string[] AllARCards
		{
			get{
				return new [] {
					"Bowser",
					"Goomba",
					"Koopa Troopa",
					"Luigi",
					"Mario",
					"Peach", 
					"Question Mark"
				};
			}
		}
	}
}

