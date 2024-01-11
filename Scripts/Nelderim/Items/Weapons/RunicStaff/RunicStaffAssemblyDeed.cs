
using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.ACC.CSS;

namespace Server.Items
{
    public class RunicStaffTarget : Target
    {
        private RunicStaffAssemblyDeed m_Deed;

        public RunicStaffTarget(RunicStaffAssemblyDeed deed) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (target is BaseShield)
            {
                BaseShield t = target as BaseShield;

                if (!t.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    return;
                }

                if (t.Attributes.SpellChanneling == 0)
                {
                    from.SendMessage("Ta tarcza nie przepuszcza zaklec.");
                    return;
                }

                from.SendMessage("Ktora ksiege chcesz przemienic?");
                from.Target = new RunicStaffTarget2(m_Deed, t);
            }
            else
            {
                from.SendMessage("To nie jest tarcza.");
            }
        }
    }

    public class RunicStaffTarget2 : Target
    {
        private RunicStaffAssemblyDeed m_Deed;
        private BaseShield m_Shield;

        public RunicStaffTarget2(RunicStaffAssemblyDeed deed, BaseShield shield) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
            m_Shield = shield;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (target is Spellbook)
            {
                Spellbook t = target as Spellbook;

                if (!t.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    return;
                }
                else
                {
                    from.SendMessage("Wybierz kij na wzor");
                    from.Target = new RunicStaffTarget3(m_Deed, m_Shield, t);
                }
            }
            else
            {
                from.SendMessage("To nie jest ksiega magii.");
            }
        }
    }

    public class RunicStaffTarget3 : Target
    {
        private RunicStaffAssemblyDeed m_Deed;
        private BaseShield m_Shield;
        private Spellbook m_Spellbook;
        private BaseStaff m_Staff;

        public RunicStaffTarget3(RunicStaffAssemblyDeed deed, BaseShield shield, Spellbook spellbook) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
            m_Shield = shield;
            m_Spellbook = spellbook;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (target is BaseStaff)
            {
                m_Staff = (BaseStaff)target;

                if (!m_Staff.IsChildOf(from.Backpack) || !m_Deed.IsChildOf(from.Backpack) || !m_Shield.IsChildOf(from.Backpack) || !m_Spellbook.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                    return;
                }

                if (m_Shield.Attributes.SpellChanneling == 0)
                {
                    from.SendMessage("Ta tarcza nie przepuszcza zaklec.");
                    return;
                }

                if (IsValidForTransmutation(m_Staff))
                {
                    RunicStaff created = new RunicStaff(m_Staff.ItemID);

                    created.Hue = m_Staff.Hue;
                    created.Weight = m_Shield.Weight + m_Spellbook.Weight;

                    created.LootType = CalculateLootType(m_Shield, m_Spellbook);

                    CopySpellbookAttributes(created);
                    CopyShieldAttributes(created);

                    from.AddToBackpack(created);

                    // nie usuwamy kostura, bo nie kopiujemy jego propsow (jedynie kolor)

                    m_Deed.Delete();

                    created.ComponentShield = m_Shield;
                    if (m_Shield.Parent is Item)
                        ((Item)m_Shield.Parent).RemoveItem(m_Shield);
                    else if (m_Shield.Parent is Mobile)
                        ((Mobile)m_Shield.Parent).RemoveItem(m_Shield);
                    m_Shield.Map = Map.Internal;

                    created.ComponentBook = m_Spellbook;
                    if (m_Spellbook.Parent is Item)
                        ((Item)m_Spellbook.Parent).RemoveItem(m_Spellbook);
                    else if (m_Spellbook.Parent is Mobile)
                        ((Mobile)m_Spellbook.Parent).RemoveItem(m_Spellbook);
                    m_Spellbook.Map = Map.Internal;

                    from.SendMessage("Stworzyles runiczny kostur.");
                }
                else
                {
                    from.SendMessage("Ten kij nie nadaje sie na wzor.");
                }
            }
            else
            {
                from.SendMessage("To nie jest kij.");
            }
        }

        private bool IsValidForTransmutation(BaseStaff staff)
        {
            return Array.IndexOf(RunicStaff.ItemIDs, staff.ItemID) > -1;
        }

        LootType CalculateLootType(BaseShield shield, Spellbook spellbook)
        {
            if (shield.LootType == LootType.Cursed || spellbook.LootType == LootType.Cursed)
                return LootType.Cursed;

            if (shield.LootType == LootType.Blessed && spellbook.LootType == LootType.Blessed)
                return LootType.Blessed;

            return LootType.Regular;
        }

        private void CopyAosAttributes(AosAttributes source, AosAttributes target)
        {
            target.RegenHits += source.RegenHits;
            target.RegenStam += source.RegenStam;
            target.RegenMana += source.RegenMana;
            target.DefendChance += source.DefendChance;
            target.AttackChance += source.AttackChance;
            target.BonusStr += source.BonusStr;
            target.BonusDex += source.BonusDex;
            target.BonusInt += source.BonusInt;
            target.BonusHits += source.BonusHits;
            target.BonusStam += source.BonusStam;
            target.BonusMana += source.BonusMana;
            target.WeaponDamage += source.WeaponDamage;
            target.WeaponSpeed += source.WeaponSpeed;
            target.SpellDamage += source.SpellDamage;
            target.CastRecovery += source.CastRecovery;
            target.CastSpeed += source.CastSpeed;
            target.LowerManaCost += source.LowerManaCost;
            target.LowerRegCost += source.LowerRegCost;
            target.ReflectPhysical += source.ReflectPhysical;
            target.EnhancePotions += source.EnhancePotions;
            target.Luck += source.Luck;
            target.NightSight = Math.Max(source.NightSight, target.NightSight);

            target.SpellChanneling = Math.Max(source.SpellChanneling, target.SpellChanneling);

        }

        private void CopySpellbookAttributes(RunicStaff target)
        {
            CopyAosAttributes(m_Spellbook.Attributes, target.Attributes);

            target.Slayer = m_Spellbook.Slayer;
            target.Slayer2 = m_Spellbook.Slayer2;

            target.SkillBonuses.Skill_1_Name = m_Spellbook.SkillBonuses.Skill_1_Name;
            target.SkillBonuses.Skill_1_Value = m_Spellbook.SkillBonuses.Skill_1_Value;

            target.SkillBonuses.Skill_2_Name = m_Spellbook.SkillBonuses.Skill_2_Name;
            target.SkillBonuses.Skill_2_Value = m_Spellbook.SkillBonuses.Skill_2_Value;

            target.SkillBonuses.Skill_3_Name = m_Spellbook.SkillBonuses.Skill_3_Name;
            target.SkillBonuses.Skill_3_Value = m_Spellbook.SkillBonuses.Skill_3_Value;

            target.SkillBonuses.Skill_4_Name = m_Spellbook.SkillBonuses.Skill_4_Name;
            target.SkillBonuses.Skill_4_Value = m_Spellbook.SkillBonuses.Skill_4_Value;

            target.SkillBonuses.Skill_5_Name = m_Spellbook.SkillBonuses.Skill_5_Name;
            target.SkillBonuses.Skill_5_Value = m_Spellbook.SkillBonuses.Skill_5_Value;

        }

        private void CopyShieldAttributes(RunicStaff target)
        {
            CopyAosAttributes(m_Shield.Attributes, target.Attributes);

            target.MaxHitPoints = m_Shield.MaxHitPoints;
            target.HitPoints = m_Shield.HitPoints;

            target.WeaponAttributes.SelfRepair += m_Shield.ArmorAttributes.SelfRepair;
            target.WeaponAttributes.DurabilityBonus += m_Shield.ArmorAttributes.DurabilityBonus;
            target.WeaponAttributes.LowerStatReq += m_Shield.ArmorAttributes.LowerStatReq;

            target.StrRequirement = m_Shield.StrRequirement;
            target.DexRequirement = m_Shield.DexRequirement;
            target.IntRequirement = m_Shield.IntRequirement;

            // Any resist of the source shield (be it base, bonus, or material) must be copied directly as bonus to the Runic Staff.
            target.WeaponAttributes.ResistPhysicalBonus = m_Shield.PhysicalResistance;
            target.WeaponAttributes.ResistFireBonus = m_Shield.FireResistance;
            target.WeaponAttributes.ResistColdBonus = m_Shield.ColdResistance;
            target.WeaponAttributes.ResistPoisonBonus = m_Shield.PoisonResistance;
            target.WeaponAttributes.ResistEnergyBonus = m_Shield.EnergyResistance;

            // add luck from Resource:
            target.Attributes.Luck += m_Shield.GetLuckBonus();
        }
    }

    public class RunicStaffAssemblyDeed : Item
    {
        public static string NameText { get { return "zwoj na runiczny kostur"; } } // uzyte rowniez w menu rzemieslniczym

        [Constructable]
        public RunicStaffAssemblyDeed() : base(0x14F0)
        {
            Weight = 1.0;
            Name = NameText;
            LootType = LootType.Blessed;
            Hue = 614;
        }

        public RunicStaffAssemblyDeed(Serial serial) : base(serial)
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
            LootType = LootType.Blessed;

            int version = reader.ReadInt();
        }

        public override bool DisplayLootType { get { return false; } }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it. 
            }
            else
            {
                from.SendMessage("Ktora tarcze chcesz przemienic?");
                from.Target = new RunicStaffTarget(this);
            }
        }
    }
}
