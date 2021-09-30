using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class DefNecromancyCrafting : CraftSystem
	{
		public override SkillName MainSkill
		{
			get{ return SkillName.Necromancy; }
		}

		public override int GumpTitleNumber
		{
			get { return 1044009; } // <CENTER>INSCRIPTION MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefNecromancyCrafting();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; // 0%
		}

		private DefNecromancyCrafting() : base( 1, 1, 1.25 )// base( 1, 2, 1.7 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !(from is PlayerMobile && from.Skills[SkillName.Necromancy].Base >= 20.0) )
				return 1044153; // You don't have the required skill
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;

		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x1F5 ); // magic

			//if ( from.Body.Type == BodyType.Human && !from.Mounted )
			//	from.Animate( 9, 5, 1, true, false, 0 );

			//new InternalTimer( from ).Start();
		}

		// Delay to synchronize the sound with the hit on the anvil
		private class InternalTimer : Timer
		{
			private Mobile m_From;

			public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 0.7 ) )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.PlaySound( 0x2A );
			}
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				from.PlaySound( 65 ); // rune breaking
				if ( lostMaterial )
					return 1044043; // You failed to create the item, and some of your materials are lost.
				else
					return 1044157; // You failed to create the item, but no materials were lost.
			}
			else
			{
				from.PlaySound( 65 ); // rune breaking
				if ( quality == 0 )
					return 502785; // You were barely able to make this item.  It's quality is below average.
				else if ( makersMark && quality == 2 )
					return 1044156; // You create an exceptional quality item and affix your maker's mark.
				else if ( quality == 2 )
					return 1044155; // You create an exceptional quality item.
				else				
					return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			int index = AddCraft( typeof( VileCrystal ), "Krysztaly", "Krysztal Zla", 20.0, 120.0, 
				typeof( NoxCrystal ), "Krysztal Trucizny", 1,"Nie masz wystarczajaco duzo krysztalow trucizny." );
				AddRes( index, typeof ( PowderOfTranslocation ), "Proszek translokacji", 1, "Nie masz wystarczajaco duzej ilosci proszkow translokacji."  );

			index = AddCraft( typeof( deceptiveCrystal ), "Krysztaly", "Zwodniczy Krysztal", 30.0, 120.0, 
				typeof( CrystallineFragments ), "Fragmenty Krysztalow", 1,"Nie masz wystarczajaco duzo fragmentow krysztalow." );
				AddRes( index, typeof ( PowderOfTranslocation ), "Proszek translokacji", 1, "Nie masz wystarczajaco duzej ilosci proszkow translokacji."  );

			index = AddCraft( typeof( treacherousCrystal ), "Krysztaly", "Zdradziecki krysztal", 40.0, 120.0, 
				typeof( Ruby ), "rubin", 1,"Nie masz wystarczajaco duzej ilosci rubinow." );
				AddRes( index, typeof ( PowderOfTranslocation ), "Proszek translokacji", 1, "Nie masz wystarczajaco duzej ilosci proszkow translokacji."  );

			index = AddCraft( typeof( wickedCrystal ), "Krysztaly", "Pokrecony krysztal", 50.0, 120.0, 
				typeof( Emerald ), "szmaragd", 1,"Nie masz wystarczajaco duzej ilosci szmaragdow." );
				AddRes( index, typeof ( PowderOfTranslocation ), "Proszek translokacji", 1, "Nie masz wystarczajaco duzej ilosci proszkow translokacji."  );

			index = AddCraft( typeof( taintedCrystal ), "Krysztaly", "zepsuty krysztal", 20.0, 120.0, 
				typeof( ShimmeringCrystals ), "Skrzący Się Kryształ", 1,"Nie masz wystarczajaco duzej ilosci skrzących się kryształów." );
				AddRes( index, typeof ( PowderOfTranslocation ), "Proszek translokacji", 1, "Nie masz wystarczajaco duzej ilosci proszkow translokacji."  );

			index = AddCraft( typeof( SorrowCrystal ), "Krysztaly", "Krysztal Cierpienia", 70.0, 120.0, 
				typeof( ReceiverCrystal ), "Krysztal Komunikacyjny - Sluchacz", 1,"Nie masz wystarczajaco duzej ilosci krysztalow komunikacyjnych." );
				AddRes( index, typeof ( PowderOfTranslocation ), "Proszek translokacji", 1, "Nie masz wystarczajaco duzej ilosci proszkow translokacji."  );

			index = AddCraft( typeof( perilousCrystal ), "Krysztaly", "Niebezpieczny krysztal", 80.0, 120.0, 
				typeof( PowerCrystal ), "Krysztal Mocy", 1,"Nie masz wystarczajaco duzej ilosci krysztalow mocy." );
				AddRes( index, typeof ( PowderOfTranslocation ), "Proszek translokacji", 1, "Nie masz wystarczajaco duzej ilosci proszkow translokacji."  );

		/*	index = AddCraft( typeof( ominousCrystal ), "Krysztaly", "Przerazajacy Krysztal", 90.0, 120.0, 
				typeof( Diamond ), "Diament", 1,"Nie masz wystarczajaco duzej ilosci diamentow." );
				AddRes( index, typeof ( PowderOfTranslocation ), "Proszek translokacji", 1, "Nie masz wystarczajaco duzej ilosci proszkow translokacji."  );
*/
			index = AddCraft( typeof( MaliceCrystal ), "Krysztaly", "Krysztal Zlego Wymiaru", 100.0, 120.0, 
				typeof( VileCrystal ), "Krysztal Zla", 1,"Nie masz wystarczajaco duzej ilosci krysztalow zla." );
				AddRes( index, typeof ( PowderOfTranslocation ), "Proszek translokacji", 1, "Nie masz wystarczajaco duzej ilosci proszkow translokacji."  );



			//Skeleton Crafts


			index = AddCraft( typeof( SkelLegs ), "Szkielety", "Nogi Szkieleta", 20.0, 120.0,
				typeof( Bone ), "Kosc", 20,"Nie masz wystarczajaco duzej ilosci kosci." );
				AddRes( index, typeof ( AnimateDeadScroll ), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1, "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich."  );
				AddSkill( index, SkillName.Anatomy, 20.0, 50.0 );


			index = AddCraft( typeof( SkelBod ), "Szkielety", "Tułów szkieleta", 20.0, 120.0,
				typeof( Bone ), "Kosc", 20,"Nie masz wystarczajaco duzej ilosci kosci." );
				AddRes( index, typeof ( Skull ), "czaszka", 1, "Nie masz wystarczajaco duzej ilosci czaszek."  );
				AddRes( index, typeof ( RibCage ), "Klatka piersiowa", 1, "Nie masz wystarczajaco duzej ilosci klatek piersiowych."  );
				AddRes( index, typeof ( Spine ), "Kregoslup", 1, "Nie masz wystarczajaco duzej ilosci kregoslupow."  );
				AddSkill( index, SkillName.Anatomy, 20.0, 50.0 );

			index = AddCraft( typeof( SkelMageBod ), "Szkielety", "Tułów szkieleta maga", 30.0, 130.0,
				typeof( SkelBod ), "Tułów szkieleta", 1,"Nie masz wystarczajaco duzej ilosci tułowiów." );
				AddRes( index, typeof ( Jawbone ), "szczęka", 1, "Nie masz wystarczajaco duzej ilosci szczęk."  );
				AddSkill( index, SkillName.Anatomy, 30.0, 60.0 );

			
			// Rotting Crafts



			index = AddCraft( typeof( RottingBod ), "Gnijące", "gnijący tułów", 20.0, 120.0,
				typeof( Head ), "głowa", 1,"Nie masz wystarczajaco duzej ilosci głów." );
				AddRes( index, typeof ( Torso ), "Tułów", 1, "Nie masz wystarczajaco duzej ilosci tułowiów."  );
				AddRes( index, typeof ( RightArm ), "prawa ręka", 1, "Nie masz wystarczajaco duzej ilosci prawych rąk (bo masz dwie lewe he he)."  );
				AddRes( index, typeof ( LeftArm ), "lewa ręka", 1, "Nie masz wystarczajaco duzej ilosci lewych rąk (a to też źle he he)."  );
				AddSkill( index, SkillName.Anatomy, 25.0, 50.0 );

			index = AddCraft( typeof( RottingLegs ), "Gnijące", "gnijące nogi", 20.0, 120.0,
				typeof( LeftLeg ), "lewa noga", 1,"Nie masz wystarczajaco duzej ilosci lewych nóg." );
				AddRes( index, typeof ( RightLeg ), "Prawa Noga", 1, "Nie masz wystarczajaco duzej ilosci prawych nóg."  );
				AddRes( index, typeof ( AnimateDeadScroll ), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1, "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich."  );
				AddSkill( index, SkillName.Anatomy, 25.0, 50.0 );
			
			index = AddCraft( typeof( ToxicBod ), "Gnijące", "toksyczne ciało", 20.0, 120.0,
				typeof( RottingBod ), "gnijący tułów", 1,"Nie masz wystarczajaco duzej ilosci gnijących tułowiów." );
				AddRes( index, typeof ( GreaterPoisonPotion ), "Butelka Mocnej Trucizny", 10, "Nie masz wystarczajaco duzej ilosci butelek mocnej trucizny"  );
				AddRes( index, typeof ( AnimateDeadScroll ), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1, "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich."  );
				AddSkill( index, SkillName.Anatomy, 25.0, 50.0 );

			//Mummy Crafts


			index = AddCraft( typeof( WrappedLegs ), "Owinięte", "Zmumifikowane nogi", 40.0, 130.0,
				typeof( SkelLegs ), "Nogi Szkieleta", 1,"Nie masz wystarczajaco duzej ilosci nóg szkieleta." );
				AddRes( index, typeof ( Bandage ), "Bandaż", 100, "Nie masz wystarczajaco duzej ilosci bandaży."  );
				AddRes( index, typeof ( AnimateDeadScroll ), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1, "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich."  );
				AddSkill( index, SkillName.Anatomy, 40.0, 80.0 );


			index = AddCraft( typeof( WrappedBod ), "Owinięte", "Zmumifikowany tułów", 40.0, 130.0,
				typeof( SkelBod ), "Tułów szkieleta", 1,"Nie masz wystarczajaco duzej ilosci tułowiów." );
				AddRes( index, typeof (Bandage), "Bandaż", 100, "Nie masz wystarczajaco duzej ilosci bandaży."  );
				AddSkill( index, SkillName.Anatomy, 40.0, 80.0 );

			index = AddCraft( typeof( WrappedMageBod ), "Owinięte", "Zmumifikowany tułów oznaczony runami", 50.0, 140.0,
				typeof( SkelMageBod ), "Tułów szkieleta maga", 1,"Nie masz wystarczajaco duzej ilosci tułowiów." );
				AddRes( index, typeof (Bandage), "Bandaż", 100, "Nie masz wystarczajaco duzej ilosci bandaży."  );
				AddRes( index, typeof ( RecallRune ), "Czysta Runa", 10, "Nie masz wystarczajaco duzej ilosci czystych run."  );
				AddSkill( index, SkillName.Anatomy, 50.0, 80.0 );


			// Phylacery

			index = AddCraft( typeof( Phylacery ), "Filakterium", "Filakterium", 100.0, 130.0,
				typeof( Soul ), "Dusza", 1,"Nie masz duszy potrzebnej do związania w filakterium." );
				AddRes( index, typeof ( ArcaneGem ), "Tajemniczy kamień", 6, "Nie masz wystarczajaco duzej ilosci tajemniczych kamieni."  );
				AddRes( index, typeof ( AnimateDeadScroll ), "Zwoj wskrzeszenia zwlok (Nekromancja)", 1, "Nie masz wystarczajaco duzej ilosci zwojow nekromanckich."  );
				AddRes( index, typeof ( WoodenChest ), "Drewniana skrzynia", 1, "Nie masz wystarczajaco duzej ilosci drewnianych skrzyń."  );
				AddSkill( index, SkillName.Anatomy, 100.0, 120.0 );




		}
	}
}