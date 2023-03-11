/* 
	Mail System - Version 1.0
	
	Newly Modified On 15/11/2016 
	
	By Veldian 
	Dragon's Legacy Uo Shard 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Server;

namespace Server.Mobiles
{
	public class PostMaster : BaseVendor
	{
		private List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos{ get { return m_SBInfos; } }

		public override NpcGuild NpcGuild{ get{ return NpcGuild.MagesGuild; } }

		[Constructable]
		public PostMaster() : base( "- listonosz" )
		{
			SetSkill( SkillName.EvalInt, 60.0, 83.0 );
			SetSkill( SkillName.Inscribe, 90.0, 100.0 );
		}

		public override void InitSBInfo()
		{
            m_SBInfos.Add(new SBPostMaster());
		}
		

        public PostMaster(Serial serial)
            : base(serial)
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
