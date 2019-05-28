using System;
using Server;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Gumps;
using System.Collections;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Commands;
using Server.Factions;
using Server.Engines.Craft;
using System.Linq;

namespace Server.SkillHandlers
{
    public class Imbuing
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Imbuing].Callback = new SkillUseCallback(OnUse);

            CommandSystem.Register("GetTotalWeight", AccessLevel.GameMaster, new CommandEventHandler(GetTotalWeight_OnCommand));
            CommandSystem.Register("GetTotalMods", AccessLevel.GameMaster, new CommandEventHandler(GetTotalMods_OnCommand));
        }

        private static void OnLogin(LoginEventArgs e)
        {
            if (!e.Mobile.CanBeginAction(typeof(Imbuing)))
                e.Mobile.EndAction(typeof(Imbuing));
        }

        private static Dictionary<Mobile, ImbuingContext> m_ContextTable = new Dictionary<Mobile, ImbuingContext>();
        public static Dictionary<Mobile, ImbuingContext> ContextTable { get { return m_ContextTable; } }

        public static TimeSpan OnUse(Mobile from)
        {
            if (!from.Alive)
            {
                from.SendLocalizedMessage(500949); //You can't do that when you're dead.
            }
            else if (from is PlayerMobile)
            {
                from.CloseGump(typeof(ImbuingGump));
                BaseGump.SendGump(new ImbuingGump((PlayerMobile)from));
                from.BeginAction(typeof(Imbuing));
            }

            return TimeSpan.FromSeconds(1.0);
        }

        public static ImbuingContext GetContext(Mobile m)
        {
            if (!m_ContextTable.ContainsKey(m))
            {
                ImbuingContext context = new ImbuingContext(m);
                m_ContextTable[m] = context;
                return context;
            }

            return m_ContextTable[m];
        }

        public static void AddContext(Mobile from, ImbuingContext context)
        {
            m_ContextTable[from] = context;
        }

        public static bool CanImbueItem(Mobile from, Item item)
        {
            if (!Imbuing.CheckSoulForge(from, 2))
            {
                return false;
            }
            if (item == null || !item.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1079575);  // The item must be in your backpack to imbue it.
            }
            else if (item.LootType == LootType.Blessed || item.LootType == LootType.Newbied)
            {
                from.SendLocalizedMessage(1080438);  // You cannot imbue a blessed item.
            }
            else if (item is BaseWeapon && Spells.Mysticism.EnchantSpell.IsUnderSpellEffects(from, (BaseWeapon)item))
            {
                from.SendLocalizedMessage(1080130);  // You cannot imbue an item that is currently enchanted.
            }
            else if (item is BaseWeapon && ((BaseWeapon)item).FocusWeilder != null)
            {
                from.SendLocalizedMessage(1080444);  //You cannot imbue an item that is under the effects of the ninjitsu focus attack ability.
            }
            else if (IsSpecialItem(item))
            {
                from.SendLocalizedMessage(1079576); // You cannot imbue this item.
            }
            else if (item is IFactionItem && ((IFactionItem)item).FactionItemState != null)
            {
                from.SendLocalizedMessage(1114312); // You cannot imbue faction items.
            }
            else if (item is BaseJewel && !(item is BaseRing) && !(item is BaseBracelet))
            {
                from.SendLocalizedMessage(1079576); // You cannot imbue this item.
            }
            else if (IsInNonImbueList(item.GetType()))
            {
                from.SendLocalizedMessage(1079576); // You cannot imbue this item.
            }
            else
            {
                return true;
            }

            return false;
        }

        public static bool OnBeforeImbue(Mobile from, Item item, int id, int value)
        {
            return OnBeforeImbue(from, item, id, value, Imbuing.GetTotalMods(item, id), GetMaxProps(item), Imbuing.GetTotalWeight(item, id), Imbuing.GetMaxWeight(item));
        }

        public static bool OnBeforeImbue(Mobile from, Item item, int id, int value, int totalprops, int maxprops, int totalitemweight, int maxweight)
        {
            if (totalprops >= maxprops || totalitemweight > maxweight)
            {
                from.SendLocalizedMessage(1079772); // You cannot imbue this item with any more item properties.
                from.CloseGump(typeof(ImbueGump));
                from.EndAction(typeof(Imbuing));
                return false;
            }

            return true;
        }

        public static bool CanUnravelItem(Mobile from, Item item, bool message = true)
        {
            if (!CheckSoulForge(from, 2, false, false))
            {
                from.SendLocalizedMessage(1080433); // You must be near a soulforge to magically unravel an item.
            }
            else if (!item.IsChildOf(from.Backpack))
            {
                if (message)
                    from.SendLocalizedMessage(1080424);  // The item must be in your backpack to magically unravel it.
            }
            else if (item.LootType == LootType.Blessed || item.LootType == LootType.Newbied)
            {
                if (message)
                    from.SendLocalizedMessage(1080421);  // You cannot unravel the magic of a blessed item.
            }
            else if (!(item is BaseWeapon || item is BaseArmor || item is BaseJewel || item is BaseHat))
            {
                if (message)
                    from.SendLocalizedMessage(1080425); // You cannot magically unravel this item.
            }
            else if (item is BaseWeapon && Spells.Mysticism.EnchantSpell.IsUnderSpellEffects(from, (BaseWeapon)item))
            {
                if (message)
                    from.SendLocalizedMessage(1080427);  // You cannot magically unravel an item that is currently enchanted.
            }
            else if (item is BaseWeapon && ((BaseWeapon)item).FocusWeilder != null)
            {
                if (message)
                    from.SendLocalizedMessage(1080445); //You cannot magically unravel an item that is under the effects of the ninjitsu focus attack ability.
            }
            else if (item is IFactionItem && ((IFactionItem)item).FactionItemState != null)
            {
                if (message)
                    from.SendLocalizedMessage(1112408); // You cannot magically unravel a faction reward item.
            }
            else
            {
                return true;
            }

            return false;
        }

        public static bool IsSpecialItem(Item item)
        {
            if (item == null)
                return true;

            if (IsSpecialImbuable(item))
                return false;

			if (item.IsArtifact)
				return true;

            if (RunicReforging.GetArtifactRarity(item) > 0)
                return true;

            if (NonCraftableImbuable(item))
                return false;

            foreach (CraftSystem system in CraftSystem.Systems)
            {
                CraftItem crItem = null;
                
                if(system != null && system.CraftItems != null)
                    crItem = system.CraftItems.SearchFor(item.GetType());

                if (crItem != null)
                    return false;
            }

            return true;
        }

        private static bool IsSpecialImbuable(Item item)
        {
            return IsSpecialImbuable(item.GetType());
        }

        private static bool IsSpecialImbuable(Type type)
        {
            if (_SpecialImbuable.Any(i => i == type))
                return true;

            if (type.IsSubclassOf(typeof(BaseGlovesOfMining)) || typeof(IFishingAttire).IsAssignableFrom(type))
                return true;

            return false;
        }

         private static Type[] _SpecialImbuable =
        {
            typeof(ClockworkLeggings), typeof(GargishClockworkLeggings), typeof(OrcishKinMask), typeof(SavageMask), typeof(VirtuososArmbands), 
            typeof(VirtuososCap), typeof(VirtuososCollar), typeof(VirtuososEarpieces), typeof(VirtuososKidGloves), typeof(VirtuososKilt), 
            typeof(VirtuososNecklace), typeof(VirtuososTunic), typeof(BestialArms), typeof(BestialEarrings), typeof(BestialGloves), typeof(BestialGorget),
            typeof(BestialHelm), typeof(BestialKilt), typeof(BestialLegs), typeof(BestialNecklace)
        };

        private static Type[] _NonCraftables =
        {
            typeof(SilverRing), typeof(SilverBracelet)
        };

        public static bool NonCraftableImbuable(Item item)
        {
            if (item is BaseWand)
                return true;

            Type type = item.GetType();

            foreach (var t in _NonCraftables)
            {
                if (t == type)
                    return true;
            }

            return false;
        }

        public static double GetSuccessChance(Mobile from, Item item, int totalItemWeight, int propWeight, out double dif)
        {
            double suc = 0;     // display difficulty
            double bonus = 0;   // bonuses

            ImbuingContext context = GetContext(from);
            double skill = from.Skills[SkillName.Imbuing].Value;

            // - Racial Bonus - SA ONLY -
            if (from.Race == Race.Gargoyle)
                bonus = 10;

            // Queen Soul Forge Bonus
            if (context.Imbue_SFBonus > 0)
                bonus += context.Imbue_SFBonus;

            bonus += GetQualityBonus(item);
            bonus /= 100;

            double resultWeight = totalItemWeight + propWeight;

            if (resultWeight <= 500)
            {
                dif = ((resultWeight) / 22) + (resultWeight / 8);
                suc = ((skill - dif) * 1);
            }
            else
            {
                dif = (((resultWeight) - 500) / 75) + (((resultWeight) - 500) / 64);
                suc = (skill - (dif + 64)) + bonus;
            }

            suc += suc * bonus;

            if (suc < 0)
                suc = 0;

            if (suc > 100 && from.AccessLevel == AccessLevel.Player) //display purposes
                suc = 100;

            suc = Math.Round(suc, 2);

            return suc;
        }

        public static int GetQualityBonus(Item item)
        {
            IQuality quality = item as IQuality;

            if (quality != null)
            {
                if (quality.Quality == ItemQuality.Exceptional)
                    return 20;

                if (quality.PlayerConstructed)
                    return 10;
            }

            return 0;
        }

        /// <summary>
        /// Imbues Item with selected id
        /// </summary>
        /// <param name="from">Player Imbuing</param>
        /// <param name="i">Item to be imbued</param>
        /// <param name="id">id to be imbued, see m_Table</param>
        /// <param name="value">value for id</param>
        public static void TryImbueItem(Mobile from, Item i, int id, int value)
        {
            if (!CheckSoulForge(from, 2))
                return;

            ImbuingContext context = Imbuing.GetContext(from);

            context.LastImbued = i;
            context.Imbue_Mod = id;
            context.Imbue_ModInt = value;

            var def = ItemPropertyInfo.GetInfo(id);

            if (def == null)
                return;

            var gem = def.GemRes;
            var primary = def.PrimaryRes;
            var special = def.SpecialRes;

            context.Imbue_ModVal = def.Weight;

            var gemAmount = GetGemAmount(i, id, value);
            var primResAmount = GetPrimaryAmount(i, id, value);
            var specResAmount = GetSpecialAmount(i, id, value);

            if (from.AccessLevel == AccessLevel.Player && 
                (from.Backpack == null || from.Backpack.GetAmount(gem) < gemAmount || 
                from.Backpack.GetAmount(primary) < primResAmount || 
                from.Backpack.GetAmount(special) < specResAmount))
                from.SendLocalizedMessage(1079773); //You do not have enough resources to imbue this item.     
            else
            {
                var maxWeight = GetMaxWeight(i);
                context.Imbue_IWmax = maxWeight;

                var totalItemWeight = GetTotalWeight(i, id);
                var totalItemMods = GetTotalMods(i, id);
                var maxint = ItemPropertyInfo.GetMaxIntensity(i, def);

                int propweight = (int)(((double)def.Weight / (double)maxint) * value);

                if ((totalItemWeight + propweight) > maxWeight)
                {
                    from.SendLocalizedMessage(1079772); // You cannot imbue this item with any more item properties.
                    from.CloseGump(typeof(ImbueGump));
                    return;
                }

                double difficulty = 0;
                double success = GetSuccessChance(from, i, totalItemWeight, propweight, out difficulty);

                if (TimesImbued(i) < 20)
                {
                    from.CheckSkill(SkillName.Imbuing, difficulty - 50, difficulty + 50);
                }

                success /= 100;

                Effects.SendPacket(from, from.Map, new GraphicalEffect(EffectType.FixedFrom, from.Serial, Server.Serial.Zero, 0x375A, from.Location, from.Location, 1, 17, true, false));
                Effects.SendTargetParticles(from, 0, 1, 0, 0x1593, EffectLayer.Waist);

                if (success >= Utility.RandomDouble() || id < 0 || id > 180)
                {
                    if (from.AccessLevel == AccessLevel.Player)
                    {
                        from.Backpack.ConsumeTotal(gem, gemAmount);
                        from.Backpack.ConsumeTotal(primary, primResAmount);

                        if (specResAmount > 0)
                            from.Backpack.ConsumeTotal(special, specResAmount);
                    }


                    ImbueItem(from, i, id, value);
                } 
                else
                {
                    // This is consumed regardless of success/fail
                    if (from.AccessLevel == AccessLevel.Player)
                    {
                        from.Backpack.ConsumeTotal(primary, primResAmount);
                    }

                    from.SendLocalizedMessage(1079774); // You attempt to imbue the item, but fail.
                    from.PlaySound(0x1E4);
                }
            }

            from.EndAction(typeof(Imbuing));
        }

        public static void ImbueItem(Mobile from, Item i, int id, int value)
        {
            from.SendLocalizedMessage(1079775); // You successfully imbue the item!
            from.PlaySound(0x1EB);

            object prop = ItemPropertyInfo.GetAttribute(id);

            if (i is BaseWeapon)
            {
                BaseWeapon wep = i as BaseWeapon;
                wep.TimesImbued++;

                if (prop is AosAttribute)
                {
                    AosAttribute attr = (AosAttribute)prop;

                    if (attr == AosAttribute.SpellChanneling)
                    {
                        wep.Attributes.SpellChanneling = value;

                        if (wep.Attributes.CastSpeed >= 0)
                            wep.Attributes.CastSpeed -= 1;
                    }
                    else if (attr == AosAttribute.CastSpeed)
                    {
                        wep.Attributes.CastSpeed += value;
                    }
                    else if (attr == AosAttribute.WeaponDamage)
                    {
                        wep.Attributes.WeaponDamage = value;

                        if (!wep.DImodded)
                            wep.DImodded = true;
                    }
                    else
                        wep.Attributes[attr] = value;
                }

                if (prop is AosWeaponAttribute)
                {
                    if (id >= 30 && id <= 34)
                    {
                        wep.WeaponAttributes.HitPhysicalArea = 0;
                        wep.WeaponAttributes.HitFireArea = 0;
                        wep.WeaponAttributes.HitColdArea = 0;
                        wep.WeaponAttributes.HitPoisonArea = 0;
                        wep.WeaponAttributes.HitEnergyArea = 0;
                    }
                    else if (id >= 35 && id <= 39)
                    {
                        wep.WeaponAttributes.HitMagicArrow = 0;
                        wep.WeaponAttributes.HitHarm = 0;
                        wep.WeaponAttributes.HitFireball = 0;
                        wep.WeaponAttributes.HitLightning = 0;
                        wep.WeaponAttributes.HitDispel = 0;
                    }

                    wep.WeaponAttributes[(AosWeaponAttribute)prop] = value;
                }

                if (prop is SlayerName)
                    wep.Slayer = (SlayerName)prop;

                if (prop is SAAbsorptionAttribute)
                    wep.AbsorptionAttributes[(SAAbsorptionAttribute)prop] = value;

                if (prop is AosElementAttribute)
                {
                    AosElementAttribute attr = (AosElementAttribute)prop;

                    switch (attr)
                    {
                        case AosElementAttribute.Physical: wep.WeaponAttributes.ResistPhysicalBonus = value; break;
                        case AosElementAttribute.Fire: wep.WeaponAttributes.ResistFireBonus = value; break;
                        case AosElementAttribute.Cold: wep.WeaponAttributes.ResistColdBonus = value; break;
                        case AosElementAttribute.Poison: wep.WeaponAttributes.ResistPoisonBonus = value; break;
                        case AosElementAttribute.Energy: wep.WeaponAttributes.ResistEnergyBonus = value; break;
                    }
                }

                if (prop is string)
                {
                    string p = prop as string;

                    if (p == "WeaponVelocity" && wep is BaseRanged)
                        ((BaseRanged)wep).Velocity = value;
                }
            }
            else if (i is BaseShield)
            {
                BaseShield shield = i as BaseShield;
                shield.TimesImbued++;

                if (prop is AosWeaponAttribute && (AosWeaponAttribute)prop == AosWeaponAttribute.DurabilityBonus)
                    prop = AosArmorAttribute.DurabilityBonus;

                if (prop is AosAttribute)
                {
                    AosAttribute attr = (AosAttribute)prop;

                    if (attr == AosAttribute.SpellChanneling)
                    {
                        shield.Attributes.SpellChanneling = value;

                        if (shield.Attributes.CastSpeed >= 0)
                            shield.Attributes.CastSpeed -= 1;
                    }
                    else if (attr == AosAttribute.CastSpeed)
                    {
                        shield.Attributes.CastSpeed += value;
                    }
                    else
                        shield.Attributes[attr] = value;
                }

                if (prop is SAAbsorptionAttribute)
                    shield.AbsorptionAttributes[(SAAbsorptionAttribute)prop] = value;

                if (prop is AosArmorAttribute)
                    shield.ArmorAttributes[(AosArmorAttribute)prop] = value;
            }
            else if (i is BaseArmor)
            {
                BaseArmor arm = i as BaseArmor;
                arm.TimesImbued++;

                if (prop is AosWeaponAttribute && (AosWeaponAttribute)prop == AosWeaponAttribute.DurabilityBonus)
                    prop = AosArmorAttribute.DurabilityBonus;

                if (prop is AosAttribute)
                {
                    AosAttribute attr = (AosAttribute)prop;

                    if (attr == AosAttribute.SpellChanneling)
                    {
                        arm.Attributes.SpellChanneling = value;
                        arm.Attributes.CastSpeed -= 1;
                    }
                    else if (attr == AosAttribute.WeaponDamage)
                        arm.Attributes.WeaponDamage = value;
                    else
                        arm.Attributes[attr] = value;
                }

                if (prop is AosElementAttribute)
                {
                    AosElementAttribute attr = (AosElementAttribute)prop;

                    switch (attr)
                    {
                        case AosElementAttribute.Physical: arm.PhysicalBonus = value; arm.PhysNonImbuing = 0; break;
                        case AosElementAttribute.Fire: arm.FireBonus = value; arm.FireNonImbuing = 0; break;
                        case AosElementAttribute.Cold: arm.ColdBonus = value; arm.ColdNonImbuing = 0; break;
                        case AosElementAttribute.Poison: arm.PoisonBonus = value; arm.PoisonNonImbuing = 0; break;
                        case AosElementAttribute.Energy: arm.EnergyBonus = value; arm.EnergyNonImbuing = 0; break;
                    }
                }

                if (prop is SAAbsorptionAttribute)
                    arm.AbsorptionAttributes[(SAAbsorptionAttribute)prop] = value;

                if (prop is AosArmorAttribute)
                    arm.ArmorAttributes[(AosArmorAttribute)prop] = value;
            }
            else if (i is BaseHat)
            {
                BaseHat hat = i as BaseHat;
                hat.TimesImbued++;

                if (prop is AosAttribute)
                    hat.Attributes[(AosAttribute)prop] = value;

                if (prop is SAAbsorptionAttribute)
                    hat.SAAbsorptionAttributes[(SAAbsorptionAttribute)prop] = value;

                if (prop is AosElementAttribute)
                {
                    AosElementAttribute attr = (AosElementAttribute)prop;

                    switch (attr)
                    {
                        case AosElementAttribute.Physical: hat.Resistances.Physical = value; hat.PhysNonImbuing = 0; break;
                        case AosElementAttribute.Fire: hat.Resistances.Fire = value; hat.FireNonImbuing = 0; break;
                        case AosElementAttribute.Cold: hat.Resistances.Cold = value; hat.ColdNonImbuing = 0; break;
                        case AosElementAttribute.Poison: hat.Resistances.Poison = value; hat.PoisonNonImbuing = 0; break;
                        case AosElementAttribute.Energy: hat.Resistances.Energy = value; hat.EnergyNonImbuing = 0; break;
                    }
                }
            }
            else if (i is BaseJewel)
            {
                BaseJewel jewel = i as BaseJewel;
                jewel.TimesImbued++;

                if (jewel.MaxHitPoints <= 0 && jewel.TimesImbued >= 1)
                {
                    jewel.MaxHitPoints = 255;
                    jewel.HitPoints = 255;
                }

                if (prop is AosAttribute)
                    jewel.Attributes[(AosAttribute)prop] = value;

                if (prop is SAAbsorptionAttribute)
                    jewel.AbsorptionAttributes[(SAAbsorptionAttribute)prop] = value;

                if (prop is AosElementAttribute)
                {
                    AosElementAttribute attr = (AosElementAttribute)prop;

                    switch (attr)
                    {
                        case AosElementAttribute.Physical: jewel.Resistances.Physical = value; break;
                        case AosElementAttribute.Fire: jewel.Resistances.Fire = value; break;
                        case AosElementAttribute.Cold: jewel.Resistances.Cold = value; break;
                        case AosElementAttribute.Poison: jewel.Resistances.Poison = value; break;
                        case AosElementAttribute.Energy: jewel.Resistances.Energy = value; break;
                    }
                }

                if (prop is SkillName)
                {
                    SkillName skill = (SkillName)prop;

                    //Removes skill bonus if jewel already exist
                    for (int j = 0; j < 5; j++)
                    {
                        if (jewel.SkillBonuses.GetSkill(j) == skill)
                        {
                            jewel.SkillBonuses.SetBonus(j, 0.0);
                            jewel.SkillBonuses.SetSkill(j, SkillName.Alchemy);
                        }
                    }

                    if (id >= 151 && id <= 155)
                        jewel.SkillBonuses.SetValues(0, skill, value);
                    else if (id >= 156 && id <= 160)
                        jewel.SkillBonuses.SetValues(1, skill, value);
                    else if (id >= 161 && id <= 166)
                        jewel.SkillBonuses.SetValues(2, skill, value);
                    else if (id >= 167 && id <= 173)
                        jewel.SkillBonuses.SetValues(3, skill, value);
                    else if (id >= 174 && id <= 180)
                        jewel.SkillBonuses.SetValues(4, skill, value);
                }
            }

            var armorAttrs = RunicReforging.GetAosArmorAttributes(i);

            if (armorAttrs != null)
            {
                armorAttrs.SelfRepair = 0;
            }
            else
            {
                var wepAttrs = RunicReforging.GetAosWeaponAttributes(i);

                if (wepAttrs != null)
                {
                    wepAttrs.SelfRepair = 0;
                }
            }

            if (i is IImbuableEquipement)
            {
                ((IImbuableEquipement)i).OnAfterImbued(from, id, value);
            }

            i.InvalidateProperties();
        }

	    public static bool UnravelItem(Mobile from, Item item, bool message = true)
	    {
		    int weight = GetTotalWeight(item);
			
		    if (weight <= 0)
			{
				if (message)
				{
					// You cannot magically unravel this item. It appears to possess little or no magic.
					from.SendLocalizedMessage(1080437);
				}

				return false;
		    }

		    ImbuingContext context = GetContext(from);

		    int bonus = context.Imbue_SFBonus;
			
		    Type resType = null;
		    var resAmount = Math.Max(1, weight / 100);

			var success = false;

		    if (weight >= 480 - bonus)
			{
				if (from.Skills[SkillName.Imbuing].Value < 95.0)
				{
					if (message)
					{
						// Your Imbuing skill is not high enough to magically unravel this item.
						from.SendLocalizedMessage(1080434);
					}

					return false;
				}
				
				if (from.CheckSkill(SkillName.Imbuing, 90.1, 120.0))
				{
					success = true;
					resType = typeof(RelicFragment);
					resAmount = 1;
				}
				else if (from.CheckSkill(SkillName.Imbuing, 45.0, 95.0))
				{
					success = true;
					resType = typeof(EnchantedEssence);
					resAmount = Math.Max(1, resAmount - Utility.Random(3));
				}
			}
		    else if (weight > 200 - bonus && weight < 480 - bonus)
			{
				if (from.Skills[SkillName.Imbuing].Value < 45.0)
				{
					if (message)
					{
						// Your Imbuing skill is not high enough to magically unravel this item.
						from.SendLocalizedMessage(1080434);
					}

					return false;
				}
				
				if (from.CheckSkill(SkillName.Imbuing, 45.0, 95.0))
				{
					success = true;
					resType = typeof(EnchantedEssence);
					resAmount = Math.Max(1, resAmount);
				}
				else if (from.CheckSkill(SkillName.Imbuing, 0.0, 45.0))
				{
					success = true;
					resType = typeof(MagicalResidue);
					resAmount = Math.Max(1, resAmount + Utility.Random(2));
				}
			}
			else if (weight <= 200 - bonus)
			{
				if (from.CheckSkill(SkillName.Imbuing, 0.0, 45.0))
				{
					success = true;
					resType = typeof(MagicalResidue);
					resAmount = Math.Max(1, resAmount + Utility.Random(2));
				}
			}
			else
			{
				if (message)
				{
					// You cannot magically unravel this item. It appears to possess little or no magic.
					from.SendLocalizedMessage(1080437);
				}

				return false;
			}

		    if (!success)
		    {
			    return false;
		    }

		    Item res;

		    while (resAmount > 0)
		    {
			    res = Activator.CreateInstance(resType) as Item;

			    if (res == null)
			    {
				    break;
			    }

			    if (res.Stackable)
			    {
				    res.Amount = Math.Max(1, Math.Min(60000, resAmount));
			    }

			    resAmount -= res.Amount;

			    from.AddToBackpack(res);
		    }

		    item.Delete();

		    return true;
	    }

        public static int GetMaxWeight(object item)
        {
            int maxWeight = 450;

            IQuality quality = item as IQuality;

            if (quality != null && quality.Quality == ItemQuality.Exceptional)
                maxWeight += 50;

            if (item is BaseWeapon)
            {
                BaseWeapon itemToImbue = item as BaseWeapon;

                if (itemToImbue is BaseThrown)
                    maxWeight += 0;
                else if (itemToImbue is BaseRanged)
                    maxWeight += 50;
                else if (itemToImbue.Layer == Layer.TwoHanded)
                    maxWeight += 100;
            }
            else if (item is BaseJewel)
            {
                maxWeight = 500;
            }

            return maxWeight;
        }

        public static int GetMaxProps(Item item)
        {
            return 5;
        }

        public static int GetGemAmount(Item item, int id, int value)
        {
            int max = ItemPropertyInfo.GetMaxIntensity(item, id);
            int inc = ItemPropertyInfo.GetScale(item, id);

            //if (item is BaseJewel && id == 12)
                //max /= 2;

            if (max == 1 && inc == 0)
                return 10;

            double v = Math.Floor(value / ((double)max / 10));

            if (v > 10) v = 10;
            if (v < 1) v = 1;

            return (int)v;
        }

        public static int GetPrimaryAmount(Item item, int id, int value)
        {
            int max = ItemPropertyInfo.GetMaxIntensity(item, id);
            int inc = ItemPropertyInfo.GetScale(item, id);

            //if (item is BaseJewel && id == 12)
            //    max /= 2;

            if (max == 1 && inc == 0)
                return 5;

            double v = Math.Floor(value / ((double)max / 5.0));

            if (v > 5) v = 5;
            if (v < 1) v = 1;

            return (int)v;
        }

        public static int GetSpecialAmount(Item item, int id, int value)
        {
            int max = ItemPropertyInfo.GetMaxIntensity(item, id);

            int intensity = (int)(((double)value / (double)max) * 100);

            if (intensity >= 100)
            {
                return 10;
            }

            else if (intensity >= 1 && intensity > 90)
            {
                return intensity - 90;
            }

            return 0;
        }

        [Usage("GetTotalMods")]
        [Description("Displays the total mods, ie AOS attributes for the targeted item.")]
        public static void GetTotalMods_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(12, false, TargetFlags.None, new TargetCallback(GetTotalMods_OnTarget));
            e.Mobile.SendMessage("Target the item to get total AOS Attributes.");
        }

        public static void GetTotalMods_OnTarget(Mobile from, object targeted)
        {
            if (targeted is Item)
            {
                int ids = GetTotalMods((Item)targeted);

                ((Item)targeted).LabelTo(from, String.Format("Total Mods: {0}", ids.ToString()));
            }
            else
                from.SendMessage("That is not an item!");
        }

        public static int GetTotalMods(Item item, int id = -1)
        {
            int total = 0;
            object prop = ItemPropertyInfo.GetAttribute(id);

            if (item is BaseWeapon)
            {
                BaseWeapon wep = item as BaseWeapon;

                foreach (int i in Enum.GetValues(typeof(AosAttribute)))
                {
                    AosAttribute attr = (AosAttribute)i;

                    if(!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (wep.Attributes[attr] > 0)
                    {
                        if (!(prop is AosAttribute) || ((AosAttribute)prop) != attr)
                            total++;
                    }
                    else if (wep.Attributes[attr] == 0 && attr == AosAttribute.CastSpeed && wep.Attributes[AosAttribute.SpellChanneling] > 0)
                    {
                        if(!(prop is AosAttribute) || (AosAttribute)prop != attr)
                            total++;
                    }
                }

                total += GetSkillBonuses(wep.SkillBonuses, prop);

                foreach (long i in Enum.GetValues(typeof(AosWeaponAttribute)))
                {
                    AosWeaponAttribute attr = (AosWeaponAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (wep.WeaponAttributes[attr] > 0)
                    {
                        if (IsHitAreaOrSpell(attr, id))
                            continue;

                        if (!(prop is AosWeaponAttribute) || ((AosWeaponAttribute)prop) != attr)
                            total++;
                    }
                }

                foreach (int i in Enum.GetValues(typeof(ExtendedWeaponAttribute)))
                {
                    ExtendedWeaponAttribute attr = (ExtendedWeaponAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (wep.ExtendedWeaponAttributes[attr] > 0)
                    {
                        if (!(prop is ExtendedWeaponAttribute) || ((ExtendedWeaponAttribute)prop) != attr)
                            total++;
                    }
                }

                if (wep.Slayer != SlayerName.None && (!(prop is SlayerName) || ((SlayerName)prop) != wep.Slayer))
                    total++;

                if (wep.Slayer2 != SlayerName.None)
                    total++;

                if (wep.Slayer3 != TalismanSlayerName.None)
                    total++;

                foreach (int i in Enum.GetValues(typeof(SAAbsorptionAttribute)))
                {
                    SAAbsorptionAttribute attr = (SAAbsorptionAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (wep.AbsorptionAttributes[attr] > 0)
                    {
                        if (!(prop is SAAbsorptionAttribute) || ((SAAbsorptionAttribute)prop) != attr)
                            total++;
                    }
                }

                if (wep is BaseRanged && !(prop is string))
                {
                    BaseRanged ranged = wep as BaseRanged;

                    if (ranged.Velocity > 0 && id != 60)
                        total++;
                }

                if (wep.SearingWeapon)
                    total++;
            }
            else if (item is BaseArmor)
            {
                BaseArmor armor = item as BaseArmor;

                foreach (int i in Enum.GetValues(typeof(AosAttribute)))
                {
                    AosAttribute attr = (AosAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (armor.Attributes[attr] > 0)
                    {
                        if (!(prop is AosAttribute) || ((AosAttribute)prop) != attr)
                            total++;
                    }
                    else if (armor.Attributes[attr] == 0 && attr == AosAttribute.CastSpeed && armor.Attributes[AosAttribute.SpellChanneling] > 0)
                    {
                        if (!(prop is AosAttribute) || (AosAttribute)prop == attr)
                            total++;
                    }
                }

                total += GetSkillBonuses(armor.SkillBonuses, prop);

                if (armor.PhysicalBonus > armor.PhysNonImbuing && id != 51) { total++; }
                if (armor.FireBonus > armor.FireNonImbuing && id != 52) { total++; }
                if (armor.ColdBonus > armor.ColdNonImbuing && id != 53) { total++; }
                if (armor.PoisonBonus > armor.PoisonNonImbuing && id != 54) { total++; }
                if (armor.EnergyBonus > armor.EnergyNonImbuing && id != 55) { total++; }

                foreach (int i in Enum.GetValues(typeof(AosArmorAttribute)))
                {
                    AosArmorAttribute attr = (AosArmorAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (armor.ArmorAttributes[attr] > 0)
                    {
                        if (!(prop is AosArmorAttribute) || ((AosArmorAttribute)prop) != attr)
                            total++;
                    }
                }


                foreach (int i in Enum.GetValues(typeof(SAAbsorptionAttribute)))
                {
                    SAAbsorptionAttribute attr = (SAAbsorptionAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (armor.AbsorptionAttributes[attr] > 0)
                    {
                        if (!(prop is SAAbsorptionAttribute) || ((SAAbsorptionAttribute)prop) != attr)
                            total++;
                    }
                }
            }
            else if (item is BaseJewel)
            {
                BaseJewel j = item as BaseJewel;

                foreach (int i in Enum.GetValues(typeof(AosAttribute)))
                {
                    AosAttribute attr = (AosAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (j.Attributes[attr] > 0)
                    {
                        if (!(prop is AosAttribute) || ((AosAttribute)prop) != attr)
                            total++;
                    }
                }

                foreach (int i in Enum.GetValues(typeof(SAAbsorptionAttribute)))
                {
                    SAAbsorptionAttribute attr = (SAAbsorptionAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (j.AbsorptionAttributes[attr] > 0)
                    {
                        if (!(prop is SAAbsorptionAttribute) || ((SAAbsorptionAttribute)prop) != attr)
                            total++;
                    }
                }

                total += GetSkillBonuses(j.SkillBonuses, prop);

                if (j.Resistances.Physical > 0 && id != 51) { total++; }
                if (j.Resistances.Fire > 0 && id != 52) { total++; }
                if (j.Resistances.Cold > 0 && id != 53) { total++; }
                if (j.Resistances.Poison > 0 && id != 54) { total++; }
                if (j.Resistances.Energy > 0 && id != 55) { total++; }
            }
            else if (item is BaseHat)
            {
                BaseHat h = item as BaseHat;

                foreach (int i in Enum.GetValues(typeof(AosAttribute)))
                {
                    AosAttribute attr = (AosAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (h.Attributes[attr] > 0)
                    {
                        if (!(prop is AosAttribute) || ((AosAttribute)prop) != attr)
                            total++;
                    }
                }

                foreach (int i in Enum.GetValues(typeof(SAAbsorptionAttribute)))
                {
                    SAAbsorptionAttribute attr = (SAAbsorptionAttribute)i;

                    if (!ItemPropertyInfo.ValidateProperty(attr))
                        continue;

                    if (h.SAAbsorptionAttributes[attr] > 0)
                    {
                        if (!(prop is SAAbsorptionAttribute) || ((SAAbsorptionAttribute)prop) != attr)
                            total++;
                    }
                }

                total += GetSkillBonuses(h.SkillBonuses, prop);

                if (h.Resistances.Physical > h.PhysNonImbuing && id != 51) { total++; }
                if (h.Resistances.Fire > h.FireNonImbuing && id != 52) { total++; }
                if (h.Resistances.Cold > h.ColdNonImbuing && id != 53) { total++; }
                if (h.Resistances.Poison > h.PoisonNonImbuing && id != 54) { total++; }
                if (h.Resistances.Energy > h.EnergyNonImbuing && id != 55) { total++; }
            }

            var type = item.GetType();

            if (IsDerivedArmorOrClothing(type))
            {
                int[] resists = null;

                if (ResistBuffer != null && ResistBuffer.ContainsKey(type))
                {
                    resists = ResistBuffer[type];
                }
                else
                {
                    var baseType = type.BaseType;

                    if (IsDerivedArmorOrClothing(baseType))
                    {
                        var temp = Loot.Construct(baseType);

                        if (temp != null)
                        {
                            resists = new int[5];

                            resists[0] = GetBaseResistBonus(item, AosElementAttribute.Physical) - GetBaseResistBonus(temp, AosElementAttribute.Physical);
                            resists[1] = GetBaseResistBonus(item, AosElementAttribute.Fire) - GetBaseResistBonus(temp, AosElementAttribute.Fire);
                            resists[2] = GetBaseResistBonus(item, AosElementAttribute.Cold) - GetBaseResistBonus(temp, AosElementAttribute.Cold);
                            resists[3] = GetBaseResistBonus(item, AosElementAttribute.Poison) - GetBaseResistBonus(temp, AosElementAttribute.Poison);
                            resists[4] = GetBaseResistBonus(item, AosElementAttribute.Energy) - GetBaseResistBonus(temp, AosElementAttribute.Energy);

                            if (ResistBuffer == null)
                                ResistBuffer = new Dictionary<Type, int[]>();

                            ResistBuffer[type] = resists;
                            temp.Delete();
                        }
                    }
                }

                if (resists != null)
                {
                    for (int i = 0; i < resists.Length; i++)
                    {
                        if (id != 51 + i && resists[i] > 0)
                        {
                            total++;
                        }
                    }
                }
            }

            return total;
        }

        private static bool IsHitAreaOrSpell(AosWeaponAttribute attr, int id)
        {
            if (attr >= AosWeaponAttribute.HitMagicArrow && attr <= AosWeaponAttribute.HitDispel)
                return id >= 35 && id <= 39;
            else if (attr >= AosWeaponAttribute.HitColdArea && attr <= AosWeaponAttribute.HitPhysicalArea)
                return id >= 30 && id <= 34;
            return false;
        }

        private static bool IsInSkillGroup(SkillName skill, int index)
        {
            if (index < 0 || index >= m_SkillGroups.Length)
                return false;

            foreach (SkillName name in m_SkillGroups[index])
            {
                if (name == skill)
                    return true;
            }
            return false;
        }

        private static int GetSkillBonuses(AosSkillBonuses bonus, object prop)
        {
            int id = 0;

            for (int j = 0; j < 5; j++)
            {
                if (bonus.GetBonus(j) > 0)
                {
                    if (!(prop is SkillName) || !IsInSkillGroup((SkillName)prop, j))
                        id += 1;
                }
            }

            return id;
        }

        [Usage("GetTotalWeight")]
        [Description("Displays the total imbuing weight of the targeted item.")]
        public static void GetTotalWeight_OnCommand(CommandEventArgs e)
        {
            e.Mobile.BeginTarget(12, false, TargetFlags.None, new TargetCallback(GetTotalWeight_OnTarget));
            e.Mobile.SendMessage("Target the item to get total imbuing weight.");
        }

        public static void GetTotalWeight_OnTarget(Mobile from, object targeted)
        {
            if (targeted is Item)
            {
                int w = GetTotalWeight((Item)targeted);

                ((Item)targeted).LabelTo(from, String.Format("Imbuing Weight: {0}", w.ToString()));
            }
            else
                from.SendMessage("That is not an item!");
        }

        public static Dictionary<Type, int[]> ResistBuffer { get; private set; }

        public static int GetTotalWeight(Item item, int id = -1)
        {
            double weight = 0;

            AosAttributes aosAttrs = RunicReforging.GetAosAttributes(item);
            AosWeaponAttributes wepAttrs = RunicReforging.GetAosWeaponAttributes(item);
            SAAbsorptionAttributes saAttrs = RunicReforging.GetSAAbsorptionAttributes(item);
            AosArmorAttributes armorAttrs = RunicReforging.GetAosArmorAttributes(item);
            AosElementAttributes resistAttrs = RunicReforging.GetElementalAttributes(item);
            ExtendedWeaponAttributes extattrs = RunicReforging.GetExtendedWeaponAttributes(item);

            if (item is BaseWeapon)
            {
                if(((BaseWeapon)item).Slayer != SlayerName.None)
                    weight += GetIntensityForAttribute(item, ((BaseWeapon)item).Slayer, id, 1);

                if (((BaseWeapon)item).Slayer2 != SlayerName.None)
                    weight += GetIntensityForAttribute(item, ((BaseWeapon)item).Slayer2, id, 1);

                if (((BaseWeapon)item).Slayer3 != TalismanSlayerName.None)
                    weight += GetIntensityForAttribute(item, ((BaseWeapon)item).Slayer3, id, 1);

                if(((BaseWeapon)item).SearingWeapon)
                    weight += GetIntensityForAttribute(item, "SearingWeapon", id, 1);

                if (item is BaseRanged)
                {
                    BaseRanged ranged = item as BaseRanged;

                    if(ranged.Velocity > 0)
                        weight += GetIntensityForAttribute(item, "WeaponVelocity", id, ranged.Velocity);
                }
            }

            if (item is BaseArmor)
            {
                var arm = (BaseArmor)item;

                if (arm.PhysicalBonus > arm.PhysNonImbuing) { if (id != 51) { weight += ((double)(100.0 / 15) * (double)(arm.PhysicalBonus - arm.PhysNonImbuing)); } }
                if (arm.FireBonus > arm.FireNonImbuing) { if (id != 52) { weight += ((double)(100.0 / 15) * (double)(arm.FireBonus - arm.FireNonImbuing)); } }
                if (arm.ColdBonus > arm.ColdNonImbuing) { if (id != 53) { weight += ((double)(100.0 / 15) * (double)(arm.ColdBonus - arm.ColdNonImbuing)); } }
                if (arm.PoisonBonus > arm.PoisonNonImbuing) { if (id != 54) { weight += ((double)(100.0 / 15) * (double)(arm.PoisonBonus - arm.PoisonNonImbuing)); } }
                if (arm.EnergyBonus > arm.EnergyNonImbuing) { if (id != 55) { weight += ((double)(100.0 / 15) * (double)(arm.EnergyBonus - arm.EnergyNonImbuing)); } }
            }

            var type = item.GetType();

            if (IsDerivedArmorOrClothing(type))
            {
                int[] resists = null;

                if (ResistBuffer != null && ResistBuffer.ContainsKey(type))
                {
                    resists = ResistBuffer[type];
                }
                else
                {
                    var baseType = type.BaseType;

                    if (IsDerivedArmorOrClothing(baseType))
                    {
                        var temp = Loot.Construct(baseType);

                        if (temp != null)
                        {
                            resists = new int[5];

                            resists[0] = GetBaseResistBonus(item, AosElementAttribute.Physical) - GetBaseResistBonus(temp, AosElementAttribute.Physical);
                            resists[1] = GetBaseResistBonus(item, AosElementAttribute.Fire) - GetBaseResistBonus(temp, AosElementAttribute.Fire);
                            resists[2] = GetBaseResistBonus(item, AosElementAttribute.Cold) - GetBaseResistBonus(temp, AosElementAttribute.Cold);
                            resists[3] = GetBaseResistBonus(item, AosElementAttribute.Poison) - GetBaseResistBonus(temp, AosElementAttribute.Poison);
                            resists[4] = GetBaseResistBonus(item, AosElementAttribute.Energy) - GetBaseResistBonus(temp, AosElementAttribute.Energy);

                            if (ResistBuffer == null)
                                ResistBuffer = new Dictionary<Type, int[]>();

                            ResistBuffer[type] = resists;
                            temp.Delete();
                        }
                    }
                }

                if (resists != null)
                {
                    for (int i = 0; i < resists.Length; i++)
                    {
                        if(id != 51 + i && resists[i] > 0)
                        {
                            weight += ((double)(100.0 / 15) * (double)resists[i]);
                        }
                    }
                }
            }

            if (aosAttrs != null)
                foreach (int i in Enum.GetValues(typeof(AosAttribute)))
                    weight += GetIntensityForAttribute(item, (AosAttribute)i, id, aosAttrs[(AosAttribute)i]);

            if (wepAttrs != null)
                foreach (long i in Enum.GetValues(typeof(AosWeaponAttribute)))
                    weight += GetIntensityForAttribute(item, (AosWeaponAttribute)i, id, wepAttrs[(AosWeaponAttribute)i]);

            if (saAttrs != null)
                foreach (int i in Enum.GetValues(typeof(SAAbsorptionAttribute)))
                    weight += GetIntensityForAttribute(item, (SAAbsorptionAttribute)i, id, saAttrs[(SAAbsorptionAttribute)i]);

            if (armorAttrs != null)
                foreach (int i in Enum.GetValues(typeof(AosArmorAttribute)))
                    weight += GetIntensityForAttribute(item, (AosArmorAttribute)i, id, armorAttrs[(AosArmorAttribute)i]);

            if (resistAttrs != null && !(item is BaseWeapon))
                foreach (int i in Enum.GetValues(typeof(AosElementAttribute)))
                    weight += GetIntensityForAttribute(item, (AosElementAttribute)i, id, resistAttrs[(AosElementAttribute)i]);

            if(extattrs != null)
                foreach (int i in Enum.GetValues(typeof(ExtendedWeaponAttribute)))
                    weight += GetIntensityForAttribute(item, (ExtendedWeaponAttribute)i, id, extattrs[(ExtendedWeaponAttribute)i]);

            weight += CheckSkillBonuses(item, id);

            return (int)weight;
        }

        public static int[] GetBaseResists(Item item)
        {
            int[] resists;

            // Special items base resist don't count as a property or weight. Once that resist is imbued, 
            // it then uses the base class resistance as the base resistance. EA is stupid.
            if (item is IImbuableEquipement && IsSpecialImbuable(item))
            {
                resists = ((IImbuableEquipement)item).BaseResists;
            }
            else
            {
                resists = new int[5];

                resists[0] = GetBaseResistBonus(item, AosElementAttribute.Physical);
                resists[1] = GetBaseResistBonus(item, AosElementAttribute.Fire);
                resists[2] = GetBaseResistBonus(item, AosElementAttribute.Cold);
                resists[3] = GetBaseResistBonus(item, AosElementAttribute.Poison);
                resists[4] = GetBaseResistBonus(item, AosElementAttribute.Energy);
            }

            return resists;
        }

        private static int GetBaseResistBonus(Item item, AosElementAttribute resist)
        {
            switch (resist)
            {
                case AosElementAttribute.Physical:
                    {
                        if (item is BaseArmor)
                            return ((BaseArmor)item).BasePhysicalResistance;

                        if (item is BaseClothing)
                            return ((BaseClothing)item).BasePhysicalResistance;

                        break;
                    }
                case AosElementAttribute.Fire:
                    {
                        if (item is BaseArmor)
                            return ((BaseArmor)item).BaseFireResistance;

                        if (item is BaseClothing)
                            return ((BaseClothing)item).BaseFireResistance;

                        break;
                    }
                case AosElementAttribute.Cold:
                    {
                        if (item is BaseArmor)
                            return ((BaseArmor)item).BaseColdResistance;

                        if (item is BaseClothing)
                            return ((BaseClothing)item).BaseColdResistance;

                        break;
                    }
                case AosElementAttribute.Poison:
                    {
                        if (item is BaseArmor)
                            return ((BaseArmor)item).BasePoisonResistance;

                        if (item is BaseClothing)
                            return ((BaseClothing)item).BasePoisonResistance;

                        break;
                    }
                case AosElementAttribute.Energy:
                    {
                        if (item is BaseArmor)
                            return ((BaseArmor)item).BaseEnergyResistance;

                        if (item is BaseClothing)
                            return ((BaseClothing)item).BaseEnergyResistance;

                        break;
                    }
            }

            return 0;
        }

        /// <summary>
        /// This is for special items such as artifacts, if you ever so chose to imbue them on your server. Without
        /// massive edits, this should never come back as true.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsDerivedArmorOrClothing(Type type)
        {
            if (IsSpecialImbuable(type))
            {
                return false;
            }

            return (type.IsSubclassOf(typeof(BaseClothing)) || type.IsSubclassOf(typeof(BaseArmor))) &&
                type != typeof(BaseHat) &&
                type != typeof(BaseShield) &&
                type != typeof(Item) &&
                type != typeof(BaseOuterTorso) &&
                type != typeof(BaseMiddleTorso) &&
                type != typeof(BaseOuterLegs) &&
                type != typeof(BasePants) &&
                type != typeof(BaseShirt) &&
                type != typeof(BaseWaist) &&
                type != typeof(BaseShoes) &&
                type != typeof(BaseCloak);
        }

        private static int CheckSkillBonuses(Item item, int modification)
        {
            double weight = 0;
            int id = -1;

            AosSkillBonuses skills = RunicReforging.GetAosSkillBonuses(item);

            if (item is BaseJewel)
            {
                id = modification;
            }

            if (skills != null)
            {
                if (skills.GetBonus(0) > 0) { if (id < 151 || id > 155) { weight += ((double)(140.0 / 15.0) * (double)skills.GetBonus(0)); } }
                if (skills.GetBonus(1) > 0) { if (id < 156 || id > 160) { weight += ((double)(140.0 / 15.0) * (double)skills.GetBonus(1)); } }
                if (skills.GetBonus(2) > 0) { if (id < 161 || id > 166) { weight += ((double)(140.0 / 15.0) * (double)skills.GetBonus(2)); } }
                if (skills.GetBonus(3) > 0) { if (id < 167 || id > 173) { weight += ((double)(140.0 / 15.0) * (double)skills.GetBonus(3)); } }
                if (skills.GetBonus(4) > 0) { if (id < 174 || id > 180) { weight += ((double)(140.0 / 15.0) * (double)skills.GetBonus(4)); } }
            }

            return (int)weight;
        }

        public static SkillName[] PossibleSkills { get { return m_PossibleSkills; } }
        private static SkillName[] m_PossibleSkills = new SkillName[]
			{
				SkillName.Swords,
				SkillName.Fencing,
				SkillName.Macing,
				SkillName.Archery,
				SkillName.Wrestling,
				SkillName.Parry,
				SkillName.Tactics,
				SkillName.Anatomy,
				SkillName.Healing,
				SkillName.Magery,
				SkillName.Meditation,
				SkillName.EvalInt,
				SkillName.MagicResist,
				SkillName.AnimalTaming,
				SkillName.AnimalLore,
				SkillName.Veterinary,
				SkillName.Musicianship,
				SkillName.Provocation,
				SkillName.Discordance,
				SkillName.Peacemaking,
				SkillName.Chivalry,
				SkillName.Focus,
				SkillName.Necromancy,
				SkillName.Stealing,
				SkillName.Stealth,
				SkillName.SpiritSpeak,
				SkillName.Bushido,
				SkillName.Ninjitsu,
                SkillName.Throwing,
                SkillName.Mysticism
			};

        private static SkillName[][] m_SkillGroups = new SkillName[][]
        {
            new SkillName[] { SkillName.Fencing, SkillName.Macing, SkillName.Swords, SkillName.Musicianship, SkillName.Magery },
            new SkillName[] { SkillName.Wrestling, SkillName.AnimalTaming, SkillName.SpiritSpeak, SkillName.Tactics, SkillName.Provocation },
            new SkillName[] { SkillName.Focus, SkillName.Parry, SkillName.Stealth, SkillName.Meditation, SkillName.AnimalLore, SkillName.Discordance },
            new SkillName[] { SkillName.Mysticism, SkillName.Bushido, SkillName.Necromancy, SkillName.Veterinary, SkillName.Stealing, SkillName.EvalInt, SkillName.Anatomy },
            new SkillName[] { SkillName.Peacemaking, SkillName.Ninjitsu, SkillName.Chivalry, SkillName.Archery, SkillName.MagicResist, SkillName.Healing, SkillName.Throwing }
        };
        
        public static bool CheckSoulForge(Mobile from, int range)
        {
            return CheckSoulForge(from, range, true);
        }

        public static bool CheckSoulForge(Mobile from, int range, bool message, bool checkqueen = true)
        {
            PlayerMobile m = from as PlayerMobile;

            ImbuingContext context = Imbuing.GetContext(m);
            context.Imbue_SFBonus = 0;
            Map map = from.Map;

            if (map == null)
                return false;

            bool isForge = false;

            IPooledEnumerable eable = map.GetItemsInRange(from.Location, range);

            foreach (Item item in eable)
            {
                if ((item.ItemID >= 0x4277 && item.ItemID <= 0x4286) || (item.ItemID >= 0x4263 && item.ItemID <= 0x4272) || (item.ItemID >= 17607 && item.ItemID <= 17610))
				{
					isForge = true;
					break;
				}
            }

            eable.Free();

			if (!isForge)
			{
				if (message)
					from.SendLocalizedMessage(1079787); // You must be near a soulforge to imbue an item.

				return false;
			}

            if (checkqueen)
            {
                if (from.Region != null && from.Region.IsPartOf("Queen's Palace"))
                {
                    if (!Server.Engines.Points.PointsSystem.QueensLoyalty.IsNoble(from))
                    {
                        if (message)
                        {
                            from.SendLocalizedMessage(1113736); // You must rise to the rank of noble in the eyes of the Gargoyle Queen before her majesty will allow you to use this soulforge.
                        }

                        return false;
                    }
                    else
                    {
                        context.Imbue_SFBonus = 10;
                    }
                }
                else if (from.Region != null && from.Region.IsPartOf("Royal City"))
                {
                    context.Imbue_SFBonus = 5;
                }
            }

            return true;
        }

        /*private static Dictionary<int, ImbuingDefinition> m_Table;
        public static Dictionary<int, ImbuingDefinition> Table { get { return m_Table; } }

        static Imbuing()
        {
            m_Table = new Dictionary<int, ImbuingDefinition>();
			
			m_Table[1] = new ImbuingDefinition(AosAttribute.DefendChance, 	        1075620, 110,	typeof(RelicFragment), 	typeof(Tourmaline), 	typeof(EssenceSingularity), 15, 1, 1111947, true, true, false, true, true);
			m_Table[2] = new ImbuingDefinition(AosAttribute.AttackChance, 	        1075616, 130, 	typeof(EnchantedEssence), typeof(Tourmaline), 	typeof(EssencePrecision), 	15, 1, 1111958, true, true, false, true, true);
			m_Table[3] = new ImbuingDefinition(AosAttribute.RegenHits,    	        1075627, 100, 	typeof(EnchantedEssence), typeof(Tourmaline),   typeof(SeedOfRenewal), 	    2, 1, 1111994, false, false, true, false, false);
            m_Table[4] = new ImbuingDefinition(AosAttribute.RegenStam,    	        1079411, 100, 	typeof(EnchantedEssence), typeof(Diamond), 		typeof(SeedOfRenewal), 	    3, 1, 1112043, false, false, true, false, false);
			m_Table[5] = new ImbuingDefinition(AosAttribute.RegenMana,      	    1079410, 100, 	typeof(EnchantedEssence), typeof(Sapphire), 	typeof(SeedOfRenewal), 	    2, 1, 1112003, false, false, true, false, false);
			m_Table[6] = new ImbuingDefinition(AosAttribute.BonusStr,     	        1079767, 110, 	typeof(EnchantedEssence), typeof(Diamond), 		typeof(FireRuby), 		    8, 1, 1112044, false, false, false, false, true);
			m_Table[7] = new ImbuingDefinition(AosAttribute.BonusDex,     	        1079732, 110, 	typeof(EnchantedEssence), typeof(Ruby), 		typeof(BlueDiamond), 	    8, 1, 1111948, false, false, false, false, true);
			m_Table[8] = new ImbuingDefinition(AosAttribute.BonusInt,     	        1079756, 110, 	typeof(EnchantedEssence), typeof(Tourmaline), 	typeof(Turquoise), 		     8, 1, 1111995, false, false, false, false, true);
			m_Table[9] = new ImbuingDefinition(AosAttribute.BonusHits,    	        1075630, 110, 	typeof(EnchantedEssence), typeof(Ruby),			typeof(LuminescentFungi),    5, 1, 1111993, false, false, true, false, false);
			m_Table[10] = new ImbuingDefinition(AosAttribute.BonusStam,     	    1075632, 110, 	typeof(EnchantedEssence), typeof(Diamond), 		typeof(LuminescentFungi),   8, 1, 1112042, false, false, true, false, false);
			m_Table[11] = new ImbuingDefinition(AosAttribute.BonusMana,   	        1075631, 110, 	typeof(EnchantedEssence), typeof(Sapphire), 	typeof(LuminescentFungi),   8, 1, 1112002, false, false, true, false, false);
			m_Table[12] = new ImbuingDefinition(AosAttribute.WeaponDamage,          1079399, 100, 	typeof(EnchantedEssence), typeof(Citrine), 		typeof(CrystalShards), 	    50, 1, 1112005, true, true, false, false, true);
			m_Table[13] = new ImbuingDefinition(AosAttribute.WeaponSpeed, 	        1075629, 110, 	typeof(RelicFragment), 	typeof(Tourmaline), 	typeof(EssenceControl),     30, 5, 1112045, true, true, false, false, false);
			m_Table[14] = new ImbuingDefinition(AosAttribute.SpellDamage,           1075628, 100, 	typeof(EnchantedEssence), typeof(Emerald), 		typeof(CrystalShards), 	    12, 1, 1112041, false, false, false, false, true);
			m_Table[15] = new ImbuingDefinition(AosAttribute.CastRecovery,          1075618, 120, 	typeof(RelicFragment), 	typeof(Amethyst), 	    typeof(EssenceDiligence),   3, 1, 1111952, false, false, false, false, true);
			m_Table[16] = new ImbuingDefinition(AosAttribute.CastSpeed,             1075617, 140, 	typeof(RelicFragment),  typeof(Ruby), 		    typeof(EssenceAchievement), 1, 1, 1111951, false, false, false, true, true);
			m_Table[17] = new ImbuingDefinition(AosAttribute.LowerManaCost,         1075621, 110,	typeof(RelicFragment),  typeof(Tourmaline), 	typeof(EssenceOrder), 	    8, 1, 1111996, false, false, true, false, true);
			m_Table[18] = new ImbuingDefinition(AosAttribute.LowerRegCost,          1075625, 100,	typeof(MagicalResidue), typeof(Amber), 		    typeof(FaeryDust), 	        20, 1, 1111997, false, false, true, false, true);
			m_Table[19] = new ImbuingDefinition(AosAttribute.ReflectPhysical,       1075626, 100, 	typeof(MagicalResidue), typeof(Citrine), 		typeof(ReflectiveWolfEye),  15, 1, 1112006, false, false, true, true, false);
			m_Table[20] = new ImbuingDefinition(AosAttribute.EnhancePotions,        1075624, 100, 	typeof(EnchantedEssence), typeof(Citrine), 		typeof(CrushedGlass), 	    25, 5, 1111950, false, false, false, false, true);
			m_Table[21] = new ImbuingDefinition(AosAttribute.Luck, 			        1061153, 100, 	typeof(MagicalResidue), typeof(Citrine), 		typeof(ChagaMushroom), 	    100, 1, 1111999, true, true, true, false, true);
			m_Table[22] = new ImbuingDefinition(AosAttribute.SpellChanneling,       1079766, 100, 	typeof(MagicalResidue), typeof(Diamond), 		typeof(SilverSnakeSkin),    1, 0, 1112040, true, true, false, true, false);
			m_Table[23] = new ImbuingDefinition(AosAttribute.NightSight, 	        1015168, 50, 	typeof(MagicalResidue), typeof(Tourmaline), 	typeof(BottleIchor), 	    1, 0, 1112004, false, false, true, false, true);

			m_Table[24] = new ImbuingDefinition(AosWeaponAttribute.LowerStatReq,	1079757, 100, 	typeof(EnchantedEssence), typeof(Amethyst), 	typeof(ElvenFletching),     100, 10, 1111998, true, true, true, true, false);
			m_Table[25] = new ImbuingDefinition(AosWeaponAttribute.HitLeechHits,  	1079698, 110, 	typeof(MagicalResidue), typeof(Ruby), 		    typeof(VoidOrb), 	        50, 2, 1111964, true, true, false, false, false);
			m_Table[26] = new ImbuingDefinition(AosWeaponAttribute.HitLeechStam,    1079707, 100, 	typeof(MagicalResidue), typeof(Diamond), 		typeof(VoidOrb), 	        50, 2, 1111992, true, true, false, false, false);
			m_Table[27] = new ImbuingDefinition(AosWeaponAttribute.HitLeechMana,    1079701, 110, 	typeof(MagicalResidue), typeof(Sapphire), 	    typeof(VoidOrb), 	        50, 2, 1111967, true, true, false, false, false);
			m_Table[28] = new ImbuingDefinition(AosWeaponAttribute.HitLowerAttack,  1079699, 110, 	typeof(EnchantedEssence), typeof(Emerald),		typeof(ParasiticPlant),     50, 2, 1111965, true, true, false, false, false);
			m_Table[29] = new ImbuingDefinition(AosWeaponAttribute.HitLowerDefend,  1079700, 130, 	typeof(EnchantedEssence), typeof(Tourmaline), 	typeof(ParasiticPlant),     50, 2, 1111966, true, true, false, false, false);
			m_Table[30] = new ImbuingDefinition(AosWeaponAttribute.HitPhysicalArea, 1079696, 100, 	typeof(MagicalResidue), typeof(Diamond), 		typeof(RaptorTeeth),        50, 2, 1111956, true, true, false, false, false);
			m_Table[31] = new ImbuingDefinition(AosWeaponAttribute.HitFireArea,  	1079695, 100, 	typeof(MagicalResidue), typeof(Ruby), 		    typeof(RaptorTeeth),        50, 2, 1111955, true, true, false, false, false);
			m_Table[32] = new ImbuingDefinition(AosWeaponAttribute.HitColdArea, 	1079693, 100, 	typeof(MagicalResidue), typeof(Sapphire), 	    typeof(RaptorTeeth),        50, 2, 1111953, true, true, false, false, false);
			m_Table[33] = new ImbuingDefinition(AosWeaponAttribute.HitPoisonArea,   1079697, 100, 	typeof(MagicalResidue), typeof(Emerald), 		typeof(RaptorTeeth),        50, 2, 1111957, true, true, false, false, false);
			m_Table[34] = new ImbuingDefinition(AosWeaponAttribute.HitEnergyArea,  	1079694, 100, 	typeof(MagicalResidue), typeof(Amethyst), 	    typeof(RaptorTeeth),        50, 2, 1111954, true, true, false, false, false);
			m_Table[35] = new ImbuingDefinition(AosWeaponAttribute.HitMagicArrow,   1079706, 120, 	typeof(RelicFragment),  typeof(Amber), 		    typeof(EssenceFeeling),     50, 2, 1111963, true, true, false, false, false);
			m_Table[36] = new ImbuingDefinition(AosWeaponAttribute.HitHarm, 		1079704, 110,	typeof(EnchantedEssence), typeof(Emerald), 		typeof(ParasiticPlant),     50, 2, 1111961, true, true, false, false, false);
			m_Table[37] = new ImbuingDefinition(AosWeaponAttribute.HitFireball,  	1079703, 140,	typeof(EnchantedEssence), typeof(Ruby), 		typeof(FireRuby), 	        50, 2, 1111960, true, true, false, false, false);
			m_Table[38] = new ImbuingDefinition(AosWeaponAttribute.HitLightning,	1079705, 140, 	typeof(RelicFragment),  typeof(Amethyst), 	    typeof(EssencePassion),     50, 2, 1111962, true, true, false, false, false);
			m_Table[39] = new ImbuingDefinition(AosWeaponAttribute.HitDispel,		1079702, 100, 	typeof(MagicalResidue), typeof(Amber), 		    typeof(SlithTongue),        50, 2, 1111959, true, true, false, false, false);
			m_Table[40] = new ImbuingDefinition(AosWeaponAttribute.UseBestSkill, 	1079592, 150, 	typeof(EnchantedEssence), typeof(Amber), 		    typeof(DelicateScales), 1, 0, 1111946, true, false, false, false, false);
			m_Table[41] = new ImbuingDefinition(AosWeaponAttribute.MageWeapon,		1079759, 100, 	typeof(EnchantedEssence), typeof(Emerald), 		typeof(ArcanicRuneStone),   10, 1, 1112001, true, true, false, false, false);
			m_Table[42] = new ImbuingDefinition(AosWeaponAttribute.DurabilityBonus,	1017323, 100, 	typeof(EnchantedEssence), typeof(Diamond), 		typeof(PowderedIron),       100, 10, 1111949, true, true, false, false, false);

            m_Table[49] = new ImbuingDefinition(AosArmorAttribute.MageArmor,        1079758, 0,     typeof(EnchantedEssence), typeof(Diamond),        typeof(AbyssalCloth), 1, 0, 1112000, false, false, true, false, false);

            m_Table[51] = new ImbuingDefinition(AosElementAttribute.Physical,       1061158, 100,   typeof(MagicalResidue), typeof(Diamond),        typeof(BouraPelt), 15, 1, 1112010, true, true, true, false, true);
            m_Table[52] = new ImbuingDefinition(AosElementAttribute.Fire,           1061159, 100,   typeof(MagicalResidue), typeof(Ruby),           typeof(BouraPelt), 15, 1, 1112009, true, true, true, false, true);
            m_Table[53] = new ImbuingDefinition(AosElementAttribute.Cold,           1061160, 100,   typeof(MagicalResidue), typeof(Sapphire),       typeof(BouraPelt), 15, 1, 1112007, true, true, true, false, true);
            m_Table[54] = new ImbuingDefinition(AosElementAttribute.Poison,         1061161, 100,   typeof(MagicalResidue), typeof(Emerald),        typeof(BouraPelt), 15, 1, 1112011, true, true, true, false, true);
            m_Table[55] = new ImbuingDefinition(AosElementAttribute.Energy,         1061162, 100,   typeof(MagicalResidue), typeof(Amethyst),       typeof(BouraPelt), 15, 1, 1112008, true, true, true, false, true);
            
            m_Table[60] = new ImbuingDefinition("WeaponVelocity",                   1080416, 130, 	typeof(RelicFragment),  typeof(Tourmaline), 	typeof(EssenceDirection),   50, 2, 1112048, false, true, false, false, false);
			m_Table[61] = new ImbuingDefinition(AosAttribute.BalancedWeapon,	    1072792, 150, 	typeof(RelicFragment),  typeof(Amber), 		    typeof(EssenceBalance),     1, 0, 1112047, false, true, false, false, false);
            m_Table[62] = new ImbuingDefinition("SearingWeapon",	                1151183, 150, 	null,                   null, 		            null,                       1, 0, -1, true, false, false, false, false);

			m_Table[101] = new ImbuingDefinition(SlayerName.OrcSlaying,         1079741, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111977, true, true);
			m_Table[102] = new ImbuingDefinition(SlayerName.TrollSlaughter,     1079754, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111990, true, true);
			m_Table[103] = new ImbuingDefinition(SlayerName.OgreTrashing,       1079739, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111975, true, true);
			m_Table[104] = new ImbuingDefinition(SlayerName.DragonSlaying,      1061284, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111970, true, true);
			m_Table[105] = new ImbuingDefinition(SlayerName.Terathan,           1079753, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111989, true, true);
			m_Table[106] = new ImbuingDefinition(SlayerName.SnakesBane,         1079744, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111980, true, true);
			m_Table[107] = new ImbuingDefinition(SlayerName.LizardmanSlaughter, 1079738, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111974, true, true);
			//m_Table[108] = new ImbuingDefinition(SlayerName.DaemonDismissal,  	 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl), 1, 0, 1112984);  //check
			m_Table[108] = new ImbuingDefinition(SlayerName.GargoylesFoe,       1079737, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111973, true, true);
			//m_Table[110] = new ImbuingDefinition(SlayerName.BalronDamnation,   	 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl), 1, 0, 1112001);  //check
			m_Table[111] = new ImbuingDefinition(SlayerName.Ophidian,           1079740, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111976, true, true);
			m_Table[112] = new ImbuingDefinition(SlayerName.SpidersDeath,       1079746, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111982, true, true);
			m_Table[113] = new ImbuingDefinition(SlayerName.ScorpionsBane,      1079743, 100,	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111979, true, true);
			m_Table[114] = new ImbuingDefinition(SlayerName.FlameDousing,       1079736, 100,	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111972, true, true);
			m_Table[115] = new ImbuingDefinition(SlayerName.WaterDissipation,   1079755, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111991, true, true);
			m_Table[116] = new ImbuingDefinition(SlayerName.Vacuum,             1079733, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111968, true, true);
			m_Table[117] = new ImbuingDefinition(SlayerName.ElementalHealth,    1079742, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111978, true, true);
			m_Table[118] = new ImbuingDefinition(SlayerName.EarthShatter,       1079735, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111971, true, true);
			m_Table[119] = new ImbuingDefinition(SlayerName.BloodDrinking,      1079734, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111969, true, true);
			m_Table[120] = new ImbuingDefinition(SlayerName.SummerWind,         1079745, 100, 	typeof(MagicalResidue), typeof(Emerald),            typeof(WhitePearl),         1, 0, 1111981, true, true);
		
            //Super Slayers
			m_Table[121] = new ImbuingDefinition(SlayerName.Silver,             1079752, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(UndyingFlesh), 	    1, 0, 1111988, true, true);
			m_Table[122] = new ImbuingDefinition(SlayerName.Repond,             1079750, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(GoblinBlood), 	    1, 0, 1111986, true, true);
            m_Table[123] = new ImbuingDefinition(SlayerName.ReptilianDeath,     1079751, 130,   typeof(RelicFragment),      typeof(Ruby),           typeof(LavaSerpentCrust),   1, 0, 1111987, true, true);
			m_Table[124] = new ImbuingDefinition(SlayerName.Exorcism,           1079748, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(DaemonClaw), 	    1, 0, 1111984, true, true);
			m_Table[125] = new ImbuingDefinition(SlayerName.ArachnidDoom,       1079747, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(SpiderCarapace),     1, 0, 1111983, true, true);
			m_Table[126] = new ImbuingDefinition(SlayerName.ElementalBan,       1079749, 130, 	typeof(RelicFragment),      typeof(Ruby), 		    typeof(VialOfVitriol), 	    1, 0, 1111985, true, true);
            m_Table[127] = new ImbuingDefinition(SlayerName.Fey,                1154652, 130,   typeof(RelicFragment),      typeof(Ruby),           typeof(FeyWings),           1, 0, 1154652, true, true);

            m_Table[128] = new ImbuingDefinition(SlayerName.Dinosaur,           1156240, 130, null, null, null, 1, 0, -1, true, true);
            m_Table[129] = new ImbuingDefinition(SlayerName.Myrmidex,           1156241, 130, null, null, null, 1, 0, -1, true, true);
            m_Table[130] = new ImbuingDefinition(SlayerName.Eodon,              1156126, 130, null, null, null, 1, 0, -1, true, true);
            m_Table[131] = new ImbuingDefinition(SlayerName.EodonTribe,         1156347, 130, null, null, null, 1, 0, -1, true, true);

            // Talisman Slayers
            m_Table[135] = new ImbuingDefinition(TalismanSlayerName.Bear,       1072504, 130, null, null, null, 1, 0, 0);
            m_Table[136] = new ImbuingDefinition(TalismanSlayerName.Vermin,     1072505, 130, null, null, null, 1, 0, 0);
            m_Table[137] = new ImbuingDefinition(TalismanSlayerName.Bat,        1072506, 130, null, null, null, 1, 0, 0);
            m_Table[138] = new ImbuingDefinition(TalismanSlayerName.Mage,       1072507, 130, null, null, null, 1, 0, 0);
            m_Table[139] = new ImbuingDefinition(TalismanSlayerName.Beetle,     1072508, 130, null, null, null, 1, 0, 0);
            m_Table[140] = new ImbuingDefinition(TalismanSlayerName.Bird,       1072509, 130, null, null, null, 1, 0, 0);
            m_Table[141] = new ImbuingDefinition(TalismanSlayerName.Ice,        1072510, 130, null, null, null, 1, 0, 0);
            m_Table[142] = new ImbuingDefinition(TalismanSlayerName.Flame,      1072511, 130, null, null, null, 1, 0, 0);
            m_Table[143] = new ImbuingDefinition(TalismanSlayerName.Bovine,     1072512, 130, null, null, null, 1, 0, 0);
            m_Table[144] = new ImbuingDefinition(TalismanSlayerName.Wolf,       1075462, 130, null, null, null, 1, 0, 0);
            m_Table[145] = new ImbuingDefinition(TalismanSlayerName.Undead,     1079752, 130, null, null, null, 1, 0, 0);
            m_Table[146] = new ImbuingDefinition(TalismanSlayerName.Goblin,     1095010, 130, null, null, null, 1, 0, 0);

            m_Table[151] = new ImbuingDefinition(SkillName.Fencing,		 	    1044102, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112012, jewels: true);
			m_Table[152] = new ImbuingDefinition(SkillName.Macing, 	            1044101, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112013, jewels: true);
			m_Table[153] = new ImbuingDefinition(SkillName.Swords,	            1044100, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112016, jewels: true);
			m_Table[154] = new ImbuingDefinition(SkillName.Musicianship,	    1044089, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112015, jewels: true);
			m_Table[155] = new ImbuingDefinition(SkillName.Magery,			    1044085, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112014, jewels: true);
			
			m_Table[156] = new ImbuingDefinition(SkillName.Wrestling,		    1044103, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112021, jewels: true);
			m_Table[157] = new ImbuingDefinition(SkillName.AnimalTaming, 	    1044095, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112017, jewels: true);
			m_Table[158] = new ImbuingDefinition(SkillName.SpiritSpeak,		    1044092, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112019, jewels: true);
			m_Table[159] = new ImbuingDefinition(SkillName.Tactics,			    1044087, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112020, jewels: true);
			m_Table[160] = new ImbuingDefinition(SkillName.Provocation,		    1044082, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112018, jewels: true);
			
			m_Table[161] = new ImbuingDefinition(SkillName.Focus,			    1044110, 140, 	typeof(EnchantedEssence),	  typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112024, jewels: true);
			m_Table[162] = new ImbuingDefinition(SkillName.Parry, 		        1044065, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112026, jewels: true);
			m_Table[163] = new ImbuingDefinition(SkillName.Stealth,			    1044107, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112027, jewels: true);
			m_Table[164] = new ImbuingDefinition(SkillName.Meditation,		    1044106, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112025, jewels: true);
			m_Table[165] = new ImbuingDefinition(SkillName.AnimalLore,		    1044062, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112022, jewels: true);
			m_Table[166] = new ImbuingDefinition(SkillName.Discordance,		    1044075, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112023, jewels: true);
			
            m_Table[167] = new ImbuingDefinition(SkillName.Mysticism,			1044115, 140, 	typeof(EnchantedEssence),	  typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1115213, jewels: true);
			m_Table[168] = new ImbuingDefinition(SkillName.Bushido,			    1044112, 140, 	typeof(EnchantedEssence),	  typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112029, jewels: true);
			m_Table[169] = new ImbuingDefinition(SkillName.Necromancy, 		    1044109, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112031, jewels: true);
			m_Table[170] = new ImbuingDefinition(SkillName.Veterinary,		    1044099, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112033, jewels: true);
			m_Table[171] = new ImbuingDefinition(SkillName.Stealing,		    1044093, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112032, jewels: true);
			m_Table[172] = new ImbuingDefinition(SkillName.EvalInt, 		    1044076, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112030, jewels: true);
			m_Table[173] = new ImbuingDefinition(SkillName.Anatomy,			    1044061, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112028, jewels: true);
			
			m_Table[174] = new ImbuingDefinition(SkillName.Peacemaking,		    1044069, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112038, jewels: true);
			m_Table[175] = new ImbuingDefinition(SkillName.Ninjitsu, 		    1044113, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112037, jewels: true);
			m_Table[176] = new ImbuingDefinition(SkillName.Chivalry,		    1044111, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112035, jewels: true);
			m_Table[177] = new ImbuingDefinition(SkillName.Archery,			    1044091, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112034, jewels: true);
			m_Table[178] = new ImbuingDefinition(SkillName.MagicResist,	        1044086, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112039, jewels: true);
			m_Table[179] = new ImbuingDefinition(SkillName.Healing,			    1044077, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1112036, jewels: true);
            m_Table[180] = new ImbuingDefinition(SkillName.Throwing,			1044117, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1115212, jewels: true);
            
            m_Table[181] = new ImbuingDefinition(SkillName.Lumberjacking,	    1002100, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1002101, jewels: true);
			m_Table[182] = new ImbuingDefinition(SkillName.Snooping,			1002138, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1002139, jewels: true);
            m_Table[183] = new ImbuingDefinition(SkillName.Mining,			    1002111, 140, 	typeof(EnchantedEssence),   typeof(StarSapphire), 	typeof(CrystallineBlackrock),  15, 1, 1002112, jewels: true);

            // Non-Imbuables for getting item intensity only
            m_Table[200] = new ImbuingDefinition(AosWeaponAttribute.BloodDrinker,           1017407, 140, null, null, null, 1, 1, 1152387);
            m_Table[201] = new ImbuingDefinition(AosWeaponAttribute.BattleLust,             1113710, 140, null, null, null, 1, 1, 1152385);
            m_Table[202] = new ImbuingDefinition(AosWeaponAttribute.HitCurse,               1154673, 140, null, null, null, 50, 1, 1152438);
            m_Table[203] = new ImbuingDefinition(AosWeaponAttribute.HitFatigue,             1154668, 140, null, null, null, 50, 1, 1152437);
            m_Table[204] = new ImbuingDefinition(AosWeaponAttribute.HitManaDrain,           1154669, 140, null, null, null, 50, 1, 1152436);
            m_Table[205] = new ImbuingDefinition(AosWeaponAttribute.SplinteringWeapon,      1154670, 140, null, null, null, 20, 1, 1152396);
            m_Table[206] = new ImbuingDefinition(AosWeaponAttribute.ReactiveParalyze,       1154660, 140, null, null, null, 1, 1, 1152400);

            m_Table[233] = new ImbuingDefinition(AosWeaponAttribute.ResistPhysicalBonus,    1061158, 100, typeof(MagicalResidue), typeof(Diamond),  typeof(BouraPelt), 15, 1, 1112010);
            m_Table[234] = new ImbuingDefinition(AosWeaponAttribute.ResistFireBonus,        1061159, 100, typeof(MagicalResidue), typeof(Ruby),     typeof(BouraPelt), 15, 1, 1112009);
            m_Table[235] = new ImbuingDefinition(AosWeaponAttribute.ResistColdBonus,        1061160, 100, typeof(MagicalResidue), typeof(Sapphire), typeof(BouraPelt), 15, 1, 1112007);
            m_Table[236] = new ImbuingDefinition(AosWeaponAttribute.ResistPoisonBonus,      1061161, 100, typeof(MagicalResidue), typeof(Emerald),  typeof(BouraPelt), 15, 1, 1112011);
            m_Table[237] = new ImbuingDefinition(AosWeaponAttribute.ResistEnergyBonus,      1061162, 100, typeof(MagicalResidue), typeof(Amethyst), typeof(BouraPelt), 15, 1, 1112008); 

            m_Table[208] = new ImbuingDefinition(SAAbsorptionAttribute.EaterFire,           1154662, 140, null, null, null, 10, 1, 1152390);
            m_Table[209] = new ImbuingDefinition(SAAbsorptionAttribute.EaterCold,           1154663, 140, null, null, null, 10, 1, 1152390);
            m_Table[210] = new ImbuingDefinition(SAAbsorptionAttribute.EaterPoison,         1154664, 140, null, null, null, 10, 1, 1152390);
            m_Table[211] = new ImbuingDefinition(SAAbsorptionAttribute.EaterEnergy,         1154665, 140, null, null, null, 10, 1, 1152390);
            m_Table[212] = new ImbuingDefinition(SAAbsorptionAttribute.EaterKinetic,        1154666, 140, null, null, null, 10, 1, 1152390);
            m_Table[213] = new ImbuingDefinition(SAAbsorptionAttribute.EaterDamage,         1154667, 140, null, null, null, 10, 1, 1152390);
            m_Table[214] = new ImbuingDefinition(SAAbsorptionAttribute.ResonanceFire,       1154655, 140, null, null, null, 20, 1, 1152391);
            m_Table[215] = new ImbuingDefinition(SAAbsorptionAttribute.ResonanceCold,       1154656, 140, null, null, null, 20, 1, 1152391);
            m_Table[216] = new ImbuingDefinition(SAAbsorptionAttribute.ResonancePoison,     1154657, 140, null, null, null, 20, 1, 1152391);
            m_Table[217] = new ImbuingDefinition(SAAbsorptionAttribute.ResonanceEnergy,     1154658, 140, null, null, null, 20, 1, 1152391);
            m_Table[218] = new ImbuingDefinition(SAAbsorptionAttribute.ResonanceKinetic,    1154659, 140, null, null, null, 20, 1, 1152391);

            m_Table[219] = new ImbuingDefinition(SAAbsorptionAttribute.CastingFocus,        1116535, 140, null, null, null, 20, 1, 1116535);

            m_Table[220] = new ImbuingDefinition(AosArmorAttribute.ReactiveParalyze,        1154660, 140, null, null, null, 1, 1,  1152400);
            m_Table[221] = new ImbuingDefinition(AosArmorAttribute.SoulCharge,              1116536, 140, null, null, null, 20, 1, 1152391);

            m_Table[500] = new ImbuingDefinition(AosArmorAttribute.SelfRepair,              1079709, 100, null, null, null, 5, 1,  1079709);
            m_Table[501] = new ImbuingDefinition(AosWeaponAttribute.SelfRepair,             1079709, 100, null, null, null, 5, 1,  1079709);
            //243 already used above

            m_Table[600] = new ImbuingDefinition(ExtendedWeaponAttribute.BoneBreaker,       1157318, 140, null, null, null, 1, 1,   1157319);
            m_Table[601] = new ImbuingDefinition(ExtendedWeaponAttribute.HitSwarm,          1157328, 140, null, null, null, 20, 1,   1157327);
            m_Table[602] = new ImbuingDefinition(ExtendedWeaponAttribute.HitSparks,         1157330, 140, null, null, null, 20, 1,   1157329);
            m_Table[603] = new ImbuingDefinition(ExtendedWeaponAttribute.Bane,              1154671, 140, null, null, null, 1, 1,   1154570);
        }*/

        public static Type[] IngredTypes { get { return m_IngredTypes; } }
        private static Type[] m_IngredTypes = new Type[]
		{
			typeof(MagicalResidue), 	typeof(EnchantedEssence), 		typeof(RelicFragment),
			
			typeof(SeedOfRenewal), 		typeof(ChagaMushroom), 			typeof(CrystalShards),
			typeof(BottleIchor), 		typeof(ReflectiveWolfEye), 		typeof(FaeryDust),
			typeof(BouraPelt), 			typeof(SilverSnakeSkin), 		typeof(ArcanicRuneStone),
			typeof(SlithTongue), 		typeof(VoidOrb), 				typeof(RaptorTeeth),
			typeof(SpiderCarapace), 	typeof(DaemonClaw), 			typeof(VialOfVitriol),
			typeof(GoblinBlood), 		typeof(LavaSerpentCrust), 		typeof(UndyingFlesh),
			typeof(CrushedGlass), 		typeof(CrystallineBlackrock), 	typeof(PowderedIron),
			typeof(ElvenFletching),     typeof(DelicateScales),
			
			typeof(EssenceSingularity), typeof(EssenceBalance), 		typeof(EssencePassion),
			typeof(EssenceDirection), 	typeof(EssencePrecision), 		typeof(EssenceControl),
			typeof(EssenceDiligence), 	typeof(EssenceAchievement), 	typeof(EssenceFeeling), 
			typeof(EssenceOrder),
			
			typeof(ParasiticPlant), 	typeof(LuminescentFungi),
			typeof(FireRuby), 			typeof(WhitePearl), 			typeof(BlueDiamond), 
			typeof(Turquoise)
		};


        public static bool IsInNonImbueList(Type itemType)
        {
            foreach (Type type in m_CannotImbue)
            {
                if (type == itemType)
                    return true;
            }

            return false;
        }

        private static Type[] m_CannotImbue = new Type[]
        {
            typeof(GargishLeatherWingArmor), typeof(GargishClothWingArmor)
        };

        public static int GetValueForMod(Item item, int id)
        {
            object attr = ItemPropertyInfo.GetAttribute(id);

            if (item is BaseWeapon)
            {
                BaseWeapon w = (BaseWeapon)item;

                if (id == 16 && w.Attributes.SpellChanneling > 0)
                    return w.Attributes[AosAttribute.CastSpeed] + 1;

                if (attr is AosAttribute)
                    return w.Attributes[(AosAttribute)attr];

                else if (attr is AosWeaponAttribute)
                    return w.WeaponAttributes[(AosWeaponAttribute)attr];

                else if (attr is ExtendedWeaponAttribute)
                    return w.ExtendedWeaponAttributes[(ExtendedWeaponAttribute)attr];

                else if (attr is SAAbsorptionAttribute)
                    return w.AbsorptionAttributes[(SAAbsorptionAttribute)attr];

                else if (attr is SlayerName && w.Slayer == (SlayerName)attr)
                    return 1;

                else if (attr is AosElementAttribute)
                {
                    AosElementAttribute ele = (AosElementAttribute)attr;

                    switch (ele)
                    {
                        case AosElementAttribute.Physical: return w.WeaponAttributes.ResistPhysicalBonus;
                        case AosElementAttribute.Fire: return w.WeaponAttributes.ResistFireBonus;
                        case AosElementAttribute.Cold: return w.WeaponAttributes.ResistColdBonus;
                        case AosElementAttribute.Poison: return w.WeaponAttributes.ResistPoisonBonus;
                        case AosElementAttribute.Energy: return w.WeaponAttributes.ResistEnergyBonus;
                    }
                }
            }
            else if (item is BaseArmor)
            {
                BaseArmor a = (BaseArmor)item;

                if (a is BaseShield && id == 16 && a.Attributes.SpellChanneling > 0)
                    return a.Attributes[AosAttribute.CastSpeed] + 1;

                if (attr is AosAttribute)
                    return a.Attributes[(AosAttribute)attr];

                else if (attr is AosArmorAttribute)
                    return a.ArmorAttributes[(AosArmorAttribute)attr];

                else if (attr is SAAbsorptionAttribute)
                    return a.AbsorptionAttributes[(SAAbsorptionAttribute)attr];

                else if (attr is AosElementAttribute)
                {
                    AosElementAttribute ele = (AosElementAttribute)attr;
                    var value = 0;

                    switch (ele)
                    {
                        case AosElementAttribute.Physical: value = a.PhysicalBonus; break;
                        case AosElementAttribute.Fire: value = a.FireBonus; break;
                        case AosElementAttribute.Cold: value = a.ColdBonus; break;
                        case AosElementAttribute.Poison: value = a.PoisonBonus; break;
                        case AosElementAttribute.Energy: value = a.EnergyBonus; break;
                    }

                    if (value > 0)
                    {
                        return value;
                    }
                }
            }
            else if (item is BaseClothing)
            {
                BaseClothing c = (BaseClothing)item;

                if (attr is AosAttribute)
                    return c.Attributes[(AosAttribute)attr];

                else if (attr is AosElementAttribute)
                {
                    var value = c.Resistances[(AosElementAttribute)attr];

                    if (value > 0)
                    {
                        return value;
                    }
                }

                else if (attr is AosArmorAttribute)
                    return c.ClothingAttributes[(AosArmorAttribute)attr];
            }
            else if (item is BaseJewel)
            {
                BaseJewel j = (BaseJewel)item;

                if (attr is AosAttribute)
                    return j.Attributes[(AosAttribute)attr];

                else if (attr is AosElementAttribute)
                    return j.Resistances[(AosElementAttribute)attr];

                else if (attr is SAAbsorptionAttribute)
                    return j.AbsorptionAttributes[(SAAbsorptionAttribute)attr];

                else if (attr is SkillName)
                {
                    SkillName sk = (SkillName)attr;

                    if (j.SkillBonuses.Skill_1_Name == sk)
                        return (int)j.SkillBonuses.Skill_1_Value;

                    if (j.SkillBonuses.Skill_2_Name == sk)
                        return (int)j.SkillBonuses.Skill_2_Value;

                    if (j.SkillBonuses.Skill_3_Name == sk)
                        return (int)j.SkillBonuses.Skill_3_Value;

                    if (j.SkillBonuses.Skill_4_Name == sk)
                        return (int)j.SkillBonuses.Skill_4_Value;

                    if (j.SkillBonuses.Skill_5_Name == sk)
                        return (int)j.SkillBonuses.Skill_5_Value;
                }
            }

            var type = item.GetType();

            if (id >= 51 && id <= 55 && IsDerivedArmorOrClothing(type))
            {
                int[] resists = null;

                if (ResistBuffer != null && ResistBuffer.ContainsKey(type))
                {
                    resists = ResistBuffer[type];
                }
                else
                {
                    var baseType = type.BaseType;

                    if (IsDerivedArmorOrClothing(baseType))
                    {
                        var temp = Loot.Construct(baseType);

                        if (temp != null)
                        {
                            resists = new int[5];

                            resists[0] = GetBaseResistBonus(item, AosElementAttribute.Physical) - GetBaseResistBonus(temp, AosElementAttribute.Physical);
                            resists[1] = GetBaseResistBonus(item, AosElementAttribute.Fire) - GetBaseResistBonus(temp, AosElementAttribute.Fire);
                            resists[2] = GetBaseResistBonus(item, AosElementAttribute.Cold) - GetBaseResistBonus(temp, AosElementAttribute.Cold);
                            resists[3] = GetBaseResistBonus(item, AosElementAttribute.Poison) - GetBaseResistBonus(temp, AosElementAttribute.Poison);
                            resists[4] = GetBaseResistBonus(item, AosElementAttribute.Energy) - GetBaseResistBonus(temp, AosElementAttribute.Energy);

                            if (ResistBuffer == null)
                                ResistBuffer = new Dictionary<Type, int[]>();

                            ResistBuffer[type] = resists;
                            temp.Delete();
                        }
                    }
                }

                if (resists != null && resists.Length == 5)
                {
                    return resists[id - 51];
                }
            }

            return 0;
        }

        public static int GetIntensityForAttribute(Item item, object attr, int checkID, int value)
        {
            // This is terribly clunky, however we're accomidating 1 out of 50+ attributes that acts differently
            if (value <= 0 && (!(attr is AosAttribute) || (AosAttribute)attr != AosAttribute.CastSpeed))
                return 0;

            int id = ItemPropertyInfo.GetID(attr);

            if ((item is BaseWeapon || item is BaseShield) && id == 16)
            {
                AosAttributes attrs = RunicReforging.GetAosAttributes(item);

                if (attrs != null && attrs.SpellChanneling > 0)
                    value++;
            }
            
            if (value <= 0)
                return 0;

            if (id != checkID)
            {
                var propInfo = ItemPropertyInfo.GetInfo(id);
                int max = 1;

                if (propInfo != null)
                {
                    var info = propInfo.GetItemTypeInfo(ItemPropertyInfo.GetItemType(item));

                    if (info != null && info.StandardMax > 1)
                    {
                        max = info.StandardMax;
                    }
                }

                return (int)(((double)propInfo.Weight / max) * (double)value);
            }

            return 0;
        }

        public static bool CanImbueProperty(Mobile from, Item item, int id)
        {
            ItemPropertyInfo info = ItemPropertyInfo.GetInfo(id);
            bool canImbue = false;

            if (info != null)
            {
                if (item is BaseWeapon)
                {
                    if (item is BaseRanged && info.CanImbue(ItemType.Ranged))
                    {
                        canImbue = true;
                    }
                    else if (info.CanImbue(ItemType.Melee))
                    {
                        canImbue = true;
                    }
                }
                else if (item is BaseArmor)
                {
                    if (item is BaseShield && info.CanImbue(ItemType.Shield))
                    {
                        canImbue = true;
                    }
                    else if (info.CanImbue(ItemType.Armor))
                    {
                        canImbue = true;
                    }
                }
                else if (item is BaseJewel && info.CanImbue(ItemType.Jewel))
                {
                    canImbue = true;
                }

            }

            if (!canImbue)
            {
                from.CloseGump(typeof(ImbueGump));
                from.SendLocalizedMessage(1114291); // You cannot imbue the last property on that item.
            }

            return canImbue;
        }

        public static int TimesImbued(Item item)
        {
            if (item is IImbuableEquipement)
            {
                return ((IImbuableEquipement)item).TimesImbued;
            }

            return 0;
        }

        public static void GetMinIntensity(Item item, object property)
        {
        }
    }
}        
