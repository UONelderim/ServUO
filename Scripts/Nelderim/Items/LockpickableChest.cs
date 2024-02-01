using System;

namespace Server.Items
{
	public class LockpickingChest : LockableContainer
	{
		private static readonly TimeSpan Interval = TimeSpan.FromSeconds(15);

		private Timer m_ResetTimer;

		[Constructable]
		public LockpickingChest() : base(0x9AA)
		{
			Weight = 4.0;
			Lock();
			StartTimer();
		}

		private void Lock()
		{
			Locked = true;
			RequiredSkill = LockLevel = Utility.Random(50, 91);
		}

		private void StartTimer()
		{
			m_ResetTimer?.Stop();
			m_ResetTimer = Timer.DelayCall(Interval, Interval, Lock);
		}

		public override void LockPick(Mobile from)
		{
			Lock();
			from.SendMessage("Skrzynia magicznie zamyka się");
		}

		public override void OnDelete()
		{
			m_ResetTimer?.Stop();
			base.OnDelete();
		}

		public LockpickingChest(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			StartTimer();
		}
	}
}
