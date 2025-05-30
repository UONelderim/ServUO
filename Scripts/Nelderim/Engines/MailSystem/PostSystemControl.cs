namespace Server.Items
{
	public class PostSystemControl : Item
	{
		private static PostSystemControl _Instance;
		public static PostSystemControl Instance => _Instance;
		
		public static bool Initialized => _Instance != null;
		
		[Constructable]
		public PostSystemControl() : base(0x1184)
		{
			if (_Instance != null)
			{
				_Instance.MoveToWorld(Location, Map);
				Timer.DelayCall(Delete);
			}
			else
			{
				_Instance = this;
			}
		}

		public static int GetMailCost(IMailItem mail, bool priority)
		{
			var costMultiplier = priority ? 2 : 1;
			if (mail is Item item)
				return (500 + item.TotalWeight * 50) * costMultiplier;
			return 1000 * costMultiplier;
		}
		
		public static string GetDeliveryDuration(IMailItem mail, bool priority)
		{
			return priority ? "30 minut" : "1 godzina";
		}

		public static void Schedule(Mobile from, IMailItem mail, bool priority)
		{
			
		}
		
		public PostSystemControl(Serial serial) : base(serial){}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			var version = reader.ReadInt();
			_Instance = this;
		}
	}
}
