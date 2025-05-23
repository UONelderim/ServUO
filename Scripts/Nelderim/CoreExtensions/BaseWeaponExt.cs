#region References

using Nelderim;

#endregion

namespace Server.Items
{
	public partial class BaseWeapon : IResource2
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource2
		{
			get { return ExtraCraftResource.Get(this).Resource2; }
			set
			{
				UnscaleDurability();
				ExtraCraftResource.Get(this).Resource2 = value;
				Hue = CraftResources.GetHue(value);
				InvalidateProperties();
				ScaleDurability();
			}
		}
	}
}
