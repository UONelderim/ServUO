#region References

using System.Xml;
using Server.Nelderim.Gumps;

#endregion

namespace Server.Regions
{
	public class JailRegion : BaseRegion
	{
		public JailRegion(XmlElement xml, Map map, Region parent) : base(xml, map, parent)
		{
		}

		public override bool AllowBeneficial(Mobile from, Mobile target)
		{
			if (from.AccessLevel == AccessLevel.Player)
				from.SendLocalizedMessage(505610); // Nie mozesz tego robic przebywajac w wiezieniu.

			return (from.AccessLevel > AccessLevel.Player);
		}

		public override bool AllowHarmful(Mobile from, IDamageable target)
		{
			if (from.AccessLevel == AccessLevel.Player)
				from.SendLocalizedMessage(505610); // Nie mozesz tego robic przebywajac w wiezieniu.

			return (from.AccessLevel > AccessLevel.Player);
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			return false;
		}

		public override void OnEnter(Mobile m)
		{
			m.SendLocalizedMessage(505611, "wiezienia"); // Wkroczyles na teren ~1_NAME~
			ProfileSetupGump.Check(m);
		}

		public override void OnExit(Mobile m)
		{
			m.SendLocalizedMessage(505612, "wiezienia"); // Opuszczasz teren ~1_NAME~
		}

		public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
		{
			global = LightCycle.JailLevel;
		}

		/*public override bool OnBeginSpellCast( Mobile from, ISpell s )
		{
			if ( from.AccessLevel == AccessLevel.Player )
				from.SendLocalizedMessage( 502629 ); // You cannot cast spells here.

			return ( from.AccessLevel > AccessLevel.Player );
		}*/

		/*public override bool OnSkillUse( Mobile from, int Skill )
		{
			if ( from.AccessLevel == AccessLevel.Player )
				from.SendLocalizedMessage( 505610 ); // Nie mozesz tego robic przebywajac w wiezieniu.

			return ( from.AccessLevel > AccessLevel.Player );
		}*/

		public override bool OnCombatantChange(Mobile from, IDamageable Old, IDamageable New)
		{
			return (from.AccessLevel > AccessLevel.Player);
		}
	}
}
