using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyControl
{
  public enum SpotifyAction : long
  {
    PlayPause = 917504,
    Mute = 524288,
    VolumeDown = 589824,
    VolumeUp = 655360,
    Stop = 851968,
    PreviousTrack = 786432,
    NextTrack = 720896,
  }
}
