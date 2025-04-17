using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Commands
{
	public class FixAbilities
	{
		private static readonly Dictionary<Type, BaseCreature> _Models = [];
		
		public static void Initialize()
		{
			CommandSystem.Register("FixAbilities", AccessLevel.Administrator, OnFixAbilities);
			CommandSystem.Register("FixCreatureAbilities", AccessLevel.Administrator, e => e.Mobile.BeginTarget(12, false, TargetFlags.None,
				(from, targeted) =>
				{
					if (targeted is BaseCreature bc)
					{
						FixCreature(from, bc);
					}
				}));
		}

		private static void OnFixAbilities(CommandEventArgs e)
		{
			var creatures = World.Mobiles.Values.OfType<BaseCreature>();
			var fixedCount = 0;
			foreach (var creature in creatures.ToArray())
			{
				if(FixCreature(e.Mobile, creature))
				{
					fixedCount++;
				}
			}

			foreach (var model in _Models.Values.ToArray())
			{
				model.Delete();
			}
			_Models.Clear();
			e.Mobile.SendMessage("Fixed {0} creatures", fixedCount);
		}

		private static bool FixCreature(Mobile from, BaseCreature creature)
		{
			var bcFixed = false;
			BaseCreature model;
			try
			{
				if (!_Models.TryGetValue(creature.GetType(), out model))
				{
					model = Activator.CreateInstance(creature.GetType()) as BaseCreature;
					_Models[creature.GetType()] = model;
				}
			}
			catch
			{
				from.SendMessage("Error while creating model for {0}", creature.GetType().Name);
				return false;
			}

			var modelProfile = model.AbilityProfile;
			if (modelProfile == null)
			{
				return false;
			}
				
			foreach (var modelWA in modelProfile.WeaponAbilities ?? [])
			{
				if (!creature.HasAbility(modelWA))
				{
					creature.SetWeaponAbility(modelWA);
					bcFixed = true;
				}
			}

				
			foreach (var modelAE in modelProfile.AreaEffects ?? [])
			{
				if (!creature.HasAbility(modelAE))
				{
					creature.SetAreaEffect(modelAE);
					bcFixed = true;
				}
			}

			foreach (var modelSA in modelProfile.SpecialAbilities ?? [])
			{
				if (!creature.HasAbility(modelSA))
				{
					creature.SetSpecialAbility(modelSA);
					bcFixed = true;
				}
			}

			creature.SetMagicalAbility(modelProfile.MagicalAbility);
			if (bcFixed)
			{
				creature.CalculateDifficulty();
				return true;
			}

			return false;
		}
	}
}
