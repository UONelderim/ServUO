#region References

using Server.Mobiles;

#endregion

namespace Server.SicknessSys
{
	static class SicknessAnimate
	{
		//Main Animations
		public static void RunInfectedAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);

			pm.Animate(6, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x31F : 0x42F);
		}

		public static void RunCureAnimation(PlayerMobile pm)
		{
			Effects.SendLocationEffect(new Point3D(pm.X, pm.Y, pm.Z + 1), pm.Map, 0x376A, 15, 0, 0); //0x47D );
			Effects.SendLocationEffect(new Point3D(pm.X, pm.Y, pm.Z + 1), pm.Map, 0x375A, 15, 0, 0);
			pm.Say("*czujesz sie dobrze*");

			pm.Animate(34, 5, 1, true, false, 0);
			pm.PlaySound(0x1E0);
		}

		public static void RunMutateAnimation(PlayerMobile pm)
		{
			Effects.SendLocationEffect(new Point3D(pm.X, pm.Y, pm.Z), pm.Map, 0x3728, 15, 0, 0);

			pm.Animate(20, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x31B : 0x42B);
		}

		//Standard Symptom Animations
		public static void RunBlowNoseAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);
			pm.Animate(34, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x30D : 0x41C);
		}

		public static void RunClearThroatAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);
			pm.Animate(34, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x310 : 0x41F);
		}

		public static void RunCoughAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);
			pm.Animate(34, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x311 : 0x420);
		}

		public static void RunGaspAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);
			pm.Animate(33, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x319 : 0x429);
		}

		public static void RunGroanAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);
			pm.Animate(6, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x31B : 0x42B);
		}

		public static void RunPukeAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);
			pm.Animate(32, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x32D : 0x43F);
		}

		public static void RunSighAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);
			pm.Animate(5, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x330 : 0x442);
		}

		public static void RunSneezeAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);
			pm.Animate(34, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x331 : 0x443);
		}

		public static void RunSiffAnimation(PlayerMobile pm)
		{
			pm.FixedEffect(0x2336, 2, 20);
			pm.Animate(34, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x332 : 0x444);
		}

		//Medic Animation
		public static void RunMedicAnimation(PlayerMobile pm, Mobile m)
		{
			TurnToMobile(pm, m);

			m.Animate(34, 5, 1, true, false, 0);
			m.PlaySound(m.Female ? 0x335 : 0x447);
		}

		public static void RunMedicGiveCureAnimation(PlayerMobile pm, Mobile m)
		{
			TurnToMobile(pm, m);

			m.Animate(34, 5, 1, true, false, 0);
			m.Say("*In Alah KaZappa Vas*");

			pm.SendMessage(53, pm.Name + ", medykamenty, ktore ulecza Twa chorobe magicznie pojawily sie w plecaku!");

			Effects.SendLocationParticles(pm, 0x376A, 9, 32, 5022);
			Effects.PlaySound(pm.Location, pm.Map, 0x1F5);
		}

		//Special Virus Weakness Animations
		public static void RunScreamAnimation(PlayerMobile pm)
		{
			Effects.SendLocationEffect(new Point3D(pm.X, pm.Y, pm.Z + 1), pm.Map, 0x3709, 15, 0, 0);

			pm.Animate(16, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x31FE : 0x440);
		}

		public static void RunGrowlAnimation(PlayerMobile pm)
		{
			Effects.SendBoltEffect(pm, true, 1177);
			pm.SendMessage("Jestes w poblizu srebra, odejdz lub gin!");

			pm.Animate(30, 5, 1, true, false, 0);
			pm.PlaySound(pm.Female ? 0x31C : 0x42C);
		}

		//Vampire Skills
		public static void RunBloodDrainAnimation(PlayerMobile pm)
		{
			TurnToMobile(pm, pm.Combatant as Mobile);

			pm.Say("*wysysasz krew*");
			pm.PlaySound(0x030);

			pm.Combatant.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
			pm.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
		}

		public static void RunBloodBurnAnimation(PlayerMobile pm)
		{
			TurnToMobile(pm, pm.Combatant as Mobile);

			pm.Say("*krew w zylach gotuje sie*");
			pm.PlaySound(0x114);

			pm.Combatant.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
			pm.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
		}

		public static void RunBloodBathAnimation(PlayerMobile pm)
		{
			pm.Say("*kapiel w krwi*");

			pm.FixedParticles(0x3728, 1, 13, 5042, EffectLayer.Waist);

			pm.Animate(33, 5, 1, true, false, 0);

			pm.PlaySound(0x118);
		}

		//Lycanthropia
		public static void RunRageFeedAnimation(PlayerMobile pm)
		{
			TurnToMobile(pm, pm.Combatant as Mobile);

			pm.Say("*gniew*");
			pm.PlaySound(0x03A);

			pm.Combatant.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
			pm.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
		}

		public static void RunRageStrikeAnimation(PlayerMobile pm)
		{
			TurnToMobile(pm, pm.Combatant as Mobile);

			pm.Say("*gniewne uderzenie*");
			pm.PlaySound(0x13C);

			pm.Combatant.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
			pm.FixedParticles(0x375A, 1, 17, 9919, 33, 7, EffectLayer.Waist);
		}

		public static void RunRagePushAnimation(PlayerMobile pm)
		{
			pm.Say("*odpychajacy gniew*");

			pm.FixedParticles(0x3728, 1, 13, 5042, EffectLayer.Waist);

			pm.Animate(33, 5, 1, true, false, 0);

			pm.PlaySound(0x229);
		}

		//Helper Methods
		private static void TurnToMobile(PlayerMobile pm, Mobile m)
		{
			Direction direction1 = m.GetDirectionTo(pm.Location);
			Direction direction2 = pm.GetDirectionTo(m.Location);

			m.Direction = direction1;
			pm.Direction = direction2;
		}
	}
}
