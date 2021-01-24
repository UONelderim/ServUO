using Server.Items;

namespace Server.Mobiles
{
	public class NBaseWarHorse : BaseMount
	{
		public NBaseWarHorse( string name, int bodyID, int itemID ) : base( name, bodyID, itemID, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0xA8;

			SetStr( 400 );
			SetDex( 120 );
			SetInt( 50 );

			SetHits( 250 );
			SetMana( 0 );

			SetDamage( 6, 8 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 25, 35 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 25, 35 );

			SetSkill( SkillName.MagicResist, 30.0, 40.0 );
			SetSkill( SkillName.Tactics, 40.0, 50.0 );
			SetSkill( SkillName.Wrestling, 40.0, 50.0 );

			Fame = 500;
			Karma = 500;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
		}

		protected bool m_isVersion0;

		private bool m_BardingExceptional;
		private Mobile m_BardingCrafter;
		private int m_BardingHP;
		private bool m_HasBarding;
		private CraftResource m_BardingResource;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile BardingCrafter
		{
			get { return m_BardingCrafter; }
			set { m_BardingCrafter = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool BardingExceptional
		{
			get { return m_BardingExceptional; }
			set { m_BardingExceptional = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int BardingHP
		{
			get { return m_BardingHP; }
			set { m_BardingHP = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool HasBarding
		{
			get { return m_HasBarding; }
			set
			{
				m_HasBarding = value;

				if ( m_HasBarding )
				{
					Hue = CraftResources.GetHue( m_BardingResource );
					BodyValue = 284;
					ItemID = 0x3E92;
				}
				else
				{
					Hue = 0x851;
					BodyValue = 284;
					ItemID = 0x78;
				}

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public CraftResource BardingResource
		{
			get { return m_BardingResource; }
			set
			{
				m_BardingResource = value;

				if ( m_HasBarding )
					Hue = CraftResources.GetHue( value );

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int BardingMaxHP
		{
			get { return m_BardingExceptional ? 12000 : 10000; }
		}

		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public override bool BardImmune { get { return false; } }

		public override double GetControlChance( Mobile m, bool useBaseSkill )
		{
			return 1.0;
		}

		public NBaseWarHorse( Serial serial ) : base( serial )
		{
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( m_HasBarding )
			{
				if ( m_BardingExceptional && m_BardingCrafter != null )
					list.Add( 1060853, m_BardingCrafter.Name ); // armor exceptionally crafted by ~1_val~
				list.Add( 1060639, "{0}\t{1}", m_BardingHP, BardingMaxHP ); // durability ~1_val~ / ~2_val~
			}
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)1 ); // version

			writer.Write( (bool)m_BardingExceptional );
			writer.Write( (Mobile)m_BardingCrafter );
			writer.Write( (bool)m_HasBarding );
			writer.Write( (int)m_BardingHP );
			writer.Write( (int)m_BardingResource );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_BardingExceptional = reader.ReadBool();
					m_BardingCrafter = reader.ReadMobile();
					m_HasBarding = reader.ReadBool();
					m_BardingHP = reader.ReadInt();
					m_BardingResource = (CraftResource)reader.ReadInt();
					m_isVersion0 = false;
					break;
				}
				case 0:
				{
					m_isVersion0 = true;
					break;
				}
			}

			if ( Hue == 0 && !m_HasBarding )
				Hue = 0;

			if ( BaseSoundID == -1 )
				BaseSoundID = 0xA8;
		}
	}
}