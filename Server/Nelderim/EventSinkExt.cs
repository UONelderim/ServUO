using System;

namespace Server
{
	public delegate void BODCompletedEventHandler(BODCompletedEventArgs e);
	public delegate void SpellCastedEventHandler(SpellCastedEventArgs e);
	public delegate void EnhanceSuccessEventHandler(EnhanceSuccessEventArgs e);
	public delegate void CreatureCarvedEventHandler(CreatureCarvedEventArgs e);
	
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

	public static partial class EventSink
	{
		public static event BODCompletedEventHandler BODCompleted;
		public static event SpellCastedEventHandler SpellCast;
		public static event EnhanceSuccessEventHandler EnhanceSuccess;
		public static event CreatureCarvedEventHandler CreatureCarved;
		
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
	}
}
