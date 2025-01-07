
using System;
using Server.Mobiles;

namespace Server.Items
{
	public static class WidowMorphSet
	{
		public static void Attributes(Item item)
		{
			AosAttributes attributes = null;
			if (item is BaseArmor ba)
			{
				attributes = ba.SetAttributes;
				ba.SetSkillBonuses.SetValues(0, SkillName.Magery, 10.0);
				ba.SetSelfRepair = 3;
				ba.SetHue = 0x47E;
			}
			else if (item is BaseClothing bc)
			{
				attributes = bc.SetAttributes;
				bc.SetSkillBonuses.SetValues(0, SkillName.Magery, 10.0);
				bc.SetSelfRepair = 3;
				bc.SetHue = 0x47E;
			}
			else
			{
				Console.WriteLine($"Invalid item type in WidowMorphSet.Attributes {item.GetType().Name}");
			}
			
			if(attributes != null)
			{
				attributes.BonusInt = 15;
				attributes.BonusDex = 15;
				attributes.BonusStr = -30;
				attributes.BonusMana = 15;
				attributes.RegenMana = 10;
				attributes.RegenStam = 10;
				attributes.DefendChance = 20;
				attributes.EnhancePotions = 50;
				attributes.SpellDamage = 10;
				attributes.LowerManaCost = 15;
				attributes.Luck = 1000;
				attributes.CastSpeed = 1;
				attributes.CastRecovery = 1;
				attributes.NightSight = 1;
			}
		}
		
		public static void CheckMorph(Item item, IEntity parent)
		{
			if (parent is PlayerMobile pm)
			{
				if (item is ISetItem setItem && SetHelper.FullSetEquipped(pm, SetItem.WidowMorph, setItem.Pieces))
				{
					Effects.PlaySound( pm.Location, pm.Map, 503 );
					pm.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
					pm.SendMessage( "Zmieniasz sie!" );
					pm.BodyMod = 157;
					pm.HueMod = 1109;
				}
				else
				{
					pm.BodyMod = 0;
					pm.HueMod = 0;
				}
			}
		}
	}
}
