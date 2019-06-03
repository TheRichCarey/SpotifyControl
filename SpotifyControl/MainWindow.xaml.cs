using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SpotifyControl
{
  using System.Text.RegularExpressions;
  using System.Threading;

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    /// <summary>
    /// The spotify instance
    /// </summary>
    private readonly Spotify _spotify;

    /// <summary>
    /// Is it currently playing or not
    /// </summary>
    public bool IsPlaying;

    /// <summary>
    /// The current/last track.
    /// </summary>
    private string _currentTrack;


    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();

      WindowStartupLocation = WindowStartupLocation.Manual;
      double screenWidth = SystemParameters.PrimaryScreenWidth;
      double screenHeight = SystemParameters.PrimaryScreenHeight;
      Left = screenWidth - 375;
      Top = screenHeight - 93;

      try
      {
        _spotify = new Spotify();
      }
      catch (Exception)
      {
        MessageBox.Show("Spotify Not Found. Exiting");
        Environment.Exit(0);
      }

      // Start polling for changes
      Task.Factory.StartNew(DoPolling);

    }

    /// <summary>
    /// Sets up the polling and fires off every second whilst the player is active.
    /// </summary>
    private void DoPolling()
    {
      while (true)
      {
        if (_spotify.IsRunning())
        {
          // Good!
          Uri uriSource = new Uri(@"/SpotifyControl;component/img/pause.png", UriKind.Relative);
          string track = _spotify.GetCurrentTrack();
          if (track == "Spotify" || track == "Spotify Premium")
          {
            // Paused or not playing
            uriSource = new Uri(@"/SpotifyControl;component/img/play.png", UriKind.Relative);
            IsPlaying = false;
            txtCurrentSong.Dispatcher.BeginInvoke(
                 new Action(() => ImgPlay.Source = new BitmapImage(uriSource)));
          }
          else
          {
            try
            {
              // If we exit spotify during this loop we can crash, just exit nicely
              // The below solves weird behaviour in the app when dragging a song around whilst we are polling
              if (track != "Drag")
              {
                IsPlaying = true;
                if (_currentTrack != null && _currentTrack != track)
                {
                  // Track has changed, don't do anything here at the moment, maybe use later...
                  _currentTrack = track;
                  // Console.WriteLine("Changing" + track);
                }

                string[] trackSplit = Regex.Split(track, " - ");
                string artist = trackSplit[0]; // Artist
                string song = trackSplit[1];   // Track

                txtCurrentSong.Dispatcher.BeginInvoke(
                  new Action(() => txtCurrentSong.Content = song));
                txtCurrentArtist.Dispatcher.BeginInvoke(
                           new Action(() => txtCurrentArtist.Content = artist));
              }

              txtCurrentSong.Dispatcher.BeginInvoke(
                 new Action(() => ImgPlay.Source = new BitmapImage(uriSource)));
            }
            catch (Exception)
            {
              MessageBox.Show("Spotify Isn't responding..");
            }
          }
        }
        else
        {
          MessageBox.Show("Spotify Isn't running. Exiting App.");
          Environment.Exit(0);
        }

        Thread.Sleep(1000);
      }
      // ReSharper disable once FunctionNeverReturns
    }

    /// <summary>
    /// The window mouse down event.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void WindowMouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton == MouseButton.Left)
      {
        DragMove();
      }
    }

    private void MainWindow_OnMouseEnter(object sender, MouseEventArgs e)
    {
      Opacity = 0.9;
    }

    private void MainWindow_OnMouseLeave(object sender, MouseEventArgs e)
    {
      Opacity = 0.5;
    }

    private void WindowDeactivated(object sender, EventArgs e)
    {
      Window window = (Window)sender;
      window.Topmost = true;
    }


    private void PlayBtn_OnClick(object sender, RoutedEventArgs e)
    {

      _spotify.PlayPause();
    }

    private void PrevBtn_OnClick(object sender, RoutedEventArgs e)
    {
      _spotify.PrevTrack();
    }

    private void NextBtn_OnClick(object sender, RoutedEventArgs e)
    {
      _spotify.NextTrack();
    }


    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      Environment.Exit(0);
    }
  }
}
