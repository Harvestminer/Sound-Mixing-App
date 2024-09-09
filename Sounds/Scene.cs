using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SL
{
	internal class Scene
	{
		public string Name { get; }
		public ObservableCollection<SoundInfo> Sounds { get; set; } = new();

		[JsonIgnore] public bool IsPlaying { get; private set; } = false;
		[JsonIgnore] public AppBarButton PlayBtn;

		public Scene(string Name)
		{
			// Make sure the name is always populated with something
			this.Name = string.IsNullOrEmpty(Name) ? "N/A" : Name;
		}

		public void PlaySounds(object sender, RoutedEventArgs e)
		{
			if (Sounds.Count == 0)
				return;

			if (this.PlayBtn == null)
				this.PlayBtn = ((AppBarButton)sender);

			foreach (var sound in Sounds)
			{
				if (!IsPlaying)
					sound.Player.Play();
				else
					sound.Player.Pause();
			}

			this.IsPlaying = !this.IsPlaying;

			// Change the icon of the play btn to pause
			((AppBarButton)sender).Icon = this.IsPlaying ? new SymbolIcon(Symbol.Pause) : new SymbolIcon(Symbol.Play);
		}

		public void AddSound()
		{
			PickSoundFileAsync();
		}

		public void RemoveSound(SoundInfo sound)
		{
			sound.Player.Dispose();
			sound.MediaSource.Dispose();

			this.Sounds.Remove(sound);

			// Make sure that when we get rid of a sound that it changes the playBtn to play
			if (this.PlayBtn != null && this.IsPlaying && Sounds.Count == 0)
			{
				this.PlayBtn.Icon = new SymbolIcon(Symbol.Play);
				this.IsPlaying = false;
			}

			Save();
		}

		public void DeleteScene()
		{
			// Dispose of data associtated with sounds
			foreach (var sound in this.Sounds)
			{
				sound.MediaSource.Dispose();
				sound.Player.Dispose();
			}

			// Remove all data associtated with the scene
			this.Sounds.Clear();

			// Remove scene from scenemanager
			SceneManager.instance.Scenes.Remove(this);

			SaveSystem.DeleteSave(this);
		}

		/// <summary>
		/// Get a sound file from computer and add data to program
		/// </summary>
		private async void PickSoundFileAsync()
		{
			FileOpenPicker picker = new()
			{
				SuggestedStartLocation = PickerLocationId.MusicLibrary,
				FileTypeFilter = { ".mp3", ".wav" },
			};

			WinRT.Interop.InitializeWithWindow.Initialize(picker, System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);

			StorageFile file = await picker.PickSingleFileAsync();

			if (file == null)
				return;

			// Create data from file
			SoundInfo sound = new()
			{
				FilePath = file.Path,
				Title = file.Name
			};

			sound.InitializeMediaSourceAsync(file.Path);

			// Add data to lists
			this.Sounds.Add(sound);

			if (IsPlaying)
				sound.Player.Play();

			Save();
		}

		public void Save() => SaveSystem.SaveScene(this);
	}
}
