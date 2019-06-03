# SpotifyControl
Spotify is great, but sometimes I want a quick-access widget to display the track and skip tracks without having to mouseover the window (Or use FN keys on my laptop). 
This widget displays on top of all windows (Including the taskbar).

Note that this isn't a fancy app that utilises the Web API for spotify etc , it's simply an "always visible" play/skip/display bar for windows.

![Screenshot](http://yer.ac/hosted/gh_spotifycontrol/screenshot.png)

This is a fairly dodgy way of displaying a small desktop widget that I made about almost 5 years ago and shockingly still works (Although you have to open spotify first with the Windows Store version). It works by monitoring the window status of Spotify and sending various keyboard shortcuts.

Simply open the SLN file, build it and run it. It will open a widget which has Skip Bwd, Skip Fwd and a pause/play. Some placeholders for mute and volume commands, but I didn't implement as wasn't required for my purpose.


Please don't judge the code! It was literally plumbed together 5 years ago over lunch - hence the graphics etc. Feel free to enhance!

