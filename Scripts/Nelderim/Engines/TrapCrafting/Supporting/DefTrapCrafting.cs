#region References

using System;
using Server.Items;

#endregion

namespace Server.Engines.Craft
{
	public class DefTrapCrafting : CraftSystem
	{
		public override SkillName MainSkill
		{
			get { return SkillName.Tinkering; }
		}

		public override string GumpTitleString
		{
			get { return "<basefont color=#FFFFFF><CENTER>TRAPCRAFTING MENU</CENTER></basefont>"; }
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if (m_CraftSystem == null)
					m_CraftSystem = new DefTrapCrafting();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin(CraftItem item)
		{
			return 0.0; // 0%
		}

		private DefTrapCrafting() : base(1, 1, 1.25) // base( 1, 2, 1.7 )
		{
		}

		public override int CanCraft(Mobile from, ITool tool, Type itemType)
		{
			if ( from.Mounted )
			{
				return 1072018;  // Nie mozesz wykonywac tej czynnosci bedac konno!
			}
			
			if (tool.Deleted || tool.UsesRemaining < 0)
				return 1044038; // You have worn out your tool!
			if (!BaseTool.CheckAccessible(tool as Item, from))
				return 1044263; // The tool must be on your person to use.

			return 0;
		}

		public override void PlayCraftEffect(Mobile from)
		{
			from.PlaySound(0x241);
			from.Emote("*wyrabia przedmiot*");
		}

		private class InternalTimer : Timer
		{
			private readonly Mobile m_From;

			public InternalTimer(Mobile from) : base(TimeSpan.FromSeconds(0.7))
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.PlaySound(0x1C6);
			}
		}

		public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality,
			bool makersMark, CraftItem item)
		{
			if (toolBroken)
				from.SendLocalizedMessage(1044038);

			if (failed)
			{
				if (lostMaterial)
					return 1044043;
				return 1044157;
			}

			from.PlaySound(0x1c6);

			if (quality == 0)
				return 502785;
			if (makersMark && quality == 2)
				return 1044156;
			if (quality == 2)
				return 1044155;
			return 1044154;
		}

