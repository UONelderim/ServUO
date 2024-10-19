#region References

using System;
using System.Collections.Generic;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class GraveNode : Item
	{
		public static void Initialize()
		{
			Timer.DelayCall(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), OnTick);
		}

		public static List<GraveNode> Nodes = new List<GraveNode>();

		private object m_target;
		private readonly bool Regs = true;

		private readonly TimeSpan m_DefaultDecayTime = TimeSpan.FromMinutes(20.0);

		private Timer m_DecayTimer;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Hits { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime DecayTime { get; set; }

		[Constructable]
		public GraveNode() : base(0xE2E)
		{
			Name = "Grobowiec";
			Visible = false;
			Movable = false;
			Hits = 40;

			BeginDecay(m_DefaultDecayTime);
		}

		public void OnMine(Mobile from, Item tool)
		{
			if (tool is IUsesRemaining && ((IUsesRemaining)tool).UsesRemaining < 1)
				return;

			if (!from.BeginAction(typeof(GraveNode)))
			{
				from.SendLocalizedMessage(1045157); //Musisz chwile poczekac, zeby moc zrobic cos innego
				return;
			}

			Timer.DelayCall(TimeSpan.FromSeconds(1), new TimerStateCallback(DoMine), new object[] { from, tool });
		}

		public void DoMine(object obj)
		{
			object[] os = (object[])obj;
			Mobile from = (Mobile)os[0];
			Item tool = (Item)os[1];

			if (from == null)
				return;

			from.EndAction(typeof(GraveNode));

			if (from.CheckSkill(SkillName.Necromancy, 00.0, 100.0) && Hits > 0)
			{
				from.Animate(11, 5, 1, true, false, 0);
				from.PlaySound(Utility.RandomMinMax(0x125, 0x126));

				double skill = from.Skills[SkillName.Necromancy].Value;

				Container pack = from.Backpack;
				int count = 1;

				if (skill > 80 && Utility.RandomBool())
					count++;

				Item item = null;
				string msg = "";

				if (Regs)
				{
					if (from.Skills[SkillName.Necromancy].Value > 75)
					{
						switch (Utility.Random(11))
						{
							case 0:
								item = new Bone();
								msg = "kosc";
								break;
							case 1:
								item = new Skull();
								msg = "czaszka";
								break;
							case 2:
								item = new RibCage();
								msg = "klatka piersiowa";
								break;
							case 3:
								item = new Spine();
								msg = "kregoslup";
								break;
							case 4:
								item = new Jawbone();
								msg = "szczeka";
								break;
							case 5:
								item = new Torso();
								msg = "tulów";
								break;
							case 6:
								item = new RightArm();
								msg = "prawa reka";
								break;
							case 7:
								item = new LeftArm();
								msg = "lewa reka";
								break;
							case 8:
								item = new RightLeg();
								msg = "prawa noga";
								break;
							case 9:
								item = new LeftLeg();
								msg = "lewa noga";
								break;
							case 10:
								item = new Bandage();
								msg = "bandaz";
								break;
						}
					}
					else if (from.Skills[SkillName.Necromancy].Value > 50)
					{
						if (Utility.RandomDouble() > 0.5)
						{
							item = new Skull();
							msg = "czaszka";
						}
						else
						{
							item = new Bone();
							msg = "kosc";
						}
					}
					else
					{
						item = new Bone();
						msg = "kosc";
					}
				}
				else if (!Regs)
				{
					if (skill > 50)
					{
						if (Utility.RandomDouble() > 0.5)
						{
							item = new Skull();
							msg = "czaszka";
						}
						else
						{
							item = new Bone();
							msg = "kosc";
						}
					}
					else
					{
						item = new Bone();
						msg = "kosc";
					}
				}

				if (item != null && item.Stackable)
					item.Amount = count;

				if (count > 1)
					msg = count + " " + msg;

				from.SendMessage("Wykopales " + msg + " z grobu.");


				if (pack == null || !pack.TryDropItem(from, item, false))
					item.MoveToWorld(from.Location, from.Map);

				Hits--;

				CheckTool(tool);

				if (Utility.RandomDouble() < 0.125)
				{
					Item loot = GenerateLoot(from);
					if (loot != null && pack == null || !pack.TryDropItem(from, loot, false))
						loot.MoveToWorld(from.Location, from.Map);
				}

				if (from is PlayerMobile pm)
				{
					pm.Statistics.GravesDigged++;
				}
			}
			else if (Hits <= 0)
			{
				from.SendMessage("Nie ma tu nic do wykopania.");
			}
			else
			{
				from.Animate(11, 5, 1, true, false, 0);
				from.PlaySound(Utility.RandomMinMax(0x125, 0x126));
				from.SendMessage("Kopiesz przez chwile, ale nie udaje Ci sie nic ciekawego wykopac.");
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

		public Item GenerateLoot(Mobile from)
		{
			double skill = from.Skills[SkillName.Necromancy].Value;

			if (skill >= 90)
			{
				switch (Utility.Random(10))
				{
					default:
					case 0:
					case 1:
					case 2: return HumanBones(from);
					case 3: return Equipment(from);
					case 4: return Scroll(from, 8);
					case 5: return GraveItem(from);
					case 6: return Map(from, Utility.RandomMinMax(1, 4));
					case 7: return Instrument(from);
					case 8: return Unearthed(from);
					case 9: return Coffin(from);
				}
			}

			if (skill >= 70)
			{
				switch (Utility.Random(9))
				{
					default:
					case 0:
					case 1:
					case 2: return HumanBones(from);
					case 3: return Equipment(from);
					case 4: return Scroll(from, 7);
					case 5:
					case 6: return GraveItem(from);
					case 7: return Map(from, Utility.RandomMinMax(1, 3));
					case 8: return Instrument(from);
				}
			}

			if (skill >= 50)
			{
				switch (Utility.Random(9))
				{
					default:
					case 0:
					case 1:
					case 2: return HumanBones(from);
					case 3: return Equipment(from);
					case 4: return Scroll(from, 5);
					case 5: return GraveItem(from);
					case 6: return Map(from, 1);
					case 7:
					case 8: return HumanBones(from);
				}
			}

			if (skill >= 30)
			{
				switch (Utility.Random(9))
				{
					default:
					case 0:
					case 1:
					case 2:
					case 3: return HumanBones(from);
					case 4: return Scroll(from, 5);
					case 5:
					case 6: return GraveItem(from);
					case 7:
					case 8: return HumanBones(from);
				}
			}

			switch (Utility.Random(5))
			{
				default:
				case 0:
				case 1:
				case 2:
				case 3: return HumanBones(@from);
				case 4: return Scroll(@from, 3);
			}
		}

		public Item Equipment(Mobile from)
		{
			double skill = from.Skills[SkillName.Necromancy].Value;
			Item item = Loot.RandomArmorOrShieldOrWeaponOrJewelry();

			int props = (Utility.RandomMinMax(0, (int)(skill / 20)));
			int luckChance = from is PlayerMobile ? ((PlayerMobile)from).Luck : from.Luck;
			int min = 1;
			int max = 100;

			if (item is BaseWeapon)
				BaseRunicTool.ApplyAttributesTo((BaseWeapon)item, false, luckChance, props, min, max);
			else if (item is BaseArmor)
				BaseRunicTool.ApplyAttributesTo((BaseArmor)item, false, luckChance, props, min, max);
			else if (item is BaseJewel)
				BaseRunicTool.ApplyAttributesTo((BaseJewel)item, false, luckChance, props, min, max);
			else if (item is BaseHat)
				BaseRunicTool.ApplyAttributesTo((BaseHat)item, false, luckChance, props, min, max);

			from.SendMessage("Wykopales troche ciekawych przedmiotow.");
			return item;
		}

		public Item HumanBones(Mobile from)
		{
			from.SendMessage("Odkopales troche kosci.");
			if (Utility.RandomDouble() > 0.125)
				return new HumanBones();
			return new HumanSkull();
		}

		public Item GraveItem(Mobile from)
		{
			from.SendMessage("Wykopales troche ciekawych przedmiotow.");
			return new GraveDiggingItem();
		}

		public Item Map(Mobile from, int level)
		{
			from.SendMessage("Wykopales mape skarbu.");
			return new TreasureMap(level, from.Map == Server.Map.Felucca ? Server.Map.Felucca : Server.Map.Felucca);
		}

		public Item Unearthed(Mobile from)
		{
			if (Utility.RandomDouble() > 0.66)
			{
				from.SendMessage("Wykopujesz zwloki.");
				return new UnearthedBones(from);
			}

			from.SendMessage("Wykopujesz skrzynie ze skarbami.");
			return new UnearthedJewelryBox(from);
		}

		public Item Coffin(Mobile from)
		{
			from.SendMessage("Cos ciekawego wylania sie z zniemi...");
			switch (Utility.Random(5))
			{
				default:
				case 0:
				case 1: return new CoffinNorthDeed();
				case 2: return new GraveWestDeed();
				case 3: return new GraveNorthDeed();
				case 4: return new GravestoneDeed();
			}
		}

		public Item Scroll(Mobile from, int max)
		{
			from.SendMessage("Wykopales zwoj.");
			switch (Utility.Random(2))
			{
				default:
				case 0: return Loot.RandomScroll(1, max, SpellbookType.Regular);
				case 1: return Loot.RandomScroll(1, max, SpellbookType.Necromancer);
			}
		}


		public Item Instrument(Mobile from)
		{
			from.SendMessage("Wykopales instrument.");
			return Loot.RandomInstrument();
		}

		public void BeginDecay(TimeSpan delay)
		{
			if (m_DecayTimer != null)
			{
				m_DecayTimer.Stop();
			}

			DecayTime = DateTime.UtcNow + delay;

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

		public GraveNode(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
			writer.Write(Hits);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			Hits = reader.ReadInt();
			Nodes.Add(this);
		}

		public static void OnTick()
		{
			List<GraveNode> list = new List<GraveNode>(Nodes);

			list.ForEach(g =>
			{
				if (!g.Deleted && ((Item)g).Map != null && ((Item)g).Map != Server.Map.Internal &&
				    g.m_DecayTimer == null)
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
