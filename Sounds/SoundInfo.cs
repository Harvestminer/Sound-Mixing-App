using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace SL
{
	internal class SoundInfo : INotifyPropertyChanged
	{
		public string Title { get; set; } = "N/A";
		public string FilePath { get; set; }
		public double Volume { get; set; } = 1d;

		[JsonIgnore] public MediaSource MediaSource { get; set; }
		[JsonIgnore] public MediaPlayer Player { get; }

		public SoundInfo()
		{
			this.Player = new();
			this.Player.IsLoopingEnabled = true;
		}

		public async Task InitializeMediaSourceAsync(string path)
		{
			StorageFile file = await StorageFile.GetFileFromPathAsync(path);

			if (file != null)
			{
				MediaSource = MediaSource.CreateFromStorageFile(file);
				Player.Source = MediaSource;
				FilePath = file.Path; // Optionally, store the file path
			}
			else
			{
				throw new InvalidOperationException("StorageFile is null.");
			}
		}

		public void VolumeChange(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
		{
			Player.Volume = ((Slider)sender).Value / 100d;
			Volume = Player.Volume;
		}

		public void SliderLoaded(object sender, RoutedEventArgs e)
		{
			if (SceneManager.instance.CurrentScene != null && SceneManager.instance.CurrentScene.Sounds.Contains(this))
				((Slider)sender).Value = this.Player.Volume * 100d;
		}

		public void DeleteSound()
		{
			SceneManager.instance.CurrentScene.RemoveSound(this);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
