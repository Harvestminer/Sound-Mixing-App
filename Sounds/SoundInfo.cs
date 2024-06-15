using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace SL
{
	internal class SoundInfo : INotifyPropertyChanged
	{
		public MediaSource MediaSource { get; }
		public MediaPlayer Player { get; }
		public StorageFile File { get; }
		public string Title { get; }
		public string SoundType { get; }

		public SoundInfo(StorageFile file, MediaSource mediaSource, MediaPlayer player, string title, string soundtype)
		{
			this.File = file;
			this.MediaSource = mediaSource;
			this.Player = player;
			this.Title = title;
			this.SoundType = soundtype;

			player.Source = mediaSource;
			player.IsLoopingEnabled = true;
		}

		public void VolumeChange(object sender, Microsoft.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
		{
			Player.Volume = ((Slider)sender).Value / 100d;
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
