using Nelderim.ExtraCraftResource;

namespace Server.Items
{
    public partial class BaseArmor
    {
        [CommandProperty( AccessLevel.GameMaster )]
        public CraftResource Resource2
        {
            get { return ExtraCraftResource.Get( this ).Resource2; }
            set { UnscaleDurability(); ExtraCraftResource.Get( this ).Resource2 = value; /*Hue = CraftResources.GetHue( m_Resource );*/ InvalidateProperties(); ScaleDurability(); }
        }
    }
}
