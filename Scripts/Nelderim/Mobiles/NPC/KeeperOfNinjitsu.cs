using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
	public class KeeperOfNinjitsu : BaseVendor
	{
		private List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos{ get { return m_SBInfos; } }

		[Constructable]
		public KeeperOfNinjitsu() : base( "- skrytobojca" )
		{
			SetSkill(SkillName.Hiding, 120.0, 120.0);
			SetSkill(SkillName.Tracking, 120.0, 120.0);
			SetSkill(SkillName.Healing, 120.0, 120.0);
			SetSkill(SkillName.Tactics, 120.0, 120.0);
			SetSkill(SkillName.Fencing, 120.0, 120.0);
			SetSkill(SkillName.Stealth, 120.0, 120.0);
			SetSkill(SkillName.Ninjitsu, 120.0, 120.0);
			SetSkill(SkillName.Throwing, 120.0, 120.0);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBKeeperOfNinjitsu() );
		}

		public override void InitOutfit()
		{
			EquipItem(new Backpack());
			EquipItem(new SamuraiTabi());
			EquipItem(new LeatherNinjaPants());
			EquipItem(new LeatherNinjaHood());
			EquipItem(new LeatherNinjaBelt());
			EquipItem(new LeatherNinjaMitts());
			EquipItem(new LeatherNinjaJacket());
		}

		public KeeperOfNinjitsu( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
