using System;

namespace Server
{
	public delegate void BODCompletedEventHandler(BODCompletedEventArgs e);
	public delegate void SpellCastedEventHandler(SpellCastedEventArgs e);
	public delegate void EnhanceSuccessEventHandler(EnhanceSuccessEventArgs e);
	public delegate void CreatureCarvedEventHandler(CreatureCarvedEventArgs e);
	public delegate void BolaThrownEventHandler(BolaThrownEventArgs e);
	public delegate void NecromancySummonCraftedEventHandler(NecromancySummonCraftedEventArgs e);
	public delegate void AchievementCompletedEventHandler(AchievementCompletedEventArgs e);
	
	public class BODCompletedEventArgs : EventArgs
	{
		public Mobile User { get; }
		public IEntity BOD { get; }

		public BODCompletedEventArgs(Mobile m, IEntity bod)
		{
			User = m;
			BOD = bod;
		}
	}
	
	public class SpellCastedEventArgs : EventArgs
	{
		public Mobile Caster { get; }
		public ISpell Spell { get; }

		public SpellCastedEventArgs(Mobile caster, ISpell spell)
		{
			Caster = caster;
			Spell = spell;
		}
	}
	
	public class EnhanceSuccessEventArgs : EventArgs
	{
		public Mobile Crafter { get; }
		public IEntity Tool { get; }
		public Item EnhancedItem { get; }

		public EnhanceSuccessEventArgs(Mobile crafter, IEntity tool, Item item)
		{
			Crafter = crafter;
			Tool = tool;
			EnhancedItem = item;
		}
	}

	public class CreatureCarvedEventArgs : EventArgs
	{
		public Mobile Carver { get; }
		public Mobile Creature { get; }
		public Item Tool { get; }
		
		public CreatureCarvedEventArgs(Mobile carver, Mobile creature, Item tool)
		{
			Carver = carver;
			Creature = creature;
			Tool = tool;
		}
	}
	
	public class BolaThrownEventArgs : EventArgs
	{
		public Mobile From { get; }
		public Mobile Target { get; }

		public BolaThrownEventArgs(Mobile m, Mobile target)
		{
			From = m;
			Target = target;
		}
	}
	
	public class NecromancySummonCraftedEventArgs : EventArgs
	{
		public Mobile Crafter { get; }
		public Mobile Summon { get; }

		public NecromancySummonCraftedEventArgs(Mobile crafter, Mobile summon)
		{
			Crafter = crafter;
			Summon = summon;
		}
	}
	
	public class AchievementCompletedEventArgs : EventArgs
	{
		public Mobile Mobile { get; }
		public int AchievementId { get; }

		public AchievementCompletedEventArgs(Mobile m, int achievementId)
		{
			Mobile = m;
			AchievementId = achievementId;
		}
	}

	public static partial class EventSink
	{
		public static event BODCompletedEventHandler BODCompleted;
		public static event SpellCastedEventHandler SpellCast;
		public static event EnhanceSuccessEventHandler EnhanceSuccess;
		public static event CreatureCarvedEventHandler CreatureCarved;
		public static event BolaThrownEventHandler BolaThrown;
		public static event NecromancySummonCraftedEventHandler NecromancySummonCrafted;
		public static event AchievementCompletedEventHandler AchievementCompleted;
		
		public static void InvokeBODCompleted(BODCompletedEventArgs e)
		{
			BODCompleted?.Invoke(e);
		}
		
		public static void InvokeSpellCasted(SpellCastedEventArgs e)
		{
			SpellCast?.Invoke(e);
		}
		
		public static void InvokeEnhanceSuccess(EnhanceSuccessEventArgs e)
		{
			EnhanceSuccess?.Invoke(e);
		}
		
		public static void InvokeCreatureCarved(CreatureCarvedEventArgs e)
		{
			CreatureCarved?.Invoke(e);
		}
		public static void InvokeBolaThrown(BolaThrownEventArgs e)
		{
			BolaThrown?.Invoke(e);
		}
		public static void InvokeNecromancySummonCrafted(NecromancySummonCraftedEventArgs e)
		{
			NecromancySummonCrafted?.Invoke(e);
		}
		public static void InvokeAchievementCompleted(AchievementCompletedEventArgs e)
		{
			AchievementCompleted?.Invoke(e);
		}
	}
}
