using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
  using System.Diagnostics;
  using System.Threading;

  class Program
  {

    static void Main(string[] args)
    {
      var s = new Spotify();
      string track = "";
      while (true)
      {

        var t = s.GetCurrentTrack();
        if (t != track)
        {
          track = t;
          Console.WriteLine("Track: " + track);

          //Thread.Sleep(2500);

         

        }
        var r = Console.ReadLine();
        if (r == "<")
          s.PrevTrack();
        if (r == ">")
          s.NextTrack();

      }





    }




  }




}
