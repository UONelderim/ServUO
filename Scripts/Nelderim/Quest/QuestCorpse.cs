using System;
using Server;
using Server.Engines.Quests;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class QuestCorpse : Item
	{

		[Constructable]
        public QuestCorpse() : base( 0x3D68 )
		{
            Name = "Zwloki";
			Movable = false;
		}

        public QuestCorpse( Serial serial ) : base( serial )
		{
		}

		public override void OnSpeech(SpeechEventArgs e)
		{
			base.OnSpeech(e);
			
			BaseQuest quest = QuestHelper.GetQuest((PlayerMobile)e.Mobile, typeof(ClericPhase4Quest));

			if (quest != null)
			{
				foreach (BaseObjective objective in quest.Objectives)
					objective.Update(e.Speech);
			}
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
