using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Linq;

namespace SL
{
	internal static class SaveSystem
	{
		public static readonly string SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SoundLayering", "Scenes");

		public static void SaveScene<T>(T scene) where T : Scene
		{
			string json = JsonConvert.SerializeObject(scene, Formatting.Indented);

			char[] invalidChars = Path.GetInvalidFileNameChars();

			string name = scene.Name;
			foreach (char c in invalidChars)
				name = name.Replace(c, '_');
			name = name.Trim();

			if (!Directory.Exists(SavePath))
				Directory.CreateDirectory(SavePath);

			File.WriteAllText(Path.Combine(SavePath, name), json);
		}

		public static T LoadScene<T>(string file) where T : Scene
		{
			if (!Directory.Exists(SavePath))
				return null;

			string fileName = Path.Combine(SavePath, file);
			if (File.Exists(fileName))
			{
				string json = File.ReadAllText(fileName);
				T obj = JsonConvert.DeserializeObject<T>(json);

				foreach (var sound in (obj as Scene).Sounds)
				{
					sound.InitializeMediaSourceAsync(sound.FilePath);
				}

				return obj;
			}

			return null;
		}

		public static void DeleteSave<T>(T scene) where T : Scene
		{
			if (!Directory.Exists(SavePath))
				return;

			char[] invalidChars = Path.GetInvalidFileNameChars();

			string name = scene.Name;
			foreach (char c in invalidChars)
				name = name.Replace(c, '_');
			name = name.Trim();

			string fileName = Path.Combine(SavePath, name);

			if (File.Exists(fileName))
				File.Delete(fileName);
		}
	}
}
