using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Engines.Quests
{
    public class TheRudimentsOfSelfDefenseQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        /* The Rudiments of Self Defense */
        public override object Title => 1077609;

        /* Head East out of town and go to Old Haven. Battle monster there until you have raised your Wrestling skill to 50.
        Listen up! If you want to learn the rudiments of self-defense, you need toughening up, and there's no better way to 
        toughen up than engaging in combat. Head East out of town to Old Haven and battle the undead there in hand to hand 
        combat. Afraid of dying, you say? Well, you should be! Being an adventurer isn't a bed of posies, or roses, or however 
        that saying goes. If you take a dirt nap, go to one of the nearby wandering healers and they'll get you back on your feet.
        Come back to me once you feel that you are worthy of the rank Apprentice Wrestler and i will reward you wit a prize. */
        public override object Description => 1077610;

        /* Ok, featherweight. come back to me if you want to learn the rudiments of self-defense. */
        public override object Refuse => 1077611;

        /* You have not achived the rank of Apprentice Wrestler. Come back to me once you feel that you are worthy of the rank A
        pprentice Wrestler and i will reward you with something useful. */
        public override object Uncomplete => 1077630;

        /* It's about time! Looks like you managed to make it through your self-defense training. As i promised, here's a little 
        something for you. When worn, these Gloves of Safeguarding will increase your awareness and resistances to most elements 
        except poison. Oh yeah, they also increase your natural health regeneration aswell. Pretty handy gloves, indeed. Oh, if you 
        are wondering if your meditation will be hinered while wearing these gloves, it won't be. Mages can wear cloth and leather 
        items without needing to worry about that. Now get out of here and make something of yourself. */
        public override object Complete => 1077613;

        public TheRudimentsOfSelfDefenseQuest()
            : base()
        {
            AddObjective(new ApprenticeObjective(SkillName.Wrestling, 50, "Trening w Starym haven", 1077492, 1077586));

            // 1077492 Your Wrestling potential is greatly enhanced while questing in this area.
            // 1077586 You are not in the quest area for Apprentice Wrestler. Your Wrestling potential is not enhanced here.

            AddReward(new BaseReward(typeof(GlovesOfSafeguarding), 1077614));
        }

        public override bool CanOffer()
        {
            #region Scroll of Alacrity
            PlayerMobile pm = Owner as PlayerMobile;
            if (pm.AcceleratedStart > DateTime.UtcNow)
            {
                Owner.SendLocalizedMessage(1077951); // You are already under the effect of an accelerated skillgain scroll.
                return false;
            }
            #endregion
            else
                return Owner.Skills.Wrestling.Base < 50;
        }

        public override void OnCompleted()
        {
            Owner.SendLocalizedMessage(1077612, null, 0x23); // You have achieved the rank of Apprentice Wrestler. Return to Dimethro in New Haven to receive your prize.
            Owner.PlaySound(CompleteSound);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Dimethro : MondainQuester
    {
        public override Type[] Quests => new Type[]
                {
                    typeof(TheRudimentsOfSelfDefenseQuest)
                };

        [Constructable]
        public Dimethro()
            : base("Dimethro", "- nauczyciel Walki Piesciami")
        {
            SetSkill(SkillName.EvalInt, 120.0, 120.0);
            SetSkill(SkillName.Inscribe, 120.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 120.0);
            SetSkill(SkillName.Wrestling, 120.0, 120.0);
            SetSkill(SkillName.Meditation, 120.0, 120.0);
        }

        public Dimethro(Serial serial)
            : base(serial)
        {
        }

        public override void Advertise()
        {
            Say(1078128); // You there! Wanna master hand to hand defense? Of course you do!
        }

        public override void OnOfferFailed()
        {
            Say(1077772); // I cannot teach you, for you know all I can teach!
        }

        public override void InitBody()
        {
            Female = false;
            CantWalk = true;
            Race = Race.Human;

            base.InitBody();
        }

        public override void InitOutfit()
        {
            SetWearable(new Backpack());
            SetWearable(new LongPants(), 0x455, 1);
            SetWearable(new Sandals(), 0x455, 1);
            SetWearable(new BodySash(), 0x455, 1);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
