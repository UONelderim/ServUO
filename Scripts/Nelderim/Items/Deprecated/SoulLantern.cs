namespace Server.Items
{
	public abstract class MagicLantern : GoldRing
	{
		public MagicLantern()
		{
		}

		public MagicLantern( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
    public class SoulLantern : MagicLantern
    {
        public Mobile owner;
        public int TrappedSouls;


        public SoulLantern()
        {
        }

        public SoulLantern(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int) 1); // version
            writer.Write((Mobile) owner);
            writer.Write(TrappedSouls);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            owner = reader.ReadMobile();
            TrappedSouls = reader.ReadInt();

            var newLantern = new DeathKnightLantern();
            newLantern.Owner = owner;
            newLantern.TrappedSouls = TrappedSouls;
            ReplaceWith(newLantern);
        }
    }
}
