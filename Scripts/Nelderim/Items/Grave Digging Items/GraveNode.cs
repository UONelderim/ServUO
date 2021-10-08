//Grave Digging by Massapequa

using System;
using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Items
{
	public class GraveNode : Item
	{
		public static void Initialize()
		{
			Timer.DelayCall(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), OnTick);
		}

		public static List<GraveNode> Nodes = new List<GraveNode>();

		private Mobile m_from;
		private object m_target;
		private bool Regs = true;

		private int m_Hits;

		private TimeSpan m_DefaultDecayTime = TimeSpan.FromMinutes(20.0);

		private Timer m_DecayTimer;
		private DateTime m_DecayTime;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Hits
		{
			get
			{
				return m_Hits;
			}
			set
			{
				m_Hits = value;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime DecayTime
		{
			get
			{
				return m_DecayTime;
			}
			set
			{
				m_DecayTime = value;
			}
		}

		[Constructable]
		public GraveNode() : base(0xE2E)
		{
			Name = "Grobowiec";
			Visible = false;
			Movable = false;
			m_Hits = 40;

			BeginDecay(m_DefaultDecayTime);
		}


		public void OnMine(Mobile from, Item tool)
		{
			m_from = from;

			if (tool is IUsesRemaining && ((IUsesRemaining)tool).UsesRemaining < 1)
				return;

			Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerStateCallback(DoMine), new object[] { from, tool });
		}

		public void DoMine(object obj)
		{
			object[] os = (object[])obj;
			Mobile from = (Mobile)os[0];
			Item tool = (Item)os[1];


			if (from != null && from.CheckSkill(SkillName.Necromancy, 00.0, 100.0) && m_Hits > 0)
			{
				from.Animate(11, 5, 1, true, false, 0);
				from.PlaySound(Utility.RandomMinMax(0x125, 0x126));

				double skill = from.Skills[SkillName.Necromancy].Value;

				Container pack = from.Backpack;
				int count = 1;

				if (skill > 80 && Utility.RandomBool())
					count++;

				Item res = null;
				string str = "";

				if (Regs)
				{
					if (from.Skills[SkillName.Necromancy].Value > 75)
					{
						switch (Utility.Random(7))
						{
							case 0:
								res = new Bone(count);
								if (count > 1)
									str = " 2 bones ";
								else
									str = " a bone ";
								break;
							case 1:
								res = new FertileDirt(count);
								str = " some fertile dirt ";
								break;
							case 2:
								res = new GraveDust(count);
								str = " some grave dust ";
								break;
							case 3:
								res = new NoxCrystal(count);
								if (count > 1)
									str = " 2 nox crystals ";
								else
									str = " a nox crystal ";
								break;
							case 4:
								res = new BatWing(count);
								if (count > 1)
									str = " 2 bat wings ";
								else
									str = " a bat wing ";
								break;
							case 5:
								res = new DaemonBlood(count);
								str = " some daemon blood ";
								break;
							case 6:
								res = new PigIron(count);
								str = " some pig iron ";
								break;
						}
					}
					else if (from.Skills[SkillName.Necromancy].Value > 50)
					{
						if (Utility.RandomDouble() > 0.5)
						{
							res = new FertileDirt(count);
							str = " some fertile dirt ";
						}
						else
						{
							res = new Bone(count);
							if (count > 1)
								str = " 2 bones ";
							else
								str = " a bone ";
						}
					}
					else
						res = new Bone(count);
				}
				else if (!Regs)
				{
					if (skill > 50)
					{
						if (Utility.RandomDouble() > 0.5)
						{
							res = new FertileDirt(count);
							str = " some fertile dirt ";
						}
						else
						{
							res = new Bone(count);
							if (count > 1)
								str = " 2 bones ";
							else
								str = " a bone ";
						}
					}
					else
					{
						res = new Bone(count);
						if (count > 1)
							str = " 2 bones ";
						else
							str = " a bone ";
					}
				}

				from.SendMessage("You dig up" + str + "from the grave.");

				if (pack == null || !pack.TryDropItem(from, res, false))
					res.MoveToWorld(from.Location, from.Map);

				Hits--;

				CheckTool(tool);

				if (Utility.RandomDouble() < 0.125)
					GenerateLoot(from);
			}
			else if (m_Hits <= 0)
			{
				from.SendMessage("There is nothing left to exhume here.");
			}
			else
			{
				from.Animate(11, 5, 1, true, false, 0);
				from.PlaySound(Utility.RandomMinMax(0x125, 0x126));
				from.SendMessage("You dig for a while but fail to unearth anything useful.");
			}
		}

		public void CheckTool(Item tool)
		{
			if (tool != null && tool is IUsesRemaining)
			{
				IUsesRemaining toolWithUses = (IUsesRemaining)tool;

				toolWithUses.ShowUsesRemaining = true;

				if (toolWithUses.UsesRemaining > 0)
					--toolWithUses.UsesRemaining;

				if (toolWithUses.UsesRemaining < 1)
					tool.Delete();
			}
		}

		public void GenerateLoot(Mobile from)
		{
			double skill = from.Skills[SkillName.Necromancy].Value;

			if (skill >= 90)
			{
				switch (Utility.Random(10))
				{
					case 0:
					case 1:
					case 2:
						AddHumanBones(from);
						break;
					case 3:
						AddEquipment(from);
						break;
					case 4:
						AddScroll(from, 8);
						break;
					case 5:
						AddGraveItem(from);
						break;
					case 6:
						AddMap(from, Utility.RandomMinMax(1, 6));
						break;
					case 7:
						AddInstrument(from);
						break;
					case 8:
						AddUnearthed(from);
						break;
					case 9:
						AddCoffin(from);
						break;
				}
			}
			else if (skill >= 70)
			{
				switch (Utility.Random(9))
				{
					case 0:
					case 1:
					case 2:
						AddHumanBones(from);
						break;
					case 3:
						AddEquipment(from);
						break;
					case 4:
						AddScroll(from, 7);
						break;
					case 5:
					case 6:
						AddGraveItem(from);
						break;
					case 7:
						AddMap(from, Utility.RandomMinMax(1, 3));
						break;
					case 8:
						AddInstrument(from);
						break;
				}
			}
			else if (skill >= 50)
			{
				switch (Utility.Random(9))
				{
					case 0:
					case 1:
					case 2:
						AddHumanBones(from);
						break;
					case 3:
						AddEquipment(from);
						break;
					case 4:
						AddScroll(from, 5);
						break;
					case 5:
						AddGraveItem(from);
						break;
					case 6:
						AddMap(from, 1);
						break;
					case 7:
					case 8:
						AddHumanBones(from);
						break;
				}
			}
			else if (skill >= 30)
			{
				switch (Utility.Random(9))
				{
					case 0:
					case 1:
					case 2:
					case 3:
						AddHumanBones(from);
						break;
					case 4:
						AddScroll(from, 5);
						break;
					case 5:
					case 6:
						AddGraveItem(from);
						break;
					case 7:
					case 8:
						AddHumanBones(from);
						break;
				}
			}
			else
			{
				switch (Utility.Random(5))
				{
					case 0:
					case 1:
					case 2:
					case 3:
						AddHumanBones(from);
						break;
					case 4:
						AddScroll(from, 3);
						break;
				}
			}
		}

		public void AddEquipment(Mobile from)
		{
			double skill = from.Skills[SkillName.Necromancy].Value;
			Container pack = from.Backpack;
			Item item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();

			int props = (Utility.RandomMinMax(0, (int)(skill / 20)));
			int luckChance =
				from is PlayerMobile ? ((PlayerMobile)from).Luck : from.Luck; //(int)(Utility.RandomDouble() * 10000);
			int min = 1;
			int max = 100;

			if (item is BaseWeapon)
				BaseRunicTool.ApplyAttributesTo((BaseWeapon)item, false, luckChance, props, min, max);
			else if (item is BaseArmor)
				BaseRunicTool.ApplyAttributesTo((BaseArmor)item, false, luckChance, props, min, max);
			else if (item is BaseRanged)
				BaseRunicTool.ApplyAttributesTo((BaseRanged)item, false, luckChance, props, min, max);
			else if (item is BaseShield)
				BaseRunicTool.ApplyAttributesTo((BaseShield)item, false, luckChance, props, min, max);
			else if (item is BaseJewel)
				BaseRunicTool.ApplyAttributesTo((BaseJewel)item, false, luckChance, props, min, max);
			else if (item is BaseHat)
				BaseRunicTool.ApplyAttributesTo((BaseHat)item, false, luckChance, props, min, max);

			pack.DropItem(item);
			from.SendMessage("You unearth some equipment.");
		}

		public void AddHumanBones(Mobile from)
		{
			Container pack = from.Backpack;
			Item item = null;

			if (Utility.RandomDouble() > 0.125)
				item = new HumanBones();
			else
				item = new HumanSkull();

			pack.DropItem(item);
			from.SendMessage("You unearth some bones.");
		}

		public void AddGraveItem(Mobile from)
		{
			Container pack = from.Backpack;
			pack.DropItem(new GraveDiggingItem());
			from.SendMessage("You unearth an item.");
		}

		public void AddMap(Mobile from, int level)
		{
			Container pack = from.Backpack;
			pack.DropItem(new TreasureMap(level, from.Map == Map.Felucca ? Map.Felucca : Map.Trammel));
			from.SendMessage("You unearth a treasure map.");
		}

		public void AddUnearthed(Mobile from)
		{
			Container pack = from.Backpack;
			Item item = null;
			string msg = "";

			if (Utility.RandomDouble() > 0.66)
			{
				item = new UnearthedBones(from);
				msg = "You exhume some remains.";
			}
			else
			{
				item = new UnearthedJewelryBox(from);
				msg = "You unearth a jewelry box.";
			}

			pack.DropItem(item);
			from.SendMessage(msg);
		}

		public void AddCoffin(Mobile from)
		{
			Container pack = from.Backpack;
			Item item = null;

			switch (Utility.Random(5))
			{
				case 0:
					item = new CoffinWestDeed();
					break;
				case 1:
					item = new CoffinNorthDeed();
					break;
				case 2:
					item = new GraveWestDeed();
					break;
				case 3:
					item = new GraveNorthDeed();
					break;
				case 4:
					item = new GravestoneDeed();
					break;
			}

			pack.DropItem(item);
			from.SendMessage("You unearth something interesting...");
		}

		public void AddScroll(Mobile from, int max)
		{
			Container pack = from.Backpack;
			Item item = null;
			switch (Utility.Random(2))
			{
				case 0:
					item = Loot.RandomScroll(1, max, SpellbookType.Regular);
					break;
				case 1:
					item = Loot.RandomScroll(1, max, SpellbookType.Necromancer);
					break;
			}

			pack.DropItem(item);
			from.SendMessage("You unearth a scroll.");
		}


		public void AddInstrument(Mobile from)
		{
			Container pack = from.Backpack;
			pack.DropItem(Loot.RandomInstrument());
			from.SendMessage("You unearth an instrument.");
		}

		public void BeginDecay(TimeSpan delay)
		{
			if (m_DecayTimer != null)
			{
				m_DecayTimer.Stop();
			}

			m_DecayTime = DateTime.UtcNow + delay;

			m_DecayTimer = new InternalTimer(this, delay);
			m_DecayTimer.Start();
		}

		public override void OnAfterDelete()
		{
			if (m_DecayTimer != null)
			{
				m_DecayTimer.Stop();
			}

			m_DecayTimer = null;

			Nodes.Remove(this);
		}

		public GraveNode(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
			writer.Write(m_Hits);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Hits = reader.ReadInt();
			Nodes.Add(this);
		}

		public static void OnTick()
		{
			List<GraveNode> list = new List<GraveNode>(Nodes);

			list.ForEach(g =>
			{
				if (!g.Deleted && g.Map != null && g.Map != Map.Internal && g.m_DecayTimer == null)
				{
					g.Delete();
				}
			});

			ColUtility.Free(list);
		}

		private class InternalTimer : Timer
		{
			private readonly GraveNode m_Grave;

			public InternalTimer(GraveNode grave, TimeSpan delay)
				: base(delay)
			{
				m_Grave = grave;
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				m_Grave.Delete();
			}
		}
	}
}
