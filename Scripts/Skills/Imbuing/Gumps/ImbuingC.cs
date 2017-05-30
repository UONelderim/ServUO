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
using Server.SkillHandlers;

namespace Server.Gumps
{
    public class ImbuingGumpC : Gump
    {
        private const int LabelHue = 0x480;
        private const int LabelColor = 0x7FFF;  //Localized
        private const int FontColor = 0xFFFFFF; //string
        private const int ValueColor = 0xCCCCFF;

        public const int MaxProps = 5;

        private int m_Mod, m_Value;
        private Item m_Item;
        private int m_GemAmount = 0, m_PrimResAmount = 0, m_SpecResAmount = 0;
		private int m_TotalItemWeight;
		private int m_TotalProps;
		private int m_PropWeight;
		private int m_MaxWeight;
		
        private ImbuingDefinition m_Definition;

        public ImbuingGumpC(Mobile from, Item item, int mod, int value) : base(520, 340)
        {
            PlayerMobile m = from as PlayerMobile;

            from.CloseGump(typeof(ImbuingGump));
            from.CloseGump(typeof(ImbuingGumpB));
            from.CloseGump(typeof(RunicReforgingGump));
            
            // SoulForge Check
            if (!Imbuing.CheckSoulForge(from, 1))
                return;

            ImbuingContext context = Imbuing.GetContext(m);

            // = Check Type of Ingredients Needed 
            if (!Imbuing.Table.ContainsKey(mod))
                return;

            m_Definition = Imbuing.Table[mod];

            if (value == -1)
                value = m_Definition.IncAmount;

            m_Item = item;
            m_Mod = mod;
            m_Value = value;

            int maxInt = Imbuing.GetMaxIntensity(item, m_Definition);
            int inc = m_Definition.IncAmount;
            int weight = m_Definition.Weight;

            if (m_Item is BaseJewel && m_Mod == 12)
                maxInt /= 2;

            if (m_Value < inc) m_Value = inc;
            if (m_Value > maxInt) m_Value = maxInt;

            if (m_Value <= 0)
                m_Value = 1;

            double currentIntensity = Math.Floor((m_Value / (double)maxInt) * 100);

			//Set context
			context.LastImbued = item;
            context.Imbue_Mod = mod;
            context.Imbue_ModVal = weight;
            context.ImbMenu_ModInc = inc;
            context.Imbue_ModInt = value;

			// - Current Mod Weight
            m_TotalItemWeight = Imbuing.GetTotalWeight(m_Item, m_Mod); 
            m_TotalProps = Imbuing.GetTotalMods(m_Item, m_Mod);
			
            if (maxInt <= 1)
				currentIntensity= 100;

            m_PropWeight = (int)Math.Floor(((double)weight / (double)maxInt) * m_Value);

            // - Maximum allowed Property Weight & Item Mod Count
            m_MaxWeight = Imbuing.GetMaxWeight(m_Item);
			
            // = Times Item has been Imbued
            int timesImbued = 0;
            if (m_Item is BaseWeapon) 
                timesImbued = ((BaseWeapon)m_Item).TimesImbued;
            if (m_Item is BaseArmor)
                timesImbued = ((BaseArmor)m_Item).TimesImbued;
            if (m_Item is BaseJewel)
                timesImbued = ((BaseJewel)m_Item).TimesImbued;
            if (m_Item is BaseHat)
                timesImbued = ((BaseHat)m_Item).TimesImbued;

            // = Check Ingredients needed at the current Intensity
            m_GemAmount = Imbuing.GetGemAmount(m_Item, m_Mod, m_Value);
            m_PrimResAmount = Imbuing.GetPrimaryAmount(m_Item, m_Mod, m_Value);
            m_SpecResAmount = Imbuing.GetSpecialAmount(m_Item, m_Mod, m_Value);

            // ------------------------------ Gump Menu -------------------------------------------------------------
            AddPage(0);
            AddBackground(0, 0, 520, 440, 5054);
            AddImageTiled(10, 10, 500, 420, 2624);

            AddImageTiled(10, 30, 500, 10, 5058);
            AddImageTiled(250, 40, 10, 290, 5058);
            AddImageTiled(10, 180, 500, 10, 5058);
            AddImageTiled(10, 330, 500, 10, 5058);
            AddImageTiled(10, 400, 500, 10, 5058);

            AddAlphaRegion(10, 10, 500, 420);

            AddHtmlLocalized(10, 12, 520, 20, 1079717, LabelColor, false, false); //<CENTER>IMBUING CONFIRMATION</CENTER>
            AddHtmlLocalized(50, 50, 200, 20, 1114269, LabelColor, false, false); //PROPERTY INFORMATION

            AddHtmlLocalized(25, 80, 80, 20, 1114270, LabelColor, false, false);  //Property:
            AddHtmlLocalized(95, 80, 150, 20, m_Definition.AttributeName, LabelColor, false, false);

            AddHtmlLocalized(25, 100, 80, 20, 1114271, LabelColor, false, false); // Replaces:
            int replace = WhatReplacesWhat(m_Mod, m_Item);

            if (replace > 0)
            {
                AddHtmlLocalized(95, 100, 150, 20, replace, LabelColor, false, false);
            }

            // - Weight Modifier
            AddHtmlLocalized(25, 120, 80, 20, 1114272, 0xFFFFFF, false, false);   //Weight:
            AddLabel(95, 120, 1153, String.Format("{0}x", ((double)m_Definition.Weight / 100.0).ToString("0.0")));

            AddHtmlLocalized(25, 140, 80, 20, 1114273, LabelColor, false, false);   //Intensity:
            AddLabel(95, 140, 1153, String.Format("{0}%", currentIntensity));

            // - Materials needed
            AddHtmlLocalized(10, 200, 245, 20, 1044055, LabelColor, false, false); //<CENTER>MATERIALS</CENTER>

            AddHtmlLocalized(40, 230, 180, 20, m_Definition.PrimaryName, LabelColor, false, false);
            AddLabel(210, 230, 1153, m_PrimResAmount.ToString());

            AddHtmlLocalized(40, 255, 180, 20, m_Definition.GemName, LabelColor, false, false);
            AddLabel(210, 255, 1153, m_GemAmount.ToString());

            if (m_SpecResAmount > 0)
            {
                AddHtmlLocalized(40, 280, 180, 17, m_Definition.SpecialName, LabelColor, false, false);
                AddLabel(210, 280, 1153, m_SpecResAmount.ToString());
            }

            // - Mod Description
            AddHtmlLocalized(280, 55, 200, 110, m_Definition.Description, LabelColor, false, false); 

            AddHtmlLocalized(350, 200, 150, 20, 1113650, LabelColor, false, false); //RESULTS
			
            AddHtmlLocalized(280, 220, 150, 20, 1113645, LabelColor, false, false); //Properties:
            AddLabel(430, 220, m_TotalProps + 1 >= 5 ? 54 : 64, String.Format("{0}/5", m_TotalProps + 1));

            int projWeight = m_TotalItemWeight + m_PropWeight;
            AddHtmlLocalized(280, 240, 150, 20, 1113646, LabelColor, false, false); //Total Property Weight:
            AddLabel(430, 240, projWeight >= m_MaxWeight ? 54 : 64, String.Format("{0}/{1}", projWeight, m_MaxWeight));

            AddHtmlLocalized(280, 260, 150, 20, 1113647, LabelColor, false, false); //Times Imbued:
            AddLabel(430, 260, timesImbued >= 20 ? 54 : 64, String.Format("{0}/20", timesImbued));

            // ===== CALCULATE DIFFICULTY =====
            double dif;
            double suc = Imbuing.GetSuccessChance(from, item, m_TotalItemWeight, m_PropWeight, out dif);
            int color = 64;

            AddHtmlLocalized(300, 300, 150, 20, 1044057, 0xFFFFFF, false, false); // Success Chance:

            if (suc > 100) suc = 100;
            if (suc < 0) suc = 0;

            if (suc < 75.0)
            {
                if (suc >= 50.0)
                {
                    color = 54;
                }
                else
                {
                    color = 46;
                }
            }

            AddLabel(420, 300, color, String.Format("{0}%", suc.ToString("0.0")));

            // - Attribute Level
            if (maxInt > 1)
            {
                // - Set Intesity to Minimum
                if (m_Value <= 0)
                    m_Value = 1;

                // = New Value:
                AddHtmlLocalized(235, 350, 100, 17, 1062300, LabelColor, false, false); // New Value:

                if (m_Mod == 41)                                     // - Mage Weapon Value ( i.e [Mage Weapon -25] )
                    AddHtml(240, 368, 40, 20, String.Format("<CENTER><BASEFONT COLOR=#CCCCFF> -{0}</CENTER>", 30 - m_Value), false, false);
                else if (maxInt <= 8 || m_Mod == 21 || m_Mod == 17)  // - Show Property Value as just Number ( i.e [Mana Regen 2] )
                    AddHtml(240, 368, 40, 20, String.Format("<CENTER><BASEFONT COLOR=#CCCCFF> {0}</CENTER>", m_Value), false, false);
                else                                                 // - Show Property Value as % ( i.e [Hit Fireball 25%] )
                    AddHtml(240, 368, 40, 20, String.Format("<CENTER><BASEFONT COLOR=#CCCCFF> {0}%</CENTER>", m_Value), false, false);

                // == Buttons ==
                AddButton(180, 370, 5230, 5230, 10053, GumpButtonType.Reply, 0); // To Minimum Value
                AddButton(200, 370, 5230, 5230, 10052, GumpButtonType.Reply, 0); // Dec Value by %
                AddButton(220, 370, 5230, 5230, 10051, GumpButtonType.Reply, 0); // dec value by 1

                AddButton(320, 370, 5230, 5230, 10056, GumpButtonType.Reply, 0); //To Maximum Value
                AddButton(300, 370, 5230, 5230, 10055, GumpButtonType.Reply, 0); // Inc Value by %
                AddButton(280, 370, 5230, 5230, 10054, GumpButtonType.Reply, 0); // inc Value by 1

                AddLabel(322, 368, 0, ">");
                AddLabel(326, 368, 0, ">");
                AddLabel(330, 368, 0, ">");

                AddLabel(304, 368, 0, ">");
                AddLabel(308, 368, 0, ">");

                AddLabel(286, 368, 0, ">");

                AddLabel(226, 368, 0, "<");

                AddLabel(203, 368, 0, "<");
                AddLabel(207, 368, 0, "<");

                AddLabel(181, 368, 0, "<");
                AddLabel(185, 368, 0, "<");
                AddLabel(189, 368, 0, "<");
            }

            AddButton(15, 410, 4005, 4007, 10099, GumpButtonType.Reply, 0);
            AddHtmlLocalized(50, 410,  100, 18, 1114268, LabelColor, false, false);           //Back 

            AddButton(390, 410, 4005, 4007, 10100, GumpButtonType.Reply, 0);
            AddHtmlLocalized(425, 410, 120, 18, 1114267, LabelColor, false, false);         //Imbue Item
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            PlayerMobile pm = from as PlayerMobile;

            ImbuingContext context = Imbuing.GetContext(pm);

            int buttonNum = 0;
            if (info.ButtonID > 0 && info.ButtonID < 10000)
                buttonNum = 0;
            else if (info.ButtonID > 20004)
                buttonNum = 30000;
            else
                buttonNum = info.ButtonID;

            switch (buttonNum)
            {
                case 0:
                    {
                        //Close
                        break;
                    }
                case 10051: // = Decrease Mod Value [<]
                    {
                        if (context.Imbue_ModInt > m_Definition.IncAmount)
                            context.Imbue_ModInt -= m_Definition.IncAmount;

                        from.SendGump(new ImbuingGumpC(from, m_Item, context.Imbue_Mod, context.Imbue_ModInt));
                        break;
                    }
                case 10052:// = Decrease Mod Value [<<]
                    {
                        if ((m_Mod == 42 || m_Mod == 24) && context.Imbue_ModInt > 20)
                            context.Imbue_ModInt -= 20;
                        if ((m_Mod == 13 || m_Mod == 20 || m_Mod == 21) && context.Imbue_ModInt > 10)
                            context.Imbue_ModInt -= 10;
                        else  if (context.Imbue_ModInt > 5)
                            context.Imbue_ModInt -= 5;

                        from.SendGump(new ImbuingGumpC(from, context.LastImbued, context.Imbue_Mod, context.Imbue_ModInt));
                        break;
                    }
                case 10053:// = Minimum Mod Value [<<<]
                    {
                        context.Imbue_ModInt = 1;
                        from.SendGump(new ImbuingGumpC(from, context.LastImbued, context.Imbue_Mod, context.Imbue_ModInt));
                        break;
                    }
                case 10054: // = Increase Mod Value [>]
                    {
                        int max = Imbuing.GetMaxIntensity(m_Item, m_Definition);

                        if(m_Mod == 12 && context.LastImbued is BaseJewel)
                            max /= 2;

                        if (context.Imbue_ModInt + m_Definition.IncAmount <= max)
                            context.Imbue_ModInt += m_Definition.IncAmount;

                        from.SendGump(new ImbuingGumpC(from, context.LastImbued, context.Imbue_Mod, context.Imbue_ModInt));
                        break;
                    }
                case 10055: // = Increase Mod Value [>>]
                    {
                        int max = Imbuing.GetMaxIntensity(m_Item, m_Definition);

                        if (m_Mod == 12 && context.LastImbued is BaseJewel)
                            max /= 2;

                        if (m_Mod == 42 || m_Mod == 24)
                        {
                            if (context.Imbue_ModInt + 20 <= max)
                                context.Imbue_ModInt += 20;
                            else
                                context.Imbue_ModInt = max;
                        }
                        if (m_Mod == 13 || m_Mod == 20 || m_Mod == 21)
                        {
                            if (context.Imbue_ModInt + 10 <= max)
                                context.Imbue_ModInt += 10;
                            else
                                context.Imbue_ModInt = max;
                        }
                        else if (context.Imbue_ModInt + 5 <= max)
                            context.Imbue_ModInt += 5;
                        else
                            context.Imbue_ModInt = Imbuing.GetMaxIntensity(m_Item, m_Definition);

                        from.SendGump(new ImbuingGumpC(from, context.LastImbued, context.Imbue_Mod, context.Imbue_ModInt));
                        break;
                    }
                case 10056: // = Maximum Mod Value [>>>]
                    {
                        int max = Imbuing.GetMaxIntensity(m_Item, m_Definition);

                        if (m_Mod == 12 && context.LastImbued is BaseJewel)
                            max /= 2;

                        context.Imbue_ModInt = max;
                        from.SendGump(new ImbuingGumpC(from, context.LastImbued, context.Imbue_Mod, context.Imbue_ModInt));
                        break;
                    }

                case 10099: // - Back
                    {
                        from.SendGump(new ImbuingGumpB(from, context.LastImbued));
                        break;
                    }
                case 10100:  // = Imbue the Item
                    {
                        context.Imbue_IWmax = m_MaxWeight;

                        if (Imbuing.OnBeforeImbue(from, m_Item, m_Mod, m_Value, m_TotalProps, MaxProps, m_TotalItemWeight, m_MaxWeight))
                        {
                            Imbuing.ImbueItem(from, m_Item, m_Mod, m_Value);
                            SendGumpDelayed(from);
                        }

                        break;
                    }
            }
        }

