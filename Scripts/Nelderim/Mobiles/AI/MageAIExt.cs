using System;
using Server.Spells;
using Server.Spells.Fifth;
using Server.Spells.First;
using Server.Spells.Fourth;
using Server.Spells.Second;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Third;

namespace Server.Mobiles
{
	public partial class MageAI
	{
		private static readonly Type[][] NCombos =
		{
			new[]
			{
				typeof(MindBlastSpell), typeof(ExplosionSpell), typeof(FireballSpell), typeof(PoisonSpell),
				typeof(EnergyBoltSpell), typeof(MagicArrowSpell), typeof(MagicArrowSpell)
			},
			new[]
			{
				typeof(MagicArrowSpell), typeof(MindBlastSpell), typeof(MagicArrowSpell), typeof(LightningSpell)
			},
			new[] { typeof(MagicArrowSpell), typeof(EnergyBoltSpell), typeof(MindBlastSpell) },
			new[]
			{
				typeof(FlameStrikeSpell), typeof(MagicArrowSpell), typeof(MagicArrowSpell),
				typeof(LightningSpell)
			},
			new[]
			{
				typeof(HarmSpell), typeof(ExplosionSpell), typeof(MagicArrowSpell), typeof(MagicArrowSpell),
				typeof(LightningSpell)
			},
			new[] { typeof(ParalyzeSpell), typeof(MagicArrowSpell), typeof(HarmSpell), typeof(LightningSpell) },
			new[] { typeof(FireballSpell), typeof(MindBlastSpell), typeof(LightningSpell) },
			new[]
			{
				typeof(ExplosionSpell), typeof(ParalyzeSpell), typeof(ExplosionSpell), typeof(HarmSpell),
				typeof(FireballSpell)
			},
			new[] { typeof(ExplosionSpell), typeof(ExplosionSpell), typeof(EnergyBoltSpell) },
			new[]
			{
				typeof(ExplosionSpell), typeof(LightningSpell), typeof(MindBlastSpell), typeof(FireballSpell),
				typeof(FireballSpell)
			},
			new[] { typeof(ParalyzeSpell), typeof(ExplosionSpell), typeof(FlameStrikeSpell) },
			new[] { typeof(ExplosionSpell), typeof(FireballSpell), typeof(FireballSpell), typeof(MindBlastSpell) },
			new[] { typeof(MindBlastSpell), typeof(CurseSpell), typeof(EnergyBoltSpell) },
			new[] { typeof(MagicArrowSpell), typeof(EnergyBoltSpell), typeof(FireballSpell) },
			new[] { typeof(MindBlastSpell), typeof(MagicArrowSpell), typeof(HarmSpell) },
			new[] { typeof(FireballSpell), typeof(MagicArrowSpell), typeof(LightningSpell) }
		};

		private static readonly int[] NSpellCombosMana;
		private static readonly SpellCircle[] NSpellCombosCircle;

		static MageAI()
		{
			NSpellCombosMana = new int[NCombos.Length];
			NSpellCombosCircle = new SpellCircle[NCombos.Length];
			for (var index = 0; index < NCombos.Length; index++)
			{
				var nSpellCombo = NCombos[index];
				var totalMana = 0;
				var maxCircle = SpellCircle.First;
				foreach (var spellType in nSpellCombo)
				{
					var spell = Activator.CreateInstance(spellType, new object[]{null, null}) as MagerySpell;
					if (spell == null)
					{
						Console.WriteLine("Invalid combo spell type " + spellType.Name);
					}
					else
					{
						totalMana += spell.GetMana();
						maxCircle = spell.Circle > maxCircle ? spell.Circle : maxCircle;
					}
				}

				NSpellCombosMana[index] = totalMana;
				NSpellCombosCircle[index] = maxCircle;
			}
		}

		private int _NComboIndex = -1;
		private int _NComboPosition = -1;
		
		private Spell NDoCombo(Mobile m)
		{
			Spell result;
			if (_NComboIndex == -1)
			{
				//TODO: Random only of what we have circleReq and manaReq 
				_NComboIndex = Utility.Random(NCombos.Length);
				_NComboPosition = 0;
				m_Mobile.DebugSay($"Starting NCombo! {String.Join(",", Array.ConvertAll(NCombos[_NComboIndex], t => t.Name))}");
			}
			var type = NCombos[_NComboIndex][_NComboPosition];
			result = (Spell)Activator.CreateInstance(type, m_Mobile, null);
			_NComboPosition++;
			NCheckEndCombo();

			return result;
		}

		private void NCheckEndCombo()
		{
			if (_NComboPosition < NCombos[_NComboIndex].Length) return;
			_NComboIndex = -1;
			_NComboPosition = -1;
			EndCombo();
		}
	}
}
