using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Spells.SkillMasteries;
using System;

namespace Server.Engines.Quests.TimeLord
{
    public class TimeForLegendsQuest : QuestSystem
    {
        public override object Name => 1156338;  		// A Time For Legends
        public override object OfferMessage => 1156339; 	/*Greetings Brave Traveler!<br><br>Throughout my travels in time I have forever 
																		  encountered those who've reached the pinnacle of their profession.  These Legends 
																		  are told of in our most cherished tales and are the fabric by which heroes are born.
																		  I offer you now a chance to walk the path of these Legends.  Complete my task and 
																		  you too shall know the secrets of the Master.*/

        public override TimeSpan RestartDelay => TimeSpan.Zero;
        public override bool IsTutorial => false;
        public override int Picture => 0x1581;

        public SkillName Mastery { get; set; }
        public Type ToSlay { get; set; }

        /// <summary>
        /// Constant for the localized message indicating player already has this mastery
        /// If there's no existing message ID, you can define a custom one
        /// </summary>
        public const int AlreadyMasteredMessage = 1158721; // You have already learned this mastery. You cannot receive this quest again.

        public TimeForLegendsQuest(PlayerMobile from)
            : base(from)
        {
        }

        public TimeForLegendsQuest()
        {
        }

        /// <summary>
        /// Checks if the player has already learned a specific mastery
        /// </summary>
        /// <param name="from">The player to check</param>
        /// <param name="skill">The mastery skill to verify</param>
        /// <returns>True if the player has already learned the mastery</returns>
        public static bool HasLearnedMastery(PlayerMobile from, SkillName skill)
        {
            if (from == null)
                return false;

            Skill sk = from.Skills[skill];
            
            // If the skill exists, is a mastery, and has been learned (VolumeLearned > 0)
            return sk != null && sk.IsMastery && sk.VolumeLearned > 0;
        }

        public override void Accept()
        {
            base.Accept();

            From.SendGump(new ChooseMasteryGump(From, this));
        }

        public override void Complete()
        {
            base.Complete();

            From.SendLocalizedMessage(1156342); // You have proven your prowess on the battlefield and have completed the first step on the path of the Master!
            From.SendLocalizedMessage(1156209); // You have received a mastery primer!
            From.SendLocalizedMessage(1152339, "#1028794"); // A reward of ~1_ITEM~ has been placed in your backpack.

            From.AddToBackpack(new BookOfMasteries());

            SkillMasteryPrimer primer = new SkillMasteryPrimer(Mastery, 1);

            From.AddToBackpack(primer);
        }

        public static Type[] Targets => _Targets;
        private static readonly Type[] _Targets =
        {
            typeof(Semidar), typeof(Mephitis), typeof(Rikktor), typeof(LordOaks), typeof(Neira), typeof(Barracoon), typeof(Serado), typeof(Meraktus), typeof(Ilhenir),
            typeof(Twaulo),
            // typeof(AbyssalInfernal), typeof(PrimevalLich), typeof(CorgulTheSoulBinder), typeof(CorgulTheSoulBinder) /*dragon turtle*/ //odkomentowac, gdy juz bedziemy mieli champy SA/HS
            typeof(DreadHorn), typeof(Travesty), typeof(ChiefParoxysmus), typeof(LadyMelisande), typeof(MonstrousInterredGrizzle), typeof(ShimmeringEffusion)
        };

        public static Type TargetOfTheDay { get; set; }
        public static DateTime NextTarget { get; set; }

        public static void OnSave(WorldSaveEventArgs e)
        {
	        Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
	        {
		        if (DateTime.UtcNow > NextTarget)
		        {
			        NextTarget = DateTime.UtcNow + TimeSpan.FromHours(24);
			        TargetOfTheDay = _Targets[Utility.Random(_Targets.Length)];
		        }
	        });
        }

        public static void Initialize()
        {
            EventSink.WorldSave += OnSave;

            TargetOfTheDay = _Targets[Utility.Random(_Targets.Length)];

            Commands.CommandSystem.Register("NewTargetOfTheDay", AccessLevel.GameMaster, e =>
                {
                    TargetOfTheDay = _Targets[Utility.Random(_Targets.Length)];

                    e.Mobile.SendMessage("Cel nowego dnia: {0}", TargetOfTheDay.Name);
                });
        }

        public override void ChildSerialize(GenericWriter writer)
        {
            writer.WriteEncodedInt(0); // version

            writer.Write((int)Mastery);

            if (ToSlay != null)
            {
                writer.Write(0);
                writer.Write(ToSlay.Name);
            }
            else
                writer.Write(1);
        }

        public override void ChildDeserialize(GenericReader reader)
        {
            int version = reader.ReadEncodedInt();

            Mastery = (SkillName)reader.ReadInt();

            string name = null;

            if (reader.ReadInt() == 0)
                name = reader.ReadString();

            if (name != null)
                ToSlay = ScriptCompiler.FindTypeByName(name);
        }
    }

    public class ChooseMasteryGump : Gump
    {
        public TimeForLegendsQuest Quest { get; set; }
        public PlayerMobile User { get; set; }

        public ChooseMasteryGump(PlayerMobile user, TimeForLegendsQuest quest)
            : base(50, 50)
        {
            Quest = quest;
            User = user;

            AddImage(0, 0, 8000);
            AddImage(20, 37, 8001);
            AddImage(20, 107, 8002);
            AddImage(20, 177, 8001);
            AddImage(20, 247, 8002);
            AddImage(20, 317, 8001);
            AddImage(20, 387, 8002);
            AddImage(20, 457, 8003);

            AddHtmlLocalized(125, 40, 345, 16, 1156340, false, false); // Choose Your Path

            int y = 60;

            foreach (SkillName skName in MasteryInfo.Skills)
            {
                Skill sk = User.Skills[skName];

                if (sk == null || skName == SkillName.Discordance || skName == SkillName.Provocation || skName == SkillName.Peacemaking)
                    continue;

                // Changed condition to skip masteries that player has already learned
                if (sk.IsMastery && sk.VolumeLearned == 0)
                {
                    AddButton(30, y, 4005, 4007, (int)skName + 1, GumpButtonType.Reply, 0);

                    AddHtmlLocalized(75, y, 200, 16, MasteryInfo.GetLocalization(skName), 0x000F, false, false);

                    y += 20;
                }
            }
        }

        public override void OnResponse(Network.NetState state, RelayInfo info)
        {
            if (info.ButtonID == 0 && User.Quest != null && User.Quest is TimeForLegendsQuest)
                User.Quest.Cancel();

            int id = info.ButtonID - 1;

            if (id >= 0 && id < SkillInfo.Table.Length)
            {
                SkillName selectedMastery = (SkillName)id;
                
                // Check if player has already learned this mastery
                if (TimeForLegendsQuest.HasLearnedMastery(User, selectedMastery))
                {
                    // Inform player they've already learned this mastery
                    User.SendLocalizedMessage(3070067); // Juz otrzymales talent. Nie mozesz go otrzymac ponownie
                    
                    // Cancel the quest
                    if (User.Quest != null && User.Quest is TimeForLegendsQuest)
                        User.Quest.Cancel();
                }
                else
                {
                    // Proceed with the quest as normal
                    Quest.Mastery = selectedMastery;
                    Quest.ToSlay = TimeForLegendsQuest.TargetOfTheDay;
                    Quest.AddObjective(new TimeForLegendsObjective());
                }
            }
        }
    }
}