        public void SendGumpDelayed(Mobile from)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(1.5), new TimerStateCallback(SendGump_Callback), from);
        }

        public void SendGump_Callback(object o)
        {
            Mobile from = o as Mobile;

            if (from != null)
                from.SendGump(new ImbuingGump(from));
        }

        // =========== Check if Choosen Attribute Replaces Another =================
        public static int WhatReplacesWhat(int mod, Item item)
        {
            if (Imbuing.GetValueForMod(item, mod) > 0)
            {
                return Imbuing.GetAttributeName(mod);
            }

            if (item is BaseWeapon)
            {
                BaseWeapon i = item as BaseWeapon;

                // Slayers replace Slayers
                if (mod >= 101 && mod <= 127)
                {
                    if (i.Slayer != SlayerName.None)
                        return GetNameForAttribute(i.Slayer);

                    if (i.Slayer2 != SlayerName.None)
                        return GetNameForAttribute(i.Slayer2);
                }
                // OnHitEffect replace OnHitEffect
                if (mod >= 35 && mod <= 39)
                {
                    if (i.WeaponAttributes.HitMagicArrow > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitMagicArrow);
                    else if (i.WeaponAttributes.HitHarm > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitHarm);
                    else if (i.WeaponAttributes.HitFireball > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitFireball);
                    else if (i.WeaponAttributes.HitLightning > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitLightning);
                    else if (i.WeaponAttributes.HitDispel > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitDispel);
                }
                // OnHitArea replace OnHitArea
                if (mod >= 30 && mod <= 34)
                {
                    if (i.WeaponAttributes.HitPhysicalArea > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitPhysicalArea);
                    else if (i.WeaponAttributes.HitColdArea > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitFireArea);
                    else if (i.WeaponAttributes.HitFireArea > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitColdArea);
                    else if (i.WeaponAttributes.HitPoisonArea > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitPoisonArea);
                    else if (i.WeaponAttributes.HitEnergyArea > 0)
                        return GetNameForAttribute(AosWeaponAttribute.HitEnergyArea);
                }
            }
            if (item is BaseJewel)
            {
                BaseJewel i = item as BaseJewel;

                // SkillGroup1 replace SkillGroup1
                if (mod >= 151 && mod <= 155)
                {
                    if (i.SkillBonuses.GetBonus(0) > 0)
                    {
                        foreach (SkillName sk in Imbuing.PossibleSkills)
                        {
                            if(i.SkillBonuses.GetSkill(0) == sk)
                                return GetNameForAttribute(sk);
                        }
                    }
                }
                // SkillGroup2 replace SkillGroup2
                if (mod >= 156 && mod <= 160)
                {
                    if (i.SkillBonuses.GetBonus(1) > 0)
                    {
                        foreach (SkillName sk in Imbuing.PossibleSkills)
                        {
                            if (i.SkillBonuses.GetSkill(1) == sk)
                                return GetNameForAttribute(sk);
                        }
                    }
                }
                // SkillGroup3 replace SkillGroup3
                if (mod >= 161 && mod <= 166)
                {
                    if (i.SkillBonuses.GetBonus(2) > 0)
                    {
                        foreach (SkillName sk in Imbuing.PossibleSkills)
                        {
                            if (i.SkillBonuses.GetSkill(2) == sk)
                                return GetNameForAttribute(sk);
                        }
                    }
                }
                // SkillGroup4 replace SkillGroup4
                if (mod >= 167 && mod <= 172)
                {
                    if (i.SkillBonuses.GetBonus(3) > 0)
                    {
                        foreach (SkillName sk in Imbuing.PossibleSkills)
                        {
                            if (i.SkillBonuses.GetSkill(3) == sk)
                                return GetNameForAttribute(sk);
                        }
                    }
                }
                // SkillGroup5 replace SkillGroup5
                if (mod >= 173 && mod <= 178)
                {
                    if (i.SkillBonuses.GetBonus(4) > 0)
                    {
                        foreach (SkillName sk in Imbuing.PossibleSkills)
                        {
                            if (i.SkillBonuses.GetSkill(4) == sk)
                                return GetNameForAttribute(sk);
                        }
                    }
                }
            }

            return -1;
        }

        public static int GetNameForAttribute(object attribute)
        {
            if(attribute is AosArmorAttribute && (AosArmorAttribute)attribute == AosArmorAttribute.LowerStatReq)
                attribute = AosWeaponAttribute.LowerStatReq;

            if (attribute is AosArmorAttribute && (AosArmorAttribute)attribute == AosArmorAttribute.DurabilityBonus)
                attribute = AosWeaponAttribute.DurabilityBonus;

            foreach (ImbuingDefinition def in Imbuing.Table.Values)
            {
                if (attribute is SlayerName && def.Attribute is SlayerName && (SlayerName)attribute == (SlayerName)def.Attribute)
                    return def.AttributeName;

                if (attribute is AosAttribute && def.Attribute is AosAttribute && (AosAttribute)attribute == (AosAttribute)def.Attribute)
                    return def.AttributeName;

                if (attribute is AosWeaponAttribute && def.Attribute is AosWeaponAttribute && (AosWeaponAttribute)attribute == (AosWeaponAttribute)def.Attribute)
                    return def.AttributeName;

                if (attribute is SkillName && def.Attribute is SkillName && (SkillName)attribute == (SkillName)def.Attribute)
                    return def.AttributeName;

                if (def.Attribute == attribute)
                    return def.AttributeName;
            }

            return -1;
        }
    }                
}