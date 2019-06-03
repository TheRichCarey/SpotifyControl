using System.Windows;

namespace SpotifyControl
{
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Runtime.InteropServices;
  using System.Threading;

  public class Spotify
  {
    /// <summary>
    /// The spotify process.
    /// </summary>
    private Process _spotifyProcess;


    /// <summary>
    /// Initializes a new instance of the <see cref="Spotify"/> class.
    /// </summary>
    public Spotify()
    {
      if (!IsSpotifyWindowLocated())
      {
        // Try to launch it.
        try
        {
          ProcessStartInfo startInfo = new ProcessStartInfo
          {
            FileName = Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Spotify\Spotify.exe"),
            WindowStyle = ProcessWindowStyle.Minimized
          };
          Process proc = Process.Start(startInfo);
          Thread.Sleep(500);
          if (!IsSpotifyWindowLocated())
          {
            MessageBox.Show("Spotify could not be located. Try launching spotify manually first. \n If you are using the Windows Store version you will have to start it before launching this.");
          }
        }
        catch (Exception)
        {
          // Throw an error
          throw new Exception("Spotify not found.");
        }
      }
      // If we get here then we have a valid window.
    }

    /// <summary>
    /// get current track that is playing
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string GetCurrentTrack()
    {
      _spotifyProcess.Refresh();
      return _spotifyProcess.MainWindowTitle;
    }


    public void PlayPause()
    {
      SendCommand(SpotifyAction.PlayPause);
    }

    public void Stop()
    {
      SendCommand(SpotifyAction.Stop);
    }

    public void NextTrack()
    {
      SendCommand(SpotifyAction.NextTrack);
    }

    public void PrevTrack()
    {
      SendCommand(SpotifyAction.PreviousTrack);
    }

    public void Mute()
    {
      SendCommand(SpotifyAction.Mute);
    }

    /// <summary>
    /// tries to refresh the process. If it fails, assume spotify is no longer running
    /// </summary>
    /// <exception cref="Exception">
    /// Any unexpected result
    /// </exception>
    /// <returns>
    /// The <see cref="bool"/> status.
    /// </returns>
    public bool IsRunning()
    {
      try
      {
        _spotifyProcess.Refresh();
        if (_spotifyProcess.HasExited)
        {
          return false;
        }

        return true;
      }
      catch
      {
        throw new Exception("Unexpected Error");
      }
    }

    /// <summary>
    /// The find spotify window. method. Returns status
    /// </summary>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    private bool IsSpotifyWindowLocated()
    {
      Process[] processlist = Process.GetProcesses();

      foreach (Process process in processlist)
      {
        if (!string.IsNullOrEmpty(process.MainWindowTitle))
        {
          if (process.ProcessName.Contains("Spotify"))
          {
            _spotifyProcess = process;
            return true;
          }
        }
      }
      return false;
    }


    private void SendCommand(SpotifyAction Code)
    {
      SendMessage(_spotifyProcess.MainWindowHandle, 0x0319, new IntPtr(0), new IntPtr((int)Code));
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
  }
}
