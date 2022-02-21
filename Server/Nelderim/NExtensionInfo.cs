#region References

using Server;

#endregion

namespace Nelderim
{
	public abstract class NExtensionInfo
	{
		public Serial Serial { get; set; }

		public abstract void Serialize(GenericWriter writer);

		public abstract void Deserialize(GenericReader reader);
	}
}
