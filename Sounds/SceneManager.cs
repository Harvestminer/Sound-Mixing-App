using System;
using System.Collections.ObjectModel;
using Windows.Media.Playback;

namespace SL
{
	internal class SceneManager
	{
		private static SceneManager _instance;
		public static SceneManager instance
		{
			get
			{
				if (_instance == null)
					throw new Exception("SceneManager instance was null.");

				return _instance;
			}
		}

		public Scene CurrentScene { get; set; } = null;

		public ObservableCollection<Scene> Scenes { get; set; } = new();
		public MediaPlayer MediaPlayer { get; } = new();

		public SceneManager() => _instance = this;
	}
}
