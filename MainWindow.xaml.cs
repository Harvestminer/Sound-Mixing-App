using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SL
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		private static MainWindow _instance;
		public static MainWindow instance
		{
			get
			{
				if (_instance == null)
					throw new Exception("SceneManager instance was null.");

				return _instance;
			}
		}

		public MainWindow()
		{
			this.InitializeComponent();
			_instance = this;

			SceneGridView.ItemsSource = SceneManager.instance.Scenes;
		}

		private void AddScene(string name)
		{
			if (SceneManager.instance.Scenes.Where(x => x.Name == name).SingleOrDefault() != null)
				return;

			SceneManager.instance.Scenes.Add(new Scene(name));

			sceneNameField.Text = string.Empty;
		}

		#region Events
		private void newSceneBtn_Click(object sender, RoutedEventArgs e) => AddScene(sceneNameField.Text);
		private void newSceneField_KeyDown(object sender, KeyRoutedEventArgs e)
		{
			if (e.Key == Windows.System.VirtualKey.Enter)
				AddScene(sceneNameField.Text);
		}

		private void sceneSelectionChanged(object sender, RoutedEventArgs e)
		{
			SceneManager.instance.CurrentScene = (Scene)SceneGridView.SelectedItem;
			SoundGridView.ItemsSource = SceneManager.instance.CurrentScene?.Sounds;
		}

		private void ListViewSwipeContainer_PointerEntered(object sender, PointerRoutedEventArgs e)
		{
			if (e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse || e.Pointer.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Pen)
			{
				VisualStateManager.GoToState(sender as Control, "HoverButtonsShown", true);
			}
		}
		private void ListViewSwipeContainer_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			VisualStateManager.GoToState(sender as Control, "HoverButtonsHidden", true);
		}
		#endregion // Events
	}
}
