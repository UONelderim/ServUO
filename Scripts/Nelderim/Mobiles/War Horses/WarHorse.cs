#region References

using System;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki konia bojowego")]
	public class WarHorse : BaseMount
	{
		private static int[] m_IDs =
		{
			0x76, 0x3EB2,
			0x77, 0x3EB1,
			0x78, 0x3EAF,
			0x79, 0x3EB0
		};
		
		[Constructable]
		public WarHorse() : this("kon bojowy")
		{
		}
		
		public WarHorse(string name) : base(name, 0x76, 0x3EB2, AIType.AI_Melee,
			FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			int random = Utility.Random( 4 );

			Body = m_IDs[random * 2];
			ItemID = m_IDs[random * 2 + 1];
			BaseSoundID = 0xA8;

			SetStr(400);
			SetDex(120);
			SetInt(50);

			SetHits(250);
			SetMana(0);

			SetDamage(6, 8);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 25, 35);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 25, 35);
			SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill(SkillName.MagicResist, 30.0, 40.0);
			SetSkill(SkillName.Tactics, 40.0, 50.0);
			SetSkill(SkillName.Wrestling, 40.0, 50.0);

			Fame = 500;
			Karma = 500;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;

			switch (random)
			{
				case 0: 
					SetResistance( ResistanceType.Energy, 55, 65 );
					break;
				case 1:
					SetResistance( ResistanceType.Cold, 55, 65 );
					break;
				case 2:
					SetResistance( ResistanceType.Fire, 55, 65 );
					break;
				case 3:
					SetResistance( ResistanceType.Poison, 45, 55 );
					break;
			}
		}

		private bool m_BardingExceptional;
		private Mobile m_BardingCrafter;
		private int m_BardingHP;
		private bool m_HasBarding;
		private CraftResource m_BardingResource;

		[CommandProperty(AccessLevel.GameMaster)]
        public Mobile BardingCrafter
        {
            get
            {
                return m_BardingCrafter;
            }
            set
            {
                m_BardingCrafter = value;
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool BardingExceptional
        {
            get
            {
                return m_BardingExceptional;
            }
            set
            {
                m_BardingExceptional = value;
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int BardingHP
        {
            get
            {
                return m_BardingHP;
            }
            set
            {
                m_BardingHP = value;
                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public bool HasBarding
        {
            get
            {
                return m_HasBarding;
            }
            set
            {
                m_HasBarding = value;

                if (m_HasBarding)
                {
	                Hue = CraftResources.GetHue(m_BardingResource);
	                BodyMod = 0x11C;
	                ItemID = 0x3E92;
                }
                else
                {
	                Hue = 0;
                    BodyMod = 0;
                    ItemID = m_IDs[Array.IndexOf(m_IDs,Body) + 1];
                }

                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource BardingResource
        {
            get
            {
                return m_BardingResource;
            }
            set
            {
                m_BardingResource = value;

                if (m_HasBarding)
                    Hue = CraftResources.GetHue(value);

                InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int BardingMaxHP
        {
            get
            {
                switch (m_BardingResource)
                {
                    default:
                        return BardingExceptional ? 12000 : 10000;
                    case CraftResource.DullCopper:
                    case CraftResource.Valorite:
                        return BardingExceptional ? 14500 : 12500;
                    case CraftResource.ShadowIron:
                        return BardingExceptional ? 17000 : 15000;
                }
            }
        }
        
        private int CalculateBardingResistance(ResistanceType type)
        {
	        if (m_BardingResource == CraftResource.None || !m_HasBarding)
		        return 0;

	        CraftResourceInfo resInfo = CraftResources.GetInfo(m_BardingResource);

	        CraftAttributeInfo attrs = resInfo?.AttributeInfo;

	        if (attrs == null)
		        return 0;

	        var expBonus = BardingExceptional ? 1 : 0;

	        var resBonus = type switch
	        {
		        ResistanceType.Physical => Math.Max(5, attrs.ArmorPhysicalResist),
		        ResistanceType.Fire => Math.Max(3, attrs.ArmorFireResist),
		        ResistanceType.Cold => Math.Max(2, attrs.ArmorColdResist),
		        ResistanceType.Poison => Math.Max(3, attrs.ArmorPoisonResist),
		        ResistanceType.Energy => Math.Max(2, attrs.ArmorEnergyResist),
		        _ => 0
	        };

	        return (resBonus + expBonus) * 5;
        }

        public override int GetResistance(ResistanceType type)
        {
	        return base.GetResistance(type) + CalculateBardingResistance(type);
        }

        public override FoodType FavoriteFood => FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
		
		public override PackInstinct PackInstinct => PackInstinct.Equine;

		public override bool BardImmune => false;

		public override double GetControlChance(Mobile m, bool useBaseSkill)
		{
			return 1.0;
		}

		public override void OnRiderDamaged(Mobile from, ref int amount, bool willKill)
		{
			base.OnRiderDamaged(from, ref amount, willKill);

			if (Rider == null)
				return;

			if ((from == null || !from.Player) && Rider.Player && Rider.Mount == this)
			{
				if (HasBarding)
				{
					int percent = (BardingExceptional ? 20 : 10);
					int absorbed = AOS.Scale(amount, percent);

					amount -= absorbed;
					BardingHP -= absorbed;

					if (BardingHP < 0)
					{
						HasBarding = false;
						BardingHP = 0;

						Rider.SendLocalizedMessage(1053031); // Your dragon's barding has been destroyed!
					}
				}
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (m_HasBarding && m_BardingExceptional && m_BardingCrafter != null)
			{
				list.Add(1060853, m_BardingCrafter.Name); // armor exceptionally crafted by ~1_val~
			}

			if (m_HasBarding)
			{
				list.Add(1115719, m_BardingHP.ToString()); // armor points: ~1_val~
			}
		}

		public WarHorse(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version

			writer.Write(m_BardingExceptional);
			writer.Write(m_BardingCrafter);
			writer.Write(m_HasBarding);
			writer.Write(m_BardingHP);
			writer.Write((int)m_BardingResource);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_BardingExceptional = reader.ReadBool();
			m_BardingCrafter = reader.ReadMobile();
			m_HasBarding = reader.ReadBool();
			m_BardingHP = reader.ReadInt();
			m_BardingResource = (CraftResource)reader.ReadInt();
			
			if (Hue == 0 && !m_HasBarding)
				Hue = 0;

			if (BaseSoundID == -1)
				BaseSoundID = 0xA8;
		}
	}
}
