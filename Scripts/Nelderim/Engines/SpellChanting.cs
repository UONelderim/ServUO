/************ SpellChanting.cs *************
 *
 *            (C) 2008, Lokai
 * 
 * Description: Speech Handler that will
 *      detect if you have a particular
 *      spell based on the mantra that
 *      you speak, and will cast it.
 *
 *******************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

#region References

using System;
using Server.Items;

#endregion

namespace Server.Spells
{
	public class SpellChanting
	{
		public static void Initialize()
		{
			EventSink.Speech += EventSink_Speech;
		}

		public static void EventSink_Speech(SpeechEventArgs e)
		{
			if (e.Blocked || e.Handled)
				return;

			string speech = e.Speech;
			Mobile m = e.Mobile;

			if (m == null || !m.Player) return;

			Spell spell = null;

			for (int i = 0; i < SpellRegistry.Types.Length; i++)
			{
				if (SpellRegistry.Types[i] == null || SpellRegistry.Types[i].IsSubclassOf(typeof(SpecialMove)))
					continue;

				Spellbook book = Spellbook.Find(m, i);

				if (book != null)
				{
					spell = Activator.CreateInstance(SpellRegistry.Types[i], m, null) as Spell;

					if (spell != null)
					{
						if (spell.Mantra != null && spell.Mantra.ToLower() == speech.ToLower())
						{
							spell.Cast();
							e.Blocked = true;
							break;
						}
					}
				}
			}
		}
	}
}
