-- UO MUSIC COMPOSER Released under GPL V3--
-- AUTHOR: PanThar
-- SCRIPT EXPLANATION --

The UO Music Composer script allows both players and admins to create custom harp and lute music for their runUO shard.  

IN ORDER TO SUCCEED AT PLAYING A SONG, YOU MUST HAVE MUSICIANSHIP SKILL, THESE ARE SKILLED ITEMS SO PLEASE KEEP THAT IN MIND BEFORE READING.

Admin:
An admin can created "Pressed" songs, that are their own unique item 6.) , 
An admin can  also use the  MusicComposer class to change any sound effect in runUO where playSong() is called (see bottom of this document for how to do this).

Player:
A player may compose a song using our simple script, that, when finished creates an item that will play that song.

-- SETUP (TESTED ON RUNUO 2.1, Build 4272.35047) --

1.)Extract entire directory into /Scripts/MusicComposer/, 
2.)Start runUO server, and  login as admin.
3.)As an admin type [add HarpTest , and then double click on the item. If you hear a series of harp notes being played its been installed properly!
4.)Now type [add MusicComposerStand , when you double click on this, you will be presented with an interface to make a song.
5)Change default sounds anywhere in runUO:

Here is an example of how to make/override custom sound effects with the harp/lute music.

All you need to do, is find the spots where PlaySound() is used in runUO, you just need to replace that
line of code, with a call to MusicComposer.PlaySong() instead.
Here is an example of how you can change the sound of magic arrow when you cast it.

5a.)Open /Scripts/Spells/First/MagicArrow.cs
5b.)Find:
source.PlaySound( 0x1E5 );

And replace with:
MusicComposer.PlaySong(source, "HD1,HD2,HA1,HA2"); 
//source.PlaySound( 0x1E5 );

5c.)Restart runUO and try magic arrow, you should hear 4 music notes instead of the usual sound.

6.)To create hard copies of songs

6a.)Navigate to /Scripts/MusicComposer/PressedSongs/ and open up HarpTest.cs. 
6b.)At the top of this file you should notice two things.
		private const string Song ="HC1,HCS1,HD1,HDS1,HE1,HF1,HFS1,HG1,HGS1,HA1,HAS1,HB1"
								+ "-,-,-," +
								"HC2,HCS2,HD2,HDS2,HE2,HF2,HFS2,HG2,HGS2,HA2,HAS2,HB2,HC3";
		private const string SongName="Harp Test";
		
		The Song string is the script, and the SongName is "Harp Test"
		
6c.)To create your own music, just "Save As" on HarpTest.cs, and save it as YourFilename.cs, and then do a find/replace on "HarpTest" with "YourFilename"
6d.)Change the Song strings for what you want your item to play, and optionally a song name.
6e.)type [add YourFilename to add your new song to the world

Thats it!


-- SONG SCRIPT EXPLANATION ( HOW TO MAKE MUSIC )--
The script itself to make the music should be very simple for everyone. 

1.)Each note is seperated by a comma, and you may only play 1 note at a time.
2.)The first letter of a note  is H for harp, and L for Lute, and M for the lap harp (what instrument you play).
3.)The next letter is the Note, if the note is a sharp note, be two letters, like FS
4.)The last number just tells us which octive were on starting at middle C, which is the C1 Note.
5.)the LOWEST NOTE is C1, and the HIGHEST note is C3. 
6.)Example: HAS1 would be a harp note with the first A Sharp note.

-- TODO --

I imagine a lot more than just this can be done, but this is a start. Hope to see people creating new songs/customizing sounds!



