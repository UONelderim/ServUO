using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class RunicStaff : BaseMeleeWeapon, ICustomWeaponAbilities
    {
		private static int[] m_ItemIDs = new int[] { 0x13F8, 0xE89, 0xDF0, 0xE81 };
        public static int[] ItemIDs { get { return m_ItemIDs; } }

        private const int ConcussionBlowIndex = 3;
        private const int CrushingBlowIndex = 4;
        private const int DisarmIndex = 5;
        private const int DoubleStrikeIndex = 7;
        private const int ParalyzingBlowIndex = 11;
        private const int WhirlwindAttackIndex = 13;
        private const int ForceOfNatureIndex = 29;
        private static Dictionary<int, int[]> LegacyWeaponAbilitiesByItemID = new Dictionary<int, int[]> {
			{ 0x13F8, new int[] { ConcussionBlowIndex, ForceOfNatureIndex } },    // GnarledStaff
			{ 0xE89, new int[] { DoubleStrikeIndex, ConcussionBlowIndex } },      // QuarterStaff
			{ 0xDF0, new int[] { WhirlwindAttackIndex, ParalyzingBlowIndex } },   // BlackStaff
			{ 0xE81, new int[] { CrushingBlowIndex, DisarmIndex } }               // ShepherdsCrook
        };
        public int LegacyPrimaryWeaponAbilityIndex { get { return LegacyWeaponAbilitiesByItemID.ContainsKey(ItemID) ? LegacyWeaponAbilitiesByItemID[ItemID][0] : -1; } }
        public int LegacySecondaryWeaponAbilityIndex { get { return LegacyWeaponAbilitiesByItemID.ContainsKey(ItemID) ? LegacyWeaponAbilitiesByItemID[ItemID][1] : -1; } }
        public int CustomPrimaryWeaponAbilityIndex { get { return DisarmIndex; } }
        public int CustomSecondaryWeaponAbilityIndex { get { return ParalyzingBlowIndex; } }

        public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.Disarm; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }

		public override int StrengthReq => 0; 
		public override int MinDamage => 1; 
		public override int MaxDamage => 4;
		public override float Speed => 50; 
        public override int DefHitSound { get { return 0x233; } }
        public override int DefMissSound { get { return 0x239; } }

        public override SkillName DefSkill{ get{ return SkillName.Wrestling; } }
		public override WeaponType DefType{ get{ return WeaponType.Staff; } }
        public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Bash2H; } }

        public override int InitMinHits { get { return 60; } }
        public override int InitMaxHits { get { return 60; } }

		private Spellbook m_ComponentBook;
        [CommandProperty(AccessLevel.GameMaster)]
        public Spellbook ComponentBook
		{
			get { return m_ComponentBook; }
			set
			{
				m_ComponentBook = value;
			}
		}

		private BaseShield m_ComponentShield;
        [CommandProperty(AccessLevel.GameMaster)]
        public BaseShield ComponentShield
		{
			get { return m_ComponentShield; }
			set
			{
				m_ComponentShield = value;
			}
		}


        [Constructable]
        public RunicStaff() : this(Utility.RandomList(ItemIDs))
		{
		}
        public RunicStaff(int itemID) : base(itemID)
        {
            Name = "Runiczny kostur";

            Weight = 1.0;
			Attributes.SpellChanneling = 1;
		}

		public override void OnDelete()
		{
			base.OnDelete();

            if (m_ComponentBook != null) 
				m_ComponentBook.Delete();
            if (m_ComponentShield != null) 
				m_ComponentShield.Delete();
		}

		public RunicStaff( Serial serial ) : base( serial )
		{
		}

		public override double GetDefendSkillValue(Mobile attacker, Mobile defender)
		{
			double wresValue = defender.Skills[SkillName.Wrestling].Value;
			double anatValue = defender.Skills[SkillName.Anatomy].Value;
			double evalValue = defender.Skills[SkillName.EvalInt].Value;
			double incrValue = (anatValue + evalValue + 20.0) * 0.5;

			if (incrValue > 120.0)
				incrValue = 120.0;

			if (wresValue > incrValue)
			{
				return wresValue;
			}
			else
			{
				return incrValue;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write((Item)m_ComponentBook);
			writer.Write((Item)m_ComponentShield);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            m_ComponentBook = (Spellbook) reader.ReadItem();
            m_ComponentShield = (BaseShield) reader.ReadItem();
		}
	}
}
