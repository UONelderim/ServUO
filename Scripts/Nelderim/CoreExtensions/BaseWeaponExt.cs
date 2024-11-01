#region References

using Nelderim;

#endregion

namespace Server.Items
{
	public partial class BaseWeapon
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource2
		{
			get { return ExtraCraftResource.Get(this).Resource2; }
			set
			{
				UnscaleDurability();
				ExtraCraftResource.Get(this).Resource2 = value; /*Hue = CraftResources.GetHue( m_Resource );*/
				InvalidateProperties();
				ScaleDurability();
			}
		}
	}
}
