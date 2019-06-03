using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
  using System.Diagnostics;
  using System.Runtime.InteropServices;

  public class Spotify
  {
    /// <summary>
    /// The spotify process.
    /// </summary>
    private Process SpotifyProcess;


    /// <summary>
    /// Initializes a new instance of the <see cref="Spotify"/> class.
    /// </summary>
    public Spotify()
    {
      if (!this.FindSpotifyWindow())
      {
        // Throw an error
        throw new Exception("Spotify Not Found");
      }


      // If we get here then we have a valid window.
    }

    /// <summary>
    /// The get current track that is playing
    /// </summary>
    /// <returns>
    /// The <see cref="string"/>.
    /// </returns>
    public string GetCurrentTrack()
    {
      this.SpotifyProcess.Refresh();
      return this.SpotifyProcess.MainWindowTitle;
    }


    public void PlayPause()
    {
      this.SendCommand(SpotifyAction.PlayPause);
    }

    public void Stop()
    {
      this.SendCommand(SpotifyAction.Stop);
    }

    public void NextTrack()
    {
      this.SendCommand(SpotifyAction.NextTrack);
    }

    public void PrevTrack()
    {
      this.SendCommand(SpotifyAction.PreviousTrack);
    }

    public void Mute()
    {
      this.SendCommand(SpotifyAction.Mute);
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
        this.SpotifyProcess.Refresh();
        if (this.SpotifyProcess.HasExited)
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
    private bool FindSpotifyWindow()
    {
      Process[] processlist = Process.GetProcesses();

      foreach (Process process in processlist)
      {
        if (!String.IsNullOrEmpty(process.MainWindowTitle))
        {
          if (process.ProcessName.Contains("Spotify"))
          {
            this.SpotifyProcess = process;
            return true;
          }
        }
      }
      return false;
    }


    private void SendCommand(SpotifyAction Code)
    {
      SendMessage(this.SpotifyProcess.MainWindowHandle, 0x0319, new IntPtr(0), new IntPtr((int)Code));
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
  }


  public enum SpotifyAction : long
  {
    PlayPause = 917504,
    Mute = 524288,
    VolumeDown = 589824,
    VolumeUp = 655360,
    Stop = 851968,
    PreviousTrack = 786432,
    NextTrack = 720896
  }
}
