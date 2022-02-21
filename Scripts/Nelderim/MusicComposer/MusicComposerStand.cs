#region References

using Server.Gumps;

#endregion

namespace Server.Items
{
	public class MusicComposerStand : Item
	{
		//this stores the song to be played
		[Constructable]
		public MusicComposerStand() : base(0x0EB8)
		{
			this.Weight = 1.0;
		}

		public MusicComposerStand(Serial serial) : base(serial)
		{
			this.Weight = 1.0;
		}

		//same as a regular song, except this is not pressed.
		public override void OnDoubleClick(Mobile from)
		{
			//if a player and alive
			if (!from.Player)
			{
				return;
			}

			if (!@from.Alive)
			{
				@from.SendMessage("Nie zrobisz tego, póki jesteś martwy.");
				return;
			}

			//send gump to user
			from.SendGump(new MusicComposerStandGump());
		}

		public override string DefaultName
		{
			get { return "Stojak do komponowania muzyki"; }
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