		public override void InitCraftList()
		{
			int index = -1;

			#region Components

			index = AddCraft(typeof(TrapFrame), "Komponenty", "Podstawa", 20.0, 40.0, typeof(IronIngot), "Sztaby", 2,
				"Potrzebujesz więcej sztab.");
			AddRes(index, typeof(Leather), "Skóra", 2, "Potrzebujesz więcej skór.");
			AddRes(index, typeof(Board), "Deska", 1, "Potrzeba więcej desek.");

			index = AddCraft(typeof(TrapSpike), "Komponenty", "Kolec do pułapki", 25.0, 45.0, typeof(Bolt), "Bełt", 1,
				"Potrzeba więcej bełtów.");
			AddRes(index, typeof(Springs), "sprężyny", 1, "Potrzeba więcej sprężyn");

			index = AddCraft(typeof(TrapCrystalTrigger), "Komponenty", "Kryształ do pułapki", 60.0, 80.0,
				typeof(PowerCrystal), "kryształ mocy", 1, "Potrzeba więcej kryształów mocy.");
			AddRes(index, typeof(DullCopperIngot), "Sztaby Matowej miedzi", 2,
				"Potrzebujesz więcej sztab matowej miedzi.");
			AddRes(index, typeof(Springs), "sprężyny", 1, "Potrzeba więcej sprężyn");

			index = AddCraft(typeof(TrapCrystalSensor), "Komponenty", "Kryształowy wykrywacz", 90.0, 110.0,
				typeof(Salt), "sól", 1, "Potrzebujesz więcej soli.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");

			#endregion

			#region Explosive Traps

			index = AddCraft(typeof(ExplosiveLesserTrap), "Wybuchowe pułapki", "Pomniejsza wybuchowa pułapka", 35.0,
				55.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(LesserExplosionPotion), "Słaba mikstura eksplozji", 1,
				"Potrzeba więcej mikstur eksplozji.");
			AddRes(index, typeof(TrapSpike), "Kolec do pułapki", 1, "Potrzeba więcej kolców do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(ExplosiveRegularTrap), "Wybuchowe pułapki", "Wybuchowa pułapka", 50.0, 70.0,
				typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(ExplosionPotion), "Mikstura eksplozji", 1, "Potrzeba więcej mikstur eksplozji.");
			AddRes(index, typeof(TrapSpike), "Kolec do pułapki", 1, "Potrzeba więcej kolców do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(ExplosiveGreaterTrap), "Wybuchowe pułapki", "Powiększa wybuchowa pułapka", 65.0,
				85.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(GreaterExplosionPotion), "Mikstura silnej eksplozji", 1,
				"Potrzeba więcej mikstur eksplozji.");
			AddRes(index, typeof(TrapSpike), "Kolec do pułapki", 1, "Potrzeba więcej kolców do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			#endregion

			#region Freezing Traps

			index = AddCraft(typeof(FreezingLesserTrap), "Zamrażające pułapki", "Pomniejsza zamrażająca pułapka", 50.0,
				70.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(DryIce), "Suchy lód", 1, "Potrzeba więcej suchego lodu.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(FreezingRegularTrap), "Zamrażające pułapki", "zamrażająca pułapka", 65.0, 85.0,
				typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(DryIce), "Suchy lód", 2, "Potrzeba więcej suchego lodu.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(FreezingGreaterTrap), "Zamrażające pułapki", "Powiększa zamrażająca pułapka", 90.0,
				110.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(DryIce), "Suchy lód", 3, "Potrzeba więcej suchego lodu.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			#endregion

			#region Lightning Traps

			index = AddCraft(typeof(LightningLesserTrap), "Porażające pułapki", "Pomniejsza porażająca pułapka", 45.0,
				65.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(LightningScroll), "Zwój Czaru Piorunów", 10, "Potrzeba więcej zwojów.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(LightningRegularTrap), "Porażające pułapki", "porażająca pułapka", 60.0, 80.0,
				typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(LightningScroll), "Zwój Czaru Piorunów", 15, "Potrzeba więcej zwojów.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(LightningGreaterTrap), "Porażające pułapki", "Powiększa porażająca pułapka", 75.0,
				95.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(LightningScroll), "Zwój Czaru Piorunów", 20, "Potrzeba więcej zwojów.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			#endregion

			#region Paralysis Traps

			index = AddCraft(typeof(ParalysisLesserTrap), "Paraliżujące pułapki", "Pomniejsza paraliżująca pułapka",
				40.0, 60.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(DragonsBlood), "Krew Smoka", 1, "Potrzeba więcej krwii smoka.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(ParalysisRegularTrap), "Paraliżujące pułapki", "Paraliżująca pułapk", 55.0, 75.0,
				typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(DragonsBlood), "Krew Smoka", 2, "Potrzeba więcej krwii smoka.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(ParalysisGreaterTrap), "Paraliżujące pułapki", "Pomniejsza paraliżująca pułapk",
				70.0, 90.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(DragonsBlood), "Krew Smoka", 3, "Potrzeba więcej krwii smoka.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			#endregion

			#region Other Traps

			index = AddCraft(typeof(BladeSpiritTrap), "Inne urządzenia", "Pułapka ducha ostrzy", 35.0, 55.0,
				typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(TrappedGhost), "uwięziony duch", 1, "Potrzeba więcej uwięzionych duchów.");
			AddRes(index, typeof(Hammer), "młotek", 1, "Potrzeba więcej młotków.");
			AddRes(index, typeof(CrescentBlade), "półksiężycowe ostrza", 4, "Potrzeba więcej połksiężycowych ostrzy.");

			index = AddCraft(typeof(GhostTrap), "Inne urządzenia", "Pułapka duchowa", 75.0, 95.0, typeof(TrapFrame),
				"Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(Bottle), "pusta butelka", 1, "Potrzebujesz więcej pustych butelek.");
			AddRes(index, typeof(TrapCrystalTrigger), "Kryształ do pułapki", 1,
				"Potrzeba więcej kryształów do pułapki.");
			AddRes(index, typeof(Garlic), "czosnek", 40, "Potrzeba więcej czosnku.");

			index = AddCraft(typeof(TrapDetector), "Inne urządzenia", "wykrywacz pułapek", 55.0, 75.0,
				typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(Springs), "sprężyny", 4, "Potrzeba więcej sprężyn");
			AddRes(index, typeof(Hammer), "Hmłotek", 4, "Potrzeba więcej młotków");
			AddRes(index, typeof(Buckler), "puklerz", 1, "Potrzeba więcej puklerzy.");

			index = AddCraft(typeof(TrapTest), "Inne urządzenia", "pułapka testowa", 25.0, 45.0, typeof(TrapFrame),
				"Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(Leather), "skóra", 2, "Potrzeba więcej skór.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");
			AddRes(index, typeof(Springs), "sprężyny", 1, "Potrzeba więcej sprężyn");

			#endregion

			#region Poison Dart Traps

			index = AddCraft(typeof(PoisonLesserDartTrap), "Pułapki z trującymi strzałkami", "Pułapka z lekką trucizną",
				25.0, 45.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(LesserPoisonPotion), "Słaba trucizna", 1, "Potrzebujesz więcej trucizny.");
			AddRes(index, typeof(TrapSpike), "Kolec do pułapki", 1, "Potrzeba więcej kolców do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(PoisonRegularDartTrap), "Pułapki z trującymi strzałkami", "Pułapka z trucizną",
				40.0, 60.0, typeof(TrapFrame), "Podstawa", 1, "Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(PoisonPotion), "trucizna", 1, "Potrzebujesz więcej trucizny.");
			AddRes(index, typeof(TrapSpike), "Kolec do pułapki", 1, "Potrzeba więcej kolców do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(PoisonGreaterDartTrap), "Pułapki z trującymi strzałkami",
				"Pułapka z mocną trucizną", 55.0, 75.0, typeof(TrapFrame), "Podstawa", 1,
				"Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(GreaterPoisonPotion), "mocna trucizna", 1, "Potrzebujesz więcej trucizny.");
			AddRes(index, typeof(TrapSpike), "Kolec do pułapki", 1, "Potrzeba więcej kolców do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			index = AddCraft(typeof(PoisonDeadlyDartTrap), "Pułapki z trującymi strzałkami",
				"Pułapka ze śmiertelną trucizną", 70.0, 90.0, typeof(TrapFrame), "Podstawa", 1,
				"Potrzeba więcej podstaw do pułapki.");
			AddRes(index, typeof(DeadlyPoisonPotion), "śmiertelna trucizna", 1, "Potrzebujesz więcej trucizny.");
			AddRes(index, typeof(TrapSpike), "Kolec do pułapki", 1, "Potrzeba więcej kolców do pułapki.");
			AddRes(index, typeof(Gears), "Przekładnie", 2, "Potrzeba więcej przekładni.");

			#endregion
		}
	}
}
