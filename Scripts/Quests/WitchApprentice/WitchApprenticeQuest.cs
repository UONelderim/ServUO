using Server.Mobiles;
using System;

namespace Server.Engines.Quests.Hag
{
    public class WitchApprenticeQuest : QuestSystem
    {
	    private static Point3D[] m_ZeefzorpulLocations = new Point3D[]
	    {
		    new Point3D( 1191, 1598, 6 ),
		    new Point3D( 957, 1425, 40 ),
		    new Point3D( 641, 1841, 40 ),
		    new Point3D( 994, 1106, 0 ),
		    new Point3D( 826, 1553, 2 ),
		    new Point3D( 268, 1547, 0 )
	    };
        public WitchApprenticeQuest(PlayerMobile from)
            : base(from)
        {
        }

        // Serialization
        public WitchApprenticeQuest()
        {
        }

        public override object Name =>
                // "The Witch's Apprentice"
                1055042;
        public override object OfferMessage =>
                /* <I>The ancient, wrinkled hag looks up from her vile-smelling cauldron.
* Her single, unblinking eye attempts to focus in on you, but to
* little avail.</I><BR><BR>
* 
* Eh? Who is it? Who's there?  Come to trouble an old woman have you?<BR><BR>
* 
* I'll split ye open and swallow yer guts!  I'll turn ye into a pile
* o' goo, I will!  Bah!  As if I didn't have enough to worry about.  As if I've
* not enough trouble as it is!<BR><BR>
* 
* Another of my blasted apprentices has gone missing!  Foolish children,
* think they know everything.  I should turn the lot of them into toads -
* if only they'd return with their task complete!  But that's the trouble, innit?
* They never return!<BR><BR>
* 
* But you don't care, do ye?  I suppose you're another one of those meddlesome kids,
* come to ask me for something?  Eh?  Is that it?  You want something from me,
* expect me to hand it over?  I've enough troubles with my apprentices, and that
* vile imp, Zeefzorpul!  Why, I bet it's him who's got the lot of them!  And who
* knows what he's done?  Vile little thing.<BR><BR>
* 
* If you expect me to help you with your silly little desires, you'll be doing
* something for me first, eh?  I expect you to go seek out my apprentice.
* I sent him along the road west of here up towards Yew's graveyard, but he never
* came back. Find him, and bring him back, and I'll give you a little reward that
* I'm sure you'll find pleasant.<BR><BR>
* 
* But I tells ye to watch out for the imp name've Zeefzorpul!  He's a despicable
* little beast who likes to fool and fiddle with folk and generally make life
* miserable for everyone.  If ye get him on your bad side, you're sure to end up
* ruing the day ye were born. As if you didn't already, with an ugly mug
* like that!<BR><BR>
* 
* Well, you little whelp?  Going to help an old hag or not?
*/
                1055001;
        public override TimeSpan RestartDelay => TimeSpan.FromMinutes(5.0);
        public override bool IsTutorial => false;
        public override int Picture => 0x15D3;
        public static Point3D RandomZeefzorpulLocation()
        {
            int index = Utility.Random(m_ZeefzorpulLocations.Length);

            return m_ZeefzorpulLocations[index];
        }

        public override void Accept()
        {
            base.Accept();

            AddConversation(new AcceptConversation());
        }
    }
}
