using System.Collections.Generic;
using Server.Commands;
using Server.Commands.Generic;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	public class CleanQuestNPC : BaseCreature
	{
		[CommandProperty(AccessLevel.Counselor)]
		public string qName
		{
			get => Name;
			set => Name = value;
		}

		[CommandProperty(AccessLevel.Counselor)]
		public string qTitle
		{
			get => Title;
			set => Title = value;
		}

		[CommandProperty(AccessLevel.Counselor)]
		public string qLabel1
		{
			get => Label1;
			set => Label1 = value;
		}

		[CommandProperty(AccessLevel.Counselor)]
		public string qLabel2
		{
			get => Label2;
			set => Label2 = value;
		}

		[CommandProperty(AccessLevel.Counselor)]
		public bool qBlessed
		{
			get => Blessed;
			set => Blessed = value;
		}

		[CommandProperty(AccessLevel.Counselor)]
		public Race qRace
		{
			get => Race;
			set => Race = value;
		}

		[CommandProperty(AccessLevel.Counselor)]
		public bool qCantWalk
		{
			get => CantWalk;
			set => CantWalk = value;
		}

		[CommandProperty(AccessLevel.Counselor)]
		public int qKills
		{
			get => Kills;
			set => Kills = value;
		}

		[CommandProperty(AccessLevel.Counselor)]
		public bool qFemale
		{
			get => Female;
			set => Female = value;
		}

		[Constructable]
		public CleanQuestNPC()
			: base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
		{
			InitStats(100, 100, 100);

			SetSkill(SkillName.Wrestling, 90);
			SetSkill(SkillName.MagicResist, 60);
			SetSkill(SkillName.DetectHidden, 0);

			CantWalk = true;

			SpeechHue = Utility.RandomDyedHue();

			Hue = Race.Human.RandomSkinHue();
			Female = Utility.RandomBool();

			if (Female)
			{
				Body = 0x191;
				Name = NameList.RandomName("female");
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
			}

			AddItem(new Doublet(Utility.RandomDyedHue()));
			AddItem(new Sandals(Utility.RandomNeutralHue()));
			AddItem(new ShortPants(Utility.RandomNeutralHue()));
			AddItem(new HalfApron(Utility.RandomDyedHue()));

			Utility.AssignRandomHair(this);

			Container pack = new Backpack();

			pack.Movable = false;

			AddItem(pack);
		}

		public void ReplaceCloth(Item wearable, PlayerMobile provider)
		{
			if (wearable is BaseClothing || wearable is BaseWeapon || wearable is BaseArmor || wearable is Spellbook ||
			    wearable is BaseHarvestTool)
			{
				Item replaced = FindItemOnLayer(wearable.Layer);
				if (replaced != null)
				{
					provider.Backpack.DropItem(replaced);

					provider.SendMessage("Ciuszek z NPC laduje w twoim plecaku.");
				}

				AddItem(wearable);
			}
		}

		public void RemoveCloth(Item worn, PlayerMobile taker)
		{
			if (worn != null && worn.Parent == this)
			{
				taker.Backpack.DropItem(worn);
				taker.SendMessage("Ciuszek z NPC laduje w twoim plecaku.");
			}
		}

		public override bool ClickTitle => false;


		public CleanQuestNPC(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version 
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
		
		public static void Initialize()
        {
            TargetCommands.Register(new DecorateNpc());
            TargetCommands.Register(new AddQuestNpc());
            TargetCommands.Register(new RemoveQuestNpc());
        }

        public class DecorateNpc : BaseCommand
        {
            private static Dictionary<Mobile, Item> m_SelectedItems = new ();
            public DecorateNpc()
            {
                AccessLevel = AccessLevel.Counselor;
                Supports = CommandSupport.Simple;
                Commands = new [] { "DecorateNpc" };
                ObjectTypes = ObjectTypes.All;
                Usage = "DecorateNpc";
                Description = "";
            }

            public override void Execute(CommandEventArgs e, object targeted)
            {
                Mobile from = e.Mobile;

                if (targeted is Item item)
                {
                    if (item.IsAccessibleTo(e.Mobile) && item.Parent == e.Mobile.Backpack)
                    {
                        m_SelectedItems[from] = item;

                        e.Mobile.SendMessage("Wskaz NPC ktorego chcesz odziac");
                        e.Mobile.BeginTarget(18, false, TargetFlags.None, OnNpcSelected);
                    }
                    else if (item.Parent is CleanQuestNPC npc)
                    {
	                    if (from is PlayerMobile pm)
		                    npc.RemoveCloth(item, pm);
                    }
                    else
                    {
                        e.Mobile.SendMessage("Mozesz wskazac jedynie przedmioty ze swojego plecaka lub z paperdola Questego Npc.");
                    }
                }
            }

            private void OnNpcSelected(Mobile from, object targeted)
            {
	            if (targeted is not CleanQuestNPC npc)
                {
                    from.SendMessage("Mozesz przebrac jedynie specjalnego NPC stworzonego komendą [AddQuestNpc");
                    return;
                }

                if (!m_SelectedItems.TryGetValue(from, out var wearable))
                {
                    from.SendMessage("Error.");
                    return;
                }

                m_SelectedItems.Remove(from);

                if (from is PlayerMobile pm)
                    npc.ReplaceCloth(wearable, pm);
            }
        }

        public class AddQuestNpc : BaseCommand
        {
            public AddQuestNpc()
            {
                AccessLevel = AccessLevel.Counselor;
                Supports = CommandSupport.Simple;
                Commands = new[] { "AddQuestNpc" };
                ObjectTypes = ObjectTypes.All;
                Usage = "AddQuestNpc [name]";
                Description = "";
            }

            public override void Execute(CommandEventArgs e, object targeted)
            {
                if (targeted is IPoint3D p)
                {
                    var npc = new CleanQuestNPC();
                    if (e.Arguments.Length > 0)
                    {
                        npc.Name = e.GetString(0);
                    }
                    npc.MoveToWorld(new Point3D(p), e.Mobile.Map);
                }
            }
        }

        public class RemoveQuestNpc : BaseCommand
        {
            public RemoveQuestNpc()
            {
                AccessLevel = AccessLevel.Counselor;
                Supports = CommandSupport.Simple;
                Commands = new[] { "RemoveQuestNpc" };
                ObjectTypes = ObjectTypes.All;
                Usage = "RemoveQuestNpc";
                Description = "";
            }

            public override void Execute(CommandEventArgs e, object targeted)
            {
                if (targeted is CleanQuestNPC npc)
                {
	                npc.Delete();
                }
                else
                {
                    e.Mobile.SendMessage("Mozesz usunac jedynie NPC dodanego specjalna komenda (wskazany NPC nie jest typu CustomQuestNpc)");
                }
            }
        }
	}
}
